<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VendorsMenuItemsAppWeb.Pages.Default" %>

<%--<%@ Import Namespace="Microsoft.SharePoint.Client" %>--%>
<%--<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>--%>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <%--  <script type="text/javascript" src="../Scripts/jquery-1.7.1.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-ui-1.10.4.js"></script>
    <link rel="stylesheet" href="../Scripts/jquery-ui-1.10.4.min.css" />
    <%--<link rel="stylesheet" type="text/css" href="../Scripts/jqueryui1.10.3.min.css"/>--%>
    <script
        src="https://ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"
        type="text/javascript">
    </script>

    <title></title>
    <style type="text/css">
        body {
            /*background:url(../Scripts/bg-tile.png);*/
            /*background:url(../Scripts/mainImage.png);*/
            /*background:url(../Scripts/Images/MainBackImageFood.jpg);
             background-position: right top 35px;
             background-repeat: no-repeat;*/
            /*font-family: "proxima-nova", sans-serif;*/
            height: 100%;
            width: 100%;
            padding: 0;
            margin: 0;
            font-family: Georgia, Times, "Times New Roman", serif !important;
        }

        ul {
            list-style: none;
            padding: 0px;
        }

        .VendorMenu {
            float: left;
            margin: 10px;
        }

        .VendorMenuName {
            font-size: 25px !important;
            padding: 0px;
            font-weight: bold;
            text-align: center;
            width: 90%;
            /*background-color: rgba(3, 3, 3, 0.94);*/
            background-color: rgba(156, 100, 100, 0.94) !important;
            float: left;
            color: white;
            border-radius: 20px;
            background-color: rgb(154, 99, 97);
            filter: alpha(opacity=95);
        }

        .vendorCategoryName {
            font-weight: bold !important;
            font-size: 18px !important;
            /*color: #E1EED0;*/
            color: darkkhaki;
        }

        .VendorLocation {
            height: 100%;
            width: 100%;
            float: left;
        }

        .VendorLocationHeader {
            float: left;
            font-style: oblique;
            color: crimson;
            margin: 10px;
            color: brown;
            font-weight: bold !important;
        }

        .subMainLocationMenuDiv {
            float: left;
            min-height: 250px;
            width: 100%;
            /*display:none;*/
            cursor: context-menu;
            /*background:#f3f3f3;*/
            /*background:url(../Scripts/bg-tile.png);*/
            /*background:url(../Scripts/mainImage.png);*/
        }

        .mainLocationMenuDiv {
            float: left;
            /*width: 100%;*/
            min-width: 200px;
            /*background-color:rgba(179, 177, 177, 0.39);*/
            /*background:url(../Scripts/Images/MainFood.jpg)  no-repeat !important;*/
            cursor: pointer;
            border-radius: 20px;
            margin: 10px;
            background-color: rgba(189, 183, 107, 0.54);
            font-size: 1.125em !important;
            line-height: 1.6 !important;
        }

        .subMainLocationMenuDiv ul {
            color: #E1EED0;
            font-weight: bold;
        }

        .mainImage {
            /*background:url(../Scripts/mainImage.png)*/
            background: none !important;
            border: none !important;
        }

        .HrClass {
            background-color: rgba(116, 61, 61, 0.39);
            width: 95%;
        }

        .ms-core-pageTitle {
            /*color: rgba(150, 141, 34, 1);*/
            color: darkkhaki !important;
        }


        .ui-tabs.ui-tabs-vertical {
            padding: 0;
            /*width: 61em;*/
            width: 100%;
        }

            .ui-tabs.ui-tabs-vertical .ui-widget-header {
                border: none;
            }

            .ui-tabs.ui-tabs-vertical .ui-tabs-nav {
                float: left;
                /*width: 10em;*/
                width: 14%;
                /*background: #CCC;*/
                background: none;
                border-radius: 4px 0 0 4px;
                /*border-right: 1px solid gray;*/
            }

                .ui-tabs.ui-tabs-vertical .ui-tabs-nav li {
                    clear: left;
                    width: 100%;
                    margin: 0.2em 0;
                    border: 1px solid gray;
                    border-width: 1px 0 1px 1px;
                    border-radius: 4px 0 0 4px;
                    overflow: hidden;
                    position: relative;
                    right: -2px;
                    z-index: 2;
                    border-right: 1px solid gray;
                }

        .ui-state-active a, .ui-state-active a:link, .ui-state-active a:visited {
            color: #C99A53 !important;
        }

        .ui-state-default a, .ui-state-default a:link, .ui-state-default a:visited {
            /*color: rgba(156, 100, 100, 1);*/
            color:#9c6464;
        }

        .ui-tabs.ui-tabs-vertical .ui-tabs-nav li a {
            display: block;
            /*width: 100%;*/
            font-size: 20px;
            padding: 0.6em 1em;
            width: 145px;
            word-break: break-all;
            white-space: normal;
            border-right: none !important;
            outline: none;
            /*color: #AC4040;*/
        }

            .ui-tabs.ui-tabs-vertical .ui-tabs-nav li a:hover {
                cursor: pointer;
                border-right: none !important;
                color: lightcyan  !important;
            }

        .ui-tabs.ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active {
            margin-bottom: 0.2em;
            padding-bottom: 0;
            border-right: none !important;
            /*border-right: 1px solid white;*/
            border: 5px solid burlywood;
        }

        .ui-tabs.ui-tabs-vertical .ui-tabs-nav li:last-child {
            margin-bottom: 10px;
        }

        .ui-tabs.ui-tabs-vertical .ui-tabs-panel {
            float: left;
            width: 78%;
            /*float: right;
width: 80%;*/
            /*border:1px solid #ddd;*/
            border-radius: 0;
            position: relative;
            left: -1px;
            /*height: 400px;
            overflow-y: scroll;*/
        }

        .ui-widget {
            font-family: inherit;
        }

        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
            background: none !important;
        }

        #full-screen-background-image {
            z-index: -999;
            min-height: 100%;
            min-width: 1024px;
            width: 100%;
            height: auto;
            /*height: 600px;*/
            position: fixed;
            top: 0;
            left: 0;
        }

        #wrapper {
            position: relative;
            /*width: 800px;
  min-height: 400px;
  margin: 100px auto;*/
            color: #333;
        }
        /*.ui-state-default a, .ui-state-default a:link, .ui-state-default a:visited {
            color: #C41C1C !important;
        }*/
        #suiteBarLeft {
            /*background-color: rgba(82, 3, 3, 0.76) !important;*/
            background-color: #432918 !important;
        }

        .vendormenusub {
            background-color: rgba(60, 18, 18, 0.49) !important;
            min-height: 10px;
            float: left;
            border-radius: 20px;
            margin-top: 10px;
            min-width: 90%;
            background-color: #3c1212;
            filter: alpha(opacity=70);
        }

        .NoItemsFound {
            background-color: rgba(100, 38, 38, 0.62) !important;
            font-size: 50px;
          
            color: white;
            background-color: #3c1212;
            filter: alpha(opacity=70);
        }
    </style>
