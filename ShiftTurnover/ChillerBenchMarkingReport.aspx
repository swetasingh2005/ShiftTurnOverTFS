<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="ChillerBenchMarkingReport.aspx.cs" Inherits="ShiftTurnover.ChillerBenchMarkingReport" %>

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
            <asp:TableCell HorizontalAlign="Left" Width="70%">
                <asp:Literal runat="server" ID="litBack"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell  HorizontalAlign="Right" Width="30%">
                 <asp:Button ID="Button1" runat="server"  CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Small"  CausesValidation="false"
                                    ForeColor="White"  Width="180px"     
                     Text="Print This Report" OnClientClick="javascript:window.print()" TabIndex="1" />
      
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center"  ColumnSpan="2" >
              <h1>  <asp:Literal runat="server" ID="lblTitle"></asp:Literal></h1>
            </asp:TableCell>
            <asp:TableCell  HorizontalAlign="Right" Width="50%">
                  
            </asp:TableCell>
        </asp:TableRow>
       
    </asp:Table>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">
       
    <div class="row" style="margin-top: 20px; border: solid 0px black;">
         <asp:Table ID="Table3" runat="server"   CellPadding="2" CellSpacing="2" BorderWidth="0" HorizontalAlign="Center" Width="900">
            <asp:TableRow>
                <asp:TableCell><h4> <strong>Report start time:</strong> 7/1/2022 12:00:00 AM </h4></asp:TableCell>
                 
                 <asp:TableCell><h4><strong> Report End time: </strong>8/1/2022 12:00:00 AM </h4></asp:TableCell>
                <asp:TableCell><h4><strong> Duration: </strong> 30 Days </h4></asp:TableCell>
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

                                            <asp:Table ID="tblLOTO" runat="server" CssClass="listing"  BorderWidth="1"  Width="100%">
                                                <asp:TableHeaderRow BorderWidth="0" Height="40px" BackColor="Black" ForeColor="White" HorizontalAlign="Center" Font-Bold="true" >
                                                    <asp:TableHeaderCell  HorizontalAlign="Center"> Analysis</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell HorizontalAlign="Center"  >Acceptable (if applicable)</asp:TableHeaderCell>
                                                    <asp:TableHeaderCell  HorizontalAlign="Center">Maximum (if applicable) </asp:TableHeaderCell>
                                                    <asp:TableHeaderCell   HorizontalAlign="Center">Result</asp:TableHeaderCell>
                                                </asp:TableHeaderRow>
                                                <asp:TableRow >
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black" >Average number of  alarms per Day per operating position
                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                       <asp:Label ID="lbl19_1" runat="server" Font-Bold="true" Font-Size="Medium" Text="150"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl19_2" runat="server" Font-Bold="true" Font-Size="Medium" Text="300"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl19_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Annunciated alarms per 10-minute period per operating position.

                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis2C1" runat="server" Font-Bold="true"  Text="1" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis2C2" runat="server" Font-Bold="true"  Text="2" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis2C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                     
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Annunciated Priority Distribution

                                                    </asp:TableCell>   
                                               <asp:TableCell >
                                                        <asp:Label ID="lblPriority_1" runat="server" Font-Bold="true" Font-Size="Medium"    Text=""  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblPriority_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblPriority_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Average number of alarms per Hour per operating position

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl21_1" runat="server" Font-Bold="true"  Text="6" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl21_2" runat="server" Font-Bold="true" Text="12" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl21_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                    
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Average number of alarms per flood

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl28_1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl28_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl28_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Percentage of hours containing more than 30 alarms

                                                    </asp:TableCell>   
                                                 <asp:TableCell >
                                                        <asp:Label ID="lbl24_1" runat="server" Font-Bold="true"  Text="< 1%" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl24_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl24_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                      
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Percentage of 10-minute periods containing more than 10 alarms

                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis7C1" runat="server" Font-Bold="true" Text="< 1%" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis7C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis7C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                         
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Maximum number of alarms in a 10-minute period

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl25_1" runat="server" Font-Bold="true" Text="<= 10" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl25_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl25_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Percentage of time the alarm system is in flood condition


                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl6_1" runat="server" Font-Bold="true"  Text="< 1%" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl6_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl6_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Percent contribution top 10 most frequent alarms to overall alarm load
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl3_1" runat="server" Font-Bold="true"  Text="< 1%" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl3_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl3_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                               
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Average number of Stale Alarms per day
                                                    </asp:TableCell>   
                                                  <asp:TableCell >
                                                        <asp:Label ID="lbl18_1" runat="server" Font-Bold="true"  Text="< 5. Any stale alarm needs to be addressed." Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl18_2" runat="server" Font-Bold="true" Font-Size="Medium" Text=" 5%"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl18_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                       
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Unauthorized Alarm Suppression

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis12C1" runat="server" Font-Bold="true" Text="0" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis12C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblAnalysis12C3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                          
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Max number of alarm count in any flood

                                                    </asp:TableCell>   
                                                 <asp:TableCell >
                                                        <asp:Label ID="lbl26_1" runat="server" Font-Bold="true" Text="N/A" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl26_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell> 
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl26_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                    
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Total number of alarm floods

                                                    </asp:TableCell>   
                                                  <asp:TableCell >
                                                        <asp:Label ID="lbl13_1" runat="server" Font-Bold="true" Font-Size="Medium" Text="N/A"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl13_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell>
                                                        <asp:Label ID="lbl13_3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Total number of alarms 

                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl1_1" runat="server" Font-Bold="true" Font-Size="Medium" Text="N/A"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lbl1_2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblTotalAlarm3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                        
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Identify Chattering alarms 

                                                    </asp:TableCell>   
                                                     <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis16C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis16C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblChattering1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                         
                                                </asp:TableRow>
                                                 <asp:TableRow>
                                                    <asp:TableCell Font-Bold="true" BackColor="#EFF3F7" ForeColor="Black">Identify Fleeting alarms 

                                                    </asp:TableCell>   
                                                   <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis17C1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>   
                                                    <asp:TableCell >
                                                        <asp:Label ID="lblAnalysis17C2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Black"> </asp:Label>
                                                    </asp:TableCell>  
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblFleeting1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                                    </asp:TableCell>                  
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:Panel>
                                    </asp:TableCell>
                              </asp:TableRow>
                                <asp:TableRow BackColor="Black" Font-Bold="true" ForeColor="White" Font-Size="Large" HorizontalAlign="Center">
                                    <asp:TableCell>
                                        Appendix
                                        </asp:TableCell>
                                    </asp:TableRow>
                               <asp:TableRow  Font-Bold="true"  Font-Size="Large" ForeColor="Black" HorizontalAlign="Center">
                                    <asp:TableCell>
                                        Definition 
                                        <asp:Table ID="Table1" runat="server" CssClass="listing" CellPadding="2" CellSpacing="2" BorderWidth="1" HorizontalAlign="Center" Width="100%">
            <asp:TableRow>
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                     Total number of  Alarms
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblTotalAlarm2" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                     Total number of  Frequency
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                     Percent contribution top 10 most frequenct alarms
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID3" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                 <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                   Total 10-min intervals
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID4" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
                 
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                   10-min intervals in flood
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID5" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Percentage of time the alarm system is in flood condition
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID6" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            
             <asp:TableRow>
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Low + Warning
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID7" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Medium Priority
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID9" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    High Priority
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID11" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Total Alarms
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblTotalAlarm1" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Total Number of Alarm Floods		
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID13" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>

             
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Longest time in Flood [min]				
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID14" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Stale Alarms			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID15" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Average Number of Stale alarms per day			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID18" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                    Average number of alarms per Day per operator				
			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID19" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                   Average number of alarms per 10 min period				
				
			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID20" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>

            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                  Average number of alarms per hour period				

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID21" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                  1-hr period more than 30 alarms	

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID22" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>
  <asp:TableRow>
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                  Total 1-hr intervals	

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID23" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                 Percentage of hours containing more than 30 alarms	

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID24" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                 Max number of alarms in 10-min period

			
                </asp:TableCell>
                 <asp:TableCell Wrap="true" Font-Bold="true"   ForeColor="Black">  
                     <asp:Label ID="lblKPIID25" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                  Max Alarm Count during any Flood     	

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID26" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            
               <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                Total alarms while in flood

			
                </asp:TableCell>
                <asp:TableCell>
                     <asp:Label ID="lblKPIID27" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
             
                <asp:TableCell Wrap="true" Font-Bold="true"  BackColor="#EFF3F7" ForeColor="Black">  
                Average number of alarms per flood     
			
                </asp:TableCell>
                 <asp:TableCell Wrap="true" Font-Bold="true"   ForeColor="Black">  
                     <asp:Label ID="lblKPIID28" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                <asp:TableRow  Font-Bold="true"  Font-Size="Large" ForeColor="Black" HorizontalAlign="Center">
                                    <asp:TableCell>
                                         Stale Alarms<br />
                                        <asp:Label ID="lblStaleNumber" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                        
                                        <asp:DataGrid ID="grStale" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        
                                                           
                                                            >  
                    <Columns> 
                       
                        
                    </Columns>  
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#EFF3F7" ForeColor="Black" />  
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                <asp:TableRow  Font-Bold="true"  Font-Size="Large" ForeColor="Black" HorizontalAlign="Center">
                                    <asp:TableCell>
                                         Chattering alarm Suspects<br />
                                        <asp:Label ID="lblChattering" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                         <asp:DataGrid ID="grChattering" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        >  
                    <Columns> 
                      
                    </Columns>  
                     <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#EFF3F7" ForeColor="Black" />  
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                  <asp:TableRow  Font-Bold="true"  Font-Size="Large" ForeColor="Black" HorizontalAlign="Center">
                                    <asp:TableCell>
                                         Fleeting alarm Suspects<br />
                                        <asp:Label ID="lblFleeting" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                         <asp:DataGrid ID="grFleeting" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        >  
                    <Columns> 
                      
                    </Columns>  
                     <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#EFF3F7" ForeColor="Black" />  
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                   <asp:TableRow  Font-Bold="true"  Font-Size="Large" ForeColor="Black" HorizontalAlign="Center">
                                    <asp:TableCell>
                                         Top 10 Alarms
                                        </asp:TableCell>
                                    </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:DataGrid ID="grdTop10" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="true" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        
                                                           
                                                            >  
                    <Columns> 
                       
                        
                    </Columns>  
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#EFF3F7" ForeColor="Black" />  
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>
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
 
