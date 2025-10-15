<%@ Page Language="C#" Title="Manage Shift Turnover Users " AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" MasterPageFile="~/ShiftTurnoverAdmin.Master" Inherits="ShiftTurnover.ManageUser" %>

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
      <style>
        /* Modal overlay style */
        #overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 999;
            display: none;
        }
    </style>
  <script type="text/javascript">
      function confirmBox() {
          return window.confirm(" Are you sure you want to archive this report?");
      }
       </script>
    <script type="text/javascript">
        function openPopup(id) {
           
            var url = 'NearMissPDF.aspx?id=' + id;
            var popup = window.open(url, "PopupWindow", "width=600,height=400,left=100,top=100, position: fixed");
            var timer = setInterval(function () {
                if (popup.closed) {
                    clearInterval(timer);
                    
                    window.location.reload(); // Refresh the parent window when the popup is closed
                }
            }, 1000);
        }
       </script>
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

 <asp:Content ID="Content10" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" Width="50%">
                <asp:Literal runat="server" ID="litBack" ></asp:Literal>
            </asp:TableCell>
            <asp:TableCell Visible="true" HorizontalAlign="Right" Width="50%">
                  
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">
 
    <div class="container">
        <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
    <asp:TableRow>
   
        <asp:TableCell HorizontalAlign="left">
                      <asp:Button ID="btnAdd" runat="server" Text=" Add New User " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Large" ForeColor="White" Width="200px" Height="50px" 
ToolTip="Add New user"  OnClick="btnAdd_Click"
/>  
           
            </asp:TableCell>
             <asp:TableCell HorizontalAlign="right">
             <asp:Button ID="btnExportExcel" runat="server" Text=" Export To Excel" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Large" ForeColor="White" Width="200px" Height="50px" 
      ToolTip="Export To Excel"  OnClick="btnExportExcel_Click"
      />
     </asp:TableCell>
    </asp:TableRow>
             <asp:TableRow>

     <asp:TableCell HorizontalAlign="left" colspan="2" >
       
                  <asp:Label runat="server" CssClass="alert" ID="Label1"   Font-Bold="true" Font-Size="Medium" Text=" A user must be in the 'PIUsers' AD group before being added to this application. Please open a
                     <a href='https://myitsm.nih.gov/nih_sd?id=service_request_catalog' target='_blank'>ticket </a> to add or contact
                      <a href='mailto:ORFDTREFAMIT@mail.nih.gov?subject=PIUsers group access'>  DTR EFAM IT </a>
                       " /> 
                     </asp:TableCell>
                 <asp:TableCell HorizontalAlign="left" >
       </asp:TableCell>
                 </asp:TableRow>
</asp:Table>
          
        <div class="row" style="margin-top: 10px; border: solid 0px black;">
                <asp:Table ID="tblConfirm" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Center">
                            <asp:Label runat="server" CssClass="alert" ID="lblConfirm" Visible="false" Font-Bold="true" Font-Size="Large" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
             <asp:Table ID="tblPastEvents" runat="server"
                                 CellPadding="0" CellSpacing="0"    
                                 BorderWidth="0" Width="100%" HorizontalAlign="Left">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <fieldset class="fs-border">
                                            <legend class="fs-border">All Shift Turnover Users</legend>
                                            <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0" HorizontalAlign="Center" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <%--<asp:Label ID="lblPast" runat="server">There are no reported Near Miss Safety Forms.</asp:Label>--%>
                                                        <asp:Panel ID="pnlPast" runat="server">
                                                            <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                         <asp:Label runat="server"   ID="lblCount" Visible="true"  Font-Bold="true" Font-Size="Medium" ForeColor="Green"  />
                                                        <div id="netTable" runat="server" Visible="true" style="border: 0px solid black;"></div> 
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
 
