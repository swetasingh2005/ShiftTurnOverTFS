<%@ Page Title="Water Treatment Alarm Benchmarking Analysis and Results" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="WTBenchMarkingReport.aspx.cs" Inherits="ShiftTurnover.WTBenchMarkingReport" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />--%>
    <link href="Content/jquery.dataTables.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery.dataTables.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" id="bootstrap-css" />
    <script>



        $(document).ready(function () {
            $('#example').DataTable({
                "aaSorting": [[0, "desc"]],
                "aLengthMenu": [[50, 100, 150, -1], [50, 100, 150, "All"]],
                "iDisplayLength": 50,
                "oLanguage": {
                    "sEmptyTable": " No data!!"
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
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">

    <div class="row" style="margin-top: 20px; border: solid 0px black;">
        <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="860">
            <asp:TableRow>
                <asp:TableCell>
                     
                </asp:TableCell>

            </asp:TableRow>

        </asp:Table>
    </div>
    <div class="container">
           
        <div class="row" style="margin-top: 10px; border: solid 0px black;">
            <asp:Table ID="Table11" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="100%">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            
                            <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        
                                        <asp:Panel ID="pnlLOTO" runat="server">

                                            <asp:Table ID="tblLOTO" runat="server" CssClass="table-bordered"  BorderWidth="1"  Width="100%">
                                                <asp:TableHeaderRow BorderWidth="0" Height="40px" BackColor="Black" ForeColor="White" Font-Bold="true" >
                                                    <asp:TableHeaderCell  > Analysis</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell  >Acceptable (if applicable)</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell  >Maximum (if applicable) </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell  >Result</asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                                <asp:TableRow >
                                                    <asp:TableCell Font-Bold="true" >Average number of  alarms per Day per operating position
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis1C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis1C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis1C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Annunciated alarms per 10-minute period per operating position.

                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis2C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis2C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis2C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                     
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Annunciated Priority Distribution

                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis3C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis3C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis3C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Average number of alarms per Hour per operating position

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis4C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis4C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis4C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                    
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Average number of alarms per flood

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis5C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis5C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis5C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Percentage of hours containing more than 30 alarms

                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis6C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis6C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis6C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                      
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Percentage of 10-minute periods containing more than 10 alarms

                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis7C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis7C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis7C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                         
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Maximum number of alarms in a 10-minute period

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis8C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis8C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis8C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Percentage of time the alarm system is in flood condition


                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis9C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis9C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis9C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Percent contribution top 10 most frequent alarms to overall alarm load
                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis10C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis10C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis10C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                               
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Average number of Stale Alarms per day
                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis11C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis11C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis11C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                       
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Unauthorized Alarm Suppression

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis12C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis12C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis12C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Max number of alarm count in any flood

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis13C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis13C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis13C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                    
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Total number of alarm floods

                                                    </asp:TableCell>   
                                                  <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis14C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis14C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis14C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Total number of alarms 

                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis15C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis15C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis15C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Identify Chattering alarms 

                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis16C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis16C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis16C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                         
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true">Identify Fleeting alarms 

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis17C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis17C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis17C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                  
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                    </asp:TableCell>
                              </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        Stale Alarms
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:DataGrid ID="grStale" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        
                                                           
                                                            >  
                    <Columns> 
                        <asp:BoundColumn HeaderText="Tag" DataField="Tag"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Occurrences" DataField="Occurrences"> </asp:BoundColumn>  
                        
                    </Columns>  
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />  
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                        Fleeting and Chattering alarm Suspects
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                         <asp:DataGrid ID="grFleetingChattering" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        >  
                    <Columns> 
                        <asp:BoundColumn HeaderText="Tag" DataField="Tag"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Occurrences" DataField="Occurrences"> </asp:BoundColumn>  
                        <asp:BoundColumn HeaderText="% total (Overall Chiller alarms" DataField="Percentage"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Avg Duration (min)" DataField="Avg"> </asp:BoundColumn>  
                    </Columns>  
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />  
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
                                        </asp:TableCell>
                                    </asp:TableRow>
                            </asp:Table>
                        </fieldset>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>

       
    </div>

</asp:Content>
 
