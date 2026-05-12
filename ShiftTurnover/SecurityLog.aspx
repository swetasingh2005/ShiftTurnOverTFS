<%@ Page Title="Shift Turnover Security Log " Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="SecurityLog.aspx.cs" Inherits="ShiftTurnover.SecurityLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
            function confirmBox() {
                return window.confirm("Are you sure you want to delete this Live Log entry?");
            }

      </script>  
    <script type="text/javascript">
        function confirmBox2() {
            return window.confirm("The selected entry and attached document will be deleted permanently! Are you sure you want to delete this entry?");
        }
    </script>
</asp:Content>
  
<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">
     <div runat="server" id="divExisting" class="instructions">
        <h1>
            <asp:Label runat="server" ID="lblPreShift" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>

        <h1>
            <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label></h1>
        <strong>Security Log Instructions</strong>: Enter Event Date/Time and Details below and click the Submit button. 
         
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
         BorderWidth="0" Width="860" HorizontalAlign="Center" >

        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlLiveLog">
                    <fieldset class="fs-border">
                        <legend class="fs-border">Security Log Submission Form</legend>
                        <asp:Table ID="tblSelectSection" runat="server"  CellPadding="20" CellSpacing="4" BackColor="#e2e2e2" BorderWidth="1">
                             <asp:TableRow>
                            <asp:TableCell   ColumnSpan="3">
                            <asp:Label ID="lblLiveLogID1" runat="server" Font-Bold="true" Text="Enter New Security Log" ForeColor="Green" ></asp:Label>
                             <asp:Label ID="lblLiveLogID" runat="server" Font-Bold="true"   ForeColor="Green" ></asp:Label>
                            </asp:TableCell>
                                 <asp:TableCell ></asp:TableCell>
                                 <asp:TableCell ></asp:TableCell>
                             </asp:TableRow>
                            <asp:TableRow>
                            <asp:TableCell  Width="250px" BorderWidth="0" ColumnSpan="1">
                                     <asp:Label Font-Bold="true" runat="server">Date/Time:<img height="12" alt="Required" src="Images/icon_required.gif"
                                        width="13" />
                                   </asp:Label>

                                    <asp:TextBox ID="txtDateTime" TabIndex="4" runat="server" 
                                        Width="250px" TextMode="DateTimeLocal" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"
                                        ValidationGroup="vgLiveLog" ControlToValidate="txtDateTime"
                                        ErrorMessage="Required" ID="rfvYN" runat="server"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                            <asp:TableCell ColumnSpan="2" Width="550px"  BorderWidth="0" >
                                       <asp:Label Font-Bold="true" runat="server">Enter a new event below:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                    <asp:TextBox ID="txtEvent" TabIndex="11" runat="server" Columns="80" Rows="4" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ValidationGroup="vgLiveLog" ControlToValidate="txtEvent" ErrorMessage="Event is Required" ID="RequiredFieldValidator1" runat="server"></asp:RequiredFieldValidator>
                                      <br />
                                      <br />
                                      <asp:CheckBox runat="server" ID="chkSvcReq" Text="&nbsp;&nbsp;Requires Service Request" ToolTip="Checking this checkbox will enter Service Request in Maximo" />
                                    </asp:TableCell>
                            <asp:TableCell  Font-Bold="true">
                                       
                                    </asp:TableCell>
                                
                                </asp:TableRow>
                            <asp:TableRow BackColor="#ffffe3" runat="server" ID="trDocument" Visible="true">
            <asp:TableCell ColumnSpan="3">
                <asp:Table ID="tblUploaded" runat="server" 
    CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"  
         BorderWidth="0"  HorizontalAlign="Left">
                    <asp:TableRow>
                                <asp:TableCell  >
                                    <asp:Table ID="tblAttach" runat="server"  CssClass="wiznavbuttons"  
                                        CellPadding="20" CellSpacing="0" BorderWidth="0"   HorizontalAlign="Left">
        <asp:TableRow runat="server" ID="trUpload" Visible="true">
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Upload New Document 
                         
                    </legend>
                   
                    <asp:Label runat="server" ID="lblStatus" Text="" CssClass="alert" />
                    <asp:Table ID="tblDoc" runat="server" CellPadding="20" CellSpacing="4" BorderWidth="0" Width="650px"  >
                       
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top">
                                <asp:Label Font-Bold="true" runat="server">File to Upload:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Display="Dynamic" ValidationGroup="vgAttach" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
                                <br />
                                <asp:FileUpload ID="fileUpload" TabIndex="14" AllowMultiple="false" CssClass="form-control" runat="server" Font-Bold="true" />
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Bottom">
                                <asp:Button ID="btnUpload" TabIndex="15" runat="server" Visible="false" CssClass="form-control" ValidationGroup="vgAttach"
                                    Text="Upload Document" OnClick="btnUpload_Click" BackColor="#b3112c" ForeColor="White" />
                                </asp:TableCell>
                                <asp:TableCell VerticalAlign="Bottom">
                           <asp:Button ID="btnCancel" TabIndex="15" runat="server" CssClass="form-control" ValidationGroup="vgAttach"
                                 CausesValidation="false"   ToolTip="Clear uploaded document"  Text="Cancel Upload" OnClick="btnCancel_Click" BackColor="Green" ForeColor="White" />
                                </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                </fieldset>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
                                </asp:TableCell>
                                
                            </asp:TableRow>
        <asp:TableRow ID="trDoc" runat="server" Visible="false">
            <asp:TableCell  Font-Bold="true">
             <asp:Table ID="Table1" runat="server"  CssClass="wiznavbuttons"  
                                        CellPadding="20" CellSpacing="0" BorderWidth="0"   HorizontalAlign="Left">
        <asp:TableRow>
          
            <asp:TableCell  BorderWidth="0"  HorizontalAlign="Left" >
                 Uploaded Documents : 
                  
            
                <asp:LinkButton ID="lnkUploadedDoc" runat="server" ToolTip="Click to view the document in a new window"  OnClick="lnkUploadedDoc_Click"   Text=" "></asp:LinkButton> 
                
                <asp:ImageButton ID="imgDeleteDoc" runat="server" ImageUrl="~/Images/Delete.png" Width="25px" Height="25px" OnClick="imgDeleteDoc_Click" />
            </asp:TableCell>
      </asp:TableRow>
                 </asp:Table>
                </asp:TableCell>
                 </asp:TableRow>
    </asp:Table>
                </asp:TableCell>
                                 <asp:TableCell  Font-Bold="true">
                                       
                                    </asp:TableCell>
                                 <asp:TableCell  Font-Bold="true">
                                       
                                    </asp:TableCell>
             </asp:TableRow>
                             <asp:TableRow>
                                <asp:TableCell ColumnSpan="3">
                                    <asp:Table ID="tblNavButtons" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="570px" HorizontalAlign="Center">
        
                                        <asp:TableRow>
            <asp:TableCell CssClass="left">

            </asp:TableCell>
            <asp:TableCell HorizontalAlign="center">
                <asp:Button ID="btnSubmit" TabIndex="14" runat="server" CssClass="form-control" ToolTip="Submit the form " ValidationGroup="vgLiveLog" CausesValidation="true" Text="Submit" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnSubmit_Click" />
            </asp:TableCell>

            <asp:TableCell HorizontalAlign="center">
                
            </asp:TableCell>
            <asp:TableCell CssClass="right">
              <asp:Button ID="btnReset" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog" ToolTip="Clear all the form controls including document"
                  CausesValidation="false" Text="Cancel" BackColor="Green" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnGoBack_Click" />
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
            <asp:TableCell ColumnSpan="3">
                <asp:Table ID="tblPastEvents" runat="server"
        CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"  
         BorderWidth="0" Width="860" HorizontalAlign="Left">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Past 30  Security Logs</legend>
                    <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0" HorizontalAlign="Center" Width="960">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblPast" runat="server">There are no reported past Security logs in last 30 days.</asp:Label>
                                <asp:Panel ID="pnlPast" runat="server">
                                    <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                   AutoGenerateColumns="false"  BorderColor="Black" CellPadding="4" CellSpacing="0"
                                                     OnItemDataBound="GrPast_ItemDataBound" OnItemCommand="GrPast_ItemCommand1">
                                                    <HeaderStyle CssClass="columnhead" HorizontalAlign="Center"></HeaderStyle>
                                                    <Columns>
                                                        
                                                        <asp:TemplateColumn HeaderText="Attachment" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <span class="buttonColumn">
                                                        <asp:LinkButton ID="lnkDoc" runat="server" ToolTip="Click to view the document in a new window"  CommandName="View" CommandArgument="View" Text=" "></asp:LinkButton> </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="SecurityLogID" Visible="true" HeaderText="Security Log ID"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Reported By" HeaderText="Reported By"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Time of Event" HeaderText="Time of Event"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="Description"  HeaderText="Description"></asp:BoundColumn>
                                                          <asp:BoundColumn  DataField="Deleted"  HeaderText="Deleted"></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <span class="buttonColumn">
                                                   
                                                                   
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" 
                                                                        ImageUrl="~/Images/Delete.png" Width="25px" Height="25px" ToolTip="Delete this entry"
                                                                        AlternateText="Delete This Entry"></asp:ImageButton>
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" 
                                                                        ImageUrl="~/Images/Modify.png" Width="25px" Height="25px" ToolTip="Edit this entry"
                                                                        AlternateText="Edit This Entry"></asp:ImageButton>
                                                                </span>
                                                                 
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="AttachmentID" Visible="true" HeaderText="AttachmentID"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Attachments" Visible="false" HeaderText="Attachments"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="personroleid" Visible="false" HeaderText="personroleid"></asp:BoundColumn>
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
             <asp:TableCell></asp:TableCell>
             <asp:TableCell></asp:TableCell>
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
 

