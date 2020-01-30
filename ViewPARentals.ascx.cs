using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.PARentals.Components;
using DotNetNuke.Common;

namespace GIBS.Modules.PARentals
{
    public partial class ViewPARentals : PortalModuleBase, IActionable
    {

        static int _CustomFieldID_RentalAmount = 0;
        static int _CustomFieldID_Bedrooms = 0;
        static int _CustomFieldID_Bathrooms = 0;
        static int _CustomFieldID_Address = 0;
        static int _CustomFieldID_City = 0;
        static int _PAModuleID = 0;
        static int _PATabID = 0;

        static string _StartDate = string.Empty;
        static string _EndDate = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetSettings();
                    if (_StartDate.Length > 0)
                    {
                        GetRentals();
                    }
                    else
                    {
                        lblDebug.Text = "CONFIGURE START AND END DATES";
                    }
                    


                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void GetSettings()
        {

            try
            {
                PARentalsSettings settingsData = new PARentalsSettings(this.TabModuleId);

                if (settingsData.StartDate != null)
                {
                    _StartDate = settingsData.StartDate.ToString();
                }

                if (settingsData.EndDate != null)
                {
                    _EndDate = settingsData.EndDate.ToString();
                }




                if (settingsData.CustomField_Address != null)
                {
                    _CustomFieldID_Address = Int32.Parse(settingsData.CustomField_Address.ToString());
                }

                if (settingsData.CustomField_Bedrooms != null)
                {
                    _CustomFieldID_Bedrooms = Int32.Parse(settingsData.CustomField_Bedrooms.ToString());
                }

                if (settingsData.CustomField_Bathrooms != null)
                {
                    _CustomFieldID_Bathrooms = Int32.Parse(settingsData.CustomField_Bathrooms.ToString());
                }

                if (settingsData.CustomField_City != null)
                {
                    _CustomFieldID_City = Int32.Parse(settingsData.CustomField_City.ToString());
                }

                if (settingsData.CustomField_RentalAmount != null)
                {
                    _CustomFieldID_RentalAmount = Int32.Parse(settingsData.CustomField_RentalAmount.ToString());
                }
                //

                if (settingsData.PA_ModuleID != null)
                {
                    //   _PAModuleID = Int32.Parse(settingsData.PA_ModuleID.ToString());

                    string s = settingsData.PA_ModuleID.ToString();
                    string[] parts = s.Split('-');
                    string i1 = parts[0];
                    string i2 = parts[1];

                    _PAModuleID = Convert.ToInt32(i2);
                    _PATabID = Convert.ToInt32(i1);


                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void GetRentals()
        {

            try
            {
                //    lblDebug.Text = "_PAModuleID: " + _PAModuleID + "  _CustomFieldID_Address: " + _CustomFieldID_Address + "  _CustomFieldID_City: " + _CustomFieldID_City + "  _CustomFieldID_Bedrooms: " + _CustomFieldID_Bedrooms + "  _CustomFieldID_RentalAmount: " + _CustomFieldID_RentalAmount;
                PARentalsController objController = new PARentalsController();
                List<PARentalsInfo> objList = objController.Rentals_GetActiveList(_PAModuleID, _CustomFieldID_Address, _CustomFieldID_City, _CustomFieldID_Bedrooms, _CustomFieldID_Bathrooms, _CustomFieldID_RentalAmount);

                Repeater1.DataSource = objList;
                Repeater1.DataBind();

                GridView1.DataSource = objList;
                GridView1.DataBind();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public String GetSetting(String settingKey, int MyModuleID)
        {
            String settingValue = "";
            DotNetNuke.Entities.Modules.ModuleController moduleCont = new DotNetNuke.Entities.Modules.ModuleController();

            if (moduleCont.GetModuleSettings(MyModuleID).ContainsKey(settingKey))
                settingValue = moduleCont.GetModuleSettings(MyModuleID)[settingKey].ToString();

            return settingValue;
        }

        protected void Repeater1_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {


            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    // EDIT PROPERTY
                    HyperLink ep = (HyperLink)e.Item.FindControl("HyperLinkEditProp");
                    
                    // VIEW PROPERTY
                    HyperLink hp = (HyperLink)e.Item.FindControl("HyperLink1");

                    string _PropertyID = DataBinder.Eval(e.Item.DataItem, "PropertyID").ToString();

                    string SEOPropertyID = GetSetting("SEOPropertyID", _PAModuleID);
                    string SEOAgentType = GetSetting("SEOAgentType", _PAModuleID);


                    string newURL = Globals.NavigateURL(_PATabID, "", SEOAgentType + "=View", SEOPropertyID + "=" + _PropertyID.ToString());

                    string editURL = Globals.NavigateURL(_PATabID, "", SEOAgentType + "=EditProperty", SEOPropertyID + "=" + _PropertyID.ToString());
                    ep.NavigateUrl = editURL.ToString();

                    hp.Text = DataBinder.Eval(e.Item.DataItem, "PropertyAddress").ToString() + ", " + DataBinder.Eval(e.Item.DataItem, "PropertyCity").ToString();
                    hp.Target = "_blank";
                    hp.NavigateUrl = newURL.ToString();


                    PlaceHolder vSchedule = (PlaceHolder)e.Item.FindControl("PlaceHolder1");

                    //    DateTime StartDate = new DateTime(2015, 6,6);
                    DateTime StartDate = new DateTime();
                    StartDate = DateTime.Parse(_StartDate.ToString());

                    DateTime EndDate = new DateTime();
                    EndDate = DateTime.Parse(_EndDate.ToString());

                    lblYear.Text = EndDate.Year.ToString();

                    string vLink1 = "";

                    foreach (DateTime day in EachDay(StartDate, EndDate))
                    {
                        Literal OpenTag = new Literal();
                        Literal CloseTag = new Literal();

                        string _status = CheckRentalStatus(Int32.Parse(_PropertyID.ToString()), day.Date);
                        vLink1 = EditUrl(this.TabId, "EditBooking", false, "mid", this.ModuleId.ToString(), "Status", _status.ToString(),
                           "AddDate", ((DateTime)day.Date).ToString(@"yyyy-MM-dd"),
                           "PropertyID", _PropertyID.ToString()).ToString();

                        OpenTag.Text = "<div style=\"float:left;text-align:center;padding:6px;font-size:9px;\"><a href=\"" + vLink1.ToString() + "\">" + day.DayOfWeek.ToString().Substring(0, 3).ToString() + "<br />" + day.Month.ToString() + "/" + day.Day.ToString() + "<br />";
                        CloseTag.Text = "</a></div>";


                        Image img = new Image(); // instantiating the control
                        img.ImageUrl = GetImageURL(_status.ToLower());
                        img.ToolTip = day.ToLongDateString();

                        vSchedule.Controls.Add(OpenTag);
                        vSchedule.Controls.Add(img);
                        vSchedule.Controls.Add(CloseTag);
                    }


                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        //"d" then 'Reservation inserted - need lease        
        //"p" then 'lease generated - sent to tenant
        //"s" then 'mailed to owner
        //"b" then 'returned to tenant 

        //"x" then 'its Rented by Another Agency
        //"o" then 'it's OWNER OCCUPIED

        //"a" then 'it's Available
        //"u"  then 'it's Not Yet Made Available

        public string GetImageURL(string _Status)
        {

            try
            {
                string result = "";

                switch (_Status.ToLower())
                {


                    case "d":
                        result = "~/DesktopModules/GIBS/PARentals/Images/Lease-Need.png";
                        break;
                    case "p":
                        result = "~/DesktopModules/GIBS/PARentals/Images/Lease-Tenant.png";
                        break;

                    case "s":
                        result = "~/DesktopModules/GIBS/PARentals/Images/Lease-Owner.png";
                        break;
                    case "b":
                        result = "~/DesktopModules/GIBS/PARentals/Images/icon_GreenCheck.png";
                        break;


                    case "x":
                        result = "~/DesktopModules/GIBS/PARentals/Images/icon_RedX.png";
                        break;
                    case "o":
                        result = "~/DesktopModules/GIBS/PARentals/Images/bredo.gif";
                        break;

                    case "u":
                        result = "~/DesktopModules/GIBS/PARentals/Images/icon_Gray.png";
                        break;
                    case "a":
                        result = "~/DesktopModules/GIBS/PARentals/Images/bgreen.gif";
                        break;

                }


                return result;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "ERROR";
            }

        }



        public string CheckRentalStatus(int _PropertyID, DateTime _BookingDate)
        {

            try
            {
                string result = "";

                //load the item
                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = controller.Rentals_Booking_CheckStatus(_PropertyID, _BookingDate);

                if (item != null)
                {
                    result = item.Status.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "ERROR";
            }

        }


        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(7))
                yield return day;
        }


        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                //    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                //     true, false);

                return actions;
            }
        }

        #endregion


        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 


        //protected void lstContent_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        //{
        //    Label content = (Label)e.Item.FindControl("lblContent");
        //    string contentValue = string.Empty;

        //    PARentalsSettings settingsData = new PARentalsSettings(this.TabModuleId);

        //    if (settingsData.Template != null)
        //    {
        //        //apply the content to the template
        //        ArrayList propInfos = CBO.GetPropertyInfo(typeof(PARentalsInfo));
        //        contentValue = settingsData.Template;

        //        if (contentValue.Length != 0)
        //        {
        //            foreach (PropertyInfo propInfo in propInfos)
        //            {
        //                object propertyValue = DataBinder.Eval(e.Item.DataItem, propInfo.Name);
        //                if (propertyValue != null)
        //                {
        //                    contentValue = contentValue.Replace("[" + propInfo.Name.ToUpper() + "]",
        //                            Server.HtmlDecode(propertyValue.ToString()));
        //                }
        //            }
        //        }
        //        else
        //            //blank template so just set the content to the value
        //            contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Content").ToString());
        //    }
        //    else
        //    {
        //        //no template so just set the content to the value
        //        contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Content").ToString());
        //    }

        //    content.Text = contentValue;
        //}

    }
}