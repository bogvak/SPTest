using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace SPTest
{
    class AutolistItemCreatingJob : SPJobDefinition
    {
        private long sumFib = 0;
        private long curFib = 1;
        public AutolistItemCreatingJob()
            : base()
        {

        }

        public AutolistItemCreatingJob(string jobName, SPService service,
               SPServer server, SPJobLockType lockType)
               : base(jobName, service, server, lockType)
        {
            this.Title = jobName;
        }

        public AutolistItemCreatingJob(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.ContentDatabase)
        {
            this.Title = jobName;
        }

        public override void Execute(Guid targetInstanceId)
        {
            long tempSum;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                SPWebApplication webapp = this.Parent as SPWebApplication;
                SPContentDatabase contentDb = webapp.ContentDatabases[targetInstanceId];

                using (SPSite mainSite = contentDb.Sites[SPTestGlobalParameters.siteCollectionURL])
                {
                    // Getting reference to target website and list

                    SPWeb newWeb = mainSite.AllWebs["subweb"];
                    SPList autoList = newWeb.Lists["AutoList"];

                    // Adding new item to list and udating it

                    SPListItem newItem = autoList.AddItem();
                    newItem["Title"] = curFib.ToString();
                    newItem["Value"] = DateTime.Now;
                    newItem.Update();
                    autoList.Update();

                    // Calculating new Fibonacci number

                    tempSum = curFib;
                    curFib = curFib + sumFib;
                    sumFib = tempSum;
                }
            });          
        }
    }
}
