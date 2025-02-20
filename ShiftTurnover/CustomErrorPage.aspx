<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="CustomErrorPage.aspx.cs" Inherits="ShiftTurnover.CustomErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_banner" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cph_title" runat="server">ERROR
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cph_main" runat="server">

    <asp:Table ID="tblAlert" runat="server" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <ul class="alert">
							<asp:Label id="lblErrorMsg" runat="server"></asp:Label>
                </ul>
            </asp:TableCell>
        </asp:TableRow>

    </asp:Table>


</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
