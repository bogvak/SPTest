using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace SPTest.Features.mainFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("222aa074-dfea-4704-9575-4137e3b681ea")]
    public class mainFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite deploySite = (SPSite)properties.Feature.Parent;
            SPWeb deployWeb = (SPWeb)deploySite.RootWeb;
            SPWebCollection subWebCollection = deployWeb.Webs;
            string currentTemplate = deployWeb.WebTemplate;
            string siteUrl = "subweb";
            string siteTitle = "SP test Bogdan Vakulyuk";
            string siteDescription = "Test task for Accenture applying";

            SPWeb newTestSite;

            try
            {
                newTestSite = subWebCollection[siteUrl];
                newTestSite.Delete();
            }
            catch
            {
            }
            finally
            {

                newTestSite = subWebCollection.Add(siteUrl, siteTitle, siteDescription, 1033,
                    currentTemplate, true, false);
            }

            // Instantiating AutoList from appropriate template
            SPListTemplate myAutoListTemplate = deployWeb.ListTemplates["AutoList"];
            Guid autoListGUID = newTestSite.Lists.Add("AutoList", "New AutoList", myAutoListTemplate);
            SPList myAutoList = newTestSite.Lists["AutoList"];

            // Instatiating LookupList from appropriate template
            SPListTemplate myLookupListTemplate = deployWeb.ListTemplates["LookupList"];
            Guid lookupListGUID = newTestSite.Lists.Add("LookupList", "New LookupList", myLookupListTemplate);
            SPList myLookupList = newTestSite.Lists["LookupList"];

            //// Adding lookup field 'Link' to LookupList
            string strLinkField = myLookupList.Fields.AddLookup("Link", autoListGUID, true);
            SPFieldLookup myLinkField = (SPFieldLookup)myLookupList.Fields.GetFieldByInternalName(strLinkField);
            myLinkField.LookupField = myAutoList.Fields["Title"].InternalName;

            // Adding 'Link' field to default view
            SPView defaultView = myLookupList.DefaultView;
            defaultView.ViewFields.Add(myLinkField);
            defaultView.Update();

            myLinkField.Update();

            //
            myLookupList.Update();
            myAutoList.Update();
            newTestSite.Update();

            // Saving AutoList reference to parameters storage
            SPTestGlobalParameters.siteCollectionURL = deploySite.Url;
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


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
    }
}
