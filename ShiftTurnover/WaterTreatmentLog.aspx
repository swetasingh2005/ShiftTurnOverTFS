<%@ Page Title="Shift Turnover Water Treatment Log " Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="WaterTreatmentLog.aspx.cs" Inherits="ShiftTurnover.WaterTreatmentLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function confirmBox() {
            return window.confirm("The selected live log entry and all it's related document will be deleted permanently!Are you sure you want to delete this Live Log entry?");
        }

    </script>
    <script type="text/javascript">
        function confirmBox2() {
            return window.confirm("The selected document will be deleted permanently! Are you sure you want to delete this document?");
        }
    </script>
      <style>
        .ChkBoxClass input {
            width: 25px;
            height: 25px;
        }
    </style>
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            var gridTable = $('#<%= GrPast.ClientID%>').prepend($("<thead></thead> <tfoot></tfoot>").append($("#<%= GrPast.ClientID%>").find("tr:first")));

            $('#cph_main_GrPast tfoot tr td').each(function () {
                var title = $(this).text();
                $(this).html('<input type="text" id=' + title.replace(" ", "_") + ' placeholder="Search ' + title + '" />');
            });

            gridTable.DataTable({
                "columnDefs": [
                    { "type": "date", "targets": [3] }
                ],
                //"ordering": false,
                "initComplete": function () {
                    // Apply the search
                    this.api().columns().every(function () {
                        var that = this;
                        $('input', this.footer()).on('keyup keypress change clear', function () {
                            if (that.search() !== this.value) {
                                that
                                    .search(this.value)
                                    .draw();
                            }
                        });
                    });
                }
            });

            $('#Actions').hide(); // Hide the last column input box

            // Move search box to header
            var r = $('#cph_main_GrPast' + ' tfoot tr');
            $('#cph_main_GrPast thead').append(r);


        });
    </script>

    <style>
        thead input {
        width: 100%;
        padding: 3px;
        box-sizing: border-box;
    }
    </style>
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
    <asp:Table ID="tblLiveLog" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"
        BorderWidth="0" Width="960" HorizontalAlign="Center">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlLiveLog">
                    <fieldset class="fs-border">
                        <legend class="fs-border">Water Chemistry Live Log Submission Form</legend>
                        <asp:Table ID="tblSelectSection" runat="server" CellPadding="20" CellSpacing="4" BackColor="#e2e2e2" BorderWidth="0">
                            <asp:TableRow Visible="false">
                                <asp:TableCell BorderWidth="0">
                                    <asp:Label Font-Bold="true" runat="server">Rounds:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgAttach" ControlToValidate="drpAttachmentType" ErrorMessage="Required" ID="RequiredFieldValidator2" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator><br />
                                    <br />
                                    <asp:DropDownList ID="drpAttachmentType" AutoPostBack="true" OnSelectedIndexChanged="drpAttachmentType_SelectedIndexChanged"
                                        TabIndex="3" runat="server" Width="260px" CssClass="form-control">
                                    </asp:DropDownList>


                                </asp:TableCell>

                                <asp:TableCell BorderWidth="0">

                                    <asp:Label Font-Bold="true" runat="server">Sections:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgAttach" ControlToValidate="drpAttachmentType" ErrorMessage="Required" ID="RequiredFieldValidator3" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator><br />
                                    <br />
                                    <asp:DropDownList ID="ddlSection" TabIndex="3" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSection_SelectedIndexChanged" runat="server" Width="260px" CssClass="form-control">
                                    </asp:DropDownList>


                                </asp:TableCell>
                                <asp:TableCell BorderWidth="0">

                                    <asp:Label Font-Bold="true" runat="server">Parameter:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgAttach" ControlToValidate="drpAttachmentType" ErrorMessage="Required" ID="RequiredFieldValidator5" runat="server" InitialValue="-SELECT-"></asp:RequiredFieldValidator><br />
                                    <br />
                                    <asp:DropDownList ID="ddlAttribute" TabIndex="3" runat="server" Width="220px" CssClass="form-control"></asp:DropDownList>


                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label Font-Bold="true" runat="server">Date/Time:<img height="12" alt="Required" src="Images/icon_required.gif"
                                        width="13" />
                                    </asp:Label>

                                    <asp:TextBox ID="txtDateTime" TabIndex="4" runat="server"
                                        Width="250px" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"
                                        ValidationGroup="vgLiveLog" ControlToValidate="txtDateTime"
                                        ErrorMessage="Required" ID="rfvYN" runat="server"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                                <asp:TableCell Font-Bold="true">
                                    <asp:CheckBox runat="server" ID="chkSvcReq" CssClass="ChkBoxClass" Text="Requires Service Request" ToolTip="Checking this checkbox will enter Service Request in Maximo" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label Font-Bold="true" runat="server">Enter a new event below:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                    <asp:TextBox ID="txtEvent" TabIndex="11" runat="server" Columns="50" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgLiveLog" ControlToValidate="txtEvent" ErrorMessage="Event is Required" ID="RequiredFieldValidator1" runat="server"></asp:RequiredFieldValidator>

                                </asp:TableCell>


                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="3">
                                    <asp:Table ID="tblNavButtons" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="570px" HorizontalAlign="Center">
                                        <asp:TableRow>
                                                       <asp:TableCell CssClass="right">
                                                <asp:Button ID="btnGoBack" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog"
                                                    CausesValidation="false"
                                                   Visible="false"  Text="Go Back to Chem Entry Page" BackColor="#b3112c" ForeColor="White" Width="250px" Font-Bold="false"
                                                    OnClick="btnGoBack_Click" />

                                            </asp:TableCell>
                                            <asp:TableCell CssClass="left">

                                            </asp:TableCell>
                                            <asp:TableCell HorizontalAlign="center">
                                                <asp:Button ID="btnSubmit" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog" CausesValidation="true" Text="Submit Live Log" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnSubmit_Click" />
                                            </asp:TableCell>

                                            <asp:TableCell HorizontalAlign="center">
                             <asp:Button ID="btnReset" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog" ToolTip="Clear all the form controls including document"
     CausesValidation="false" Text="Cancel" BackColor="Green" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnReset_Click" />
                                                
                                                <asp:Button ID="btnLiveLogSearch" Visible="false" TabIndex="18" runat="server" CssClass="form-control" OnClick="btnLiveLogSearch_Click" Text=" Live Log Search" BackColor="#b3112c" ForeColor="White" Width="160px" Font-Bold="false" CausesValidation="false" />
                                            </asp:TableCell>
                                 
                                        </asp:TableRow>
                                    </asp:Table>
                                </asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                                <asp:TableCell></asp:TableCell>
                            </asp:TableRow>

                        </asp:Table>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell><hr /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow BackColor="#ffffe3" runat="server" ID="trDocument" Visible="false" BorderWidth="1">
            <asp:TableCell>
                <asp:Table ID="tblUploaded" runat="server"
                    CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"
                    BorderWidth="0" Width="860" HorizontalAlign="Left">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Table ID="tblAttach" runat="server" CssClass="wiznavbuttons"
                                CellPadding="20" CellSpacing="0" BorderWidth="0" Width="860" HorizontalAlign="Left">
                                <asp:TableRow runat="server" ID="trUpload" Visible="true">
                                    <asp:TableCell>
                                        <fieldset class="fs-border">
                                            <legend class="fs-border">Upload New Document for Live Log ID =
                          <asp:Label ID="lblLiveLogID1" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label></legend>

                                            <asp:Label runat="server" ID="lblStatus" Text="" CssClass="alert" />
                                            <asp:Table ID="tblDoc" runat="server" CellPadding="20" CellSpacing="4" BorderWidth="0">

                                                <asp:TableRow>
                                                    <asp:TableCell VerticalAlign="Top">
                                                        <asp:Label Font-Bold="true" runat="server">File to Upload:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ValidationGroup="vgAttach" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
                                                        <br />
                                                        <asp:FileUpload ID="fileUpload" TabIndex="14" AllowMultiple="false" CssClass="form-control" runat="server" Font-Bold="true" />
                                                    </asp:TableCell>
                                                     
                            <asp:TableCell >
                                <asp:Label Font-Bold="true" runat="server">Document Description:</asp:Label>
                                <asp:TextBox runat="server" ID="txtDescription" TabIndex="7" Columns="30" Rows="2" TextMode="MultiLine" CssClass="form-control" />
                            </asp:TableCell>
                                   <asp:TableCell VerticalAlign="Bottom">
                                                        <asp:Button ID="btnUpload" TabIndex="15" runat="server" CssClass="form-control" ValidationGroup="vgAttach"
                                                            Text="Upload" OnClick="btnUpload_Click" BackColor="#b3112c" ForeColor="White" />
                                                    </asp:TableCell>
                                                    <asp:TableCell VerticalAlign="Bottom">
                                                        <asp:Button ID="btnCancel" TabIndex="15" runat="server" CssClass="form-control" ValidationGroup="vgAttach"
                                                            CausesValidation="false" Text=" Close " OnClick="btnCancel_Click" BackColor="Green" ForeColor="White" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>

                                        </fieldset>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>

                    </asp:TableRow>
                    <asp:TableRow >
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">&nbsp;&nbsp;&nbsp;&nbsp;Total No. Uploaded Documents : 
                        <asp:Label ID="lblNoOfDoc" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>

                                </legend>
                                <asp:Table ID="Table3" runat="server" CellPadding="20" CellSpacing="4" BorderWidth="0" Width="860"
                                    HorizontalAlign="Left">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblActive" runat="server">There are no documents uploaded.</asp:Label>
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
                                                                                <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="View" ImageUrl="~/Images/pdf_icon.gif"
                                                                                    AlternateText="View This Document"></asp:ImageButton>
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="~/Images/Close.png"
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
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Table ID="tblPastEvents" runat="server"
                    CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"
                    BorderWidth="0" Width="1060" HorizontalAlign="Left">
                    <asp:TableRow>
                        <asp:TableCell>
                            <fieldset class="fs-border">
                                <legend class="fs-border">Past 30 Days Live Logs</legend>
                                <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0" HorizontalAlign="Center" Width="1060">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <asp:Label ID="lblPast" runat="server">There are no reported past logs in last 30 days.</asp:Label>
                                            <asp:Panel ID="pnlPast" runat="server">
                                                <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                    <asp:TableRow>
                                                        <asp:TableCell>
                                                            <asp:DataGrid ID="GrPast" runat="server" EnableViewState="true" AutoGenerateColumns="false"
                                                                HeaderStyle-BackColor="#cee7ff" HeaderStyle-Font-Bold="true"
                                                                HeaderStyle-ForeColor="White"  HeaderStyle-Font-Size="Medium"
                                                                          CssClass="table table-striped" Style="width: 100%">
                                                                <HeaderStyle CssClass="columnhead" HorizontalAlign="left"></HeaderStyle>
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="Actions" HeaderStyle-HorizontalAlign="Center">
                                                                        <ItemTemplate>
                                                                            <span class="buttonColumn">
                       

                                                                               <asp:ImageButton ID="btnDoc" runat="server" Width="25px" Height="25px" CommandName="Add" ImageUrl="~/Images/pdf_icon.gif"
                                                                        AlternateText="Add View Attachments" CausesValidation="false"></asp:ImageButton>
                                                                            
                                                                    <asp:ImageButton ID="btnDeleteEntry" runat="server" Width="25px" Height="25px"  CommandName="Delete" ImageUrl="~/Images/Close.png"
                                                                        AlternateText="Delete This Entry" CausesValidation="false"></asp:ImageButton>
                                                                </span>
                                                                           
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                    <asp:BoundColumn DataField="LiveLogID" HeaderText="Live  <br />  LogID" Visible="false" >
                                                                       
                                                                    </asp:BoundColumn>
                                                                      <asp:BoundColumn DataField="Time of Event" HeaderText="Time <br />   of  <br />  Event" >
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Deleted" HeaderText="Deleted" Visible="false"   >
                                                                        <ItemStyle Wrap="True" Width="2px" />
                                                                    </asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="Description" HeaderText="Description" >
                                                                        <HeaderStyle Width="550px" />
                                                                       <ItemStyle Width="550px" />
                                                                    </asp:BoundColumn>
                                                                    
                                                                   
                                                                     <asp:BoundColumn DataField="Reported By" HeaderText="Reporter" >
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundColumn>
                                                                  
                                                                     <asp:BoundColumn DataField="Service Request" HeaderText="SR " >
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="No of Attachments" HeaderText="No  <br />   of <br />   Doc" >
                                                                        <ItemStyle Wrap="True" />
                                                                    </asp:BoundColumn>
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
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell><hr /></asp:TableCell>
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
 