</head>
<body style="display: none">
    <img alt="full screen background image" src="../Scripts/Images/MainBackImageFood1660.png" id="full-screen-background-image" />

    <form id="form1" runat="server">
         <div class="ms-status-yellow" style="padding:10px 10px 10px 10px" id="WarningStatusPeriod" runat="server">
         <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label></div>
        <div id="chrome_ctrl_placeholder"></div>
        <div id="wrapper">
            <div class="VendorLocation">
                <input type="hidden" id="AppwebURLEdit" runat="server" />
                <input type="hidden" id="AppwebURLNew" runat="server" />
                <input type="hidden" id="ListSettingsAppWebURL" runat="server" />
                <%--  <input type="hidden" id="NoItemsFound" runat="server" />--%>
                <input type="hidden" id="HostURLmain" runat="server" />
                <%-- <a href="javascript:HostURLmain()">Home</a><span> >> Daily Menu</span>
             <br />--%>
                <br />
                <div style="width: 100%; height: 60px;">
                    <a href="javascript:AppWebURLcreate()" id="CreateItem" style="color: darkkhaki" runat="server">Click to Create item
                    </a>
                    <br />
                     <a href="javascript:AppWebURL()" id="EditItem" style="color: darkkhaki" runat="server">Click to Edit item
                    </a>
                   
                    <br />
                    <a href="javascript:AppWebURLListSettings()" id="ListSettings" style="color: darkkhaki" runat="server">Click to List Settings page
                    </a>
                    <br />
                </div>
                <p id="NoItems" runat="server" class="NoItemsFound">Vendors  not available, Please click on "Click to Create item" Link to create vendors </p>
                <%--  <asp:GridView ID="GridView1" runat="server"></asp:GridView>--%>
                <div id="faq-row-container" class="mainImage" style="height:450px !important;overflow-y:scroll">
                    <ul>
                        <asp:Repeater ID="RepeaterVerticleMenu" runat="server">
                            <ItemTemplate>
                                <li><a href="#<%# DataBinder.Eval(Container.DataItem, "Location_Updated") %>"><%# DataBinder.Eval(Container.DataItem, "Location") %></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <asp:Repeater ID="rptrLocationItems" runat="server">

                        <ItemTemplate>
                            <%-- <div class="mainLocationMenuDiv" id="<%# DataBinder.Eval(Container.DataItem, "Location") %>">
                        <h2 class="VendorLocationHeader">
                            <%# DataBinder.Eval(Container.DataItem, "Location") %>
                        </h2>
                      
                        <hr class="HrClass"  />
                        <div class="subMainLocationMenuDiv" >


                            <%# DataBinder.Eval(Container.DataItem, "VendortitleListName") %>
                        </div>
                    </div>--%>
                            <div class="mainLocationMenuDiv" id="<%# DataBinder.Eval(Container.DataItem, "Location_Updated") %>">
                                <%-- <h2 class="VendorLocationHeader">
                            <%# DataBinder.Eval(Container.DataItem, "Location") %>
                        </h2>
                                --%>

                                <div class="subMainLocationMenuDiv">


                                    <%# DataBinder.Eval(Container.DataItem, "VendortitleListName") %>
                                </div>
                            </div>


                        </ItemTemplate>
                    </asp:Repeater>

                </div>
            </div>
        </div>
    </form>
    
