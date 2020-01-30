using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;

namespace GIBS.PARentals.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class PARentalsSettings
    {
        ModuleController controller;
        int tabModuleId;

        public PARentalsSettings(int tabModuleId)
        {
            controller = new ModuleController();
            this.tabModuleId = tabModuleId;
        }

        protected T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = controller.GetTabModuleSettings(this.tabModuleId);

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        protected void WriteSetting(string settingName, string value)
        {
            controller.UpdateTabModuleSetting(this.tabModuleId, settingName, value);
        }

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        public string Template
        {
            get { return ReadSetting<string>("template", null); }
            set { WriteSetting("template", value); }
        }

        public string CustomField_RentalAmount
        {
            get { return ReadSetting<string>("customField_RentalAmount", null); }
            set { WriteSetting("customField_RentalAmount", value); }
        }

        public string CustomField_Bedrooms
        {
            get { return ReadSetting<string>("customField_Bedrooms", null); }
            set { WriteSetting("customField_Bedrooms", value); }
        }

        public string CustomField_Bathrooms
        {
            get { return ReadSetting<string>("customField_Bathrooms", null); }
            set { WriteSetting("customField_Bathrooms", value); }
        }

        public string CustomField_Address
        {
            get { return ReadSetting<string>("customField_Address", null); }
            set { WriteSetting("customField_Address", value); }
        }

        public string CustomField_City
        {
            get { return ReadSetting<string>("customField_City", null); }
            set { WriteSetting("customField_City", value); }
        }

        public string PA_ModuleID
        {
            get { return ReadSetting<string>("pA_ModuleID", null); }
            set { WriteSetting("pA_ModuleID", value); }
        }

        public string RoleAgent
        {
            get { return ReadSetting<string>("roleAgent", null); }
            set { WriteSetting("roleAgent", value); }
        }

        public string RoleOwner
        {
            get { return ReadSetting<string>("roleOwner", null); }
            set { WriteSetting("roleOwner", value); }
        }

        public string RoleTenant
        {
            get { return ReadSetting<string>("roleTenant", null); }
            set { WriteSetting("roleTenant", value); }
        }

        public string ReportServerURL
        {
            get { return ReadSetting<string>("reportServerURL", null); }
            set { WriteSetting("reportServerURL", value); }
        }

        public string ReportPath
        {
            get { return ReadSetting<string>("reportPath", null); }
            set { WriteSetting("reportPath", value); }
        }

        public string ReportCredentialsUserName
        {
            get { return ReadSetting<string>("reportCredentialsUserName", null); }
            set { WriteSetting("reportCredentialsUserName", value); }
        }

        public string ReportCredentialsPassword
        {
            get { return ReadSetting<string>("reportCredentialsPassword", null); }
            set { WriteSetting("reportCredentialsPassword", value); }
        }

        public string ReportCredentialsDomain
        {
            get { return ReadSetting<string>("reportCredentialsDomain", null); }
            set { WriteSetting("reportCredentialsDomain", value); }
        }

        public string StartDate
        {
            get { return ReadSetting<string>("startDate", null); }
            set { WriteSetting("startDate", value); }
        }


        public string EndDate
        {
            get { return ReadSetting<string>("endDate", null); }
            set { WriteSetting("endDate", value); }
        }

        #endregion
    }
}
