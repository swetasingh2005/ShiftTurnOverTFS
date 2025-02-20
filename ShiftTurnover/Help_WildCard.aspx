<%@ Page Title="Using WildCard Characters in Search" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="Help_WildCard.aspx.cs" Inherits="ShiftTurnover.Help_WildCard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title><%: Page.Title %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Label ID="lblHeader" runat="server"><h1><%=Title %></h1></asp:Label>
    <p>&nbsp;</p>
    <asp:Table ID="tblWildCards" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BorderStyle="Solid" GridLines="Both" BorderWidth="1" Width="600px" HorizontalAlign="Center">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>Symbol</asp:TableHeaderCell>
            <asp:TableHeaderCell>Description</asp:TableHeaderCell>
            <asp:TableHeaderCell>Example</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell Font-Bold="true" Font-Size="Large" ForeColor="Navy" HorizontalAlign="Center">%</asp:TableCell><asp:TableCell>Represents zero or more characters</asp:TableCell><asp:TableCell Wrap="false">val% finds valve, value, and valid</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Font-Bold="true" Font-Size="Large" ForeColor="Navy" HorizontalAlign="Center">_</asp:TableCell><asp:TableCell>Represents a single character</asp:TableCell><asp:TableCell Wrap="false">h_t finds hot, hat, and hit</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Font-Bold="true" Font-Size="Large" ForeColor="Navy" HorizontalAlign="Center">[ ]</asp:TableCell><asp:TableCell>Represents any single character within the brackets</asp:TableCell><asp:TableCell Wrap="false">h[oa]t finds hot and hat, but not hit</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Font-Bold="true" Font-Size="Large" ForeColor="Navy" HorizontalAlign="Center">^</asp:TableCell><asp:TableCell>Represents any character not in the brackets</asp:TableCell><asp:TableCell Wrap="false">h[^oa]t finds hit, but not hot and hat</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Font-Bold="true" Font-Size="Large" ForeColor="Navy" HorizontalAlign="Center">-</asp:TableCell><asp:TableCell>Represents a range of characters</asp:TableCell><asp:TableCell Wrap="false">c[a-b]t finds cat and cbt</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <p><asp:Button ID="btnPrint" runat="server" Text="Print this page" OnClientClick="javascript:window.print()" TabIndex="1" /></p>
    <div class="closewindow"><a href="javascript:window.close();" tabindex="2">Close this window</a></div>

</asp:Content>
