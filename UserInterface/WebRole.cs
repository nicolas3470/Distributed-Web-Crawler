using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace UserInterface
{
    public class WebRole : RoleEntryPoint
    {
        #region Private Fields
        private static CloudStorageAccount storageAccount;
        private static CloudBlobContainer databaseContainer;
        private static CloudBlobClient databaseClient; 
        #endregion

        public override bool OnStart()
        {
            //Initialize Indexer
            storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("CrawlerStorage"));

            //Initialize Database Blob
            databaseClient = storageAccount.CreateCloudBlobClient();
            databaseContainer = databaseClient.GetContainerReference("wordfrequencies");
            databaseContainer.CreateIfNotExist();
            var permission = databaseContainer.GetPermissions();
            permission.PublicAccess = BlobContainerPublicAccessType.Container;
            databaseContainer.SetPermissions(permission);

            return base.OnStart();
        }
    }
}
