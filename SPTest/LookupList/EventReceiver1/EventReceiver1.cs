using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace SPTest.LookupList.EventReceiver1
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
        /// <summary>
        /// An item was added.
        /// </summary>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            base.ItemAdded(properties);

            // Getting reference to working lists and selected value
            SPList lookupList = properties.List;
            SPList autoList = lookupList.ParentWeb.Lists["AutoList"];
            SPListItem selectedListItem = properties.ListItem;
            string selectedAutoIDListItem = selectedListItem["Link"] as string;
            var currentLookUpValue = new SPFieldLookupValue(selectedAutoIDListItem);

            // Copying description from LookupList to AutoList
            SPListItem itemToChange = autoList.GetItemById(currentLookUpValue.LookupId);
            itemToChange["Description"] = selectedListItem["Description"];
            itemToChange.Update();
            autoList.Update();
        }


    }
}