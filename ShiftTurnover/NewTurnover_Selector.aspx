<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_Selector.aspx.cs" Inherits="ShiftTurnover.NewTurnover_Selector" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
  

     
     
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
    
</asp:Content>
 

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">


    <center>
         <div class="row" style="margin-top:20px; margin-left:20px; margin-bottom:20px;   border:solid 0px black;   "  >
            <asp:Table ID="Table16" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"  HorizontalAlign="Center" Width="960"  BorderWidth="1"      >
                 <asp:TableRow >
                                              <asp:TableCell  BackColor="#cce6f9" HorizontalAlign="Center" Font-Bold="true"  ForeColor="Red">   
                                                 <asp:Label runat="server" ID="lblMShift"  Font-Size="Medium" ForeColor="Black"        ></asp:Label> 
                                                </asp:TableCell>
                                            </asp:TableRow>
                                          <asp:TableRow >
                                               
                                             <asp:TableCell HorizontalAlign="Center">
                                                  <br /><br />
                                             </asp:TableCell>
                                          
                                          
                                            </asp:TableRow>
                                        <asp:TableRow  >
                                              <asp:TableCell Font-Bold="true"  ForeColor="Red" HorizontalAlign="Center" >   
                                                 <asp:Label runat="server" ID="lblPShift"     Font-Size="Medium"      ></asp:Label> turnover report has not been submitted, Please click on following button to review and submit Previous shift's report first.
                                                </asp:TableCell>
                                              </asp:TableRow>
                                        <asp:TableRow >
                                               
                                             <asp:TableCell HorizontalAlign="Center">
                                                 <asp:Button ID="btnPre" runat="server" Text="Review And Submit Previous Shift TurnOver Report" CssClass="btn"  CausesValidation="true" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="80%" Height="50px"   ToolTip="Go And Edit Previous Shift"    OnClick="btnPre_Click"  />
                                                 
                                             </asp:TableCell>
                                          
                                          
                                            </asp:TableRow>
                                          <asp:TableRow >
                                               
                                             <asp:TableCell HorizontalAlign="Center">
                                                  <br /><br />
                                             </asp:TableCell>
                                          
                                          
                                            </asp:TableRow>
                </asp:Table>
             </div>


       
    
 </center>
    </asp:Content>

 
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
 