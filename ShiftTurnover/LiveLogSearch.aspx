<%@ Page Title="Shift Turnover Logs Search " Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="LiveLogSearch.aspx.cs" Inherits="ShiftTurnover.LiveLogSearch" %>

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

            $('#example').DataTable({

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
    <script type="text/javascript">
        var cwin = '';
        function PopupWildCardHelp() {
            if (!cwin.closed && cwin.location) {
                cwin.focus();
            } else {
                cwin = window.open("Help_WildCard.aspx", "wildcardhelppopup",
                    "width=900,height=600,directories=no,location=no,"
                    + "menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");

                cwin.moveTo(150, 150);
            }
          }
          </script>

</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">

    <asp:Table ID="Table16" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BorderWidth="0" BackColor="#cee7ff"
        HorizontalAlign="Center" Width="1200">

        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
            <asp:TableCell Height="50px">

                <label style="font-size: large; left: 20px;"> Log Type:</label>
                <asp:DropDownList ID="ddlLogType" TabIndex="2" Width="180px" runat="server" CssClass="form-control">
                    
                    <asp:ListItem Text="Live Log" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Engineering Log" Value="1"></asp:ListItem>
                    <asp:ListItem Text="HVG Log" Value="2" ></asp:ListItem>
                    <asp:ListItem Text="Fuel Log" Value="3" ></asp:ListItem>
                    <asp:ListItem Text="Water Treatment Log" Value="4" ></asp:ListItem>
                   <asp:ListItem Text="Security  Log" Value="5" ></asp:ListItem>
                    </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell Height="50px">

                <label style="font-size: large; left: 20px;">Start Date:</label>
                <asp:TextBox ID="txtSDate" TabIndex="6" runat="server"
                    Width="250px" Font-Bold="true" Font-Size="Large" TextMode="Date" CssClass="form-control"></asp:TextBox>

            </asp:TableCell>
            <asp:TableCell Height="50px">
                <label style="font-size: large; left: 20px;">End Date:</label>
                <asp:TextBox ID="txtEDate" TabIndex="6" runat="server"
                    Width="250px" Font-Bold="true" Font-Size="Large" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell Height="50px">
                <label style="font-size: large; left: 20px;">Reporter:</label>
                <asp:DropDownList ID="ddlReporter" TabIndex="2" Width="250px" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell Height="50px">
                <label style="font-size: large; left: 20px;">Service Request:</label>
                <asp:DropDownList ID="ddlSR" TabIndex="2" Width="150px" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                </asp:DropDownList>

            </asp:TableCell>

        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" BackColor="#cee7ff"
        HorizontalAlign="Center" Width="1200">
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
            <asp:TableCell Height="40px">
                <asp:CompareValidator ID="CompareValidator1" ValidationGroup="Date" ForeColor="Red" runat="server" Font-Size="Medium"
                    ControlToValidate="txtSDate" ControlToCompare="txtEDate" Operator="LessThan" Type="Date"
                    ErrorMessage="Start date must be less than End date."></asp:CompareValidator>
            </asp:TableCell>
            <asp:TableCell Height="40px"></asp:TableCell>
            <asp:TableCell Height="40px">
                <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Height="40px"></asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="Table4" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" BackColor="#cee7ff" HorizontalAlign="Center" Width="1200">
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0" HorizontalAlign="Center">
            <asp:TableCell>
                <asp:Table ID="Table6" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" BackColor="#cee7ff" HorizontalAlign="Center" Width="800">
                    <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0" HorizontalAlign="Center">
                        <asp:TableCell>
                            <label style="font-size: large; left: 20px;">Search Keyword 1:</label>
                            <asp:TextBox ID="txtSearch1" TabIndex="6" runat="server" Height="45px"
                                Width="200px" Font-Bold="true" Font-Size="Large" CssClass="form-control">
                            </asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell>
                            <label style="font-size: large; left: 20px;">Condition:</label>
                            <asp:DropDownList ID="ddlSearch1" TabIndex="2" Width="100px" runat="server" CssClass="form-control"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSearch1_SelectedIndexChanged">
                                <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="1"></asp:ListItem>
                                <asp:ListItem Text="AND" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblCondition" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell>
                            <label style="font-size: large; left: 20px;">Search Keyword 2:</label>
                            <asp:TextBox ID="txtSearch2" TabIndex="6" runat="server" Height="45px"
                                Width="200px" Font-Bold="true" Font-Size="Large" CssClass="form-control">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="txtSearch2"
                                Enabled="false" ErrorMessage="Required" ID="rfvSearch2" runat="server">
                            </asp:RequiredFieldValidator>
                        </asp:TableCell>
                        

                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Bottom"><asp:LinkButton runat="server" CausesValidation="false" OnClientClick="return PopupWildCardHelp()" Font-Bold="true" Font-Size="Small" CssClass="wiznavbuttons" Text="[Help Using WildCard Characters in Search]" ToolTip=" Help Using Wild Characters " /></asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="Table2" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BorderWidth="0" HorizontalAlign="Center" Width="1200">
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0" HorizontalAlign="Center">

            <asp:TableCell>
                <label style="font-size: large; left: 20px;"></label>
                <asp:Button ID="btnSubmit" runat="server" Text="Get Search Results" CausesValidation="true" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Get Search Results " OnClick="btnSubmit_Click" />

            </asp:TableCell>
            <asp:TableCell>
                <label style="font-size: large; left: 20px;"></label>
                <asp:Button ID="btnReset" runat="server" Text="Reset Search Results" CausesValidation="false" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Reset Search Results " OnClick="btnReset_Click" />

            </asp:TableCell>
            <asp:TableCell>
                <label style="font-size: large; right: 20px;"></label>
                <asp:Button ID="btnExport" runat="server" Text="Export Results to Excel" CausesValidation="false" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Export to Excel " OnClick="btnExport_Click" />

            </asp:TableCell>

        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="tblReport" runat="server" CssClass="wiznavbuttonsPrint" Font-Size="Medium" CellPadding="10" CellSpacing="10" BorderWidth="0" Width="1200"
        HorizontalAlign="Center">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlResult" Visible="false">
                    <asp:Table ID="Table5" runat="server" BackColor="#ffffdd" BorderWidth="0" Width="100%">
                        <asp:TableRow BorderWidth="0">
                            <asp:TableCell HorizontalAlign="Center">
                                <label style="font-size: large; left: 20px;">Reporter:</label>
                                <asp:Label ID="lblReporter" runat="server" Text=" " Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Green"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Center">
                                <label style="font-size: large; left: 20px;">Date Range:</label>
                                <asp:Label ID="lblDateRange" runat="server" Text=" " Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Green"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Center">
                                <label style="font-size: large; left: 20px;">Service Request:</label>
                                <asp:Label ID="lblSR" runat="server" Text=" " Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Green"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="Center">
                                <label style="font-size: large; left: 20px;">Keyword:</label>
                                <asp:Label ID="lblKeywordlist" runat="server" Text=" " Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Green"></asp:Label>
                            </asp:TableCell>

                        </asp:TableRow>
                    </asp:Table>
                    <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="0">
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Total number of Result  = 
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
 
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/jquery.dataTables.min.js"></script>
</asp:Content>


