using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.PARentals.Components;
using DotNetNuke.Entities.Tabs;
using System.Collections;
using System.Collections.Generic;
using DotNetNuke.Security;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using Ventrian.PropertyAgent;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace GIBS.Modules.PARentals
{
    public partial class Settings : ModuleSettingsBase
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        /// 

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
            // JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            //<link rel="stylesheet" href="//code.jquery.com/ui/1.11.0/themes/smoothness/jquery-ui.css">
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Style", ("https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css"));

        }



        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetRoles();
                    BindModules();

                    PARentalsSettings settingsData = new PARentalsSettings(this.TabModuleId);

                    if (settingsData.PA_ModuleID != null)
                    {
                        drpModuleID.SelectedValue = settingsData.PA_ModuleID.ToString();
                        GetCustomFields();
                    }

                    if (settingsData.CustomField_Address != null)
                    {
                        ddlPropertyAddress.SelectedValue = settingsData.CustomField_Address.ToString();
                    }

                    if (settingsData.CustomField_City != null)
                    {
                        ddlPropertyCity.SelectedValue = settingsData.CustomField_City.ToString();
                    }

                    if (settingsData.CustomField_RentalAmount != null)
                    {
                        ddlRentalAmount.SelectedValue = settingsData.CustomField_RentalAmount.ToString();
                    }
                    if (settingsData.CustomField_Bedrooms != null)
                    {
                        ddlBedrooms.SelectedValue = settingsData.CustomField_Bedrooms.ToString();
                    }

                    if (settingsData.CustomField_Bathrooms != null)
                    {
                        ddlBathrooms.SelectedValue = settingsData.CustomField_Bathrooms.ToString();
                    }




                    if (settingsData.RoleAgent != null)
                    {
                        ddlAgentRole.SelectedValue = settingsData.RoleAgent.ToString();
                    }
                    if (settingsData.RoleOwner != null)
                    {
                        ddlRentalOwnerRole.SelectedValue = settingsData.RoleOwner.ToString();
                    }
                    if (settingsData.RoleTenant != null)
                    {
                        ddlAddUserRole.SelectedValue = settingsData.RoleTenant.ToString();
                    }


                    if (settingsData.ReportServerURL != null)
                    {
                        txturlReportServer.Text = settingsData.ReportServerURL.ToString();
                    }

                    if (settingsData.ReportPath != null)
                    {
                        txtReportPath.Text = settingsData.ReportPath.ToString();
                    }

                    if (settingsData.ReportCredentialsUserName != null)
                    {
                        txtRSCredentialsUserName.Text = settingsData.ReportCredentialsUserName.ToString();
                    }

                    if (settingsData.ReportCredentialsPassword != null)
                    {

                        txtRSCredentialsPassword.Text = settingsData.ReportCredentialsPassword.ToString();
                    }
                    if (settingsData.ReportCredentialsDomain != null)
                    {
                        txtRSCredentialsDomain.Text = settingsData.ReportCredentialsDomain.ToString();
                    }


                    if (settingsData.StartDate != null)
                    {
                        txtStartDate.Text = settingsData.StartDate.ToString();
                    }

                    if (settingsData.EndDate != null)
                    {
                        txtEndDate.Text = settingsData.EndDate.ToString();
                    }


                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void GetRoles()
        {

            try
            {

                ArrayList myRoles = new ArrayList();

                DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();

                myRoles = rc.GetPortalRoles(this.PortalId);



                ddlAgentRole.DataSource = myRoles;
                ddlAgentRole.DataBind();

                // ADD FIRST (NULL) ITEM
                ListItem item = new ListItem();
                item.Text = "-- Select Role to Assign --";
                item.Value = "";
                ddlAgentRole.Items.Insert(0, item);
                // REMOVE DEFAULT ROLES
                ddlAgentRole.Items.Remove("Administrators");
                ddlAgentRole.Items.Remove("Registered Users");
                ddlAgentRole.Items.Remove("Subscribers");


                ddlRentalOwnerRole.DataSource = myRoles;
                ddlRentalOwnerRole.DataBind();

                ddlRentalOwnerRole.Items.Insert(0, item);
                // REMOVE DEFAULT ROLES
                ddlRentalOwnerRole.Items.Remove("Administrators");
                ddlRentalOwnerRole.Items.Remove("Registered Users");
                ddlRentalOwnerRole.Items.Remove("Subscribers");

                // ADD TENANT TO ROLE
                ddlAddUserRole.DataSource = myRoles;
                ddlAddUserRole.DataBind();

                ddlAddUserRole.Items.Insert(0, item);
                // REMOVE DEFAULT ROLES
                ddlAddUserRole.Items.Remove("Administrators");
                ddlAddUserRole.Items.Remove("Registered Users");
                ddlAddUserRole.Items.Remove("Subscribers");

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        private void BindModules()
        {
            DotNetNuke.Entities.Modules.ModuleController mc = new ModuleController();
            ArrayList existMods = mc.GetModulesByDefinition(this.PortalId, "Property Agent");

            foreach (DotNetNuke.Entities.Modules.ModuleInfo mi in existMods)
            {
                if (!mi.IsDeleted)
                {
                    DotNetNuke.Entities.Tabs.TabController tabController = new DotNetNuke.Entities.Tabs.TabController();
                    DotNetNuke.Entities.Tabs.TabInfo tabInfo = tabController.GetTab(mi.TabID,this.PortalId);
                    string strPath = tabInfo.TabName.ToString();
                    ListItem objListItem = new ListItem();
                    objListItem.Value = mi.TabID.ToString() + "-" + mi.ModuleID.ToString();     // TabID & ModuleID   
                    objListItem.Text = strPath + " -> " + mi.ModuleTitle.ToString();

                    drpModuleID.Items.Add(objListItem);

                }
            }

            drpModuleID.Items.Insert(0, new ListItem(Localization.GetString("SelectModule", this.LocalResourceFile), "-1"));
        }


        //private void BindModules()
        //{
        //    DesktopModuleController objDesktopModuleController = new DesktopModuleController();
        //    DesktopModuleInfo objDesktopModuleInfo = objDesktopModuleController.GetDesktopModuleByModuleName("PropertyAgent");


        //    if ((objDesktopModuleInfo != null))
        //    {
        //        TabController objTabController = new TabController();
        //        ArrayList objTabs = objTabController.GetTabs(PortalId);
        //        // ArrayList objTabs = objTabController.GetTabsByPortal(PortalId);
        //        foreach (DotNetNuke.Entities.Tabs.TabInfo objTab in objTabs)
        //        {
        //            if ((objTab != null))
        //            {
        //                if ((objTab.IsDeleted == false))
        //                {
        //                    ModuleController objModules = new ModuleController();
        //                    foreach (KeyValuePair<int, ModuleInfo> pair in objModules.GetTabModules(objTab.TabID))
        //                    {
        //                        ModuleInfo objModule = pair.Value;
        //                        if ((objModule.IsDeleted == false))
        //                        {
        //                            if ((objModule.DesktopModuleID == objDesktopModuleInfo.DesktopModuleID))
        //                            {
        //                                if (PortalSecurity.IsInRoles(objModule.AuthorizedEditRoles) == true & objModule.IsDeleted == false)
        //                                {
        //                                    string strPath = objTab.TabName;
        //                                    TabInfo objTabSelected = objTab;
        //                                    while (objTabSelected.ParentId != Null.NullInteger)
        //                                    {
        //                                        objTabSelected = objTabController.GetTab(objTabSelected.ParentId, objTab.PortalID, false);
        //                                        if ((objTabSelected == null))
        //                                        {
        //                                            break; // TODO: might not be correct. Was : Exit While
        //                                        }
        //                                        strPath = objTabSelected.TabName + " -> " + strPath;
        //                                    }

        //                                    ListItem objListItem = new ListItem();

        //                                    objListItem.Value = objModule.TabID.ToString() + "-" + objModule.ModuleID.ToString();     // TabID & ModuleID
        //                                    //  objListItem.Value = objModule.ModuleID.ToString();
        //                                    objListItem.Text = strPath + " -> " + objModule.ModuleTitle;

        //                                    drpModuleID.Items.Add(objListItem);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        drpModuleID.Items.Insert(0, new ListItem(Localization.GetString("SelectModule", this.LocalResourceFile), "-1"));

        //    }
        //}


        public void GetCustomFields()
        {

            try
            {

                string TabModuleCombined = drpModuleID.SelectedValue.ToString();
                // 65-496
                string s = TabModuleCombined;
                string[] parts = s.Split('-');
                //      string i1 = parts[0];
                string i2 = parts[1];

                int myModID = Convert.ToInt32(i2);
                //         int myTabID = Convert.ToInt32(i1);

                CustomFieldController objCustomFieldController = new CustomFieldController();
                List<CustomFieldInfo> objCustomFields = objCustomFieldController.List(myModID, true);

                ddlPropertyAddress.DataSource = objCustomFields;
                ddlPropertyAddress.DataBind();
                ddlPropertyAddress.Items.Insert(0, new ListItem("--Select--", ""));

                ddlPropertyCity.DataSource = objCustomFields;
                ddlPropertyCity.DataBind();
                ddlPropertyCity.Items.Insert(0, new ListItem("--Select--", ""));

                ddlRentalAmount.DataSource = objCustomFields;
                ddlRentalAmount.DataBind();
                ddlRentalAmount.Items.Insert(0, new ListItem("--Select--", ""));

                ddlBedrooms.DataSource = objCustomFields;
                ddlBedrooms.DataBind();
                ddlBedrooms.Items.Insert(0, new ListItem("--Select--", ""));

                //
                ddlBathrooms.DataSource = objCustomFields;
                ddlBathrooms.DataBind();
                ddlBathrooms.Items.Insert(0, new ListItem("--Select--", ""));

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                PARentalsSettings settingsData = new PARentalsSettings(this.TabModuleId);

                settingsData.PA_ModuleID = drpModuleID.SelectedValue.ToString();
                settingsData.CustomField_Address = ddlPropertyAddress.SelectedValue.ToString();
                settingsData.CustomField_City = ddlPropertyCity.SelectedValue.ToString();
                settingsData.CustomField_RentalAmount = ddlRentalAmount.SelectedValue.ToString();
                settingsData.CustomField_Bedrooms = ddlBedrooms.SelectedValue.ToString();
                settingsData.CustomField_Bathrooms = ddlBathrooms.SelectedValue.ToString();
                settingsData.RoleAgent = ddlAgentRole.SelectedValue.ToString();
                settingsData.RoleOwner = ddlRentalOwnerRole.SelectedValue.ToString();
                settingsData.RoleTenant = ddlAddUserRole.SelectedValue.ToString();
                settingsData.ReportCredentialsDomain = txtRSCredentialsDomain.Text.ToString();
                settingsData.ReportCredentialsPassword = txtRSCredentialsPassword.Text.ToString();
                settingsData.ReportCredentialsUserName = txtRSCredentialsUserName.Text.ToString();
                settingsData.ReportPath = txtReportPath.Text.ToString();
                settingsData.ReportServerURL = txturlReportServer.Text.ToString();

                settingsData.StartDate = txtStartDate.Text.ToString();
                settingsData.EndDate = txtEndDate.Text.ToString();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}