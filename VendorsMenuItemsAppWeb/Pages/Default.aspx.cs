using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.Data;
using VendorsMenuItemsAppWeb.LicenseVerificationService;
//using Microsoft.SharePoint;


namespace VendorsMenuItemsAppWeb.Pages
{
    public partial class Default : System.Web.UI.Page
    {
         bool _testMode = true;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // The following code gets the client context and Title property by using TokenHelper.
            // To access other properties, you may need to request permissions on the host web.

            var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);
            var hostWeb = Page.Request["SPHostUrl"];
            var appweburl = Page.Request["SPAppWebUrl"];
            using (var clientContext = TokenHelper.GetClientContextWithContextToken(hostWeb, contextToken, Request.Url.Authority))
            {

                clientContext.Load(clientContext.Web, web => web.Title, s => s.CurrentUser.LoginName);
                clientContext.ExecuteQuery();


                 //ProductId must match the one on the AppManifest.xml of the SharePoint App that is retrieving its license
            //An app can only retrieve its own licenses for security reasons (e.g. you cannot retrieve other Apps licenses)
            //The one hardcoded here matches this test app

                Guid ProductId = new Guid("840c0bfa-45d0-4bcb-b8b3-2bce846614ae");
            string uiWarningMessage = null;
            string AppVersionPeriod = null;

            //Get Context token to call SP; standard oAuth
            //string contextToken = Session["contexToken"].ToString();

            TokenHelper.TrustAllCertificates();
            //ClientContext ctx = TokenHelper.GetClientContextWithContextToken(Session["SPHostUrl"].ToString(), Session["contexToken"].ToString(), Request.Url.Authority);

            //Use helper method to retrieve a processed license object
            //It is recommended to CACHE the VerifyEntitlementTokenResponse result until the storeLicense.tokenExpirationDate
            VerifyEntitlementTokenResponse verifiedLicense = SPAppLicenseHelper.GetAndVerifyLicense(ProductId, clientContext);

            //Get UI warning. 
            //Note that the name of the app is being hardcoded to Cheezburgers because is an app that already exists on the marketplace
            //You should use your exact app display name instead (make sure name matches with the metadata you submit to seller dashboard
            uiWarningMessage = SPAppLicenseHelper.GetUIStringText(verifiedLicense, hostWeb, Request.Url.ToString(), "Daily Menu App", " ");
            //AppVersionPeriod = SPAppLicenseHelper.GetVersionPeriodApp(verifiedLicense, hostWeb, Request.Url.ToString(), "Daily Menu App");
            if (verifiedLicense == null)
            {
                //No license found or the license was malformed
                //The UI string retrieved above will already contain the appropiate info
                //In real app code you could take additional steps (e.g. provide reduced functionality)
            }
            else
            {


                //There is a well-formed license; must look at properties to determine validity
                if (verifiedLicense.IsValid && verifiedLicense.IsTest!=true)
                {
                    //Valid production license
                    //For app sample purposes display 'Production License' + the corresponding warning text; replace this with your own logic. 
                    //uiWarningMessage = "Production License: " + uiWarningMessage;
                    processLicense(verifiedLicense, appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
                }
                //else if (verifiedLicense.IsTest && _testMode == true)
                //{
                //    //Test mode with valid test token
                //    //For debug we just display 'Test License' plus the corresponding UI warning text; in a real world production scenario _testMode should be set to false and the test license should be rejected. 
                //    uiWarningMessage = "Test License: " + uiWarningMessage;
                //    processLicense(verifiedLicense, appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
                //    _testMode = false;
                //}
                else
                {
                    //Beep, production mode with invalid license
                    //Warn the user about missing/invalid license
                    uiWarningMessage = "Invalid License!!! " + uiWarningMessage;
                    NoItems.Visible = false;
                    EditItem.Visible = false;
                    CreateItem.Visible = false;
                    ListSettings.Visible = false;
                }
            }

            //Sets the text of the alert
            lblWarning.Text = uiWarningMessage;





            //if (AppVersionPeriod == "Paid" && verifiedLicense.IsTest!=true)
            //{
            //    maincode(appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
            //}
            //else if (AppVersionPeriod == "TrialRemaining" && verifiedLicense.IsTest != true)
            //{
            //    maincode(appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
            //}
            //else if (AppVersionPeriod == "TrialExpired" || verifiedLicense.IsTest == true)
            //{
            //    NoItems.Visible = false;
            //    EditItem.Visible = false;
            //    CreateItem.Visible = false;
            //    ListSettings.Visible = false;
            //}
                 
          



                // Response.Write(hostWeb);
                //Response.Write(appweburl + "/Lists/VendorMenuItemsList/calendar.aspx");
              
            }
        }


