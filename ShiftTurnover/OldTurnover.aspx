<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="OldTurnover.aspx.cs" Inherits="ShiftTurnover.OldTurnover" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" id="bootstrap-css" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <style>
        @media print {
            .myDivToPrint {
                background-color: white;
                height: 100%;
                width: 100%;
                position: fixed;
                top: 0;
                left: 0;
                margin: 0;
                padding: 15px;
                font-size: 14px;
                line-height: 18px;
            }
        }
        .notification {
            position: relative;
            display: inline-block;
        }

        .notification .badge {
            position: absolute;
            top: -6px;
            right: -8px;
            border-radius: 50%;
        }
    </style>
      <style type="text/css">
        /* unvisited link */
        a:link {
            color: white;
        }

        a:visited {
            color: red;
        }
    </style>

    <script type="text/javascript">
        google.charts.load('current', {'packages':['timeline']});
        //google.charts.setOnLoadCallback(drawChart);
    </script>

    <script type="text/javascript">
         var cwin = '';
        function PopupReco(livelogid,shiftID) {
            if (!cwin.closed && cwin.location) {
                cwin.focus();
            } else {
                cwin = window.open("ShowChart.aspx?LID=" + livelogid + "&SID=" + shiftID , "ShowChart",
                    "width=800,height=600,directories=no,location=no,"
                    + "menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");

                cwin.moveTo(150, 150);
            }
        }
        $(document).ready(function () {
            var Date = $("#<%= txtDate.ClientID %>").val();
            var Period = $("#<%= ddlPeriod.ClientID %>").val();
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetChillerData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {
                        drawChillerChart(response.d);
                    }
            });

            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetChillerPumpData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {

                        drawChillerPumpChart(response.d);


                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetBlrData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {

                        drawBlrChart(response.d);


                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetFreeCoData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {

                        drawFreeCoChart(response.d);


                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetROPumpData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {

                        drawROPumpChart(response.d);


                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'OldTurnover.aspx/GetCTData',
                data: '{"Date":"' + Date + '","Period":"' + Period + '"}',
                success:
                    function (response) {

                        drawCTChart(response.d);


                    }
            });
        })
        function drawROPumpChart(dataValues) {

            var container = document.getElementById('ROPumpChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }



            var options = {
                "title": "Chiller PUMP Statuses",
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
            };
            chart.draw(dataTable, options);
        }
        function drawBlrChart(dataValues) {

            var container = document.getElementById('BlrChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller PUMP Statuses",
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },

            };
            chart.draw(dataTable, options);
        }
        function drawFreeCoChart(dataValues) {

            var container = document.getElementById('FreeCoChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller PUMP Statuses",
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },

            };
            chart.draw(dataTable, options);
        }
        function drawCTChart(dataValues) {

            var container = document.getElementById('CTChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller PUMP Statuses",
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },

            };
            chart.draw(dataTable, options);
        }
        function drawChillerChart(dataValues) {

            var container = document.getElementById('CHLChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller Statuses",
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },

            };
            chart.draw(dataTable, options);
        }
        function drawChillerPumpChart(dataValues) {

            var container = document.getElementById('CHLPumpChart');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller PUMP Statuses",
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },

            };
            chart.draw(dataTable, options);
        }

    </script>

    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />

