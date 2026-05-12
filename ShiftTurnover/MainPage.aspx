<%@ Page Title="Shift Turnover Live Log" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="ShiftTurnover.MainPage" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var cwin = '';
        function PopupReco(livelogid,shiftID) {
            if (!cwin.closed && cwin.location) {
                cwin.focus();
            } else {
                cwin = window.open("ShowChart.aspx?LID=" + livelogid + "&SID=" + shiftID , "ShowChart",
                    "width=800,height=600,directories=no,location=no,"
                    + "menubar=no,resizable=yes,scrollbars=yes,status=no,toolbar=no");

                cwin.moveTo(150, 150);
            }
        }
        function confirmBox() {
            return window.confirm("Are you sure you want to delete this Live Log entry?");
        }

      </script>  
    <script type="text/javascript">
        function confirmBox2() {
            return window.confirm("The selected document will be deleted permanently! Are you sure you want to delete this document?");
        }
    </script>
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>



<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">

    <div runat="server" id="divExisting" class="instructions">
        <h1>
            <asp:Label runat="server" ID="lblPreShift" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>

        <h1>
            <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>
        <strong>Live Log Instructions</strong>: Enter Event Date/Time and Details below and click the Submit button. Check the "Requires Service Request" checkbox if you wish to create a Service Request in Maximo.
    </div>
    <p>
        &nbsp;<br />
    </p>
</asp:Content>

<asp:Content ID="Content15" ContentPlaceHolderID="cph_error" runat="server">
    <asp:Table ID="tblConfirm" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Label runat="server" CssClass="alert" ID="lblConfirm" Visible="false" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">

    <!---
    <asp:Table ID="tblData" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="300" HorizontalAlign="Left">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Actions</legend>
                    <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                        <asp:TableRow>
                            <asp:TableCell>
                                <ul>
                                    <li>Live Log</li>
                                    <li>Equipment Status</li>
                                    <li>New Shift Turnover Report</li>
                                    <li>Past Shift Turnover Reports</li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </fieldset>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
        --->
    <asp:Table ID="tblLiveLog" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlLiveLog">
                    <fieldset class="fs-border">
                        <legend class="fs-border">Live Log:
                            <asp:Label ID="lblLiveLogID" runat="server" ></asp:Label></legend>
                        
                        <asp:Table ID="tblSelectSection" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <div>Date/Time:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /><br />
                                    </div>

                                    <asp:TextBox ID="txtDateTime" TabIndex="4" runat="server" Width="250px" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgLiveLog" ControlToValidate="txtDateTime" ErrorMessage="Required" ID="rfvYN" runat="server"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <div>Enter a new event below:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></div>
                                    <asp:TextBox ID="txtEvent" TabIndex="11" runat="server" Columns="108" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgLiveLog" ControlToValidate="txtEvent" ErrorMessage="Event is Required" ID="RequiredFieldValidator1" runat="server"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    <asp:CheckBox runat="server" ID="chkSvcReq" Text="&nbsp;&nbsp;Requires Service Request" ToolTip="Checking this checkbox will enter Service Request in Maximo" />
                                </asp:TableCell>
                            </asp:TableRow>

                        </asp:Table>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblNavButtons" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="570px" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell CssClass="left">
              <asp:Button ID="btnCancel" TabIndex="14" runat="server" CssClass="form-control"  CausesValidation="false" Text="Cancel" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnCancel_Click" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="center">
                <asp:Button ID="btnSubmit" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog" CausesValidation="true" Text="Submit" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnSubmit_Click" />
            </asp:TableCell>

            <asp:TableCell HorizontalAlign="center">
                
            </asp:TableCell>
            <asp:TableCell CssClass="right">

            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblAttach" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Upload Document</legend>
                    <asp:Label runat="server" ID="lblStatus" Text="" CssClass="alert" />
                    <asp:Table ID="tblDoc" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top" ColumnSpan="2">
                                <asp:Label Font-Bold="true" runat="server">Document Type:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgAttach" ControlToValidate="drpAttachmentType" ErrorMessage="Required" ID="RequiredFieldValidator2" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator><br />
                                <asp:DropDownList ID="drpAttachmentType" TabIndex="3" runat="server" Width="320px" CssClass="form-control"></asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top" ColumnSpan="2">
                                <asp:Label Font-Bold="true" runat="server">Document Description:</asp:Label>
                                <asp:TextBox runat="server" ID="txtDescription" TabIndex="7" Columns="108" Rows="2" TextMode="MultiLine" CssClass="form-control" />
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top">
                                <asp:Label Font-Bold="true" runat="server">File to Upload:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ValidationGroup="vgAttach" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
                                <br />
                                <asp:FileUpload ID="fileUpload" TabIndex="14" AllowMultiple="false" CssClass="form-control" runat="server" Font-Bold="true" />
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Bottom">
                                <asp:Button ID="btnUpload" TabIndex="15" runat="server" CssClass="form-control" ValidationGroup="vgAttach" Text="Upload" OnClick="btnUpload_Click" BackColor="#b3112c" ForeColor="White" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                </fieldset>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    <asp:Table ID="tblPastEvents" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Past Events</legend>
                    <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0" HorizontalAlign="Center" Width="760">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPast" runat="server">There are no reported past events for this shift.</asp:Label>
                                <asp:Panel ID="pnlPast" runat="server">
                                    <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                    BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                    <HeaderStyle CssClass="columnhead" HorizontalAlign="Center"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Actions" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <span class="buttonColumn">
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="Images/actionbutton_delete_off.gif"
                                                                        AlternateText="Delete This Entry" CausesValidation="false"></asp:ImageButton>
                                                                <asp:ImageButton ID="btnEdit" runat="server" CommandName="Edit" ImageUrl="Images/actionbutton_edit_off.gif"
                                                                        AlternateText="Edit This Entry" CausesValidation="false"></asp:ImageButton>
                                                                     <asp:ImageButton ID="btnView" runat="server" CommandName="View" ImageUrl="~/Images/actionbutton_view_off.gif"
                                                                        AlternateText="View This Entry" CausesValidation="false"></asp:ImageButton>
                                                                </span>
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
    <asp:Table ID="tblUploaded" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Uploaded Documents</legend>
                    <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0" Width="760" HorizontalAlign="Center">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblActive" runat="server">There are no documents uploaded for this shift.</asp:Label>
                                <asp:Panel ID="pnlActive" runat="server">
                                    <asp:Table ID="tblActive" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:DataGrid ID="GrActive" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                    BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                    <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Actions">
                                                            <ItemTemplate>
                                                                <span class="buttonColumn">
                                                                    <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="View" ImageUrl="Images/actionbutton_view_off.gif"
                                                                        AlternateText="View This Document"></asp:ImageButton>
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="Images/actionbutton_delete_off.gif"
                                                                        AlternateText="Delete This Document"></asp:ImageButton>
                                                                </span>
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