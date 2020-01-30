<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPARentals.ascx.cs" Inherits="GIBS.Modules.PARentals.EditPARentals" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>

<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css" />

<script src="http://labelwriter.com/software/dls/sdk/js/DYMO.Label.Framework.latest.js"
        type="text/javascript" charset="UTF-8"> </script>


<script type="text/javascript">


    function printMyLabel(addressString) {
        alert(addressString);

        var labelXml = '<DieCutLabel Version="8.0" Units="twips">\
             <PaperOrientation>Landscape</PaperOrientation>\
             <Id>Address</Id>\
             <PaperName>30252 Address</PaperName>\
             <DrawCommands/>\
             <ObjectInfo>\
             <TextObject>\
             <Name>Text</Name>\
             <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
             <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
             <LinkedObjectName></LinkedObjectName>\
             <Rotation>Rotation0</Rotation>\
             <IsMirrored>False</IsMirrored>\
             <IsVariable>True</IsVariable>\
             <HorizontalAlignment>Left</HorizontalAlignment>\
             <VerticalAlignment>Middle</VerticalAlignment>\
             <TextFitMode>ShrinkToFit</TextFitMode>\
             <UseFullFontHeight>True</UseFullFontHeight>\
             <Verticalized>False</Verticalized>\
             <StyledText/>\
             </TextObject>\
             <Bounds X="332" Y="150" Width="4455" Height="1260" />\
             </ObjectInfo>\
             </DieCutLabel>';

        var label = dymo.label.framework.openLabelXml(labelXml);

        //  var label = DYMO.Label.Framework.Label.Open("Rental.label");
        //hidAddressLandlord
        var printAddress = addressString;
        label.setObjectText("Text", printAddress);


        var printers = dymo.label.framework.getPrinters();
        if (printers.length == 0)
            throw "No DYMO printers are installed. Install DYMO printers.";

        var printerName = "";
        for (var i = 0; i < printers.length; ++i) {
            var printer = printers[i];
            if (printer.printerType == "LabelWriterPrinter") {
                printerName = printer.name;
                break;
            }
        }

        label.print(printerName);
    }


    $(function () {
        $("#printDivLandlord").click(function () {

            var printAddress = $("#<%= hidAddressLandlord.ClientID %>").val();
            printMyLabel(printAddress);

        }
      );
    });


    $(function () {
        $("#printDivTenant").click(function () {

            var printAddress = $("#<%= hidAddressTenant.ClientID %>").val();
            printMyLabel(printAddress);

        }
      );
    });




    $(document).ready(function () {
        //set initial state.
        $("#<%= cbxLeaseSentToOwner.ClientID %>").click(function () {

            var isChecked = $('#<%= cbxLeaseSentToOwner.ClientID %>').is(':checked');
           // alert(isChecked);
            if (isChecked) {
                var myDate = new Date();
                var prettyDate = (myDate.getMonth() + 1) + '/' + myDate.getDate() + '/' + myDate.getFullYear();
                $("#<%= txtPayBalancePaidDate.ClientID %>").val(prettyDate);
            }
            else {
                $("#<%= txtPayBalancePaidDate.ClientID %>").val("");
            }

        });

    });




    $(function () {
        $("#<% = txtPayDepositPaidDate.ClientID %>").datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        $("#<% = txtPayBalancePaidDate.ClientID %>").datepicker({
            numberOfMonths: 1,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
    });

 </script>









<div style="text-align:center;">
     <h4><asp:Label ID="lblReservationDate" runat="server" Text="" /></h4>
</div>


<div>


</div>


<div>
    <div class="dnnFormMessage dnnFormWarning" style="width:180px;float:left;margin-right:10px;" id="divPropertyInfo" runat="server"><asp:Label ID="lblPropertyInfo" runat="server" Text="lblPropertyInfo" /></div>


    <div class="dnnFormMessage dnnFormWarning" style="width:260px;float:left; margin-right:10px;" id="divLandlordInfo" runat="server">
    <div><b>LANDLORD INFO</b></div>
        <asp:Label ID="lblLandlordInfo" runat="server" Text="" />
        <br />        <div id="printDivLandlord" style="border:1px solid black; float:left;cursor: pointer; padding-left:6px; padding-right:6px; margin-right:6px; background-color:#E8E8E8;">
            Print Label

        </div> 
        
        <asp:HyperLink ID="hyperLinkEditOwner" runat="server">Edit Owner</asp:HyperLink>
      
    </div>	

    <div class="dnnFormMessage dnnFormWarning" style="width:260px;float:left;margin-right:10px;" id="divTenantInfo" runat="server">
    <div><b>TENANT INFO</b></div>
        <asp:Label ID="lblTenantInfo" runat="server" Text="" />
        <br /><div id="printDivTenant" 
            style="border:1px solid black; float:left;cursor: pointer; padding-left:6px; padding-right:6px; margin-right:6px; background-color:#E8E8E8;">
            Print Label

        </div>
        
            <asp:LinkButton ID="cmdEditTenant" runat="server" resourcekey="cmdEditTenant" OnClick="cmdEditTenant_Click" />
    </div>



    <div class="dnnFormMessage dnnFormWarning" style="width:220px;float:left;margin-right:10px;" id="divStatusPayment" runat="server">
    <div><b>PAYMENT INFO</b></div>
    <asp:Label ID="lblStatusPayment" runat="server" Text="" /></div>

    <div class="dnnFormMessage dnnFormSuccess" style="width:220px;float:left;"><b>CURRENT STATUS</b>
    <asp:linkbutton id="cmdDeleteReservation" runat="server" OnClick="cmdDeleteReservation_Click"><img src="/DesktopModules/GIBS/PARentals/Images/icon_delete.png" alt="Delete" /></asp:linkbutton>
    <br /><b><asp:Label ID="lblCurrentStatus" runat="server" /></b>
    <asp:RadioButtonList ID="rblStatus" runat="server" Enabled="false" RepeatLayout="Flow">  
                    <asp:ListItem Text="Pending Lease" Value="d" />
                    <asp:ListItem Text="Lease Mailed to Tenant" Value="p" />
                    <asp:ListItem Text="Lease Mailed to Owner" Value="s" />
                    <asp:ListItem Text="Lease Returned to Tenant" Value="b" />

 
                </asp:RadioButtonList>  </div>

</div>





<div style="clear:both">&nbsp;</div>



<asp:Panel ID="PanelManageBooking" runat="server">



<div class="dnnForm" id="form-settings-status">


    <fieldset>


		
		<div class="dnnFormItem">					
		<dnn:label id="lblStatus" runat="server" suffix=":" controlname="ddlStatus" ResourceKey="lblStatus" />
            <asp:DropDownList ID="ddlStatus" runat="server">
                  <asp:ListItem Text="Available" Value="a" />
				  <asp:ListItem Text="Owner Occupied" Value="o" />
				  <asp:ListItem Text="Rented by Another Agency" Value="x" />
            </asp:DropDownList>
		</div>	



		<div class="dnnFormItem">					
		<dnn:label id="lblRentalAmount" runat="server" suffix=":" controlname="txtRentalAmount" ResourceKey="lblRentalAmount" />
		<asp:TextBox ID="txtRentalAmount" runat="server" />
		</div>	


		<div class="dnnFormItem" id="divNumWeeks" runat="server" visible="false">					
		<dnn:label id="lblNumWeeks" runat="server" suffix=":" controlname="ddlNumWeeks" ResourceKey="lblNumWeeks" />
		 <asp:DropDownList ID="ddlNumWeeks" runat="server" AutoPostBack="True">
				  <asp:ListItem Text="1" Value="1" />
				  <asp:ListItem Text="2" Value="2" />
				  <asp:ListItem Text="3" Value="3" />
				  <asp:ListItem Text="4" Value="4" />
				  <asp:ListItem Text="5" Value="5" />
				  <asp:ListItem Text="6" Value="6" />
				  <asp:ListItem Text="7" Value="7" />
				  <asp:ListItem Text="8" Value="8" />
				  </asp:DropDownList>
		</div>	
		

    </fieldset>

</div>
<p style="text-align:center;">
    <asp:linkbutton id="cmdUpdate" resourcekey="cmdUpdate" runat="server" cssclass="dnnPrimaryAction" text="Update" OnClick="cmdUpdate_Click"></asp:linkbutton>&nbsp;
    <asp:linkbutton id="cmdCancel" resourcekey="cmdCancel" runat="server" cssclass="dnnSecondaryAction" text="Cancel" causesvalidation="False" OnClick="cmdCancel_Click"></asp:linkbutton>&nbsp;

</p>

</asp:Panel>

<asp:Panel ID="PanelManageReservation" runat="server">


<div class="dnnForm" id="form-BookReservation">
    
<div class="dnnFormMessage dnnFormWarning" style="width:180px;float:right;"><asp:Label ID="lblBookingStatus" runat="server" Text="" /></div>

		<div style="text-align:center;">
        <h4><asp:Label ID="lblBookingDate" runat="server" Text="" /></h4>
		</div>
    <fieldset>


		
		<div class="dnnFormItem" id="divRentalAgent" runat="server">		

		<dnn:label id="lblAgent" runat="server" suffix=":" controlname="ddlAgent" ResourceKey="lblAgent" />
            <asp:DropDownList ID="ddlAgent" runat="server" DataTextField="FullName" DataValueField="ItemId">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="AddReservation" ControlToValidate="ddlAgent" 
            InitialValue="0" ErrorMessage="Please select an agent." Display="Dynamic" />
		</div>	
		
        <div class="dnnFormItem" id="divRenter" runat="server">
		<dnn:label id="lblRenter" runat="server" suffix=":" controlname="ddlRenters" ResourceKey="lblRenter" />
            <asp:DropDownList ID="ddlRenters" DataTextField="FullName" 
                DataValueField="ItemId" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlRenters_SelectedIndexChanged">
            </asp:DropDownList>
		</div>	



		<div class="dnnFormItem" id="divRentRentalAmount" runat="server">					
		<dnn:label id="lblRentRentalAmount" runat="server" suffix=":" controlname="txtRentRentalAmount" ResourceKey="lblRentRentalAmount" />
		<asp:TextBox ID="txtRentRentalAmount" runat="server" />
		</div>	


		<div class="dnnFormItem" id="divRentNumWeeks" runat="server">					
		<dnn:label id="lblRentNumWeeks" runat="server" suffix=":" controlname="ddlRentNumWeeks" ResourceKey="lblRentNumWeeks" />
		 <asp:DropDownList ID="ddlRentNumWeeks" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddlRentNumWeeks_SelectedIndexChanged">

				  </asp:DropDownList>
		</div>	
		
		
                <div class="dnnFormItem">
                    <dnn:Label ID="lblFirstLastName" runat="server" ControlName="txtFirstName" Suffix=":" />
                    <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="UserForm" CssClass="dnnFormRequired"
                        Width="210px" /><asp:RequiredFieldValidator runat="server" ID="reqFirstName" CssClass="dnnFormMessage dnnFormError"
                        resourcekey="reqFirstName" ControlToValidate="txtFirstName" ErrorMessage="First Name Required!" Display="Dynamic"
                        ValidationGroup="UserForm" />
                    
                    <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="UserForm" CssClass="dnnFormRequired"
                        Width="210px" />
                    
                    <asp:RequiredFieldValidator runat="server" ID="reqLastName" resourcekey="reqLastName"
                        CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtLastName" ErrorMessage="Lanst Name Required!" Display="Dynamic"
                        ValidationGroup="UserForm" />
                </div>


                <div class="dnnFormItem">
                    <dnn:Label ID="lblEmail" runat="server" ControlName="txtEmail" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblStreet" runat="server" ControlName="txtStreet" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtStreet" runat="server"></asp:TextBox>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblCityStateZip" runat="server" ControlName="txtCity" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtCity" runat="server" Width="220px"></asp:TextBox>&nbsp;<asp:DropDownList
                        ID="ddlState" runat="server" Width="80px">
                    </asp:DropDownList>
                    &nbsp;<asp:TextBox ID="txtZip" runat="server" Width="100px"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblTelephone" runat="server" ControlName="txtTelephone" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtTelephone" runat="server"></asp:TextBox>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblCellPhone" runat="server" ControlName="txtCellPhone" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox>
                </div>
	
	<p style="text-align:center;">
    <asp:linkbutton cssclass="dnnPrimaryAction" id="cmdBookingUpdate" resourcekey="cmdBookingUpdate" runat="server" ValidationGroup="AddReservation" OnClick="cmdBookingUpdate_Click" /> 
    <asp:linkbutton cssclass="dnnSecondaryAction" id="cmdReturnToPayment" resourcekey="cmdReturnToPayment" runat="server" OnClick="cmdReturnToPayment_Click" Visible="false" />
        

</p>		

    </fieldset>

</div>


</asp:Panel>



<asp:Panel ID="PanelPayment" runat="server" Visible="true">

<div class="dnnForm" id="DivPanelPayment">
    


    <fieldset>

		<div class="dnnFormItem">					
		<dnn:label id="lblPayDepositAmt" runat="server" suffix=":" controlname="txtPayDepositAmt" ResourceKey="lblPayDepositAmt" />
		<asp:TextBox ID="txtPayDepositAmt" runat="server" />
		</div>	
		<div class="dnnFormItem">					
		<dnn:label id="lblBalanceAmt" runat="server" suffix=":" controlname="txtPayBalanceAmt" ResourceKey="lblBalanceAmt" />
		<asp:TextBox ID="txtPayBalanceAmt" runat="server" />
		</div>	
		<div class="dnnFormItem">					
		<dnn:label id="lblLeasePrint" runat="server" suffix=":" controlname="cbxLeasePrint" ResourceKey="lblLeasePrint" />
            <asp:CheckBox ID="cbxLeasePrint" runat="server" />
		</div>	

		<div class="dnnFormItem">					
		<dnn:label id="lblPayDepositPaidDate" runat="server" suffix=":" controlname="txtPayDepositPaidDate" ResourceKey="lblPayDepositPaidDate" />
		<asp:TextBox ID="txtPayDepositPaidDate" runat="server" />
		</div>	
		<div class="dnnFormItem">					
		<dnn:label id="lblLeaseSentToOwner" runat="server" suffix=":" controlname="cbxLeaseSentToOwner" ResourceKey="lblLeaseSentToOwner" />
            <asp:CheckBox ID="cbxLeaseSentToOwner" runat="server" />
		</div>	

		
		<div class="dnnFormItem">					
		<dnn:label id="lblPayBalancePaidDate" runat="server" suffix=":" controlname="txtPayBalancePaidDate" ResourceKey="lblPayBalancePaidDate" />
		<asp:TextBox ID="txtPayBalancePaidDate" runat="server" />
		</div>			

	
				<div class="dnnFormItem">					
		<dnn:label id="lblLeaseProcessComplete" runat="server" suffix=":" controlname="cbxLeaseProcessComplete" ResourceKey="lblLeaseProcessComplete" />
		<asp:CheckBox ID="cbxLeaseProcessComplete" runat="server" />
		</div>
 
    
    	<div class="dnnFormItem">					
		<dnn:label id="lblPayNotes" runat="server" suffix=":" controlname="txtPayNotes" ResourceKey="lblPayNotes"  />
		<asp:TextBox ID="txtPayNotes" runat="server" TextMode="MultiLine" />
		</div>	
	
	<p style="text-align:center;">
    <asp:linkbutton cssclass="dnnPrimaryAction" id="cmdPaymentUpdate" resourcekey="cmdPaymentUpdate" runat="server" text="Update" OnClick="cmdPaymentUpdate_Click" />
    <asp:linkbutton cssclass="dnnSecondaryAction" id="cmdMakeLease" resourcekey="cmdMakeLease" runat="server" text="Update" OnClick="cmdMakeLease_Click" />	
	</p>		

    </fieldset>

</div>

</asp:Panel>


<div style="text-align:right"><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdReturn" resourcekey="cmdReturn" runat="server" OnClick="cmdReturn_Click" /></div>

<asp:HiddenField ID="hidPropertyID" runat="server" Value="0" />
<asp:HiddenField ID="hidAgentID" runat="server" Value="0" />
<asp:HiddenField ID="hidBookingID" runat="server" Value="0" />
<asp:HiddenField ID="hidReservationID" runat="server" Value="0" />
<asp:HiddenField ID="hidPreviousTenantID" runat="server" Value="0" />
<asp:HiddenField ID="hidTotalRentAmount" runat="server" Value="0" />
<asp:HiddenField ID="hidAddressTenant" runat="server" Value="" />
<asp:HiddenField ID="hidAddressLandlord" runat="server" Value="" />
<dnn:Audit id="ctlAudit" runat="server" Visible="false" />