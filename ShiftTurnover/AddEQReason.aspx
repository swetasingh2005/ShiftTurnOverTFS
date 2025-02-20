<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="AddEQReason.aspx.cs" Inherits="ShiftTurnover.AddEQReason" %>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
     <script type="text/javascript">
    $(document).ready(function () {
        $('#btn').click(function () {
            window.opener.location.reload(true);
            window.close();
        });
    });
</script>
       
   
    <asp:Table ID="tblHistory" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
         
                <asp:TableRow>
                      <asp:TableCell CssClass="tableheader"  ></asp:TableCell>
                    <asp:TableCell  HorizontalAlign="Left" CssClass="tableheader" >
                     &nbsp;&nbsp;&nbsp;Add Reason For PI Equipment Status Change in Last 12 hours 
                   
                        </asp:TableCell>
                </asp:TableRow>
         <asp:TableRow>
            
              <asp:TableCell Font-Bold="true">Tag:
               <asp:Label runat="server" ID="lblParent" Visible="true" CssClass="form-control"
                        Font-Bold="true" ForeColor="Green"></asp:Label>
            </asp:TableCell>
               <asp:TableCell Font-Bold="true"> 
                
            </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
            
              <asp:TableCell Font-Bold="true">Last Reason Added By:
               <asp:Label runat="server" ID="lblReasonAddedBy" Visible="true" CssClass="form-control" Font-Bold="true" ForeColor="Green"></asp:Label>
            </asp:TableCell>
               <asp:TableCell Font-Bold="true">Last Reason Added Date:
               <asp:Label runat="server" ID="lblReasonAddedDate" Visible="true" CssClass="form-control" Font-Bold="true" ForeColor="Green"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
                <asp:TableRow>
                    
                    <asp:TableCell  HorizontalAlign="Left" ColumnSpan="2">
 <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="3"  Width="100%"  MaxLength="960" Font-Bold="true" Font-Size="Large" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvReason" runat="server" ControlToValidate="txtReason" ErrorMessage="Please add your Reason" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell  HorizontalAlign="Left" ></asp:TableCell>
                </asp:TableRow>
          <asp:TableRow>
              <asp:TableCell  HorizontalAlign="Left" ></asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
        <asp:Label runat="server" CssClass="alert" ID="lblConfirm" Visible="false" />
                         </asp:TableCell>
                </asp:TableRow>
          <asp:TableRow>
              <asp:TableCell  HorizontalAlign="Left" >
                   <asp:Button ID="Button1" runat="server" Text="Submit Reason " OnClick="Button1_Click" CssClass="form-control" CausesValidation="true"  BackColor="#b3112c" ForeColor="White" Width="180px" TabIndex ="1" />
              </asp:TableCell>
             <asp:TableCell>
                
         <asp:Button ID="Button2" runat="server" Text=" Go Back " OnClick="Button2_Click"  CssClass="form-control" CausesValidation="false"  BackColor="#b3112c" ForeColor="White" Width="180px" TabIndex ="1" />
                 </asp:TableCell>
              </asp:TableRow>
        </asp:Table>
  
     
</asp:Content>