</asp:Content>


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">

    <div class="row" style="margin-top: 20px; margin-left: 20px; margin-bottom: 20px; border: solid 0px black;">
        <asp:Table ID="Table16" runat="server" CssClass="wiznavbuttons" CellPadding="10" CellSpacing="10" BackColor="#99ccff" HorizontalAlign="Center" Width="800">
            <asp:TableRow>
                <asp:TableCell Width="150px">&nbsp;</asp:TableCell>
                <asp:TableCell>
                    <label style="font-size: large; left: 20px;">Select Date:</label><br />
                    <asp:TextBox ID="txtDate" TabIndex="6" runat="server" Height="45px"
                        Width="250px" Font-Bold="true" Font-Size="Large" TextMode="Date" CssClass="form-control"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                    <label style="font-size: large; left: 20px;">Select Period:</label><br />
                    <asp:DropDownList ID="ddlPeriod" TabIndex="2" runat="server" Height="45px" Font-Bold="true" Width="200px" Font-Size="Large" CssClass="form-control">
                        <asp:ListItem Text="  Day " Value="D">

                        </asp:ListItem>
                        <asp:ListItem Text="  Night  " Value="N">

                        </asp:ListItem>
                    </asp:DropDownList>

                </asp:TableCell>
                <asp:TableCell Width="150px">&nbsp;</asp:TableCell>

            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <div class="row" style="margin-top: 10px; margin-left: 60px; border: solid 0px black;">
        <asp:Table ID="Table10" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" HorizontalAlign="Center" Width="800">

            <asp:TableRow Style="margin-left: 15px;">
                <asp:TableCell>
                    <asp:Button ID="btnLoadReport" runat="server" Text="Show Report" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="170px" ToolTip=" Load Report " OnClick="btnLoadReport_Click" />
                    </asp:TableCell>

                <asp:TableCell>
                    <asp:Button ID="btnPrintPDF" runat="server" Text="Reset Report" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="170px" ToolTip=" Reset Report " OnClick="btnResetReport_Click" />

                </asp:TableCell>
                <asp:TableCell Style="margin-left: 15px;">
                    <asp:Button ID="btnResetReport" runat="server" Text="Print PDF" CssClass="btn" BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="170px" ToolTip=" PRINT PDF " OnClick="btnResetReport_Click1" />

                </asp:TableCell>
                <asp:TableCell Style="margin-left: 15px;">

                    <asp:Button ID="btnNotSubmitted" runat="server" Text="Show Not Submitted Report" CssClass="btn"  Visible="false"
                        BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Show All Not Submitted Report "
                        OnClick="btnNotSubmitted_Click" />

                </asp:TableCell>

            </asp:TableRow>

        </asp:Table>
    </div>
    <div class="row" style="margin-top: 10px; border: solid 0px black;">
        <asp:Table ID="Table9" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Center">
                    <h3>
                        <asp:Label ID="lblErrorMsg" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
                    </h3>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <asp:Panel ID="pnlRpt" runat="server" Visible="false">

        <div class="row" style="margin-top: 0px; border: solid 0px black;">
            <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="1" HorizontalAlign="Center" Width="760">

                <asp:TableHeaderRow CssClass="columnhead">
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">&nbsp;Shift</asp:TableHeaderCell>
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">&nbsp;Created </asp:TableHeaderCell>
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">&nbsp;Submitted </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="lblMShift" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="lblCreatedShift" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="lblSubmittedShift" Font-Bold="true"
                            Font-Size="Medium"></asp:Label>
                         <asp:Button ID="btnSubmitNow" runat="server" Text=" Submit Now  " CssClass="btn"  Visible="false"
                        BackColor="#b3112c" Font-Bold="true" ForeColor="White" Width="220px" ToolTip=" Show All Not Submitted Report "
                        OnClick="btnSubmitNow_Click" />
                    </asp:TableCell>
                </asp:TableRow>

            </asp:Table>
        </div>
        <div class="container">
            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Shift Workers</legend>
                                <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="1" HorizontalAlign="Center" Width="760">

                                    <asp:TableHeaderRow CssClass="columnhead">
                                        <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">&nbsp;Position</asp:TableHeaderCell>

                                        <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">&nbsp;Name</asp:TableHeaderCell>

                                    </asp:TableHeaderRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Shift Supervisor: 
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblShiftSup" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Crew Leader: 
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblCrewChief" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Chiller Operator:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblChillerOperator" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Boiler Operator:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblBoilerOperator" runat="server"></asp:Label>
                                        </asp:TableCell>

                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Auxiliary  Operator:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblAuxOperator" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Cogen Operator:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblCogenOperator" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Electrician:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblElectrician" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell>
                                                                                Additional Comments:
                                                                            </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Label ID="lblShiftWorkersComment" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div class="row" style="margin-top: 0px; border: solid 0px black;">
                <asp:Table ID="Table23" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="1" HorizontalAlign="Center" Width="760">
                    <asp:TableRow>
                        <asp:TableCell>
                            <strong>Previous Shift's Live Log Entries was acknowledged?
                        <asp:Label runat="server" ID="lblPrevAch" Font-Size="Medium"></asp:Label>
                            </strong>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="tblPastEvents" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Live Log Entries</legend>
                                <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblPast" runat="server">There are no live log entries for this shift.</asp:Label>
                                            <asp:Panel ID="pnlPast" runat="server">
                                                <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                                BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                                <HeaderStyle CssClass="columnhead" HorizontalAlign="Center"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Deleted" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                              <span class="buttonColumn">
                                                                     <asp:ImageButton ID="btnView" runat="server" CommandName="View" ImageUrl="~/Images/actionbutton_view_off.gif"
                                                                        AlternateText="View This Entry" CausesValidation="false"></asp:ImageButton>
                                                                </span>
                                                                            <asp:Label ID="lblDeleted" runat="server" Visible="false">Deleted</asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                            </asp:DataGrid>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                                <asp:Table ID="tblActive" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        Uploaded Documents
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <asp:DataGrid ID="GrActive" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                            BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                            <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Actions">
                                                                    <ItemTemplate>
                                                                        <span class="buttonColumn">
                                                                            <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="View" ImageUrl="Images/actionbutton_view_off.gif"
                                                                                AlternateText="View This Document"></asp:ImageButton>
                                                                        </span>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                            </asp:Panel>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <!--
            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="Table6" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Maintenance Work Orders</legend>
                                <asp:Table ID="Table17" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblWO" runat="server" ForeColor="Green">There are no Maintenance Work Orders for this shift.</asp:Label>
                                            <asp:Panel ID="pnlWO" runat="server">
                                                <asp:Table ID="tblWO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell ColumnSpan="4">
                                                            <asp:DataGrid ID="grdWO" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                                BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                                <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                            </asp:DataGrid>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                            <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell Wrap="true">
                                                        <strong>All work orders completed or closed during the current shift are shown here.
                                                            <br />
                                                            Are all the required Maximo work orders completed?</strong>
                                                        <asp:Label ID="lblWOComplete" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                        <strong>Are the work orders shown in Maximo correct?</strong>
                                                        <asp:Label ID="lblWOCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                                    </asp:TableCell>

                                                </asp:TableRow>
                                            </asp:Table>

                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="Table18" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Scheduled  Work Orders</legend>
                                <asp:Table ID="Table19" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblSche" runat="server" ForeColor="Green">There are no Scheduled  Work Orders for this shift.</asp:Label>
                                            <asp:Panel ID="pnlSche" runat="server">
                                                <asp:Table ID="tblSche" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell ColumnSpan="2">
                                                            <asp:DataGrid ID="grdSche" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                                BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                                <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                            </asp:DataGrid>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                            <asp:Table ID="Table4" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell Wrap="true">
                                                        <strong>The work orders shown here are scheduled to be completed during the next shift, except water chemistry.
                                                      Are all the required maintenance work orders correct? </strong>
                                                        <asp:Label ID="lblWOOther" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                                    </asp:TableCell>

                                                </asp:TableRow>
                                            </asp:Table>

                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            -->

            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="Table8" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Critical Alarms</legend>
                               <asp:Table ID="Table21" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                <asp:TableRow>
                                    <asp:TableCell>
    
                                            <asp:Panel ID="pnlCAlm" runat="server">
                                                <asp:Table ID="tblCAlm" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                     <asp:TableRow>
                                                        <asp:TableCell  ColumnSpan="4" Font-Bold="true" ForeColor="Green">
                                                            Click to see the Top View Alarms of the selected system in a seperate window. 
                                                            </asp:TableCell>
                                                           <asp:TableCell  >
                                                            </asp:TableCell>
                                                           <asp:TableCell  >
                                                            </asp:TableCell>
                                                           <asp:TableCell  >
                                                            </asp:TableCell>
                                                         </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left"  Width="25%"  >
                                                              <a id="TopViewAlarmAux" runat="server" class="btn btn-primary"  style="background-color:lightblue:white;  font-size:x-large; font-weight:bold;"  target="_blank" title="Click here to view Auxiliary TopView Alarms in seperate window">
                                                         <span class="notification">
                                                            Auxiliary    <img height="36" width="32" alt="bell" src="Images/bell_icon.gif" />
                                                                <span class="position-absolute translate-middle badge rounded-pill bg-danger">
                                                                    <asp:Label ID="AuxAlarmCount" runat="server" ForeColor="white" Font-Italic="true"></asp:Label>
                                                                </span>
                                                            </span>
                                                        </a>
                                                           
                                                        </asp:TableCell>
                                                         
                                                          <asp:TableCell HorizontalAlign="Left" Width="25%">
                                                              <a id="TopViewAlarmB" runat="server"  class="btn btn-primary"  style="background-color:lightblue:white; font-size:x-large; font-weight:bold;" target="_blank" title="Click here to View Boiler ToView alarms in seperate window">
                                                     Boiler   <span class="notification">
                                                                <img height="36" width="32" alt="bell" src="Images/bell_icon.gif" />
                                                                <span class="position-absolute translate-middle badge rounded-pill bg-danger">
                                                                    <asp:Label ID="BoilerAlarmCount" runat="server" ForeColor="white" Font-Italic="true"></asp:Label>
                                                                </span>
                                                            </span> 
                                                        </a>
                                                                                                                     
                                                        </asp:TableCell>
                                                          <asp:TableCell HorizontalAlign="Left" Width="25%" >
                                                              <a id="TopViewAlarmC" runat="server"  class="btn btn-primary"  style="background-color:lightblue:white;  font-size:x-large; font-weight:bold;" target="_blank" title="Click here to view all Chiller TopVIew Alarms in seperate window">
                                                       <span class="notification">
                                                          Chiller      <img height="36" width="32" alt="bell" src="Images/bell_icon.gif" />
                                                                <span class="position-absolute translate-middle badge rounded-pill bg-danger">
                                                                    <asp:Label ID="ChillerAlarmCount" runat="server" ForeColor="white" Font-Italic="true"></asp:Label>
                                                                </span>
                                                            </span> 
                                                        </a>
                                                                                                         
                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" Width="25%">
                                                              <a id="TopViewAlarmWT" runat="server"  class="btn btn-primary"  style="background-color:lightblue:white;  font-size:x-large; font-weight:bold;" target="_blank" title="Click here to view Water Treatment TopView in seperate window">
                                                        <span class="notification">
                                                           Water Treatment     <img height="36" width="32" alt="bell" src="Images/bell_icon.gif" />
                                                                <span class="position-absolute translate-middle badge rounded-pill bg-danger">
                                                                    <asp:Label ID="WTAlarmCount" runat="server" ForeColor="white" Font-Italic="true"></asp:Label>
                                                                </span>
                                                            </span>       
                                                        </a>
                                                                                                                                          
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </asp:Panel>
                                            <asp:Table ID="Table7" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <strong>Are all critical alarms captured?</strong>
                                                        <asp:Label ID="lblCriAlsCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />

                                                        <strong>Comments:</strong><br />
                                                        <asp:Label ID="lblCriAlsComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                                    </asp:TableCell>

                                                </asp:TableRow>
                                            </asp:Table>

                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

            <%--<div class="row" style="margin-top: 20px; border: solid 0px black;">
                <asp:Table ID="Table20" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
                    <asp:TableRow>
                        <asp:TableCell>
                            <a id="hrCheckAll" runat="server" target="_blank" title="Click here to enlarge all charts in seperate window">
                                <h4>Click here to enlarge all the charts in seperate window   
                          <img src="Images/retro-tv-icon.jpg" style="width: 50px; height: 40px" />
                                </h4>
                            </a>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="tblStatus" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlCHLStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>Chillers Status </strong>:
                                        <br />
                                        Instructions:   Verify that the chiller statuses shown are correct.
                                    </legend>
                                    <asp:Table ID="tblCHLStatus" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>

                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                  <div id="CHLChart"  style="width:780px; height:600px;" ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblChillersCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblChillersComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Was Optimization Plan Followed?</strong>
                                                <asp:Label ID="lblChlOptYN" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblChlOptComments" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>

                                </fieldset>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlChrPmpStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>Chiller Pumps Status </strong>: 
                                    </legend>

                                    <asp:Table ID="Table11" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>
                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                   <div id="CHLPumpChart" style="width:780px; height:1150px;" ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblChillersPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblChillersPmpComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>

                                        </asp:TableRow>
                                    </asp:Table>

                                </fieldset>

                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlCTStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>Cooling Towers Status </strong>:  
                                    </legend>

                                    <asp:Table ID="Table12" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>
                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                   <div id="CTChart" style="width:780px; height:1150px;"  ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblCTCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblCTComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>

                                        </asp:TableRow>
                                    </asp:Table>

                                </fieldset>

                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlFreeCoStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>Free Cooling And Pumps Status </strong>:  
                                    </legend>

                                    <asp:Table ID="Table13" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>


                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                    <div id="FreeCoChart" style="width:780px; height:400px;"  ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblFreeCoCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblFreeCoComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>

                                        </asp:TableRow>
                                    </asp:Table>

                                </fieldset>

                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlBlrStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>Boilers Status </strong>: 
                                    </legend>

                                    <asp:Table ID="Table14" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>

                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                   <div id="BlrChart" style="width:780px; height:540px;" ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblBoilersCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblBoilersComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>

                                </fieldset>

                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Panel runat="server" ID="pnlROPumpStatus">
                                <fieldset class="fs-border">
                                    <legend class="fs-border">
                                        <strong>RO and Pumps Status </strong>:  
                                    </legend>
                                    <asp:Table ID="Table15" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                                        <asp:TableRow>

                                            <asp:TableCell VerticalAlign="top" ColumnSpan="2">
                                  <div id="ROPumpChart" style="width:780px; height:540px;" ></div>
                                            </asp:TableCell>
                                        </asp:TableRow>

                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <strong>Is the data correct?</strong>
                                                <asp:Label ID="lblBlrPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label><br />
                                                <strong>Comments:</strong><br />
                                                <asp:Label ID="lblBlrPmpComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </fieldset>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

            </div>--%>
        </div>
    </asp:Panel>

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