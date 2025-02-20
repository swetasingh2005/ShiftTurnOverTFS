<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_Review.aspx.cs" Inherits="ShiftTurnover.NewTurnover_Review" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </style>

    <script type="text/javascript">
        function confirmBox() {
            return window.confirm("Are you sure you want to SUBMIT this Shift Turnover Report? You will not be able to make any changes after submitting!");
        }

    </script>

    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['timeline'] });
        //google.charts.setOnLoadCallback(drawChart);
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetChillerData',
                data: '{}',
                success:
                    function (response) {
                        drawChillerChart(response.d);
                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetChillerPumpData',
                data: '{}',
                success:
                    function (response) {
                        drawChillerPumpChart(response.d);
                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetBlrData',
                data: '{}',
                success:
                    function (response) {
                        drawBlrChart(response.d);
                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetFreeCoData',
                data: '{}',
                success:
                    function (response) {
                        drawFreeCoChart(response.d);
                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetROPumpData',
                data: '{}',
                success:
                    function (response) {
                        drawROPumpChart(response.d);
                    }
            });
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'NewTurnover_Review.aspx/GetCTData',
                data: '{}',
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

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333', '#eb4034', '#eb4034'],

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
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],
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
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],

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
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],

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
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],

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
                colors: ['#A6CD4E', '#DAAC57', '#EA4E5B', '0000ff', '#333333'],

            };
            chart.draw(dataTable, options);
        }
    </script>

    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />

</asp:Content>


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">




    <div class="container">
        <div class="row" style="margin-top: 20px; border: solid 0px black;">
            <asp:Table ID="Table5" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <h1 style="color: #9A0000; font-weight: bold;">Shift Turnover Report
                                                <br />
                            <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label>
                        </h1>
                    </asp:TableCell>

                </asp:TableRow>

            </asp:Table>
        </div>
        <div class="row" style="margin-top: 0px; border: solid 0px black;">
            <asp:Table ID="Table16" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            <legend class="fs-border">Acknowledgement</legend>
                            <asp:Table ID="Table23" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                <asp:TableRow>
                                    <asp:TableCell>

                                        <strong>Previous Shift's Live Log Entries Acknowledged?
                        <asp:Label runat="server" ID="lblPrevAch" Font-Size="Medium"></asp:Label>
                                        </strong>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </fieldset>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="row" style="margin-top: 20px; border: solid 0px black;">
            <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            <legend class="fs-border">Shift Workers</legend>
                            <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="1" HorizontalAlign="Center" Width="760">

                                <asp:TableHeaderRow CssClass="columnhead">
                                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">Position</asp:TableHeaderCell>

                                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">Name</asp:TableHeaderCell>

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

        <!--- removing LOTO from review page
        <div class="row" style="margin-top: 20px; border: solid 0px black;">
            <asp:Table ID="Table6" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            <legend class="fs-border">NIH LOTO Index</legend>
                            <asp:Table ID="Table17" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblLOTO" runat="server" ForeColor="Green">There is currently no NIH LOTO Index data.</asp:Label>
                                        <asp:Panel ID="pnlLOTO" runat="server">
                                            <asp:Table ID="tblLOTO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell ColumnSpan="4">
                                                        <asp:DataGrid ID="grLOTO" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                            BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                            <HeaderStyle CssClass="columnhead"></HeaderStyle>
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
            --->


        <div class="row" style="margin-top: 20px; border: solid 0px black;">
            <asp:Table ID="Table8" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            <legend class="fs-border">Critical Alarms</legend>
                            <asp:Table ID="Table21" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                <asp:TableRow>
                                    <asp:TableCell>
                                          <a class="btn btn-primary"  style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Auxiliary" target="iframe_criticalAlarms" role="button" aria-pressed="true">Auxiliary</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Boiler" target="iframe_criticalAlarms" role="button" aria-pressed="true">Boiler Plant</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Chiller" target="iframe_criticalAlarms" role="button">Chiller Plant</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=WaterTreatment" target="iframe_criticalAlarms" role="button">Water Treatment</a>
                            <br />
                                         
                                        <asp:Panel ID="pnlCAlm" runat="server">
                                             <iframe src="LoadHtml.aspx?report=Auxiliary" name="iframe_criticalAlarms" width="980" height="500"></iframe>
                                        </asp:Panel>
                                        <asp:Table ID="Table7" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                            <asp:TableRow>
                                                <asp:TableCell Font-Bold="true">
                                                    <strong>Are all critical alarms captured?</strong>

                                                    <asp:Label ID="lblCriAlsCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                                    <br />

                                                    <strong>Comments:</strong>
                                                    <br />
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

        <div class="row" style="margin-top: 20px; border: solid 0px black;">
           <%-- <asp:Table ID="Table9" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
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
                                    <strong>Chiller Status </strong>:
                                    <br />
                                    Instructions:   Verify that the chiller statuses shown are correct and enter comments if data appears incorrect.
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

                                            <asp:Label ID="lblChillersCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
                                            <asp:Label ID="lblChillersComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Was Optimization Plan Followed?</strong>

                                            <asp:Label ID="lblChlOptYN" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
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
                                    <strong>Chillers Pump Status </strong>: 
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

                                            <asp:Label ID="lblChillersPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
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

                                            <asp:Label ID="lblCTCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
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

                                            <asp:Label ID="lblFreeCoCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
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

                                            <asp:Label ID="lblBoilersCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
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

                                            <asp:Label ID="lblBlrPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                            <br />

                                            <strong>Comments:</strong>
                                            <br />
                                            <asp:Label ID="lblBlrPmpComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </fieldset>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>--%>

        </div>
        <div class="row" style="margin-top: 20px; border: solid 0px black;">
            <asp:Table ID="Table10" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
                <asp:TableRow>

                    <asp:TableCell VerticalAlign="top">
                        <asp:Panel runat="server" ID="pnlConfirmation" Visible="true" BackColor="#cce6f9">
                            <fieldset class="fs-border">
                                <legend class="fs-border">
                                    <strong></strong>

                                </legend>
                                <asp:Table ID="tblReqFields" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">

                                    <asp:TableRow>

                                        <asp:TableCell VerticalAlign="top" Font-Bold="true">
                                            <asp:Literal ID="litSDK" runat="server"></asp:Literal>
                                            <br />
                                            <asp:Label ID="lblTimeRemain" runat="server" ForeColor="Blue"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                </asp:Table>

                            </fieldset>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 35px; padding-right: 35px;">
            <asp:Button ID="btnPre" runat="server" Text=" Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back " CausesValidation="true" OnClick="btnPre_Click" />

            &nbsp;&nbsp;     
            <asp:Button ID="btnNext" runat="server" Text=" Save And Submit" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" CausesValidation="true" ToolTip="Save And Submit" OnClick="btnNext_Click" />
        </div>
    </div>


</asp:Content>


<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
