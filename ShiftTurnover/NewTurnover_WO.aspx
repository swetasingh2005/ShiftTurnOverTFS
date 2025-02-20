<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_WO.aspx.cs" Inherits="ShiftTurnover.NewTurnover_WO" %>
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
                   "sEmptyTable": " No Maximo Work Order found with given parameters!!"
               }
           });
           
         
       });
    </script>
</asp:Content>
<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <div class="container" >
     
      <div class="row" style="margin-top:85px;"  >
      <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-left  bhoechie-tab-container">
                    <center>
                           <div class="row"  >
                                 
                                  <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center  bhoechie-tab-container" style="border:solid 0px Black;">
                                      <div class="row" style="margin-top:20px; border:solid 0px black;"  >
                                          
                                    <asp:Table ID="Table6" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <fieldset class="fs-border">
                                                    <legend class="fs-border"></legend>
                                                    <asp:Table ID="Table17" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                                         <asp:TableRow>
                                                            <asp:TableCell>
                                                                <h1><asp:Label ID="lblHeader" runat="server"  ForeColor="#9A0000" Font-Bold="true" Text="Maintenance Work Orders"></asp:Label>
                                            
                                       <br />
                      <asp:Label runat="server" ID="lblMShift"  Font-Size="Medium" ForeColor="Black"      ></asp:Label>
                                       </h1>
                                                                </asp:TableCell>
                                                             </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblWO" ForeColor="Green" runat="server">There are no Maintenance Work Orders for this shift.</asp:Label>
                                                                <asp:Panel ID="pnlWO" runat="server">
                                                                    <asp:Table ID="tblWO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                        <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                <asp:DataGrid ID="grdWO" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                                                    BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                                                    <HeaderStyle CssClass="columnhead"></HeaderStyle>
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
                                       </div>
                                    <div class="row" style="margin-top:20px; border:solid 0px black;"  > 
                                        <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <fieldset class="fs-border">
                                                    <legend class="fs-border"> </legend>
                                                    <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="left" Width="760">
                                                        <asp:TableRow>
                                                            <asp:TableCell  HorizontalAlign="Left">
                                                                <asp:Label ID="Label1" runat="server" Font-Bold="true">
                                                                    All work orders completed or closed during the current shift are shown here. Are all the required Maximo work orders completed?
                                                                    <img height="12" alt="Required" src="Images/icon_required.gif" width="13" />
                                                                </asp:Label>
                                                                
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                 <asp:RadioButtonList runat="server" ID="rdoWOComplete" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Yes" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
                                                            <asp:ListItem Value="No" Text="&nbsp;No&nbsp;"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                                <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="rdoWOComplete" ErrorMessage="Required" ID="rfvWOComplete" runat="server"  ></asp:RequiredFieldValidator>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                         <asp:TableRow>
                                                            <asp:TableCell HorizontalAlign="Left">
                                                                <asp:Label ID="Label2" runat="server" Font-Bold="true">Are the work orders shown in Maximo correct?</asp:Label>
                                                                <img height="12" alt="Required" src="Images/icon_required.gif" width="13" />
                                                            </asp:TableCell>
                                                            <asp:TableCell>
                                                                 <asp:RadioButtonList runat="server" ID="rdoWOCorrect" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="Yes" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
                                                            <asp:ListItem Value="No" Text="&nbsp;No&nbsp;"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                                <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="rdoWOCorrect" ErrorMessage="Required" ID="RequiredFieldValidator1" runat="server"  ></asp:RequiredFieldValidator>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </fieldset>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                            
                                          
                                      </div>
                                                         
                               <br />
                                      
                    
                 
                                     <!------ Tabs ---------->
                                   <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left:35px;padding-right:35px;">
                                             <asp:Button ID="btnPre" runat="server" Text=" Save And Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"   ToolTip="Save And Go Back "    OnClick="btnSave_Click"  />
                                     &nbsp;&nbsp;   <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"  ToolTip="Save And Go Back "      OnClick="btnSave_Click"  />
                                    &nbsp;&nbsp;      <asp:Button ID="btnNext" runat="server" Text=" Save And Next"  CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White"  Width="100%" Height="50px"  ToolTip="Save And Go to Ground "   OnClick="btnSave_Click"     />
                                    </div> 

                                  </div>
                       </div>
                             
                  </div>
                               </center>
                </div>

        </div>


    </asp:Content>

 
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>