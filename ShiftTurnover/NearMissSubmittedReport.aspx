<%@ Page Title="Safety Observation Forms Submission Report" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NearMissSubmittedReport.aspx.cs" Inherits="ShiftTurnover.NearMissSubmittedReport" %>

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
                  <a target="_blank" style="font-size:medium;" href= "<%= ResolveUrl(lblGuide.Text) %>">Users Guide for Safety Observation Form</a>
                <asp:Label ID="lblGuide" runat="server" Visible="false"></asp:Label>  
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_main" runat="server">
 
    <div class="container">
          <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 5px; padding-right: 5px; padding-top: 35px;">
            <asp:Button ID="btnExportExcel" runat="server" Text=" Export To Excel" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Large" ForeColor="White" Width="200px" Height="50px" 
                ToolTip="Export To Excel"  OnClick="btnExportExcel_Click"
                />
          
        </div>
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
                                            <legend class="fs-border">All Safety Observation Forms </legend>
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

                                                <%--<asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True" Visible="false"
                                               AutoGenerateColumns="false"   CellPadding="4" CellSpacing="1"      OnItemDataBound="GrPast_ItemDataBound" Font-Size="Larger"
                                                   OnItemCommand="GrPast_ItemCommand">
                                                <HeaderStyle CssClass="columnhead"     HorizontalAlign="Center"   ></HeaderStyle>
                                                <Columns>
        
                                                    <asp:TemplateColumn HeaderText="Attachment" HeaderStyle-HorizontalAlign="Center" >
                                                        <ItemTemplate>
                                                            <span class="buttonColumn">
                                                    <asp:LinkButton ID="lnkDoc" runat="server" ToolTip="Click to view the document in a new window"  CommandName="View" CommandArgument="View" Text=" "></asp:LinkButton> </span>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:BoundColumn    DataField="NearMissSecurityID" Visible="true" HeaderText="Form ID"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CreatedBy" HeaderText="Reported By"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="IncidentDateTime" HeaderText="Time of Event"></asp:BoundColumn>
                                                     <asp:BoundColumn DataField="ContactInfo" HeaderText="Contact Info"></asp:BoundColumn>
                                                   
                                                     
                                                      <asp:BoundColumn DataField="Location"  HeaderText="Location"></asp:BoundColumn>
                                                      <asp:BoundColumn DataField="Task"  HeaderText="Task"></asp:BoundColumn>
                                                      <asp:BoundColumn DataField="Description"  HeaderText="Description"></asp:BoundColumn>
                                                      <asp:BoundColumn DataField="AdditionalInfo"  HeaderText="Additional Info"></asp:BoundColumn>
                                                       <asp:BoundColumn DataField="ResolvedText" HeaderText="Resolved?"></asp:BoundColumn>
                                                      <asp:BoundColumn DataField="ResolvedComments" HeaderText="ResolvedComments?"  Visible="true"></asp:BoundColumn>
                                                       <asp:TemplateColumn HeaderText="Download" HeaderStyle-HorizontalAlign="Center" Visible="true">
                                                        <ItemTemplate>
                                                            <span class="buttonColumn">
                                                               <%-- <asp:ImageButton ID="btnDownload" runat="server" CausesValidation="false" CommandName="download" 
                                                                    ImageUrl="~/Images/download.png" Width="25px" Height="25px" ToolTip="Download this entry" OnClick="btnDownload_Click"
                                                                     AlternateText="Download This Entry"></asp:ImageButton>--%>
                                                                <%-- <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" 
                                                                      ImageUrl="~/Images/Delete.png" Width="25px" Height="25px" ToolTip="Delete this entry"
                                                                      AlternateText="Delete This Entry"></asp:ImageButton>
                                                                  <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" 
                                                                      ImageUrl="~/Images/Modify.png" Width="25px" Height="25px" ToolTip="Edit this entry" 
                                                                      AlternateText="Edit This Entry"  OnClientClick='<%# "openPopup(" + Eval("NearMissSecurityID") + "); return false;" %>' />

                                                                 
                                                            </span>
         
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>

                                                    <asp:BoundColumn DataField="AttachmentID" Visible="false" HeaderText="AttachmentID"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="Attachments" Visible="false" HeaderText="Attachments"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="personroleid" Visible="false" HeaderText="personroleid"></asp:BoundColumn>
                                                    </Columns>
                                            </asp:DataGrid>--%> 
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