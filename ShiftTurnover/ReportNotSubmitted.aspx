<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="ReportNotSubmitted.aspx.cs" Inherits="ShiftTurnover.ReportNotSubmitted" %>

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
                    <h1 style="color: #9A0000; font-weight: bold; width: 860px;">Shift Turnover Report Not Submitted Report
                        
                                                <br />
                        <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label>
                    </h1>
                </asp:TableCell>

            </asp:TableRow>

        </asp:Table>
    </div>
    <div class="container">
          <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 5px; padding-right: 5px; padding-top: 35px;">
            <asp:Button ID="btnExportExcel" runat="server" Text=" Export To Excel" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" 
                ToolTip="Export To Excel"  OnClick="btnExportExcel_Click"
                />
           
        </div>
        <div class="row" style="margin-top: 10px; border: solid 0px black;">
            <asp:Table ID="Table11" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="900">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            
                            <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblLOTO" runat="server" Font-Bold="true" Font-Size="Medium"  ForeColor="Green"> </asp:Label>
                                        <asp:Panel ID="pnlLOTO" runat="server">

                                            <asp:Table ID="tblLOTO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                          <div id="netTable" runat="server" style="border: 0px solid black;"></div>

                                                        <%--<asp:DataGrid ID="grLOTO" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                           AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        
                                                           
                                                            >  
                    <Columns> 
                        <asp:BoundColumn HeaderText="ReportID" DataField="ReportID"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Shift" DataField="Shift"> </asp:BoundColumn>  
                        <asp:BoundColumn HeaderText="CreatedBy" DataField="CreatedBy"> </asp:BoundColumn> 
                          <asp:BoundColumn HeaderText="Created Date"   Visible="true"
                               DataField="CreateDate"> 
                        
                       </asp:BoundColumn>
                         <asp:BoundColumn HeaderText="Submittedby" DataField="Submittedby"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="CrewLeader" DataField="CrewLeader"> </asp:BoundColumn>  
                        <asp:BoundColumn HeaderText="ChillerOperator" DataField="ChillerOperator"> </asp:BoundColumn>  
                          <asp:BoundColumn HeaderText="BoilerOperator" DataField="BoilerOperator"> </asp:BoundColumn>  
                        <asp:BoundColumn HeaderText="AuxiliaryOperator" DataField="AuxiliaryOperator"> </asp:BoundColumn>  
                         <asp:BoundColumn HeaderText="ShiftElectrician" DataField="ShiftElectrician"> </asp:BoundColumn>  
                         <asp:BoundColumn HeaderText="CogenOperator" DataField="CogenOperator"> </asp:BoundColumn>  
 
                      
                       
                                                    
                        
                    </Columns>  
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />  
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />  
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />  
                    <AlternatingItemStyle BackColor="White" />  
                    <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />  
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" /> </asp:DataGrid>--%>
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

       
    </div>

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
 
 
