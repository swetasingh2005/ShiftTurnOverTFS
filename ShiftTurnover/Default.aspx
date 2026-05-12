<%@ Page Title="Main Page" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ShiftTurnover.Default" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cph_banner" runat="server">
    </asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Panel id="pnlLogin" runat="server">

				<p>
                    <asp:Table ID="tblLogin" runat="server" HorizontalAlign="Center" Width="400px">
                        <asp:TableRow>
                            <asp:TableCell>
                                User ID:
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox id="txtUserID" runat="server" Columns="20" Width="160px"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                Password:
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox id="txtPassword" runat="server" TextMode="Password" Columns="20" Width="160px"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Button id="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click"></asp:Button>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </p>
				<p>&nbsp;</p>
				
                
				
			</asp:Panel>
			<asp:Label id="lblErrMsg" runat="server" Visible="False"></asp:Label>
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
