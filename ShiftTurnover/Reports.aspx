<%@ Page Title="Reports" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="ShiftTurnover.Reports" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="cph_instructions" runat="server">
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderID="cph_error" runat="server">
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblReports" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlReports">
                    <fieldset class="fs-border">
                        <legend class="fs-border">Compliance Reports</legend>
                            <ul>
                                <li > <a href="SubmissionReport.aspx"><strong>Shift Turnover Submissions Report</strong></a></li>
                            </ul>
                        <ul>
                                <li > <a href="ReportNotSubmitted.aspx"><strong>Shift Turnover Not Submissions Report</strong></a></li>
                            </ul>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>

