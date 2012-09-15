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
using System.IO;

namespace WebCrawlerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        #region Private Fields
        private string currentURL;
        private string currentData;
        private int numCrawls;
        private int numInvalidURLs;
        private static CloudStorageAccount storageAccount;
        private static CloudQueue urlQueue;
        private static CloudQueue indexQueue;
        private static CloudQueueClient urlQueueClient;
        private static CloudQueueClient indexQueueClient;
        private const string startURL = "http://dmoz.org/";
        #endregion

        #region Main Method
        public override void Run()
        {
            while (true)
            {
                //Get next URL
                CloudQueueMessage nextURL = urlQueue.GetMessage();
                if (nextURL != null)
                {
                    currentURL = nextURL.AsString;
                    urlQueue.DeleteMessage(nextURL);

                    //Check if URL is valid
                    if (isURLValid(currentURL))
                    {
                        //Access HTML text
                        currentData = urlText(currentURL);

                        //Send html to Indexer
                        CloudQueueMessage newDataToIndex = new CloudQueueMessage(currentURL + "|n|i|c|c|" + currentData);
                        try
                        {
                            indexQueue.AddMessage(newDataToIndex);
                            Trace.WriteLine("\n\nSent data from " + currentURL + " to Index Queue\n\n");
                            numCrawls++;
                        }
                        catch (Exception)
                        {
                            //Message too large so don't send
                        }
                    }
                }
            }
        }
        #endregion

        #region Initialization
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 100;

            //Initialize Crawler
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

            ////Initialize Index Queue
            indexQueueClient = storageAccount.CreateCloudQueueClient();
            indexQueue = indexQueueClient.GetQueueReference("indexqueue");
            indexQueue.CreateIfNotExist();

            return base.OnStart();
        }
        #endregion

        #region Internal Helper Functions
        internal string urlText(string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            webRequest.UserAgent = "A C# Web Crawler for Academic Purposes";
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            StreamReader sReader = new StreamReader(stream);
            string text = sReader.ReadToEnd();
            return text;
        }

        internal bool isURLValid(string url)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.Timeout = 5000;
                webRequest.Method = "HEAD";
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                int pageStatus = (int)webResponse.StatusCode;
                if (pageStatus >= 100 && pageStatus < 400)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                numInvalidURLs++;
                return false;
            }
        }
        #endregion
    }
}
