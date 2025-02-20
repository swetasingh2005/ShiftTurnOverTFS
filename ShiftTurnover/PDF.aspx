<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="PDF.aspx.cs" Inherits="ShiftTurnover.PDF" %>

<%@ MasterType VirtualPath="~/ShiftTurnoverNoHeader.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />

</asp:Content>


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">

    <div class="container">

        <div class="row" style="margin-top: 0px; border: solid 0px black;">
            <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="1" HorizontalAlign="Center" Width="760">

                <asp:TableHeaderRow CssClass="columnhead">
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">Shift</asp:TableHeaderCell>
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">Created </asp:TableHeaderCell>
                    <asp:TableHeaderCell Font-Bold="true" BorderWidth="1">Submitted </asp:TableHeaderCell>
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
                    </asp:TableCell>
                </asp:TableRow>

            </asp:Table>

            <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
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
            <div class="row" style="margin-top: 0px; border: solid 0px black;">
             <asp:Table ID="Table23" runat="server" CellPadding="0" CellSpacing="0" CssClass="listing" BorderWidth="0" HorizontalAlign="Center" Width="760">
                <asp:TableRow>
                    <asp:TableCell>
                         
                         <strong>    Previous Shift's Live Log Entries was acknowledged?
                        <asp:Label runat="server" ID="lblPrevAch" Font-Size="Medium"   ></asp:Label>
                   </strong> 
                        </asp:TableCell>
                    </asp:TableRow>
                 </asp:Table>
            </div>
            <asp:Table ID="tblPastEvents" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
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
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </fieldset>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>

            <%--<asp:Table ID="Table6" runat="server"   CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
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
                                                                          </asp:Table> </asp:Panel>
                                                                     <asp:Table ID="Table3" runat="server" CellPadding="0"  CellSpacing="0" Width="100%">
                                                                        <asp:TableRow >
                                                                            <asp:TableCell BorderWidth="0" HorizontalAlign="Left" VerticalAlign="Top" > <strong>
                                                                                All work orders completed or closed during the current shift are shown here. <br />
                                                                                Are all the required Maximo work orders completed?</strong> 
                                                                       
                                                                          <asp:Label ID="lblWOComplete" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />
                                                                     <strong>  Are the work orders shown in Maximo correct?</strong> 
                                                                     
                                                                          <asp:Label ID="lblWOCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
        
            <asp:Table ID="Table18" runat="server"  CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
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
                                                                          </asp:Table> </asp:Panel>
                                                                     <asp:Table ID="Table4" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                         <asp:TableRow>
                                                                            <asp:TableCell BorderWidth="0" HorizontalAlign="Left" VerticalAlign="Top" > 
                                                                                <strong> The work orders shown here are scheduled to be completed during the next shift, except water chemistry.
                                                      Are all the required maintenance work orders correct? </strong> 
                                                                          <asp:Label ID="lblWOOther" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
            
            <asp:Table ID="Table8" runat="server"   CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800" >
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <fieldset class="fs-border">
                                                    <legend class="fs-border">Critical Alarms</legend>
                                                    <asp:Table ID="Table21" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblCAlm" runat="server" ForeColor="Green">There are no Critical Alarms for this shift.</asp:Label>
                                                                <asp:Panel ID="pnlCAlm" runat="server">
                                                                    <asp:Table ID="tblCAlm" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                        <asp:TableRow>
                                                                            <asp:TableCell ColumnSpan="3">
                                                                                  <asp:Literal ID="ltCriAlm" runat="server"></asp:Literal>
                                                                            </asp:TableCell>
                                                                        </asp:TableRow>
                                                                          </asp:Table> </asp:Panel>
                                                                     <asp:Table ID="Table7" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                         <asp:TableRow>
                                                                            <asp:TableCell BorderWidth="0" HorizontalAlign="Left" VerticalAlign="Top" >
                                                                               <br /><strong>  Are all critical alarms captured?</strong> 
                                                                             
                                                                                <asp:Label ID="lblCriAlsCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />
                                                                           
                                                                           <strong>Comments:</strong><br />     <asp:Label ID="lblCriAlsComment" runat="server" ForeColor="Blue" Font-Italic="true"></asp:Label>
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
        
           <asp:Table ID="tblStatus" runat="server"   CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlCHLStatus"  >
                    <fieldset class="fs-border">
                        <legend class="fs-border"> 
                            <strong>Chillers Status </strong>: 
                       
                        </legend>
                        <asp:Table ID="tblCHLStatus" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                           
                           <asp:TableRow>
                                                                            <asp:TableCell BorderWidth="0" HorizontalAlign="Left" VerticalAlign="Top" >
                                                                                <strong>Is the data correct?</strong>
                                                                             
                                                                                <asp:Label ID="lblChillersCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                               <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblChillersComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
                                                                            </asp:TableCell>
                                                            </asp:TableRow>
                                                               <asp:TableRow>
                                                                            <asp:TableCell BorderWidth="0" HorizontalAlign="Left" VerticalAlign="Top" >
                                                                               <strong>Was Optimization Plan Followed?</strong>
                                                                             
                                                                                <asp:Label ID="lblChlOptYN" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                               <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblChlOptComments" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
                            <strong>Chiller Pumps Status </strong>: 
                        </legend>
                         
                        <asp:Table ID="Table11" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                           
                             <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                 <strong>Is the data correct?</strong>
                                                                           
                                                                                <asp:Label ID="lblChillersPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                              <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblChillersPmpComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
                            <strong>Cooling Towers Status </strong>:  
                        </legend>
                        
                        <asp:Table ID="Table12" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                           
                             <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                 <strong>Is the data correct?</strong>
                                                                            
                                                                                <asp:Label ID="lblCTCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                               <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblCTComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
                            <strong>Free Cooling And Pumps Status </strong>:  
                        </legend>
                        
                        <asp:Table ID="Table13" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          
                            <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                 <strong>Is the data correct?</strong>
                                                                            
                                                                                <asp:Label ID="lblFreeCoCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                         <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblFreeCoComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
                            <strong>Boilers Status </strong>: 
                        </legend>
                        
                        <asp:Table ID="Table14" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          
                              <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                 <strong>Is the data correct?</strong>
                                                                            
                                                                                <asp:Label ID="lblBoilersCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                              <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblBoilersComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
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
                            <strong>RO and Pumps Status </strong>:  
                        </legend>
                        <asp:Table ID="Table15" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                          
                            
                                                         <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                <strong>Is the data correct?</strong>
                                                                           
                                                                                <asp:Label ID="lblBlrPmpCorrect" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label><br />

                                                                            <strong>Comments:</strong><br />
                                                                                <asp:Label ID="lblBlrPmpComment" runat="server" ForeColor="Blue" Font-Italic="true" ></asp:Label>
                                                                            </asp:TableCell>
                                                            </asp:TableRow>
                            </asp:Table>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
   </asp:Table>--%>
        </div>
    </div>



</asp:Content>


