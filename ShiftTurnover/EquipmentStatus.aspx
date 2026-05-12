<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="EquipmentStatus.aspx.cs" Inherits="ShiftTurnover.EquipmentStatus" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        
        <asp:TableRow>
              
            <asp:TableCell HorizontalAlign="left" Width="100%">
                <asp:Label runat="server" ID="lblMShift"  Font-Size="Medium" ForeColor="Black"      ></asp:Label>
                <iframe  class="iframe" width="100%" height="1000"  style="margin-top:40px;" 
				src="https://orfp-pivision1.ors.nih.gov/PIVision/#/Displays/22204/"></iframe>

 
                <iframe  class="iframe" width="100%" height="1000" 
                    style="margin-top:40px;" 
                   	src="https://dtr-dsaiab-application-4002182689698781.1.azure.databricksapps.com/"></iframe>

               
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

 
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
        <div id="footer" style="display:flex; justify-content:center; align-items:center; gap:12px; padding:10px 0; background:#333; color:#fff; min-height:60px; box-sizing:border-box;">
        <strong style="white-space:nowrap;">
            For anonymous Safety reporting, please scan the QR code:
        </strong>

        <a href="https://dtrdata.orf.od.nih.gov/sto/NearMissSecurity.aspx" target="_blank">
            <img src="Images/Barcode.png"
                 alt="QR Code"
                 style="width:60px; height:60px; display:block;" />
        </a>
    </div>
</asp:Content>
 