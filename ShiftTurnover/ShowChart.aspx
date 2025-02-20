<%@ Page Title="Work Flow History for Live Log Entry" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="ShowChart.aspx.cs" Inherits="ShiftTurnover.ShowChart" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>
 


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   

</asp:Content>

 


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
       
   <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                   
       <asp:TableRow>
                                         <asp:TableCell>
                                                <asp:Label ID="lblShiftID" runat="server" Font-Size="Larger" ForeColor="Green" Font-Bold="true"></asp:Label>
                                                </asp:TableCell>
                                        </asp:TableRow>
        <asp:TableRow>
                                            <asp:TableCell>
                                              
                                                </asp:TableCell>
                                         </asp:TableRow>
       <asp:TableRow>
            
                                            <asp:TableCell  >
                                                <asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                    BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                    <HeaderStyle CssClass="columnhead" HorizontalAlign="Center"></HeaderStyle>
                                                    <Columns>
                                                        
                                                    </Columns>
                                                </asp:DataGrid>
                                            </asp:TableCell>
          
                                        </asp:TableRow>
         <asp:TableRow>
             
                                            <asp:TableCell  >
                                                   <div class="closewindow"><a href="javascript:window.close();" tabindex="2">Close this window</a></div>
 
                                                </asp:TableCell>
            
                                        </asp:TableRow>
                                    </asp:Table>
</asp:Content>
     
     
                                      
 
 
