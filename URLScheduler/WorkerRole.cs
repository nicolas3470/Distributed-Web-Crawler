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

namespace URLScheduler
{
    public class WorkerRole : RoleEntryPoint
    {
        #region Private Fields
        private const int crawlsToPerform = 1;
        private const string startURL = "http://dmoz.org/";
        private int numCrawls;
        private CloudQueue queue;
        private static CloudStorageAccount storageAccount;
        private CloudQueueClient queueClient;
        #endregion

        public override void Run()
        {
            while (true)
            {

            }

            
            //        if (crawlerQueue.Count != 0)
            //        {
            //            Console.WriteLine("Using available crawler");
            //            //Use an available crawler
            //            WebCrawler nextCrawler = crawlerQueue.Dequeue();
            //            string nextURL = urlQueue.Dequeue();
            //            new Thread(nextCrawler.Crawl).Start(nextURL);
            //            crawlsScheduled++;
            //        }
            //        else if (numCrawlers <= maxCrawlers)
            //        {
            //            //Console.WriteLine("Create a crawler");
            //            //Create a new crawler
            //            WebCrawler newCrawler = new WebCrawler(numCrawlers++);
            //            string nextURL = urlQueue.Dequeue();
            //            new Thread(newCrawler.Crawl).Start(nextURL);
            //            crawlsScheduled++;
            //            //Console.WriteLine("Scheduled new crawler");
            //        } // Otherwise, there are no available resources
            //        crawlerSema.Release();
            //    }
            //    queueSema.Release();
            //    //Console.WriteLine("URL scheduler done");
            //}

            ////Stop Scheduling, clear crawler queue
            //crawlerSema.WaitOne();
            //crawlerQueue.Clear();
            //crawlerSema.Release();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //Initialize Storage
            storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("CrawlerStorage"));

            //Create Scheduler Queue
            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("urlschedulerqueue");
            queue.CreateIfNotExist();

            //Add first URL to the queue
            CloudQueueMessage firstURL = new CloudQueueMessage(startURL);
            queue.AddMessage(firstURL);

            return base.OnStart();
        }
    }
}