        protected void processLicense(VerifyEntitlementTokenResponse verifiedLicense, string appweburl, string hostWeb, string contextToken, ClientContext clientContext)
        {
            SPAppLicenseType licenseType = SPAppLicenseHelper.GetLicenseTypeFromLicense(verifiedLicense);
            switch (licenseType)
            {
                case SPAppLicenseType.Trial:
                    //Do something for a valid trial besides presenting a message on the UI
                    if (SPAppLicenseHelper.GetRemainingDays(verifiedLicense) > 0)
                    {
                        //Valid trial
                        //The UI string retrieved above will already contain the appropiate info
                        //In real app code you could take additional steps (we encourage trials to be fully featured) 
                        //Helper code will return int.MaxValue for an unlimited trial
                        maincode(appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
                    }
                    else
                    {
                        //Expired trial
                        //The UI string retrieved above will already contain the appropiate info
                        //In real app code you could take additional steps (e.g. provide reduced functionality) 
                        NoItems.Visible = false;
                        EditItem.Visible = false;
                        CreateItem.Visible = false;
                        ListSettings.Visible = false;
                    }
                    break;
                case SPAppLicenseType.Paid:
                    //Do something for a paid app
                    WarningStatusPeriod.Visible = false;
                    maincode(appweburl.ToString(), hostWeb.ToString(), contextToken.ToString(), clientContext);
                    break;
                case SPAppLicenseType.Free:
                    //Do something for a free app
                    NoItems.Visible = false;
                        EditItem.Visible = false;
                        CreateItem.Visible = false;
                        ListSettings.Visible = false;
                    break;
                default:
                    throw new Exception("Unknown License Type");

            }
        }
        public void maincode(string appweburl,string hostWeb,string contextToken,ClientContext clientContext)
        {
            AppwebURLEdit.Value = appweburl + "/Lists/VendorMenuItemsList/calendar.aspx";
            AppwebURLNew.Value = appweburl + "/Lists/VendorMenuItemsList/NewForm.aspx";
            HostURLmain.Value = hostWeb;

            var AppWebContext = TokenHelper.GetClientContextWithContextToken(appweburl, contextToken, Request.Url.Authority);

            Web site = AppWebContext.Web;
            List menuList = site.Lists.GetByTitle("VendorMenuItemsList");

            //string currentUser = clientContext.Web.CurrentUser.LoginName;
            string currentUser = clientContext.Web.CurrentUser.LoginName;
            AppWebContext.Load(menuList, m => m.EffectiveBasePermissions, id => id.Id);
            AppWebContext.ExecuteQuery();
            Guid listid = menuList.Id;
            //string listIDReplace1=listid.ToString().Replace("{","%7B");
            string mainLisIDSetting = "%7B" + listid.ToString() + "%7D";
            ListSettingsAppWebURL.Value = appweburl + "/_layouts/15/ListEdit.aspx?List=" + mainLisIDSetting;

            if (menuList.EffectiveBasePermissions.Has(Microsoft.SharePoint.Client.PermissionKind.AddListItems))
            {
                EditItem.Visible = true;
                CreateItem.Visible = true;
                ListSettings.Visible = true;
            }
            else
            {
                EditItem.Visible = false;
                CreateItem.Visible = false;
                ListSettings.Visible = false;
            }

            CamlQuery qry = new CamlQuery();

            qry.ViewXml = @"<View> <Query>
                                                  <Where>
                                                  <And> 
                                                <DateRangesOverlap>
                                                    <FieldRef Name='EventDate' />
                                                    <FieldRef Name='EndDate'/>
                                                    <FieldRef Name='RecurrenceID'/>
                                                    <Value Type='DateTime' IncludeTimeValue='False'>
                                                    <Today/>
                                                    </Value>
                                                 </DateRangesOverlap>

                                                     <Lt>
                                                     <FieldRef Name='EventDate'/>
                                                          <Value Type='DateTime'>
                                                         <Today/>
                                                      </Value>
                                                          </Lt> 


                                                   </And>
                                                  </Where>
                                             <QueryOptions>
        
                                         <ViewAttributes Scope='Recursive'/>
                                        <RecurrencePatternXMLVersion>v3</RecurrencePatternXMLVersion>
                                       <ExpandRecurrence>true</ExpandRecurrence>
                                       <RecurrenceOrderBy>true</RecurrenceOrderBy>
                                        <ViewAttributes Scope='RecursiveAll'/>
                                            <CalendarDate>
                                                   <Today/>
                                                     </CalendarDate>
                                               </QueryOptions>

                                                   <OrderBy>
                                                     <FieldRef Name='Location' />
                                                    <FieldRef Name='Modified' />
                                                   </OrderBy>
                                                </Query>
                                               <ViewFields>
                                                  <FieldRef Name='Vendor_Names_Item' />
                                                  <FieldRef Name='Vendor_Location' />
                                                  <FieldRef Name='EventDate' />
                                                  <FieldRef Name='EndDate' />
                                                    <FieldRef Name='Title' />
                                                  <FieldRef Name='Breakfast_Menu_Items' />
                                                  <FieldRef Name='Lunch_Menu_Items' />
                                                  <FieldRef Name='Dinner_Menu_Items' />
                                                  <FieldRef Name='Common_Menu_Items' />
                                                    <FieldRef Name='Modified' />
                                               </ViewFields>
                                        </View>";
            ListItemCollection listItems = menuList.GetItems(qry);
            AppWebContext.Load(listItems);
            AppWebContext.ExecuteQuery();

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Vednor Name");
            //dt.Columns.Add("Vendor Location");
            //dt.Columns.Add("StartDate");
            //dt.Columns.Add("EndDate");
            //dt.Columns.Add("Breakfast menu");
            //dt.Columns.Add("Lunch menu");
            //dt.Columns.Add("Dinner menu");
            //dt.Columns.Add("Common menu");
            //dt.Columns.Add("Modified");


            if (listItems.Count.ToString() == "0")
            {
                //NoItemsFound.Value = "There are no items present ";
                NoItems.Visible = true;
            }
            else
            {
                NoItems.Visible = false;
                List<Vendor> vendor_obj = new List<Vendor>();

                List<VendorTitle> title = new List<VendorTitle>();
                List<Categories> categories = new List<Categories>();
                bool flag = false;
                bool title_flag = false;
                bool different_flag = false;
                bool sub_flag = false;
                //  Timings time = new Timings();

                foreach (ListItem items in listItems)
                {
                    //DataRow dr = dt.NewRow();
                    //dr["Vednor Name"] = items["Vendor_Names_Item"];
                    //dr["Vendor Location"] = items["Location"];
                    //dr["StartDate"] = items["EventDate"];
                    //dr["EndDate"] = items["EndDate"];
                    //dr["Breakfast menu"] = items["Breakfast_Menu_Items"];
                    //dr["Lunch menu"] = items["Lunch_Menu_Items"];
                    //dr["Dinner menu"] = items["Dinner_Menu_Items"];
                    //dr["Common menu"] = items["Common_Menu_Items"];
                    //dr["Modified"] = items["Modified"];
                    //dt.Rows.Add(dr);
                    string vendorLocation = "";
                    string vendorName = "";

                    string BreakFast = "";
                    string Lunch = "";
                    string Dinner = "";
                    string CommonMenu = "";
                    string ItemTitle = "";

                    if (items["Vendor_Location"] != null)
                    {
                        vendorLocation = items["Vendor_Location"].ToString();
                    }
                    if (items["Vendor_Names_Item"] != null)
                    {
                        vendorName = items["Vendor_Names_Item"].ToString();
                    }

                    DateTime modifiedDate = Convert.ToDateTime(items["Modified"].ToString());

                    if (items["Breakfast_Menu_Items"] != null)
                    {
                        BreakFast = items["Breakfast_Menu_Items"].ToString();
                    }
                    if (items["Lunch_Menu_Items"] != null)
                    {
                        Lunch = items["Lunch_Menu_Items"].ToString();
                    }
                    if (items["Dinner_Menu_Items"] != null)
                    {
                        Dinner = items["Dinner_Menu_Items"].ToString();
                    }
                    if (items["Common_Menu_Items"] != null)
                    {
                        CommonMenu = items["Common_Menu_Items"].ToString();
                    }
                    if (items["Title"] != null)
                    {
                        ItemTitle = items["Title"].ToString();
                    }
                    different_flag = false;
                    title_flag = false;
                    sub_flag = false;
                    //flag = false;
                    bool vendorCount = true;
                    if (vendor_obj.Count == 0)
                    {

                        categories.Add(new Categories { Breakfast = BreakFast, Lunch = Lunch, Dinner = Dinner, CommonMenu = CommonMenu, ModifiedDate = modifiedDate });
                        title.Add(new VendorTitle { VendorName = vendorName, categories = categories });
                        vendor_obj.Add(new Vendor { Location = vendorLocation, VendorTitle = title });
                    }
                    else
                    {
                        vendorCount = true;
                        for (int i = 0; i < vendor_obj.Count; i++)
                        {
                            different_flag = false;
                            flag = false;
                            if (vendor_obj[i].Location == vendorLocation)
                            {
                                flag = true;
                            }
                            if (flag == true)
                            {

                                if (different_flag == false)
                                {
                                    List<Categories> differnt_categories1 = new List<Categories>();
                                    vendorCount = false;
                                    for (int j = 0; j < vendor_obj[i].VendorTitle.Count; j++)
                                    {
                                        for (int k = 0; k < vendor_obj[i].VendorTitle[j].categories.Count; k++)
                                        {
                                            title_flag = false;
                                            if (vendor_obj[i].VendorTitle[j].VendorName != vendorName)
                                            {
                                                title_flag = true;
                                            }
                                            if (sub_flag == false)
                                            {
                                                //if (title_flag == true && k == vendor_obj[i].VendorTitle.Count - 1)
                                                if (title_flag == true && j == vendor_obj[i].VendorTitle.Count - 1)
                                                {
                                                    // if(vendor_obj)
                                                    differnt_categories1.Add(new Categories { Breakfast = BreakFast, Lunch = Lunch, Dinner = Dinner, CommonMenu = CommonMenu, ModifiedDate = modifiedDate });
                                                    vendor_obj[i].VendorTitle.Add(new VendorTitle { VendorName = vendorName, categories = differnt_categories1 });
                                                    k = vendor_obj[i].VendorTitle[j].categories.Count;
                                                    j = vendor_obj[i].VendorTitle.Count;

                                                    break;
                                                }
                                                else
                                                {
                                                    if (vendor_obj[i].VendorTitle[j].categories[k].ModifiedDate < modifiedDate && vendor_obj[i].VendorTitle[j].VendorName == vendorName)
                                                    {
                                                        if (ItemTitle == "Deleted: ")
                                                        {
                                                            vendor_obj[i].VendorTitle[j].categories.Remove(vendor_obj[i].VendorTitle[j].categories[k]);
                                                            vendor_obj[i].VendorTitle.Remove(vendor_obj[i].VendorTitle[j]);
                                                            vendorCount = false;
                                                            //vendor_obj[i].VendorTitle[j].categories[k].Breakfast = "";
                                                            //vendor_obj[i].VendorTitle[j].categories[k].Lunch = Lunch;
                                                            //vendor_obj[i].VendorTitle[j].categories[k].Dinner = Dinner;
                                                            //vendor_obj[i].VendorTitle[j].categories[k].CommonMenu = CommonMenu;
                                                            //vendor_obj[i].VendorTitle[j].categories[k].ModifiedDate = modifiedDate;
                                                            if (1 <= vendor_obj[i].VendorTitle.Count)
                                                            {
                                                                k = vendor_obj[i].VendorTitle[j].categories.Count;
                                                                j = vendor_obj[i].VendorTitle.Count;

                                                            }

                                                            break;
                                                        }
                                                        else
                                                        {
                                                            vendor_obj[i].VendorTitle[j].categories[k].Breakfast = BreakFast;
                                                            vendor_obj[i].VendorTitle[j].categories[k].Lunch = Lunch;
                                                            vendor_obj[i].VendorTitle[j].categories[k].Dinner = Dinner;
                                                            vendor_obj[i].VendorTitle[j].categories[k].CommonMenu = CommonMenu;
                                                            vendor_obj[i].VendorTitle[j].categories[k].ModifiedDate = modifiedDate;
                                                            k = vendor_obj[i].VendorTitle[j].categories.Count;
                                                            j = vendor_obj[i].VendorTitle.Count;
                                                            vendorCount = false;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sub_flag = true;

                                                    }
                                                }
                                            }

                                            title_flag = false;
                                            sub_flag = false;

                                        }

                                    }

                                    if (vendor_obj[i].VendorTitle.Count == 0)
                                    {
                                        vendor_obj.Remove(vendor_obj[i]);
                                    }
                                    if (vendorCount == false)
                                    {
                                        i = vendor_obj.Count;
                                    }
                                }
                            }
                            else
                            {

                                if (i == vendor_obj.Count - 1)
                                {
                                    List<VendorTitle> different_title = new List<VendorTitle>();
                                    List<Categories> differnt_categories = new List<Categories>();

                                    differnt_categories.Add(new Categories { Breakfast = BreakFast, Lunch = Lunch, Dinner = Dinner, CommonMenu = CommonMenu, ModifiedDate = modifiedDate });
                                    different_title.Add(new VendorTitle { VendorName = vendorName, categories = differnt_categories });
                                    vendor_obj.Add(new Vendor { Location = vendorLocation, VendorTitle = different_title });
                                    different_flag = true;
                                    break;

                                }
                            }
                        }
                    }

                }
                //GridView1.DataSource = dt;
                //GridView1.DataBind();

                DataTable locationDT = new DataTable();
                locationDT.Columns.Add("Location");
                locationDT.Columns.Add("Location_Updated");
                locationDT.Columns.Add("VendortitleListName");
                locationDT.Columns.Add("Vendortitlecategory");
                DataTable VerticleMenulocationdt = new DataTable();
                VerticleMenulocationdt.Columns.Add("Location");
                VerticleMenulocationdt.Columns.Add("Location_Updated");

                for (int i = 0; i < vendor_obj.Count; i++)
                {
                    DataRow locationdr = locationDT.NewRow();
                    locationdr["Location"] = vendor_obj[i].Location;
                    locationdr["Location_Updated"] = vendor_obj[i].Location.Replace(" ", "_");

                    DataRow VerticleMenulocationdr = VerticleMenulocationdt.NewRow();
                    VerticleMenulocationdr["Location"] = vendor_obj[i].Location;
                    VerticleMenulocationdr["Location_Updated"] = vendor_obj[i].Location.Replace(" ", "_");

                    string vendornameHTMLs = "";

                    for (int t = 0; t < vendor_obj[i].VendorTitle.Count; t++)
                    {
                        string vendorcategoryHTMLs = "";
                        string vendornames = vendor_obj[i].VendorTitle[t].VendorName;



                        for (int c = 0; c < vendor_obj[i].VendorTitle[t].categories.Count; c++)
                        {
                            string Lunch = vendor_obj[i].VendorTitle[t].categories[c].Lunch;
                            string Breakfast = vendor_obj[i].VendorTitle[t].categories[c].Breakfast;
                            string Dinner = vendor_obj[i].VendorTitle[t].categories[c].Dinner;
                            string CommonMenu = vendor_obj[i].VendorTitle[t].categories[c].CommonMenu;
                            string breakfastMenuItemsHTML = "";
                            string lunchMenuItemsHTML = "";
                            string dinnerMenuItemsHTML = "";
                            string commonMenuItemsHTML = "";

                            string categoryhtml = "";

                            string breakfastmenu = "";
                            string lunchMenu = "";
                            string dinnerMenu = "";
                            string commonMenu = "";
                            if (Breakfast == string.Empty && Lunch == string.Empty && Dinner == string.Empty && CommonMenu == string.Empty)
                            {
                                categoryhtml = "<span style='color:white'>" + "No Menu Available" + "</span>";
                            }
                            else
                            {
                                if (Breakfast != string.Empty)
                                {
                                    string[] breakfastArray = Breakfast.Split(',');

                                    foreach (var breakfastsplititem in breakfastArray)
                                    {
                                        string breakfastMenuItems = "<li>" + breakfastsplititem + "</li>";
                                        breakfastMenuItemsHTML = breakfastMenuItemsHTML + breakfastMenuItems;
                                    }
                                    breakfastmenu = "<div style='float:left;margin:10px'>" + "<span class='vendorCategoryName'>" + "BreakFast" + "</span>" + "<ul>" + breakfastMenuItemsHTML + "</ul>" + "</div>";
                                }
                                if (Lunch != string.Empty)
                                {
                                    string[] lunchArray = Lunch.Split(',');

                                    foreach (var lunchsplititem in lunchArray)
                                    {
                                        string lunchMenuItems = "<li>" + lunchsplititem + "</li>";
                                        lunchMenuItemsHTML = lunchMenuItemsHTML + lunchMenuItems;
                                    }
                                    lunchMenu = "<div style='float:left;margin:10px'>" + "<span class='vendorCategoryName'>" + "Lunch" + "</span>" + "<ul>" + lunchMenuItemsHTML + "</ul>" + "</div>";
                                }
                                if (Dinner != string.Empty)
                                {
                                    string[] DinnerArray = Dinner.Split(',');

                                    foreach (var dinnersplititem in DinnerArray)
                                    {
                                        string dinnerMenuItems = "<li>" + dinnersplititem + "</li>";
                                        dinnerMenuItemsHTML = dinnerMenuItemsHTML + dinnerMenuItems;
                                    }
                                    dinnerMenu = "<div style='float:left;margin:10px'>" + "<span class='vendorCategoryName'>" + "Dinner" + "</span>" + "<ul>" + dinnerMenuItemsHTML + "</ul>" + "</div>";
                                }
                                if (CommonMenu != string.Empty)
                                {
                                    string[] commonMenuArray = CommonMenu.Split(',');

                                    foreach (var commonsplititem in commonMenuArray)
                                    {
                                        string commonMenuItems = "<li>" + commonsplititem + "</li>";
                                        commonMenuItemsHTML = commonMenuItemsHTML + commonMenuItems;
                                    }
                                    commonMenu = "<div style='float:left;margin:10px'>" + "<span class='vendorCategoryName'>" + "Common Menu" + "</span>" + "<ul>" + commonMenuItemsHTML + "</ul>" + "</div>";
                                }
                                categoryhtml = breakfastmenu + lunchMenu + dinnerMenu + commonMenu;

                            }
                            vendorcategoryHTMLs = vendorcategoryHTMLs + categoryhtml;

                            //        DateTime timing = vendor_obj[i].VendorTitle[t].categories[c].timings.StartDate;
                            //        DateTime endtiming = vendor_obj[i].VendorTitle[t].categories[c].timings.EndDate;
                        }

                        string namehtml = "<div class='VendorMenu' >" + "<span class='VendorMenuName'>" + vendor_obj[i].VendorTitle[t].VendorName + "</span>" + "<div class='vendormenusub'>" + vendorcategoryHTMLs + "</div>" + "</div>";

                        vendornameHTMLs = vendornameHTMLs + namehtml;
                    }

                    locationdr["VendortitleListName"] = "<div>" + vendornameHTMLs + "</div>";

                    locationDT.Rows.Add(locationdr);
                    VerticleMenulocationdt.Rows.Add(VerticleMenulocationdr);
                }
                RepeaterVerticleMenu.DataSource = VerticleMenulocationdt;
                RepeaterVerticleMenu.DataBind();
                rptrLocationItems.DataSource = locationDT;
                rptrLocationItems.DataBind();
            }

        }
    }


    public class Vendor
    {

        public List<VendorTitle> VendorTitle;

        public string Location { get; set; }
    }
    public class VendorTitle
    {
        public string VendorName { get; set; }
        public List<Categories> categories { get; set; }
    }
    public class Categories
    {
        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public string CommonMenu { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

  

}
