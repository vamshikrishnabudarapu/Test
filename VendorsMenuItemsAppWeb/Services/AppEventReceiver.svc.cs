using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.EventReceivers;

namespace VendorsMenuItemsAppWeb.Services
{
    public class AppEventReceiver : IRemoteEventService
    {
        public SPRemoteEventResult ProcessEvent(SPRemoteEventProperties properties)
        {
            SPRemoteEventResult result = new SPRemoteEventResult();

            using (ClientContext clientContext = TokenHelper.CreateAppEventClientContext(properties, false))
            {
                if (clientContext != null)
                {
                    clientContext.Load(clientContext.Web);
                    clientContext.ExecuteQuery();
                    Web web = clientContext.Web;
                  //  List menuwebList = web.Lists.GetByTitle("VendorMenuItemsList");
                    //string a = clientContext.Web.Title;
                    string ab = clientContext.Web.Url;
                    Console.WriteLine(ab);
                    Web oWebsite = clientContext.Web;
                    GroupCollection collGroup = clientContext.Web.SiteGroups;
                    clientContext.Load(collGroup);
                    clientContext.ExecuteQuery();
                    //CreateGroup(collGroup, "VendorDailyMenuSecurityGroup", oWebsite, clientContext);
                    GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
                    groupCreationInfo.Title = "VendorDailyMenuSecurityGroup";
                    groupCreationInfo.Description = "VendorDailyMenuSecurityGroup";
                    Group spGroup = web.SiteGroups.Add(groupCreationInfo);

                    RoleDefinition rd =web.RoleDefinitions.GetByName("Contribute");

                    RoleDefinitionBindingCollection rdb = new RoleDefinitionBindingCollection(clientContext);

                    rdb.Add(rd);

                    web.RoleAssignments.Add(spGroup, rdb);

                    spGroup.Update();
                    clientContext.ExecuteQuery();

                }
            }

            return result;
        }

        public void ProcessOneWayEvent(SPRemoteEventProperties properties)
        {
            // This method is not used by app events
        }
        //private void CreateGroup(GroupCollection collGroup, string groupName, Web oWebsite, ClientContext clientContext)
        //{
        //    Group grp = collGroup.Where(g => g.Title == groupName).FirstOrDefault();
        //    if (grp == null)
        //    {
        //        GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
        //        groupCreationInfo.Title = groupName;
        //        groupCreationInfo.Description = "Description of " + groupName;
        //        Group oGroup = oWebsite.SiteGroups.Add(groupCreationInfo);
        //        RoleDefinitionBindingCollection collRoleDefinitionBinding = new RoleDefinitionBindingCollection(clientContext);
        //        RoleDefinition oRoleDefinition = oWebsite.RoleDefinitions.GetByType(RoleType.Contributor);
        //        collRoleDefinitionBinding.Add(oRoleDefinition);
        //        oWebsite.RoleAssignments.Add(oGroup, collRoleDefinitionBinding);
        //        clientContext.Load(oGroup, group => group.Title);
        //        clientContext.Load(oRoleDefinition, role => role.Name);
        //        clientContext.ExecuteQuery();
        //    }
        //}
    }
}
