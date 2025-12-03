<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NearMissSecurity.aspx.cs"  MasterPageFile="~/ShiftTurnover.Master" Inherits="ShiftTurnover.NearMissSecurity" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>
 


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   

</asp:Content>

 


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <div class="container-fluid" >
      <h1>Safety Observation Report  </h1>
            <a target="_blank" style="font-size:medium;" href= "<%= ResolveUrl(lblGuide.Text) %>"> Users Guide for Safety Observation Form </a>
<asp:Label ID="lblGuide" runat="server" Visible="false"></asp:Label>  
        <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
         <div class="form-group">
    <asp:Label ID="lblResult" runat="server"  Font-Bold="true" Font-Size="Larger" /><br />
                  <asp:Button ID="btnPrintPDF" runat="server" CssClass="form-control" Text="Print Form" BackColor="#b3112c" ForeColor="White" Width="100%" Visible="false"
OnClick="btnPrintPDF_Click" /><br />
      </div>
         </asp:Panel>
         <asp:Panel ID="pnlSubmission" runat="server" Visible="true">
               <div class="form-group" style="background-color:antiquewhite; padding-left:10px;padding-top:10px;padding-bottom:10px;" >
    <label   >
      <strong>    A near-miss is a potential hazard or incident in which no property was damaged and no personal injury was sustained, but where, given a slight shift in time or position, damage or injury easily could have occurred.
        <br />    *Note-If property was damaged or injury did occur, please reach out to the nearest supervisor to complete the <a href="https://nih.sharepoint.com/:w:/s/ORS-ORF-CUPWeeklySchedulingMeetings-Safety/IQABTMvbM_6WTood-qZQ_-sqAfH9nbsqhd6N1EPTQa1fJms?e=gpG20H" target="_blank"> Incident Investigation Form </a>  for Supervisors.
         
          </strong>
        </label>
                  
                 
               </div>
      <div class="form-group" >
        <label for="txtReporterName" >1. Reporter Name:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>


        <asp:TextBox ID="txtReporterName" runat="server" CssClass="form-control" />
           <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"   ControlToValidate="txtReporterName" ErrorMessage="Required" ID="RequiredFieldValidator1" runat="server"></asp:RequiredFieldValidator>
                             
      </div>
               <div class="form-group" >
    <label for="txtContactInfo" >2. Contact Information: </label>


    <asp:TextBox ID="txtContactInfo" runat="server" CssClass="form-control" />
                               
  </div>
               <div class="form-group">
    <label for="txtLocation">3. Location of Near Miss: </label>
    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" />
      
  </div>
      <div class="form-group">
        <label for="txtIncidentDateTime">4. Date/Time of Incident:       </label>
        <asp:TextBox ID="txtIncidentDateTime" runat="server" CssClass="form-control"
                     TextMode="DateTimeLocal" />
            
      </div>
              <div class="form-group">
   <label for="txtDescription">5. Please describe in detail the potential incident/hazard/concern that was witnessed: <img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></label>
   <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                CssClass="form-control" Rows="5" />
     <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"   ControlToValidate="txtDescription" ErrorMessage="Required" ID="RequiredFieldValidator4" runat="server"></asp:RequiredFieldValidator>
    
 </div>
      

      

      <div class="form-group">
        <label for="rdoResolved">6. Was the incident resolved? :  </label>
            <asp:RadioButtonList runat="server"  ID="rdoResolved" RepeatDirection="Horizontal">
    <asp:ListItem Value="1" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
    <asp:ListItem Value="0" Text="&nbsp;No&nbsp;" Selected="True"></asp:ListItem>
    
</asp:RadioButtonList>
      </div>

    

      <div class="form-group">
        <label for="txtTask">7. Please explain how the issue was resolved or the recommended actions to resolve:</label>
        <asp:TextBox ID="txtTask" runat="server" TextMode="MultiLine"
                CssClass="form-control" Rows="5"  />
      </div>

     

      <div class="form-group">
        <label for="txtWitnesses">8. Additional information, witnesses, etc.:</label>
        <asp:TextBox ID="txtWitnesses" runat="server" TextMode="MultiLine"
                CssClass="form-control" Rows="5" />
      </div>
               <div class="form-group">
    <label for="rblNearMiss">9. Near Miss?:</label>
    <asp:RadioButtonList ID="rblNearMiss" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Value="1" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
    <asp:ListItem Value="0" Text="&nbsp;No&nbsp;" Selected="True"></asp:ListItem>
    
</asp:RadioButtonList>
  </div>
       
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
                   
                    <asp:TableRow >
                        <asp:TableCell VerticalAlign="Top">
                            <asp:Label Font-Bold="true" runat="server">File to Upload:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Display="Dynamic" ValidationGroup="vgAttach" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
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
             Attachement : 
              
        
            <asp:LinkButton ID="lnkUploadedDoc" runat="server" ToolTip="Click to view the document in a new window"  OnClick="lnkUploadedDoc_Click"  Text=" "></asp:LinkButton> 
            
            <asp:ImageButton ID="imgDeleteDoc" runat="server" ImageUrl="~/Images/Delete.png" Width="25px" Height="25px" OnClick="imgDeleteDoc_Click" />
        </asp:TableCell>
  </asp:TableRow>
             </asp:Table>
            </asp:TableCell>
             </asp:TableRow>
</asp:Table>
                                 <asp:Table ID="Table2" runat="server" 
CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"   
     BorderWidth="1"  HorizontalAlign="Left">
                                     <asp:TableRow BackColor="antiquewhite"  >
            <asp:TableCell  ColumnSpan="2"  >
                                   <label >
                    <strong>   Please submit this form to the main office or your supervisor. For questions or cases deemed immediately dangerous, call the Shift Supervisor or the Control Room: (301) 594-1175
immediately dangerous, call the Shift Supervisor or the Control Room: (301) 594-1175</strong>
                   </label>
                </asp:TableCell>
                                         <asp:TableCell></asp:TableCell>
                                         </asp:TableRow>
                                                              <asp:TableRow BackColor="antiquewhite">
<asp:TableCell ColumnSpan="2">
     <label>
   <strong>   For anonymous Safety reporting, please scan the QR code here: </strong> <img height="90" width="90" alt="Required" src="Images/Barcode.png"    />
 </label>
                </asp:TableCell>
                                                                   <asp:TableCell HorizontalAlign="Left">
                                                                       
                                                                   </asp:TableCell>
</asp:TableRow>
    
                <asp:TableRow>
                            <asp:TableCell HorizontalAlign="Center"  >
      <asp:Button ID="btnSubmit" runat="server" CssClass="form-control" Text="Submit Form" BackColor="#b3112c" ForeColor="White"   Width="280"
                  OnClick="btnSubmit_Click" /> 
       </asp:TableCell> <asp:TableCell HorizontalAlign="Left">
               <asp:Button ID="btnClearForm" runat="server" CssClass="form-control" Text="Clear Form" BackColor="#b3112c" ForeColor="White"   Width="280"
                  OnClick="btnCancel_Click" />
                                </asp:TableCell>
                    </asp:TableRow>
                                     </asp:Table>
             </asp:Panel>
                      
                          
      
    </div>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_footer" runat="server">
    </asp:Content>