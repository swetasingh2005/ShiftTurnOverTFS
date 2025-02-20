<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="CheckStatus.aspx.cs" Inherits="ShiftTurnover.CheckStatus" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>
 


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
	   .invoice-container {
      	max-width: 100%;
      	margin-left:0px;
        margin-right:0px;
      	padding: 0px;
      	border: 1px solid #eee;
      	/*box-shadow: 0 0 10px rgba(0, 0, 0, .15);*/
      	font-size: 16px;
      	line-height: 15px;
        border:1px solid black;
      	font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
      	color: black;
      }
      .invoice-container table {
      	width: 100%;
      	line-height: inherit;
      	text-align: left;
   
      }
      .invoice-container table td {
       
      	padding: 3px;
      	vertical-align: top;
      }
      .invoice-container table tr td:nth-child(2) {
      	text-align: right;
      }
      .invoice-container table tr.top table td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.top table td.title {
      	font-size: 20px;
      	/*line-height: 8px;*/
      	color: black;
        font-weight:bold;
         background-color:white;
        text-align:center;
         vertical-align:top;
      }
        .invoice-container table tr.top table td.subtitle {
      	font-size: 12px;
      	/*line-height: 6px;*/
      	color: black;
        font-weight:bold;
         background-color:white;
      }
          .invoice-container table tr.section {
      	font-size: 14px;
      	line-height: 20px;
      	color: black;
        font-weight:bold;
         background-color:white;
      }
      .invoice-container table tr.information table td {
      	padding-bottom: 40px;
      }
      .invoice-container table tr.heading td {
      	background: #eee;
      	border-bottom: 1px solid #ddd;
      	font-weight: bold;
      }
      .invoice-container table tr.details td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.item td {
      	border-bottom: 1px solid #eee;
      }
      .invoice-container table tr.item.last td {
      	border-bottom: none;
      }
      .invoice-container table tr.total td:nth-child(2) {
      	border-top: 2px solid #eee;
      	font-weight: bold;
      }
      @media only print {
      	.invoice-container table tr.top table td {
      		width: 100%;
      		display: block;
      		text-align: center;
      	}
      	.invoice-container table tr.information table td {
      		width: 100%;
      		display: block;
      		text-align: center; 
            
      	}
          /*@media print {
            .page-break { display: block; page-break-before: always; }
            }*/
      }
	</style>
 <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>


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
                url: 'CheckStatus.aspx/GetChillerData',
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
                url: 'CheckStatus.aspx/GetChillerPumpData',
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
                url: 'CheckStatus.aspx/GetBlrData',
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
                url: 'CheckStatus.aspx/GetFreeCoData',
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
                url: 'CheckStatus.aspx/GetROPumpData',
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
                url: 'CheckStatus.aspx/GetCTData',
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
                 colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
                 width: $(window).width(),
                height: $(window).height()*0.75
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
                 colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
                width: $(window).width(),
                height: $(window).height()*0.75
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
                colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
               width: $(window).width(),
                height: $(window).height()*0.75
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
                colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
              width: $(window).width(),
                height: $(window).height()*0.75
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
                colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
               width: $(window).width(),
                height: $(window).height()*0.75
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
                 colors: ['#A6CD4E','#DAAC57','#EA4E5B','0000ff'   ,'#333333'  ],
               width: $(window).width(),
                height: $(window).height()*0.75
            };
            chart.draw(dataTable, options);
        }
    </script>
     
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>


<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">
    


    <div runat="server" id="divExisting" class="instructions"></div>
    <p>
        &nbsp;<br />
    </p>
    <asp:Literal runat="server" ID="litTest" Visible="false"></asp:Literal>
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblStatus" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlCHLStatus"  >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Chiller Status </strong>: <br />
                         
                        </legend>
                        <asp:Table ID="tblCHLStatus" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>
                                
                              <asp:TableCell VerticalAlign="top">
                                  <div id="CHLChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>

                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlChrPmpStatus"   >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Chiller Pump Status </strong>: 
                        </legend>
                         
                        <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>
                              <asp:TableCell VerticalAlign="top">
                                   <div id="CHLPumpChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>

                    </fieldset>
               
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlCTStatus" >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Cooling Tower Status </strong>:  
                        </legend>
                        
                        <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>
                              <asp:TableCell VerticalAlign="top">
                                   <div id="CTChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>

                    </fieldset>
              
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlFreeCoStatus"  >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Free Cooling Status </strong>:  
                        </legend>
                        
                        <asp:Table ID="Table4" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>

                              
                              <asp:TableCell VerticalAlign="top">
                                    <div id="FreeCoChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>

                    </fieldset>
               
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" id="pnlBlrStatus" >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Boiler Status </strong>: 
                        </legend>
                        
                        <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>
                                
                              <asp:TableCell VerticalAlign="top">
                                   <div id="BlrChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>

                    </fieldset>
             
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlROPumpStatus"  >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>RO and Pump Status </strong>:  
                        </legend>
                        <asp:Table ID="Table6" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          <asp:TableRow>
                                
                              <asp:TableCell VerticalAlign="top">
                                  <div id="ROPumpChart" style="height:600px;" ></div>
                                </asp:TableCell>
                              </asp:TableRow>
                           
                            </asp:Table>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
   
</asp:Content>
     
     
                                      
 
 
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
 