</body>
</html>

<script type="text/javascript">
    "use strict";

    var hostweburl;

    //load the SharePoint resources
    $(document).ready(function () {
        document.title = 'Daily Menu App';
        //Get the URI decoded URL.
        hostweburl =
            decodeURIComponent(
                getQueryStringParameter("SPHostUrl")
        );

        // The SharePoint js files URL are in the form:
        // web_url/_layouts/15/resource
        var scriptbase = hostweburl + "/_layouts/15/";

        // Load the js file and continue to the 
        //   success handler
        $.getScript(scriptbase + "SP.UI.Controls.js", renderChrome)
    });

    // Callback for the onCssLoaded event defined
    //  in the options object of the chrome control
    function chromeLoaded() {
        // When the page has loaded the required
        //  resources for the chrome control,
        //  display the page body.
        $("body").show();
    }
    
    //Function to prepare the options and render the control
    function renderChrome() {
        // The Help, Account and Contact pages receive the 
        //   same query string parameters as the main page
        var options = {
            "appIconUrl": "/Scripts/Images/MenuImageMain96.png",
            "appTitle": "Daily Menu App",
            "appHelpPageUrl": "Help.html?"
                + document.URL.split("?")[1],
            // The onCssLoaded event allows you to 
            //  specify a callback to execute when the
            //  chrome resources have been loaded.
            "onCssLoaded": "chromeLoaded()",
            "settingsLinks": [
                 {
                     "linkUrl": hostweburl + "/_layouts/15/start.aspx#/_layouts/15/viewlsts.aspx",
                     "displayName": "Site Contents"
                 }
                 //,
                //{
                //    "linkUrl": "Account.html?"
                //        + document.URL.split("?")[1],
                //    "displayName": "Account settings"
                //},
                //{
                //    "linkUrl": "Contact.html?"
                //        + document.URL.split("?")[1],
                //    "displayName": "Contact us"
                //}
            ]
        };

        var nav = new SP.UI.Controls.Navigation(
                                "chrome_ctrl_placeholder",
                                options
                            );
        nav.setVisible(true);
    }

    // Function to retrieve a query string value.
    // For production purposes you may want to use
    //  a library to handle the query string.
    function getQueryStringParameter(paramToRetrieve) {
        var params =
            document.URL.split("?")[1].split("&");
        var strParams = "";
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == paramToRetrieve)
                return singleParam[1];
        }
    }
</script>


<script type="text/javascript">

    function AppWebURL() {

        window.open($('#<%= AppwebURLEdit.ClientID %>').val(), '_blank');
    }
    function HostURLmain() {

        window.open($('#<%= HostURLmain.ClientID %>').val());
    }
    function AppWebURLcreate() {

        window.open($('#<%= AppwebURLNew.ClientID %>').val(), '_blank');
    }
    function AppWebURLListSettings() {

        window.open($('#<%= ListSettingsAppWebURL.ClientID %>').val(), '_blank');
    }
    function checkLocationsExists() {
        alert("location");
    }
    $(document).ready(function () {
       
        $('#faq-row-container').tabs().addClass('ui-tabs-vertical ui-helper-clearfix');
        //$(".VendorLocation").on("click", ".mainLocationMenuDiv", function (ev) {
        //     var targetDiv = ev.target;

        //   $(targetDiv).find(".subMainLocationMenuDiv").toggle();

        //});



        // var faqTab = $('.VendorLocationHeader'),
        //faqTabContainer = $('.faq-row-container');

        // if (faqTab.length) {

        //     faqTab.off('click').on('click', function () {
        //         var faqRow = $(this).parent(),
        //             faqContent = $(this).parent().find('.subMainLocationMenuDiv');

        //         faqTabContainer.find('.subMainLocationMenuDiv').not(faqContent).stop().slideUp('fast');
        //         faqTabContainer.find('.mainLocationMenuDiv').not(faqRow).removeClass('open');

        //         faqContent.stop().slideToggle('fast', function () {
        //             faqRow.toggleClass('open', faqContent.is(':visible'));
        //         });
        //     });

        // }
        //$("#accordion").accordion();
    });

</script>




