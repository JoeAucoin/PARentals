using System;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.PARentals.Components;
using Ventrian.PropertyAgent;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Lists;
using DotNetNuke.Entities.Users;

using DotNetNuke.Services.Log.EventLog;
using DotNetNuke.Framework.JavaScriptLibraries;
using System.Collections;
using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.Common;

using GIBS.Modules.PARentals.Components;
using System.Reflection;




namespace GIBS.Modules.PARentals
{
    public partial class Lease : PortalModuleBase
    {

        public int _reservationID = -1;

        static string _ReportCredentialsDomain = "";
        static string _ReportCredentialsPassword = "";
        static string _ReportCredentialsUserName = "";
        static string _ReportPath = "";
        static string _ReportServerURL = "";
        
        protected void Page_Load(object sender, EventArgs e)
        {
           
            
            if (Page.IsPostBack == false)
            {
                GetSettings();
                ShowReport();
            }

        }


        public void GetSettings()
        {

            try
            {
                PARentalsSettings settingsData = new PARentalsSettings(this.TabModuleId);



                if (settingsData.ReportCredentialsDomain != null)
                {
                    _ReportCredentialsDomain = settingsData.ReportCredentialsDomain.ToString();
                }

                if (settingsData.ReportCredentialsPassword != null)
                {
                    _ReportCredentialsPassword = settingsData.ReportCredentialsPassword.ToString();
                }

                if (settingsData.ReportCredentialsUserName != null)
                {
                    _ReportCredentialsUserName = settingsData.ReportCredentialsUserName.ToString();
                }

                if (settingsData.ReportPath != null)
                {
                    _ReportPath = settingsData.ReportPath.ToString();
                }

                if (settingsData.ReportServerURL != null)
                {
                    _ReportServerURL = settingsData.ReportServerURL.ToString();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void DisableUnwantedExportFormats(ServerReport rvServer)
        {
            foreach (RenderingExtension extension in rvServer.ListRenderingExtensions())
            {
                if (extension.Name == "XML" || extension.Name == "TIFF" || extension.Name == "MHTML" || extension.Name == "EXCEL" || extension.Name == "CSV")
                {
                    ReflectivelySetVisibilityFalse(extension);
                }
            }
        }


        private void ReflectivelySetVisibilityFalse(RenderingExtension extension)
        {
            FieldInfo info = extension.GetType().GetField("m_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);


            if (info != null)
            {
                info.SetValue(extension, false);
            }
        }


        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            DisableUnwantedExportFormats(ReportViewer1.ServerReport);
        }

        private ArrayList ReportDefaultParam()
        {

            if (Request.QueryString["ResID"] != null)
            {
                _reservationID = Convert.ToInt32(Request.QueryString["ResID"].ToString());
            }

            
            //myContext = (ContextMgr)Session["ContextObj"];
            //string _connectString = myContext.Club_DB_Conn;

            //System.Data.SqlClient.SqlConnectionStringBuilder connBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            //connBuilder.ConnectionString = _connectString.ToString();
            //string _databaseName = connBuilder.InitialCatalog;     // Db name.
            //string _databaseServer = connBuilder.DataSource;
            // DataBaseServer  DataBaseUser DataBasePwd DataBaseName
            ArrayList arrLstDefaultParam = new ArrayList();
            arrLstDefaultParam.Add(CreateReportParameter("ReservationID", _reservationID.ToString()));// get this from mycontext
       //     arrLstDefaultParam.Add(CreateReportParameter("DatabaseServer", "Orleans")); //get this from web.config
            // All report parameters will be passed in the QueryString. 
            // The name - key will be prefaced by p and the name will match the p
            // EXample : businessnet.pureaxess.com/Admin/reports/SSRSReportViewer.aspx?reportname=" + reportName + '&p_locationid=' + plocationid;
            //  EFT_Batch_Breakdown
            // We will lop over the query string and pull out the p-keys/values
            string sKey;
            string sValue;
            foreach (String key in Request.QueryString.AllKeys)
            {

                sKey = key;

                //  Response.Write("Key: " + key + " Value: " + Request.QueryString[key] + " -||| " + sKey.Substring(0, 2));

                sValue = Request.QueryString[key];
                if (sKey.Substring(0, 2).ToString() == "p_")
                {
                    sKey = sKey.Substring(2, sKey.Length - 2);
                    arrLstDefaultParam.Add(CreateReportParameter(sKey, sValue));    // get this from the query string

                }

            }
            return arrLstDefaultParam;
        }
        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }
        private void ShowReport()
        {
            try
            {
                string urlReportServer = _ReportServerURL.ToString();   // "http://orleans:8081/Reportserver"; //get from web config
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
                ReportViewer1.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
                ReportViewer1.ServerReport.ReportPath = _ReportPath.ToString(); // "/PetersonRealty/Report1"; //get from the query string                

                //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
                ArrayList reportParam = new ArrayList();
                reportParam = ReportDefaultParam();
                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                

                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "Bouyea9213", "Orleans");
                ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials(_ReportCredentialsUserName.ToString(), _ReportCredentialsPassword.ToString(), _ReportCredentialsDomain.ToString());
                //pass parmeters to report
                ReportViewer1.ServerReport.SetParameters(param); //Set Report Parameters
            //    ReportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            //Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);
            // NEED PropertyID
            Response.Redirect(Globals.NavigateURL());
            //    Response.Redirect(EditUrl("Lease"));


        }

    }
}