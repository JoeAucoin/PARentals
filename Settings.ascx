<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.PARentals.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css" />
<script type="text/javascript">

    $(function () {
        $("#txtStartDate").datepicker({
            numberOfMonths: 1,
            minDate: 0,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        $("#txtEndDate").datepicker({
            showButtonPanel: false
        });
    });

 </script>

<div class="dnnForm" id="form-settings">


    <fieldset>

<dnn:sectionhead id="sectGeneralSettings" cssclass="Head" runat="server" text="General Settings" section="GeneralSection"
	includerule="False" isexpanded="True"></dnn:sectionhead>

<div id="GeneralSection" runat="server">   

     <div class="dnnFormItem">					
    <dnn:label id="lblStartDate" runat="server" suffix=":" controlname="txtStartDate" ResourceKey="lblStartDate" />
         <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static" />		
    </div>


	     <div class="dnnFormItem">					
    <dnn:label id="lblEndDate" runat="server" suffix=":" controlname="txtEndDate" ResourceKey="lblEndDate" />
         <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" />		
    </div>
                    		
<div class="dnnFormItem">					
<dnn:label id="lblPAModuleID" runat="server" suffix=":" controlname="drpModuleID" ResourceKey="lblPAModuleID" />
			<asp:dropdownlist id="drpModuleID" Runat="server" datavaluefield="ModuleID" datatextfield="ModuleTitle"
				AutoPostBack="True"></asp:dropdownlist></div>	

	<div class="dnnFormItem">
    
        <dnn:label id="lblAddUserRole" runat="server" controlname="ddlAddUserRole" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlAddUserRole" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>
	
	<div class="dnnFormItem">
    
        <dnn:label id="lblAgentRole" runat="server" controlname="ddlRoles" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlAgentRole" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>	

    	<div class="dnnFormItem">
    
        <dnn:label id="lblRentalOwnerRole" runat="server" controlname="ddlRentalOwnerRole" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlRentalOwnerRole" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>		


<div class="dnnFormItem">					
<dnn:label id="lblPropertyAddress" runat="server" suffix=":" controlname="ddlPropertyAddress" ResourceKey="lblPropertyAddress" />
			<asp:dropdownlist id="ddlPropertyAddress" Runat="server" datavaluefield="CustomFieldID" datatextfield="Name"></asp:dropdownlist></div>	


<div class="dnnFormItem">					
<dnn:label id="lblPropertyCity" runat="server" suffix=":" controlname="ddlPropertyCity" ResourceKey="lblPropertyCity" />
			<asp:dropdownlist id="ddlPropertyCity" Runat="server" datavaluefield="CustomFieldID" datatextfield="Name"></asp:dropdownlist></div>	
				
<div class="dnnFormItem">					
<dnn:label id="lblRentalAmount" runat="server" suffix=":" controlname="ddlRentalAmount" ResourceKey="lblRentalAmount" />
			<asp:dropdownlist id="ddlRentalAmount" Runat="server" datavaluefield="CustomFieldID" datatextfield="Name"></asp:dropdownlist></div>	        

<div class="dnnFormItem">
<dnn:label id="lblBedrooms" runat="server" suffix=":" controlname="ddlBedrooms" ResourceKey="lblBedrooms" />
			<asp:dropdownlist id="ddlBedrooms" Runat="server" datavaluefield="CustomFieldID" datatextfield="Name"></asp:dropdownlist></div>	
            

<div class="dnnFormItem">
<dnn:label id="lblBathrooms" runat="server" suffix=":" controlname="ddlBathrooms" ResourceKey="lblBathrooms" />
			<asp:dropdownlist id="ddlBathrooms" Runat="server" datavaluefield="CustomFieldID" datatextfield="Name"></asp:dropdownlist></div>			

     <div class="dnnFormItem">					
    <dnn:label id="lblurlReportServer" runat="server" suffix=":" controlname="txturlReportServer" ResourceKey="lblurlReportServer" />
         <asp:TextBox ID="txturlReportServer" runat="server" />		
    </div>
 
      <div class="dnnFormItem">					
    <dnn:label id="lblReportPath" runat="server" suffix=":" controlname="txtReportPath" ResourceKey="lblReportPath" />
         <asp:TextBox ID="txtReportPath" runat="server" />		
    </div>
 
      <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsUserName" runat="server" suffix=":" controlname="txtRSCredentialsUserName" ResourceKey="lblRSCredentialsUserName" />
         <asp:TextBox ID="txtRSCredentialsUserName" runat="server" />		
    </div>



     <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsPassword" runat="server" suffix=":" controlname="txtRSCredentialsPassword" ResourceKey="lblRSCredentialsPassword" />
         <asp:TextBox ID="txtRSCredentialsPassword" runat="server" />		
    </div>



     <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsDomain" runat="server" suffix=":" controlname="txtRSCredentialsDomain" ResourceKey="lblRSCredentialsDomain" />
         <asp:TextBox ID="txtRSCredentialsDomain" runat="server" />		
    </div>
 
 </div>
        			


  




    </fieldset>

</div>



	