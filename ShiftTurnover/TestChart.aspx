<%@ Page Title="Testing Audit Trail" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="TestChart.aspx.cs" Inherits="ShiftTurnover.TestChart" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      

</asp:Content>

 
<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="Table2" runat="server"   CssClass="wiznavbuttonsPrint" CellPadding="10" CellSpacing="10" BorderWidth="1" Width="1000" HorizontalAlign="Center">
           <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="1">
                                    <asp:TableCell>
        <label>Start Date:</label>
                                                        <asp:TextBox ID="txtEndDate" TabIndex="6" runat="server"  Width="240px"
                                                           
                                                            TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox> </asp:TableCell><asp:TableCell>
                                          <label>End Date:</label>
                                                        <asp:TextBox ID="txtStartDate" TabIndex="6" runat="server"  Width="240px"
                                                           
                                                            TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox> </asp:TableCell><asp:TableCell>
                                          <label>Max Count:</label>
                                                        <asp:TextBox ID="txtMax" TabIndex="6" runat="server"  Width="100px"
                                                           
                                                            TextMode="Number" CssClass="form-control"></asp:TextBox> </asp:TableCell><asp:TableCell>
                                         <asp:Button ID="btnShow" runat="server" Text="Show Data"  ForeColor="white" BackColor="#b3112c"  CssClass="btn"  Width="160px" OnClick="btnShow_Click" /> 
                                                  <asp:Button ID="btnExport" runat="server" Text="Export Data"  ForeColor="white" BackColor="Blue"  CssClass="btn"  Width="160px" OnClick="btnExport_Click" />  
                                         
                                                                </asp:TableCell>
               </asp:TableRow>
       
        </asp:Table>
          <asp:Table ID="Table1" runat="server"   CssClass="wiznavbuttonsPrint" CellPadding="10" CellSpacing="10" BorderWidth="1" Width="1000" HorizontalAlign="Center">
        <asp:TableRow Font-Bold="true" Font-Size="Medium" BorderWidth="1">
                                    <asp:TableCell>
                                         <h5><strong>Total Record :</strong>
                                            <asp:Label ID="lblTotalList" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>
                                        
                                         </h5>
                                       
                                          
                                           <asp:DataGrid ID="grdResult" runat="server" CssClass="listing" HorizontalAlign="Center" 
                                                AutoGenerateColumns="false" ItemStyle-Wrap="true" Width="1000" 
                                               UseAccessibleHeader="True"   
                                                   BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                            <HeaderStyle CssClass="columnhead" ForeColor="Black"></HeaderStyle>
                                                    <Columns>
                                                         <asp:TemplateColumn HeaderText="">
                                                           <ItemTemplate>
                                                              
                                                              <%# Container.DataSetIndex + 1 %>
                                                                </ItemTemplate>
                                                               </asp:TemplateColumn>
                                                         <asp:BoundColumn DataField="Date" Visible="true" HeaderText="Timestamp"></asp:BoundColumn>
                                                         <asp:BoundColumn DataField="Action" Visible="true" HeaderText="Action"></asp:BoundColumn>
                                                         <asp:BoundColumn DataField="Type" Visible="true" HeaderText="Type"></asp:BoundColumn>
                                                          <asp:BoundColumn DataField="Database" Visible="true" HeaderText="Database"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Path" Visible="true" HeaderText="Path"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Name" Visible="true" HeaderText="Name"></asp:BoundColumn>
                                                       <asp:BoundColumn DataField="User" Visible="true"   HeaderText="User"></asp:BoundColumn>
                                                             
                                                           
                                                        </Columns>
                                               
                                           </asp:DataGrid>
                                        </asp:TableCell>
                                 </asp:TableRow>

        </asp:Table>

</asp:Content>
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>

