<%@ Page Title="Shift Turnover Submissions Report" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="SubmissionReport.aspx.cs" Inherits="ShiftTurnover.SubmissionReport" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ChkBoxClass input {
            width: 25px;
            height: 25px;
        }
    </style>

    <link href="Content/jquery.dataTables.css" rel="stylesheet" />

    <script>

        $(document).ready(function () {

            $('#results').DataTable({

                "aaSorting": [[0, "asc"]],
                "aLengthMenu": [[50, 100, 500, 1000, -1], [50, 100, 500, 1000, "All"]],
                "iDisplayLength": 50,
                "pagingType": "full_numbers",
                "sDom": '<"top"<"actions">lfpi<"clear">><"clear">rt<"bottom">',
                "oLanguage": {
                    "sEmptyTable": "No Results !!",
                    "sSearch": "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Filter: "
                }
            });

        });

    </script>

    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

<asp:Content ID="Content10" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" Width="50%">
                <asp:Literal runat="server" ID="litBack"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell Visible="false" HorizontalAlign="Right" Width="50%">
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="cph_instructions" runat="server">
</asp:Content>
<asp:Content ID="Content12" ContentPlaceHolderID="cph_error" runat="server">
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblSelect" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BorderWidth="0" BackColor="#cee7ff" HorizontalAlign="Center" Width="1200px">

        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
            <asp:TableCell Height="50px">
                <label style="font-size: large; left: 20px;">Start Date:</label>
                <asp:TextBox ID="txtStartDate" TabIndex="6" runat="server" Width="250px" Font-Bold="true" Font-Size="Large" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell Height="50px">
                <label style="font-size: large; left: 20px;">End Date:</label>
                <asp:TextBox ID="txtEndDate" TabIndex="6" runat="server" Width="250px" Font-Bold="true" Font-Size="Large" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Bottom">
                <label style="font-size: large; left: 20px;"></label>
                <asp:Button ID="btnSubmit" runat="server" Text="Run Report" CausesValidation="true" ValidationGroup="Date" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Run Report " OnClick="btnSubmit_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
            <asp:TableCell>
                <asp:CompareValidator ID="CompareValidator1" ValidationGroup="Date" ForeColor="Red" runat="server" Font-Size="Medium"
                    ControlToValidate="txtStartDate" ControlToCompare="txtEndDate" Operator="LessThan" Type="Date"
                    ErrorMessage="Start Date must be less than End Date.<br />" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator2" ValidationGroup="Date" ForeColor="Red" runat="server" Font-Size="Medium"
                    ControlToValidate="txtStartDate" Operator="LessThanEqual" Type="Date"
                    ErrorMessage="Start Date must be less than or equal to today.<br />" Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator3" ValidationGroup="Date" ForeColor="Red" runat="server" Font-Size="Medium"
                    ControlToValidate="txtEndDate" Operator="LessThanEqual" Type="Date"
                    ErrorMessage="End Date must be less than or equal to today.<br />" Display="Dynamic"></asp:CompareValidator>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblSubmitButtons" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BorderWidth="0" HorizontalAlign="Center" Width="1200">
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0" HorizontalAlign="Center">
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblReport" runat="server" CssClass="wiznavbuttonsPrint" Font-Size="Medium" CellPadding="10" CellSpacing="10" BorderWidth="0" Width="1200" HorizontalAlign="Center">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlResult" Visible="false">
                    <asp:Table ID="Table5" runat="server" BackColor="#ffffdd" BorderWidth="0" Width="100%">
                        <asp:TableRow BorderWidth="0">
                            <asp:TableCell HorizontalAlign="Left">
                                <label style="font-size: large; left: 20px;">Date Range:</label>
                                <asp:Label ID="lblDateRange" runat="server" Text=" " Font-Size="Medium" Font-Bold="true" ForeColor="Green"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Right">
                                <label style="font-size: large; right: 20px;"></label>
                                <asp:Button ID="btnExport" runat="server" Text="Export Results to Excel" CausesValidation="true" ValidationGroup="Date" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Export to Excel " OnClick="btnExport_Click" />

                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Total number of Results  = 
                                    <asp:Label ID="lblCount" runat="server" Text=" " Font-Size="Medium" Font-Bold="true" ForeColor="Black"></asp:Label>
                                </legend>

                                <asp:Table ID="Table3" runat="server" BorderWidth="0" Width="100%">
                                    <asp:TableRow BorderWidth="0">
                                        <asp:TableCell HorizontalAlign="Center">
                                            <div id="netTable" runat="server" style="border: 0px solid black;"></div>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderID="cph_footer" runat="server">
 
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
 
 
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/jquery.dataTables.min.js"></script>
</asp:Content>

