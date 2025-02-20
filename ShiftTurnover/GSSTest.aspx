<%@ Page Title="Test Page" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="GSSTest.aspx.cs" Inherits="ShiftTurnover.GSSTest" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
    <script type="text/javascript" src="Scripts/loader.js"></script>


    <script type="text/javascript">
        function ShowProgress() {
                 document.getElementById('loadingGif').style.display = "block";
            }


        google.charts.load('current', { 'packages': ['timeline'] });
        //google.charts.setOnLoadCallback(drawChart);
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
           ShowProgress();
             $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'GSSTest.aspx/GetData',
                data: '{}',
                success:
                    function (response) {
                        drawChart(response.d);
                        document.getElementById('loadingGif').style.display = "none";
                    }
            });

        })
        function drawChart(dataValues) {

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
            var options = {
              
                
            };
            chart.draw(dataTable, options);
        }
       
   
    </script>
</asp:Content>
<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <div class="align-items-center">
 <asp:Literal runat="server" ID="litSDK" />
</div>
  <div class="container" >
    <div id="loadingGif" style="display:none;position: fixed;z-index: 1031;top: 50%;right: 50%; margin-top: -100px; margin-right: -100px;"><img src="Images/giphy.gif" alt="loading" /></div>
      <div class="row" style="margin-top:100px;"  >
      <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-left  bhoechie-tab-container">
                    <center>
                           <div class="row"  >
                                  <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center  bhoechie-tab-container" style="border:solid 0px Black;">
                                       <h4><asp:Label ID="lblHeader" runat="server" Text="Chiller  Statuses"></asp:Label>
                                           <a href="CheckStatus.aspx?PID=1" target="_blank" title="Click here to enlarge the chart in seperate window"><img src="Images/retro-tv-icon.jpg" style="width:50px; height:40px" /></a></h4> 
                                      <div class="row" style="padding-left:5px;padding-right:5px; border:solid 0px Black;"> 
                                          <div class="col-xs-12 col-sm-12 col-md-12 col-lg-13" style="border:solid 0px Black;">
                                          <div id="CHLChart"    style="width:780px; height:600px;"   ></div>  
                                              </div>
                                          
                                           </div>
                                  
                                      <div class="row" style="padding-left:35px;padding-right:35px;" > 
                                           <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                                                  <div>Is the data correct?</div>

                                           </div>
                                           <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                                                        <asp:RadioButtonList runat="server" ID="rdoChiller" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Yes" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
                                                            <asp:ListItem Value="No" Text="&nbsp;No&nbsp;"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                           </div>
                                           <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
                                               <div>Please enter a comment if the data appears incorrect:</div>
                                               </div>
                                           <div class="col-xs-5 col-sm-5 col-md-5 col-lg-5">
                                                        <asp:TextBox ID="txtChiller" TabIndex="11" runat="server" CssClass="form-control"   Rows="3"   TextMode="MultiLine"></asp:TextBox> 
                                           </div>
                                      </div>
                                                         
                               <br />
                                      
                    
                 
                                       <!------ Tabs ---------->
                                   <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left:35px;padding-right:35px;">
                                             <asp:Button ID="btnPre" runat="server" Text=" Save And Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"   ToolTip="Save And Go Back "    OnClick="btnSave_Click"  />
                                     &nbsp;&nbsp;   <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"  ToolTip="Save And Go Back "      OnClick="btnSave_Click"  />
                                    &nbsp;&nbsp;      <asp:Button ID="btnNext" runat="server" Text=" Save And Next"  CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"  ToolTip="Save And Go to Ground "   OnClick="btnSave_Click"     />
                                    </div> </div>
                       </div>
                             
                    </center>
                </div>

        </div>
   </div>
    </asp:Content>

 
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
 