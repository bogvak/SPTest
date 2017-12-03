using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SPTest.Features.timerJobFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7bc406c5-120d-4e7a-aa98-1d259c0a2580")]
    public class TimerJobFeatureEventReceiver : SPFeatureReceiver
    {
        const string JobName = "Autolist New Item Creating";

        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSite parentSite = (SPSite)properties.Feature.Parent;
                SPWebApplication parentWebApp = parentSite.WebApplication;
                DeleteExistingJob(JobName, parentWebApp);
                CreateJob(parentWebApp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSite parentSite = (SPSite)properties.Feature.Parent;
                SPWebApplication parentWebApp = parentSite.WebApplication;
                DeleteExistingJob(JobName, parentWebApp);
            }
            catch (Exception ex)
            {
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}

        private void CreateJob(SPWebApplication site)
        {
            try
            {

                AutolistItemCreatingJob job = new AutolistItemCreatingJob(JobName, site);
                SPMinuteSchedule schedule = new SPMinuteSchedule
                {
                    BeginSecond = 0,
                    EndSecond = 59,
                    Interval = 5
                };
                job.Schedule = schedule;
                job.Update();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void DeleteExistingJob(string jobName, SPWebApplication site)
        {
            try
            {
                foreach (SPJobDefinition job in site.JobDefinitions)
                {
                    if (job.Name == jobName)
                    {
                        job.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
