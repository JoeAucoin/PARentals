<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPARentals.ascx.cs" Inherits="GIBS.Modules.PARentals.ViewPARentals" %>


<style type='text/css'>
@media screen and (min-width: 47.5em ) {
 
  .columns-container {
      float: left;
  }
 
  .left-column 
  {
  	width: 280px;
  	margin-right: 20em;
    float: left;
  }
 
  .right-column {
    
    margin-left: -19.3em;
    float: left;
  }
}

	fieldset {
		margin:20px;
		padding:0 10px 10px;
		border:1px solid #666;
		border-radius:8px;
		box-shadow:0 0 10px #666;
	}
	legend {
		padding:2px 4px;
		background:#fff; /* For better legibility against the box-shadow */
		font-size:20px;
	}

	/* position:absolute */
	#position {
		position:relative;
		padding-top:20px;
		padding-left:10px;
	}
	#position > legend {
		position:absolute;
		top:-20px;
		right:auto;
		left:40px;
		width:auto;
	}

	/* float */
	#float {
		padding-top:10px;
	}
	#float > legend {
		float:left;
		margin-top:-20px;
	}
	#float > legend + * {
		clear:both;
	}

</style>


<div><asp:Label ID="lblDebug" runat="server" Text=""></asp:Label></div>





 <div style="text-align:center;">
                <asp:Label ID="lblYear" runat="server" Text="" /></div>

<asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_OnItemDataBound">

    <ItemTemplate>


  	<div class="columns-container">

	  	<div class="left-column">
	  	
        <div>
            <asp:HyperLink ID="HyperLinkEditProp" Target="_blank" runat="server"><asp:image ID="imgEdit" runat="server" imageurl="~/DesktopModules/GIBS/PARentals/Images/icon_edit.png" AlternateText="Edit Record" /></asp:HyperLink>
        <strong><asp:HyperLink ID="HyperLink1" runat="server">HyperLink</asp:HyperLink></strong></div>
        
        <div style="padding-left:25px;"><%# Eval("Bedrooms")%> Bedrooms - InSeason: $<%# Eval("RentalAmount").ToString()%></div>

	  	</div>

	  	<div class="right-column">
	  		

        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
	  	</div>

  	</div>




        <hr />
        
    </ItemTemplate>

</asp:Repeater>


<div style="clear:both;" />

<div style="width:260px;">
<fieldset id="position">
<legend>Legend</legend>
    <img src="/DesktopModules/GIBS/PARentals/Images/icon_Gray.png" alt="Not Yet Available" style="padding-right:10px;" />Not Yet Available<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/bgreen.gif" alt="Available" style="padding-right:10px;" />Available<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/Lease-Need.png" alt="Pending Lease" style="padding-right:10px;" />Pending Lease<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/Lease-Tenant.png" alt="Lease Sent to Tenant" style="padding-right:10px;" />Lease Sent to Tenant<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/Lease-Owner.png" alt="Lease Sent to Owner" style="padding-right:10px;" />Lease Sent to Owner<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/icon_GreenCheck.png" alt="Complete" style="padding-left:2px;padding-right:13px;" />Complete
   <hr />
    <img src="/DesktopModules/GIBS/PARentals/Images/bredo.gif" alt="Owner Occupied" style="padding-right:10px;" />Owner Occupied<br />
    <img src="/DesktopModules/GIBS/PARentals/Images/icon_RedX.png" alt="Rented by Another Agency" style="padding-right:10px;" />Rented by Another Agency

</fieldset>
</div>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Visible="false">
<Columns>
<asp:BoundField DataField="PropertyID" HeaderText="PropertyID" />
<asp:BoundField DataField="PropertyAddress" HeaderText="Address" />
<asp:BoundField DataField="PropertyCity" HeaderText="City" />
<asp:BoundField DataField="Bedrooms" HeaderText="Bedrooms" />
<asp:BoundField DataField="RentalAmount" HeaderText="RentalAmount" />

</Columns>
</asp:GridView>
