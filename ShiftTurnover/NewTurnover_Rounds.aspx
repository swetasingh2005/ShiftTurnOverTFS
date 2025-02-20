<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_Rounds.aspx.cs" Inherits="ShiftTurnover.NewTurnover_Rounds" %>

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
                "aaSorting": [[0, "asc"]],
                "aLengthMenu": [[50, 100, 150, -1], [50, 100, 150, "All"]],
                "iDisplayLength": 50,
                "oLanguage": {
                    "sEmptyTable": " No NIH LOTO Index data found with given parameters!!"
                }
            });


        });
    </script>
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>


<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">

    <div class="row" style="margin-top: 20px; border: solid 0px black;">
        <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="860">
            <asp:TableRow>
                <asp:TableCell>
                    <h1 style="color: #9A0000; font-weight: bold; width: 860px;">NIH Maximo Rounds Data
                        
                                                <br />
                        <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" Visible="false" ForeColor="Black"></asp:Label>
                    </h1>
                </asp:TableCell>

            </asp:TableRow>

        </asp:Table>
    </div>
    <div class="container">
          <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 5px; padding-right: 5px; padding-top: 35px;">
            <asp:Button ID="btnBack" runat="server" Text=" Upload To PI Server" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Go Back" OnClick="btn_Click" />
            &nbsp;&nbsp;      
            <asp:Button ID="btnNext" runat="server" Text=" Export to Excel" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Go to Next tab" OnClick="btn_Click" />
        </div>
        <div class="row" style="margin-top: 10px; border: solid 0px black;">
            <asp:Table ID="Table11" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="1000">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            
                            <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblLOTO" runat="server">There is currently no NIH Maximo Rounds data.</asp:Label>
                                        <asp:Panel ID="pnlLOTO" runat="server">

                                            <asp:Table ID="tblLOTO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        
                                                        <asp:DataGrid ID="grLOTO" runat="server" CssClass="listing" HorizontalAlign="Center"
                                                            AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" GridLines="None" 
                                                        OnItemDataBound="grLOTO_ItemDataBound"        
                                                           
                                                            >  
                    <Columns> 
                        <asp:BoundColumn HeaderText="ASSET NUM" DataField="ASSETNUM"> </asp:BoundColumn>
                        <asp:BoundColumn HeaderText="DESCRIPTION" DataField="DESCRIPTION"> </asp:BoundColumn>  
                       
                        <asp:BoundColumn HeaderText="METER NAME" DataField="assetmeter.metername"> </asp:BoundColumn>  
                          
                        <asp:BoundColumn HeaderText="LAST READING" DataField="assetmeter.lastreading"> </asp:BoundColumn>  
                        <asp:TemplateColumn   HeaderText="LAST READING DATE" >
                           <ItemTemplate >
                               <asp:Label  runat="server"  ID="lblDate" />
                           </ItemTemplate>
                       </asp:TemplateColumn>
                        
                        <asp:BoundColumn HeaderText="Authorized Date"   Visible="false"
                               DataField="assetmeter.lastreadingdate"> </asp:BoundColumn>
                                         <asp:BoundColumn HeaderText="METER DESCRIPTION" DataField="assetmeter.meter.description"> </asp:BoundColumn>             
                        
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
<asp:Content ID="Content14" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
