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
using System.Web;




namespace GIBS.Modules.PARentals
{
    public partial class EditPARentals : PortalModuleBase
    {

        static int _propertyID = Null.NullInteger;
        static int _bookingID = Null.NullInteger;

        static string _status = "";
        static string _tenantUserRole;

        static string _bookingDate = "";

        static int _CustomFieldID_RentalAmount = 0;
        static int _CustomFieldID_Bedrooms = 0;
        static int _CustomFieldID_Bathrooms = 0;
        static int _CustomFieldID_Address = 0;
        static int _CustomFieldID_City = 0;
        static int _PAModuleID = 0;
        static int _PATabID = 0;


        //protected void Page_Init(object sender, EventArgs e)
        //{

        ////    ReadQueryString();

        //    LoadControlType();

        
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
       //     Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "DymoScript", (this.TemplateSourceDirectory + "/JavaScript/dymo.label.framework.js"));
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (!IsPostBack)
                {
                    GetStates();
                    GetSettings();
                    
                    ReadQueryString();
                    CheckRentalStatus(_propertyID, DateTime.Parse(_bookingDate.ToString()));
                    SetPanels(_status.ToString());

                    GetProperty(_propertyID);

                    //rblMyRadioButtonList.Items[x].Attributes.CssStyle.Add("margin-right:5px;")
                //    rblStatus.Items[0].Attributes.CssStyle.Add("margin-right", "5px;");

                    //foreach (ListItem li in rblStatus.Items)
                    //{
                    //    //add margin as css style  
                    //    li.Attributes.CssStyle.Add("padding-left", "5px");
                    //    li.Attributes.CssStyle.Add("padding-right", "5px");
                    //}  



                }

                
                lblBookingDate.Text = "From " + DateTime.Parse(_bookingDate.ToString()).ToLongDateString() + " to " 
                    + DateTime.Parse(_bookingDate.ToString()).AddDays(7).ToLongDateString();

                //if (!IsPostBack)
                //{
                //    //load the data into the control the first time
                //    //we hit this page
                //}

                if (Request.QueryString["Status"].ToString() == "a")
                {
                    cmdDeleteReservation.Attributes.Add("onClick", "javascript:return confirm('Do you want to change the rental Price or Status?');");
                }

                else
                {
                    cmdDeleteReservation.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this record?');");
                }

       //         cmdDeleteReservation.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteReservation") + "');");


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void ReadQueryString()
        {

            try
            {


                if (Request.QueryString["AddDate"] != null)
                {
                    _bookingDate = Request.QueryString["AddDate"].ToString();
                }

                if (Request.QueryString["PropertyID"] != null)
                {
                    _propertyID = Int32.Parse(Request.QueryString["PropertyID"]);
                    
                }

                if (Request.QueryString["Status"] != null)
                {
                    _status = Request.QueryString["Status"].ToString();
                    
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
                    
                    string s = settingsData.PA_ModuleID.ToString();
                    string[] parts = s.Split('-');
                    string i1 = parts[0];
                    string i2 = parts[1];

                    _PAModuleID = Convert.ToInt32(i2);
                    _PATabID = Convert.ToInt32(i1); 
                }

                if (settingsData.RoleAgent != null)
                {
                    PARentalsController controller = new PARentalsController();
                    List<PARentalsInfo> items = controller.Rentals_GetUsersByRoleName(this.PortalId, settingsData.RoleAgent.ToString());

                    if (items != null)
                    {
                        ddlAgent.DataSource = items;
                        ddlAgent.DataBind();

                        // ADD FIRST (NULL) ITEM
                        ListItem item = new ListItem();
                        item.Text = "-- Select Agent --";
                        item.Value = "0";
                        ddlAgent.Items.Insert(0, item);

                        hidAgentID.Value = this.UserId.ToString();

                        ListItem liFind = ddlAgent.Items.FindByValue(this.UserId.ToString());
                        if (liFind != null)
                        {
                            // value found
                            ddlAgent.SelectedValue = this.UserId.ToString();
                        }
                        else
                        {
                            //Value not found
                        }

                        
                    }
                }

                if (settingsData.RoleTenant != null)
                {
                    PARentalsController controller = new PARentalsController();
                    List<PARentalsInfo> items = controller.Rentals_GetUsersByRoleName(this.PortalId, settingsData.RoleTenant.ToString());

                    if (items != null)
                    {
                        ddlRenters.DataSource = items;
                        ddlRenters.DataBind();

                        // ADD FIRST (NULL) ITEM
                        ListItem item = new ListItem();
                        item.Text = "-- Select Previous Tenant to Autofill --";
                        item.Value = "0";
                        ddlRenters.Items.Insert(0, item);
                        _tenantUserRole = settingsData.RoleTenant.ToString();
                    }
                }





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

        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(7))
                yield return day;
        }


