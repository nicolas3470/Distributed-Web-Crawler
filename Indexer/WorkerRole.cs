using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Indexer
{
    public class WorkerRole : RoleEntryPoint
    {
        #region Private Fields
        private string currentData;
        private string currentURL;
        private int numIndexes;
        private static CloudStorageAccount storageAccount;
        private static CloudQueue urlQueue;
        private static CloudQueue indexQueue;
        private static CloudBlobContainer databaseContainer;
        private static CloudQueueClient urlQueueClient;
        private static CloudQueueClient indexQueueClient;
        private static CloudBlobClient databaseClient;
        private const string startURL = "http://dmoz.org/";
        private static String[] unwantedExtensions = { ".doc", ".rtf", ".txt", ".pdf", ".xls", ".xpi", ".rss", ".atom", ".opml", ".vcard", ".exe", ".dmg", ".app", ".pps", ".ical", ".jpg", ".gif", ".png", ".bmp", ".svg", ".eps", ".swf", ".fla", ".css", ".mp3", ".wav", ".ogg", ".wma", ".m4a", ".zip", ".rar", ".gzip", ".bzip", ".ace", ".ttf", ".mov", ".wmv", ".mp4", ".avi", ".mpg", ".phps", ".torrent.", ".ico" };
        #endregion

        #region Main Method
        public override void Run()
        {
            while (true)
            {
                //Get the next html data
                CloudQueueMessage nextToIndex = indexQueue.GetMessage();
                if (nextToIndex != null)
                {
                    currentData = nextToIndex.AsString;
                    indexQueue.DeleteMessage(nextToIndex);
                    string[] urlAndData = Regex.Split(currentData, @"\|n\|i\|c\|c\|");
                    currentURL = urlAndData[0];
                    currentData = urlAndData[1];
                    //Trace.WriteLine("\n\nURL:\n" + currentURL + "\n\n");
                    //Trace.WriteLine("\n\nData:\n" + currentData + "\n\n");

                    //Process Data
                    Tuple<Dictionary<string, int>, List<string>> frequenciesAndURLs = getFrequenciesAndURLs(currentURL, currentData);

                    //Send info to database
                    storeData(currentURL, frequenciesAndURLs.Item1);

                    int i = 0;
                    //Send new URLs to URL queue if they haven't been crawled
                    foreach (var newURL in frequenciesAndURLs.Item2)
                    {
                        //Check if URL in storage
                        if (!doesURLDataExist(newURL))
                        {
                            i++;
                            //If not then add to url queue
                            CloudQueueMessage newLink = new CloudQueueMessage(newURL);
                            urlQueue.AddMessage(newLink);
                        }
                    }
                    //Trace.WriteLine("\n\n" + frequenciesAndURLs.Item2.Count + " URLs added to scheduler");
                    Trace.WriteLine("\n\n" + i + " URLs added to scheduler");
                    Trace.WriteLine("\n\nSent indexed data from " + currentURL + " to Storage\n\n");
                    numIndexes++;
                }
            }
        }
        #endregion

        #region Initialization
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 100;

            //Initialize Indexer
            storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("CrawlerStorage"));

            //Initialize URL Queue
            urlQueueClient = storageAccount.CreateCloudQueueClient();
            urlQueue = urlQueueClient.GetQueueReference("urlqueue");
            if (urlQueue.CreateIfNotExist())
            {
                //Add first URL to the queue
                CloudQueueMessage firstURL = new CloudQueueMessage(startURL);
                urlQueue.AddMessage(firstURL);
            }

            //Initialize Index Queue
            indexQueueClient = storageAccount.CreateCloudQueueClient();
            indexQueue = indexQueueClient.GetQueueReference("indexqueue");
            indexQueue.CreateIfNotExist();

            //Initialize Database Blob
            databaseClient = storageAccount.CreateCloudBlobClient();
            databaseContainer = databaseClient.GetContainerReference("wordfrequencies");
            databaseContainer.CreateIfNotExist();
            var permission = databaseContainer.GetPermissions();
            permission.PublicAccess = BlobContainerPublicAccessType.Container;
            databaseContainer.SetPermissions(permission);
    
            return base.OnStart();
        }
        #endregion

        #region Internal Helper Functions
        internal Tuple<Dictionary<string, int>, List<string>> getFrequenciesAndURLs(string url, string data)
        {
            Dictionary<string, int> frequencies = dataTextFrequencies(data);
            List<string> urls = getURLS(url, data);
            var freqURLTuple = Tuple.Create(frequencies, urls);
            return freqURLTuple;
        }

        internal List<string> getURLS(string originalURL, string data)
        {
            var urlList = new List<string>();
            MatchCollection urls = Regex.Matches(data, "href=\"[a-zA-Z./:&\\d_-]+\"");
            string currentURL;
            foreach (Match url in urls)
            {
                currentURL = url.Value.Replace("href=\"", "");
                currentURL = currentURL.Substring(0, currentURL.IndexOf("\""));
                if (currentURL.Length < 4 || !currentURL.Substring(0, 3).Equals("http"))
                {
                    currentURL = originalURL + currentURL;
                }
                if (!unwantedExtensions.Any(currentURL.Contains))
                {
                    urlList.Add(currentURL);
                }
            }
            return urlList;
        }

        internal Dictionary<string, int> dataTextFrequencies(string data)
        {
            var frequencies = new Dictionary<string, int>();

            //Get text only
            string text = pageTextOnly(data);

            //Remove punctuation from text
            text = Regex.Replace(text, @"[\p{P}+]", "", RegexOptions.Singleline);
            text = Regex.Replace(text, @"\|", " ", RegexOptions.Singleline);
            text = Regex.Replace(text, @"\s+", " ", RegexOptions.Singleline);

            //Get Frequencies
            string[] words = text.ToLower().Split(' ');
            foreach (string word in words)
            {
                if (!word.Equals(""))
                {
                    if (frequencies.ContainsKey(word))
                    {
                        frequencies[word]++;
                    }
                    else
                    {
                        frequencies[word] = 1;
                    }
                }
            }
            return frequencies;
        }

        internal string pageTextOnly(string data)
        {
            //Remove CSS styles
            string text = Regex.Replace(data, "<style(.| )*?>*</style>", " ", RegexOptions.Singleline);

            //Remove script blocks
            text = Regex.Replace(text, "<script(.| )*?>*</script>", " ", RegexOptions.Singleline);

            //Remove all images
            text = Regex.Replace(text, "<img(.| )*?/>", " ", RegexOptions.Singleline);

            //Remove all HTML tags, leaving hyperlink text
            text = Regex.Replace(text, "<(.| )*?>", " ", RegexOptions.Singleline);
            return text;
        }

        internal void storeData(string url, Dictionary<string, int> wordFrequencies)
        {
            CloudBlob newBlob = databaseContainer.GetBlobReference(url);
            //Stream Dictionary to blob
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, wordFrequencies.ToArray());
                    stream.Position = 0L;
                    newBlob.UploadFromStream(stream);
                }
            }
            catch (Exception)
            {
                //Trace.WriteLine("\n\nDAMNNNNN\n\n");
            }
        }

        internal bool doesURLDataExist(string url)
        {
            //Trace.WriteLine("\n\n" + url + "\n\n");
            try
            {
                CloudBlob blob = databaseContainer.GetBlobReference(url);
                blob.FetchAttributes();
                Trace.WriteLine("\n\nPLEASEEEE\n\n");
                return true;
            }
            catch (Exception e)
            {
                //if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                //{
                //    return false;
                //}
                //else
                //{
                //    Trace.WriteLine(e.ErrorCode.ToString());
                //    //throw;
                //    return false;
                //}
                return false;
            }
        }
        #endregion
    }
}