        //"d" then 'Reservation inserted - need lease        
        //"p" then 'lease generated - sent to tenant
        //"s" then 'mailed to owner
        //"b" then 'returned to tenant - FINISHED

        //"x" then 'its Rented by Another Agency
        //"o" then 'it's OWNER OCCUPIED

        //"a" then 'it's Available
        //"u"  then 'it's Not Yet Made Available


        public void CheckAvailableWeeks(string _startingDate)
        {

            try
            {
                DateTime _StartDate = new DateTime();
                _StartDate = DateTime.Parse(_startingDate.ToString());

                string _IsAvailable = "";
                for (int i = 0; i < 7; i++)
                {
                    _IsAvailable = CheckRentalWeekStatus(_propertyID, _StartDate);
                    _StartDate = _StartDate.AddDays(7);
                    if (_IsAvailable == "a")
                    {
                        int _week = i + 1;
                        ddlRentNumWeeks.Items.Add(new ListItem(_week.ToString() + " Week", _week.ToString()));

                    }
                    else
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }	


        public void SetPanels(string _Status) 
        {

            try
            {
             //   string result = "";

              ////  you tried this . . . .

              //  DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
              //  objEventLog.AddLog("Sample Message", "Something Interesting Happened!", PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

              //  EventLogController eventLog = new EventLogController();
              //  DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new LogInfo();
              //  logInfo.LogUserID = UserId;
              //  logInfo.LogPortalID = PortalSettings.PortalId;
              //  logInfo.LogTypeKey = EventLogController.EventLogType.ADMIN_ALERT.ToString();
              //  logInfo.AddProperty("Status: ", _Status.ToString());
              //  logInfo.AddProperty("KeyWordLike: ", "An Example");
              //  eventLog.AddLog(logInfo);

                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = controller.Rentals_Booking_CheckStatus(_propertyID, DateTime.Parse(_bookingDate));

                if (item != null)
                {
                    
                    hidBookingID.Value = item.BookingID.ToString();
                    hidReservationID.Value = item.ReservationID.ToString();
                }

                switch (_Status.ToLower())
                {
                    case "u":
                        cmdDeleteReservation.Visible = false;
                        PanelManageBooking.Visible = true;
                        divNumWeeks.Visible = true;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = false;
                        divTenantInfo.Visible = false;
                        divLandlordInfo.Visible = true;
                        divStatusPayment.Visible = false;
                        lblCurrentStatus.Text = "Not Yet Made Available";
                        rblStatus.Visible = false;
                        break;
                    //ChangePrice
                    case "a":
                        
                        PanelManageBooking.Visible = false;
                        PanelManageReservation.Visible = true;
                        PanelPayment.Visible = false;
                        divTenantInfo.Visible = false;
                        divStatusPayment.Visible = false;

                        lblCurrentStatus.Text = "Available";
                        rblStatus.Visible = false;
                        CheckAvailableWeeks(_bookingDate);
                        break;

                    case "d":
                    //    lblCurrentStatus.Text = "Need To Generate Lease";
                        
                        PanelManageBooking.Visible = false;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = true;
                        divTenantInfo.Visible = true;
                        divLandlordInfo.Visible = true;
                        divPropertyInfo.Visible = false;

                        break;

                    case "p":
                   //     lblCurrentStatus.Text = "Lease Generated - Mailed to Tenant";
                        
                        PanelManageBooking.Visible = false;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = true;
                        divRenter.Visible = false;
                        divRentNumWeeks.Visible = false;
                        divPropertyInfo.Visible = false;
                        
                        break;

                    case "s":
                    //    lblCurrentStatus.Text = "Mailed to Owner";
                        
                        PanelManageBooking.Visible = false;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = true;
                        divTenantInfo.Visible = true;
                        divLandlordInfo.Visible = true;
                        divPropertyInfo.Visible = false;

                        break;
                    case "b":
                     //   lblCurrentStatus.Text = "Complete";
                        
                        PanelManageBooking.Visible = false;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = false;
                        PanelPayment.Visible = false;
                        divTenantInfo.Visible = true;
                        divLandlordInfo.Visible = true;
                        divPropertyInfo.Visible = false;
                        break;


                    case "x":
                        cmdDeleteReservation.Visible = false;

                        rblStatus.Visible = false;
                        PanelManageBooking.Visible = true;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = false;
                        divTenantInfo.Visible = false;
                        divLandlordInfo.Visible = false;
                        divStatusPayment.Visible = false;
                        lblCurrentStatus.Text = "Rented by Another Agency";
                        break;
                    case "o":
                        cmdDeleteReservation.Visible = false;

                        rblStatus.Visible = false;
                        PanelManageBooking.Visible = true;
                        PanelManageReservation.Visible = false;
                        PanelPayment.Visible = false;
                        divTenantInfo.Visible = false;
                        divLandlordInfo.Visible = false;
                        divStatusPayment.Visible = false;
                        lblCurrentStatus.Text = "Owner Occupied";
                        break;



                }
               
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                
            }

        }

        public void LoadControlType()
        {

            try
            {
               // Ventrian.PropertyAgent.Controls.
                PortalModuleBase pmb = (PortalModuleBase)this.LoadControl("/DesktopModules/PropertyAgent/ViewProperty.ascx");

                if (pmb != null)
                {
                    ctlAudit.CreatedByUser = "JOEAUCOIN";
                    pmb.ModuleConfiguration = this.ModuleConfiguration;
                    pmb.ID = System.IO.Path.GetFileNameWithoutExtension("ViewProperty");
             //       PlaceHolder1.Controls.Add(pmb);

                }
                else
                {
                    ctlAudit.CreatedByUser = "JOEAUCOIN-error";
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }	

        public void GetProperty(int _propertyID)
        {

            try
            {

                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = controller.Rentals_Get_PropertyDetails(_propertyID);
                

                if (item != null)
                {
                    string _Bath = "";

                    if (item.FullBaths > 0)
                    {
                        _Bath = item.FullBaths.ToString() + " Full";
                    }
                    if (item.HalfBaths > 0)
                    {
                        _Bath += " - " + item.HalfBaths.ToString() + " Half";
                    }
                    _Bath += " Bath(s)";
                    
                    string _lit = "";
                    _lit = "<div>" + item.Bedrooms.ToString() + " Bedrooms<br />" +
                        _Bath.ToString() + "<br />" +

                        String.Format("{0:C0}", item.Price).ToString() + " In-Season<br />" +
                   "<span style='color:red;'>" + String.Format("{0:C0}", item.SecurityDeposit).ToString() + " Security Deposit</span></div>";

                    _lit += "<div style='color:red;'>" + item.Notes.ToString() + "</div>";

                    lblPropertyInfo.Text = _lit.ToString();



                    this.ModuleConfiguration.ModuleControl.ControlTitle = item.Address.ToString() + ", " + item.Village.ToString();
                    txtRentalAmount.Text = item.Price.ToString();

             //       txtRentRentalAmount.Text = item.RentalAmount.ToString();

                    hidPropertyID.Value = item.PropertyID.ToString();
                }

                PARentalsInfo owner = controller.Rentals_Get_PropertyOwner(_propertyID, this.PortalId);

                var currentUrl = Globals.NavigateURL(this.TabId, this.Request.QueryString["ctl"], UrlUtils.GetQSParamsForNavigateURL());

         //       string path = HttpContext.Current.Request.Url.AbsolutePath;
                currentUrl = HttpUtility.UrlEncode(currentUrl);
                string SEOPropertyID = GetSetting("SEOPropertyID", _PAModuleID);
                string SEOAgentType = GetSetting("SEOAgentType", _PAModuleID);
                string newURL = Globals.NavigateURL(_PATabID, "", SEOAgentType + "=EditProperty", SEOPropertyID + "=" + _propertyID.ToString()) + "?ReturnUrl=" + currentUrl.ToString();
                hyperLinkEditOwner.NavigateUrl = newURL.ToString();
                if (owner != null)
                {
                    lblLandlordInfo.Text = owner.OwnerFirstName.ToString() + " " + owner.OwnerLastName.ToString() + "<br /> " +
                        owner.OwnerAddress.ToString() + "<br /> " +
                        owner.OwnerCity.ToString() + ", " + owner.OwnerState.ToString() + " " + owner.OwnerZip.ToString() + "<br /> " +
                        "T: " + owner.OwnerTelephone.ToString() + " &nbsp; " +
                        "C: " + owner.OwnerCell.ToString() + "<br /> " +
                         owner.OwnerEmail.ToString();

                    hidAddressLandlord.Value = owner.OwnerFirstName.ToString() + " " + owner.OwnerLastName.ToString() + Environment.NewLine +
                        owner.OwnerAddress.ToString() + Environment.NewLine +
                        owner.OwnerCity.ToString() + ", " + owner.OwnerState.ToString() + " " + owner.OwnerZip.ToString();

                }
                else
                {
                    lblLandlordInfo.Text = "ERROR - CANNOT FIND THE OWNER";
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);

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
                    hidBookingID.Value = item.BookingID.ToString();
                    hidReservationID.Value = item.ReservationID.ToString();
                    lblBookingStatus.Text += "BookingID: " + item.BookingID.ToString() + "<br />ReservationID: " + item.ReservationID.ToString();
                    _status = item.Status.ToString();
                    txtRentRentalAmount.Text = item.RentalAmount.ToString();

                    rblStatus.SelectedValue = _status.ToString();

                    ListItem liFind = ddlStatus.Items.FindByValue(item.Status.ToString());
                    if (liFind != null)
                    {
                        // value found
                        ddlStatus.SelectedValue = item.Status.ToString();
                    }
                    else
                    {
                        //Value not found
                    }

                    if (item.ReservationID > 0)
                    {
                        GetReservation(item.ReservationID);
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "ERROR";
            }

        }

        public string CheckRentalWeekStatus(int _PropertyID, DateTime _BookingDate)
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
                else
                {
                    result = "";
                }

                return result;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "ERROR";
            }

        }

        public void GetReservation(int _reservationID)
        {

            try
            {
                divStatusPayment.Visible = true;

                //load the item
                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = controller.Rentals_Get_Reservation(_reservationID);

                if (item != null)
                {

                    ListItem liFind = ddlAgent.Items.FindByValue(item.AgentID.ToString());
                    if (liFind != null)
                    {
                        // value found
                        ddlAgent.SelectedValue = item.AgentID.ToString();
                    }
                    else
                    {
                        //Value not found
                    }

                    if (item.BalancePaidDate > DateTime.Parse("1/1/2000"))
                    {
                        txtPayBalancePaidDate.Text = item.BalancePaidDate.ToShortDateString();
                        cbxLeaseSentToOwner.Checked = true;
                    }

                    if (item.DepositPaidDate > DateTime.Parse("1/1/2000"))
                    {
                        txtPayDepositPaidDate.Text = item.DepositPaidDate.ToShortDateString();
                    }

                    cbxLeasePrint.Checked = item.LeasePrint;

                    

                    txtRentRentalAmount.Text = item.RentalAmount.ToString();
                    LoadTenantRecord(item.TenantID);

             //       txtPayBalanceAmt.Text = item.BalanceAmt.ToString();
                    txtPayDepositAmt.Text = item.DepositAmt.ToString();

                    double _balancePayment = (item.RentalAmount - item.DepositAmt);
                    txtPayBalanceAmt.Text = _balancePayment.ToString();
                    txtPayBalanceAmt.ReadOnly = true;


                    txtPayNotes.Text = item.Notes.ToString();

                    hidTotalRentAmount.Value = item.RentalAmount.ToString();
                    
         //           lblCurrentStatus.Text += "<br />AgentID: " + item.AgentID.ToString();

                    decimal numba = Convert.ToDecimal(item.RentalAmount);
                    string _totalDue = numba.ToString("C0");

                    numba = Convert.ToDecimal(item.DepositAmt);
                    string _deposit = numba.ToString("C0");

                    numba = Convert.ToDecimal(item.RentalAmount - item.DepositAmt);
                    string _balanceDue = numba.ToString("C0");

                    lblStatusPayment.Text = "<b>" + item.DateStart.ToShortDateString()
                        + " - " + item.DateEnd.ToShortDateString()
                        + "</b><br />Total Rent Due: " + _totalDue.ToString()
                        + "<br />Deposit Due: " + _deposit.ToString()   
                        //       + "<br />Deposit Date: " + txtPayDepositPaidDate.Text.ToString()
                        + "<br /><span style='color:red;'>Check-In Balance: " + _balanceDue.ToString() + "</span>";
                 //       + "<br />Paid On: " + txtPayBalancePaidDate.Text.ToString();
                    
                    //lblReservationDate
                    lblReservationDate.Text = "From " + item.DateStart.ToLongDateString() + " to "
                        + item.DateEnd.ToLongDateString();

                    SetPanels(item.Status.ToString());
                    

                }

               
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                
            }

        }	

        public void AddNewBooking(string __BookingDate)
        {

            try
            {
                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = new PARentalsInfo();

                item.ModuleId = this.ModuleId;
                item.PropertyID = _propertyID;
                item.DateStart = DateTime.Parse(__BookingDate.ToString());
                item.DateEnd = DateTime.Parse(__BookingDate.ToString()).AddDays(7);
                item.CreatedByUserID = this.UserId;
                item.Status = ddlStatus.SelectedValue.ToString();
                item.RentalAmount = double.Parse(txtRentalAmount.Text.ToString());

                controller.Rentals_Booking_Insert(item);



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void UpdateBookingStatus(string __status)
        {

            try
            {
                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = new PARentalsInfo();

                item.Status = __status;
                item.BookingID = Int32.Parse(hidBookingID.Value.ToString());
                item.UpdatedByUserID = this.UserId;
                item.RentalAmount = double.Parse(txtRentalAmount.Text.ToString());
                controller.Rentals_Booking_Update_Status(item);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }	

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //HERE

                if (Int32.Parse(hidBookingID.Value.ToString()) > 0)
                {
                    UpdateBookingStatus(ddlStatus.SelectedValue.ToString());
                }
                else
                {
                    DateTime _StartDate = new DateTime();
                    _StartDate = DateTime.Parse(_bookingDate.ToString());

                    for (int i = 0; i < Int32.Parse(ddlNumWeeks.SelectedValue.ToString()); i++)
                    {
                        AddNewBooking(_StartDate.ToShortDateString());
                        _StartDate = _StartDate.AddDays(7);
                    }
                }



                Response.Redirect(Globals.NavigateURL(), true);
            

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdDeleteReservation_Click(object sender, EventArgs e)
        {
            try
            {
                if (hidReservationID.Value.ToString() != "0")
                {
                    PARentalsController controller = new PARentalsController();
                    controller.Rentals_Reservation_Delete(Int32.Parse(hidReservationID.Value.ToString()), this.UserId);
                    Response.Redirect(Globals.NavigateURL(), true);
                }
                else
                {
                    PanelManageBooking.Visible = true;
                    //divNumWeeks.Visible = true;
                    PanelManageReservation.Visible = false;
                    PanelPayment.Visible = false;
                    divTenantInfo.Visible = false;
                    divLandlordInfo.Visible = true;
                    divStatusPayment.Visible = false;
                 //   lblCurrentStatus.Text = "Not Yet Made Available";
                    rblStatus.Visible = false;
                
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void LoadTenantRecord(int RecordID)
        {
            try
            {

                //DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                //objEventLog.AddLog("LoadTenantRecordID", "LoadTenantRecordID: " + RecordID.ToString(), PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);

                divTenantInfo.Visible = true;
                if (RecordID > 0)
                {
                    DotNetNuke.Entities.Users.UserInfo PreviousTenant = DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, RecordID);

                    if (PreviousTenant != null)
                    {
                        txtFirstName.Text = PreviousTenant.FirstName;
                        txtLastName.Text = PreviousTenant.LastName;

                        txtEmail.Text = PreviousTenant.Email;


                        

                        txtStreet.Text = PreviousTenant.Profile.Street;

                        txtCity.Text = PreviousTenant.Profile.City;

                        txtTelephone.Text = PreviousTenant.Profile.Telephone;
                        txtCellPhone.Text = PreviousTenant.Profile.Cell;

                        txtZip.Text = PreviousTenant.Profile.PostalCode;


                        ListItem liregion = ddlState.Items.FindByText(PreviousTenant.Profile.Region);      //.FindByValue(DonationUser.Profile.Region);
                        if (liregion != null)
                        {
                            // value found
                            ddlState.SelectedValue = ddlState.Items.FindByText(PreviousTenant.Profile.Region).Value;    //DonationUser.Profile.Region;
                        }
                        else
                        {
                            //Value not found
                            //   ddlTown.SelectedValue = item.ClientTown;
                            lblStatus.Text += "<br />STATE NOT FOUND - PLEASE UPDATE";
                        }


                        hidPreviousTenantID.Value = RecordID.ToString();

                        lblTenantInfo.Text = PreviousTenant.FirstName + " " +
                            PreviousTenant.LastName + "<br />" +
                            PreviousTenant.Profile.Street + "<br />" +
                            PreviousTenant.Profile.City + ", " + PreviousTenant.Profile.Region
                            + " " + PreviousTenant.Profile.PostalCode
                            + "<br />T: " + PreviousTenant.Profile.Telephone
                             + " &nbsp;C: " + PreviousTenant.Profile.Cell
                             + "<br />" + PreviousTenant.Email;

                        hidAddressTenant.Value = PreviousTenant.FirstName + " " +
                            PreviousTenant.LastName + Environment.NewLine +
                            PreviousTenant.Profile.Street + Environment.NewLine +
                            PreviousTenant.Profile.City + ", " + PreviousTenant.Profile.Region
                            + " " + PreviousTenant.Profile.PostalCode;

                    }
                    else
                    {
                        lblStatus.Text = "ERROR ON LoadTenantRecord: " + RecordID.ToString();
                    }
                }
                else
                {
                    lblStatus.Text = "Null RecordID: " + RecordID.ToString();
                }

               
                
               
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void ddlRenters_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //DotNetNuke.Services.Log.EventLog.EventLogController objEventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
                //objEventLog.AddLog("Tenant Selected", ddlRenters.SelectedValue.ToString(), PortalSettings, -1, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);
                LoadTenantRecord(Convert.ToInt32(ddlRenters.SelectedValue.ToString()));
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void UpdateTenantRecord(int RecordID)
        {
            try
            {
                //  DotNetNuke.Entities.Users.UserInfo uUser = DotNetNuke.Entities.Users.UserController.GetUserById(PortalSettings.PortalId, RecordID);

                UserController objUserController = new UserController();
                UserInfo uUser = objUserController.GetUser(this.PortalId, RecordID);

                uUser.FirstName = txtFirstName.Text.ToString();
                uUser.LastName = txtLastName.Text.ToString();
                uUser.Email = txtEmail.Text.ToString();

                uUser.Profile.Street = txtStreet.Text.ToString();
                uUser.Profile.City = txtCity.Text.ToString();




                uUser.Profile.Telephone = txtTelephone.Text.ToString();

                uUser.Profile.Cell = txtCellPhone.Text.ToString();

                uUser.Profile.PostalCode = txtZip.Text.ToString();
                uUser.Profile.Region = ddlState.SelectedValue.ToString();



                UserController.UpdateUser(this.PortalId, uUser);

                // RELOAD RECORD
                LoadTenantRecord(RecordID);

                lblStatus.Text = "Record Successully Updated";


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        

        public void CreateNewTenant(string AddUserRole)
        {

            try
            {

                string vPassword = GenerateRandomString(7);

                UserInfo oUser = new UserInfo();

                oUser.PortalID = this.PortalId;
                oUser.IsSuperUser = false;
                oUser.FirstName = txtFirstName.Text.ToString();
                oUser.LastName = txtLastName.Text.ToString();
                oUser.Email = txtEmail.Text.ToString();
                //
                string NewUserName = "";
                if (txtEmail.Text.ToString().Length >= 7)
                {
                    NewUserName = txtEmail.Text.ToString();
                }
               
                else
                {
                    NewUserName = txtLastName.Text.ToString().Trim().Replace(" ", "").Replace("'", "") + txtFirstName.Text.ToString().Trim().Replace(" ", "").Replace("'", "");
                }
                if (NewUserName.ToString().Length < 7)
                {
                    NewUserName += "!!!";
                }

                oUser.Username = NewUserName.ToString();

                oUser.DisplayName = txtFirstName.Text.ToString() + " " + txtLastName.Text.ToString();

                //Fill MINIMUM Profile Items (KEY PIECE)
                oUser.Profile.PreferredLocale = PortalSettings.DefaultLanguage;
                oUser.Profile.PreferredTimeZone = PortalSettings.TimeZone;
                oUser.Profile.FirstName = oUser.FirstName;
                oUser.Profile.LastName = oUser.LastName;

                oUser.Profile.Street = txtStreet.Text.ToString();
                oUser.Profile.City = txtCity.Text.ToString();


                oUser.Profile.Country = "United States";
                oUser.Profile.Telephone = txtTelephone.Text.ToString();
                oUser.Profile.Cell = txtCellPhone.Text.ToString();

                oUser.Profile.PostalCode = txtZip.Text.ToString();
                oUser.Profile.Region = ddlState.SelectedValue.ToString();

                //Set Membership
                UserMembership oNewMembership = new UserMembership(oUser);
                oNewMembership.Approved = true;
                oNewMembership.CreatedDate = System.DateTime.Now;

                //oNewMembership.Email = oUser.Email;
                oNewMembership.IsOnLine = false;
               // oNewMembership.Username = oUser.Username;
                oNewMembership.Password = vPassword.ToString();

                //Bind membership to user
                oUser.Membership = oNewMembership;

                //Add the user, ensure it was successful 
                if (DotNetNuke.Security.Membership.UserCreateStatus.Success == UserController.CreateUser(ref oUser))
                {
                    //Add Role if passed something from module settings

                    if (AddUserRole.Length > 0)
                    {
                        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
                        //retrieve role
                        string groupName = AddUserRole;
                        DotNetNuke.Security.Roles.RoleInfo ri = rc.GetRoleByName(this.PortalId, groupName);
                        rc.AddUserRole(this.PortalId, oUser.UserID, ri.RoleID, Null.NullDate);
                    }


                    hidPreviousTenantID.Value = oUser.UserID.ToString();

                    ////string EmailContent = settingsData.EmailMessage + content;
                    //DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);

                    //if (settingsData.EmailNewUserCredentials != null)
                    //{
                    //    if (Convert.ToBoolean(settingsData.EmailNewUserCredentials) == true)
                    //    {
                    //        string EmailContent = settingsData.EmailMessage + "<p>UserName: " + txtEmail.Text.ToString() + "<br />";
                    //        EmailContent += "Password: " + vPassword.ToString() + "<br />";
                    //        EmailContent += "Site: http://" + Request.Url.Host + "</p>";

                    //        EmailContent = EmailContent.ToString().Replace("[FULLNAME]", oUser.DisplayName);

                    //        // SEND EMAIL
                    //        EmailNotification(EmailContent.ToString(), txtEmail.Text.ToString());
                    //    }
                    //    else
                    //    {
                    //        // DO NOT SEND EMAIL
                    //    }

                    //}




                    //// THIS URL WILL GIVE YOU THE ADD NEW DONATION PANEL
                    //string newURL = Globals.NavigateURL(this.TabId, "", "UserId=" + oUser.UserID, "ctl=Edit", "mid=" + this.ModuleId, "Status=Success");

                    //// THIS URL WILL GIVE YOU A BLANK FORM TO ADD A NEW USER RECORD
                    ////string newURL = Globals.NavigateURL(this.TabId, "", "ctl=Edit", "mid=" + this.ModuleId, "Status=Success");

                    //Response.Redirect(newURL, false);

                }
                else
                {
                    DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserByName(oUser.Username);
                //    LoadRecord(DonationUser.UserID);
                    LoadTenantRecord(DonationUser.UserID);
                    lblStatus.Text = "Error on Insert. Thay user already exists . . . I've loaded the record for you!";
                }






            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void GetStates()
        {

            try
            {

                var _states = new ListController().GetListEntryInfoItems("Region", "Country.US", this.PortalId);

                //ddlState.DataTextField = "Value";
                //ddlState.DataValueField = "Text";

                ddlState.DataTextField = "Text";
                ddlState.DataValueField = "EntryID";

                ddlState.DataSource = _states;
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("--", ""));

                ddlState.SelectedValue = ddlState.Items.FindByText("Massachusetts").Value;
                

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected void cmdBookingUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //HERE
                if (hidPreviousTenantID.Value.ToString() == "0")
                {
                    CreateNewTenant(_tenantUserRole.ToString());
                }
                else
                {
                    UpdateTenantRecord(Int32.Parse(hidPreviousTenantID.Value.ToString()));
                }

                if (hidReservationID.Value.ToString() == "0")
                {
                    AddNewReservation();
                }
                

               

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public static string GenerateRandomString(int length)
        {
            //Removed O, o, 0, l, 1 
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[length];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
        }



       

        protected void cmdEditTenant_Click(object sender, EventArgs e)
        {
            PanelManageReservation.Visible = true;
            cmdReturnToPayment.Visible = true;
            PanelPayment.Visible = false;
            divRenter.Visible = false;
            divRentRentalAmount.Visible = false;
            divRentNumWeeks.Visible = false;
            divRentalAgent.Visible = false;

        }
        protected void cmdReturnToPayment_Click(object sender, EventArgs e)
        {
            PanelManageReservation.Visible = false;
            PanelPayment.Visible = true;
            
            divRenter.Visible = false;
            divRentRentalAmount.Visible = false;
            divRentNumWeeks.Visible = false;
            divRentalAgent.Visible = false;

        }

        //cmdReturnToPayment_Click

        public void Print(string firstName, string lastName)
        {

            //var label = Dymo
            //label.SetObjectText("lblFirstName", firstName);
            //label.SetObjectText("lblLastName", lastName);
            //label.Print("DYMO LabelWriter 450");
        }



        protected void cmdPaymentUpdate_Click(object sender, EventArgs e)
        {

            UpdatePayment();
            GetReservation(Int32.Parse(hidReservationID.Value.ToString()));

        }

        protected void cmdMakeLease_Click(object sender, EventArgs e)
        {
         //   Response.Redirect(EditUrl(this.TabId, "Lease", true, "ResID=" + hidReservationID.Value.ToString()));
            
            cbxLeasePrint.Checked = true;
            txtPayDepositPaidDate.Text = DateTime.Today.ToShortDateString();
            UpdatePayment();

            Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Lease", "mid=" + ModuleId.ToString() + "&ResID=" + hidReservationID.Value.ToString() + "&p_PropertyID=" + _propertyID.ToString()));
        //    Response.Redirect(EditUrl("Lease"));

        }

        //cmdReturn_Click
        protected void cmdReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(), true);

        }

        public void UpdatePayment()
        {

            try
            {

                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = new PARentalsInfo();

                item.ReservationID = Int32.Parse(hidReservationID.Value.ToString());
                item.DepositAmt = Int32.Parse(txtPayDepositAmt.Text.ToString());

                DateTime _DepositPaidDate = DateTime.MinValue;
                if (DateTime.TryParse(txtPayDepositPaidDate.Text.ToString(), out _DepositPaidDate))
                {
                    //SUCCESS
                    item.DepositPaidDate = _DepositPaidDate;
                }
                else
                {
                    //FAILURE
                    item.DepositPaidDate = DateTime.Parse("1/1/1900"); ;
                }

                
                DateTime _BalancePaidDate = DateTime.MinValue;
                if (DateTime.TryParse(txtPayBalancePaidDate.Text.ToString(), out _BalancePaidDate))
                {
                    item.BalancePaidDate = _BalancePaidDate;
                }
                //SUCCESS 
                else
                //FAILURE
                {
                    item.BalancePaidDate = DateTime.Parse("1/1/1900"); ;
                }
                
                
                item.BalanceAmt = Int32.Parse(txtPayBalanceAmt.Text.ToString());


                // NEED TO ADD WAY TO COMPLETE - Set status to "b"  cbxLeaseProcessComplete

                if (cbxLeaseProcessComplete.Checked)
                {
                    item.Status = "b";
                }

                else if (cbxLeasePrint.Checked && txtPayDepositPaidDate.Text.ToString().Length > 7 && cbxLeaseSentToOwner.Checked && txtPayBalancePaidDate.Text.ToString().Length > 7)
                {
                    item.Status = "s";
                }


                else if (cbxLeasePrint.Checked && txtPayDepositPaidDate.Text.ToString().Length > 7)
                {
                    item.Status = "p";
                }

                else
                {
                    item.Status = "d";
                }

                rblStatus.SelectedValue = item.Status.ToString();
            
                item.Notes = txtPayNotes.Text.ToString();
                item.UpdatedByUserID = this.UserId;
                item.LeasePrint = cbxLeasePrint.Checked;
                

                //lblStatusPayment.Text = "ModuleId: " + item.ModuleId.ToString()
                //    + " PropertyID: " + item.PropertyID.ToString()
                //    + " TenantID: " + item.TenantID.ToString()
                //    + " AgentID: " + item.AgentID.ToString()
                //    + " DateStart: " + item.DateStart.ToString()
                //    + " CreatedByUserID: " + item.CreatedByUserID.ToString() + " Status: " + item.Status.ToString();
                controller.Rentals_Update_Reservation_Payment(item);

                //"d" then 'Reservation inserted - need lease        
                //"p" then 'lease generated - sent to tenant
                //"s" then 'mailed to owner
                //"b" then 'returned to tenant 

                //"x" then 'its Rented by Another Agency
                //"o" then 'it's OWNER OCCUPIED

                //"a" then 'it's Available
                //"u"  then 'it's Not Yet Made Available


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }		

        public void AddNewReservation()
        {

            try
            {
                double _RentAmt = double.Parse(txtRentalAmount.Text.ToString());
                _RentAmt = _RentAmt * Int32.Parse(ddlRentNumWeeks.SelectedValue.ToString());

                DateTime _EndDate = new DateTime();
                _EndDate = DateTime.Parse(_bookingDate.ToString());

                for (int i = 0; i < Int32.Parse(ddlRentNumWeeks.SelectedValue.ToString()); i++)
                {
                    _EndDate = _EndDate.AddDays(7);
                }

                double HowManyDays = (_EndDate - DateTime.Parse(_bookingDate)).TotalDays;
                double HowMuchDeposit = 0;
                if (HowManyDays > 7)
                {
                    HowMuchDeposit = _RentAmt / 2;
                }

                else
                {
                    HowMuchDeposit = _RentAmt;
                }
                
                PARentalsController controller = new PARentalsController();
                PARentalsInfo item = new PARentalsInfo();


                item.DepositAmt = HowMuchDeposit;
                item.ModuleId = this.ModuleId;
                item.PropertyID = Int32.Parse(hidPropertyID.Value.ToString());
                item.TenantID = Int32.Parse(hidPreviousTenantID.Value.ToString());
                item.AgentID = Int32.Parse(ddlAgent.SelectedValue.ToString());
                item.DateStart = DateTime.Parse(_bookingDate.ToString());
                item.DateEnd = _EndDate;
                item.RentalAmount = _RentAmt;
                item.CreatedByUserID = this.UserId;

                //"d" then 'Reservation inserted - need lease       
                item.Status = "d";

                lblStatusPayment.Text = "ModuleId: " + item.ModuleId.ToString() 
                    + " PropertyID: " + item.PropertyID.ToString() 
                    + " TenantID: " + item.TenantID.ToString() 
                    + " AgentID: " + item.AgentID.ToString() 
                    + " DateStart: " + item.DateStart.ToString() 
                    + " CreatedByUserID: " + item.CreatedByUserID.ToString() + " Status: " + item.Status.ToString();
                controller.Rentals_Reservation_Insert(item);


                CheckRentalStatus(_propertyID, DateTime.Parse(_bookingDate.ToString()));
                SetPanels("d");
                

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected void ddlRentNumWeeks_SelectedIndexChanged(object sender, EventArgs e)
        {

            DateTime _EndDate = new DateTime();
            _EndDate = DateTime.Parse(_bookingDate.ToString());

            for (int i = 0; i < Int32.Parse(ddlRentNumWeeks.SelectedValue.ToString()); i++)
            {
                _EndDate = _EndDate.AddDays(7);
            }
            
            lblBookingDate.Text = "From " + DateTime.Parse(_bookingDate.ToString()).ToLongDateString() + " to "
                    + _EndDate.ToLongDateString();
        }		


    }
}



//Dymo.Label.Framework.Label.Open("LargeAddressTestLabel.label");