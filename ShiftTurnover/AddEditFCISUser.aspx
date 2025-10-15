<%@ Page Language="C#" Title=""  AutoEventWireup="true" MasterPageFile="~/ShiftTurnoverAdmin.Master" CodeBehind="AddEditFCISUser.aspx.cs" Inherits="ShiftTurnover.AddEditFCISUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style type="text/css">
            .bigCheckBox {
    font-size: 18px; /* Bigger text */
}
.bigCheckBox input {
    width: 30px;   /* Bigger box */
    height: 30px;
}
        .h1 {
    text-align: center;
    font: bold 2em Myriad, Myriad Web, Verdana, Arial, Helvetica, sans-serif;
    color: #9A0000;
    line-height: 1.3em;
    margin-top: 0.7em;
    margin-bottom: 0.7em;
    padding-left: 3em;
}
        </style>
      <style type="text/css">
          .description-label {
    display: block;          /* makes it behave like a paragraph */
    white-space: pre-wrap;   /* preserves line breaks and wrapping */
    word-wrap: break-word;   /* prevents long words from overflowing */
    min-height: 100px;       /* optional: gives space for multiple lines */
}
	   .invoice-container {
      	max-width: 100%;
      	margin-left:0px;
        margin-right:0px;
      	padding: 0px;
      	border: 1px solid #eee;
      	/*box-shadow: 0 0 10px rgba(0, 0, 0, .15);*/
      	font-size: 16px;
      	line-height: 15px;
        border:1px solid black;
      	font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
      	color: black;
      }
      .invoice-container table {
      	width: 100%;
      	line-height: inherit;
      	text-align: left;
   
      }
      .invoice-container table td {
       
      	padding: 3px;
      	vertical-align: top;
      }
      .invoice-container table tr td:nth-child(2) {
      	text-align: right;
      }
      .invoice-container table tr.top table td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.top table td.title {
      	font-size: 20px;
      	/*line-height: 8px;*/
      	color: black;
        font-weight:bold;
         background-color:white;
        text-align:center;
         vertical-align:top;
      }
        .invoice-container table tr.top table td.subtitle {
      	font-size: 12px;
      	/*line-height: 6px;*/
      	color: black;
        font-weight:bold;
         background-color:white;
      }
          .invoice-container table tr.section {
      	font-size: 14px;
      	line-height: 20px;
      	color: black;
        font-weight:bold;
         background-color:white;
      }
      .invoice-container table tr.information table td {
      	padding-bottom: 40px;
      }
      .invoice-container table tr.heading td {
      	background: #eee;
      	border-bottom: 1px solid #ddd;
      	font-weight: bold;
      }
      .invoice-container table tr.details td {
      	padding-bottom: 20px;
      }
      .invoice-container table tr.item td {
      	border-bottom: 1px solid #eee;
      }
      .invoice-container table tr.item.last td {
      	border-bottom: none;
      }
      .invoice-container table tr.total td:nth-child(2) {
      	border-top: 2px solid #eee;
      	font-weight: bold;
      }
      @media only print {
      	.invoice-container table tr.top table td {
      		width: 100%;
      		display: block;
      		text-align: center;
      	}
      	.invoice-container table tr.information table td {
      		width: 100%;
      		display: block;
      		text-align: center; 
            
      	}
          /*@media print {
            .page-break { display: block; page-break-before: always; }
            }*/
      }
	</style>
    <script type="text/javascript">
        function validateForm() {
            var name = document.getElementById('<%= txtLastName.ClientID %>').value.trim();
            var chkResolve = document.getElementById('<%= txtLastName.ClientID %>');
            // Require checkbox to be checked
            if (!chkResolve.checked) {
                alert("Please check Resolve!");
                chkResolve.focus();
                return false;
            }
            if (name === "") {
                alert("Please enter Resolve Comments!");
                return false; // prevent button action
            }
            return true; // allow button action
        }
    </script>
    <script type="text/javascript">
        function closeAndRefreshParent() {
            window.opener.location.reload(); // Refresh the parent window
            window.close(); // Close the child window
        }
        function printPage() {
            window.print();
        }
        function confirmBox() {
            return window.confirm(" Are you sure you want to resolve this report?");
        }
   
    </script>
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>
 <asp:Content ID="Content10" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" Width="50%">
                <asp:Literal runat="server" ID="litBack"></asp:Literal>
            </asp:TableCell>
            <asp:TableCell Visible="false" HorizontalAlign="Right" Width="50%">
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_main" runat="server">
                          <div class="container-fluid" >
      <h1><asp:Literal runat="server" ID="lblTitle"></asp:Literal></h1>
       
          <div>
                                                         
             <asp:Table ID="tblPastEvents" runat="server"
                      CellPadding="4" CellSpacing="4"     Font-Bold="true" BackColor="#cccccc"
                      BorderWidth="0" Width="100%" HorizontalAlign="Left">
                 <asp:TableRow>
                 <asp:TableCell>Last Name: </asp:TableCell>
                 <asp:TableCell> <asp:TextBox ID="txtLastName" runat="server" MaxLength="255"></asp:TextBox>
                     <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"  
                         ControlToValidate="txtLastName" ErrorMessage="Last Name is Required" ID="RequiredFieldValidator1" runat="server">
                     </asp:RequiredFieldValidator>
                 </asp:TableCell>
                 
                <asp:TableCell>First Name: </asp:TableCell>
                <asp:TableCell> <asp:TextBox ID="txtFirstName" runat="server" MaxLength="255"></asp:TextBox> 
                        <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"  
                        ControlToValidate="txtFirstName" ErrorMessage="First Name is Required" ID="RequiredFieldValidator2" runat="server">
                    </asp:RequiredFieldValidator>
                </asp:TableCell>
                
                <asp:TableCell>Middle Name: </asp:TableCell>
                <asp:TableCell> <asp:TextBox ID="txtMName" runat="server" MaxLength="255"></asp:TextBox>  </asp:TableCell>
                </asp:TableRow>

                    <asp:TableRow>
               <asp:TableCell>User ID: </asp:TableCell>
               <asp:TableCell> <asp:TextBox ID="txtUserID" runat="server" MaxLength="255"></asp:TextBox>  
                       <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"  
    ControlToValidate="txtUserID" ErrorMessage="User ID is Required" ID="RequiredFieldValidator3" runat="server">
</asp:RequiredFieldValidator>
               </asp:TableCell>
                
              <asp:TableCell>NIH ID: </asp:TableCell>
              <asp:TableCell> <asp:TextBox ID="txtNIHID" runat="server" MaxLength="255"></asp:TextBox> 
                                         <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"  
    ControlToValidate="txtNIHID" ErrorMessage="NIH ID is Required" ID="RequiredFieldValidator4" runat="server">
</asp:RequiredFieldValidator>
              </asp:TableCell>
              
              <asp:TableCell>Email : </asp:TableCell>
              <asp:TableCell> <asp:TextBox ID="txtEmail" runat="server" MaxLength="255"></asp:TextBox>
                  <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert"  
                    ControlToValidate="txtEmail" ErrorMessage="Email is Required" ID="RequiredFieldValidator5" runat="server">
                </asp:RequiredFieldValidator>
              </asp:TableCell>
              </asp:TableRow>

                  <asp:TableRow>
                <asp:TableCell>Phone: </asp:TableCell>
                <asp:TableCell> <asp:TextBox ID="txtPhone" runat="server" MaxLength="255"></asp:TextBox>  </asp:TableCell>
                
                <asp:TableCell>IC: </asp:TableCell>
                <asp:TableCell> <asp:TextBox ID="txtIC" runat="server" MaxLength="255"></asp:TextBox>  </asp:TableCell>
               
                    <asp:TableCell>Organization: </asp:TableCell>
                    <asp:TableCell> <asp:TextBox ID="txtOrg" runat="server" MaxLength="255"></asp:TextBox>  </asp:TableCell>
                    </asp:TableRow>
                  <asp:TableRow>
                 <asp:TableCell>Title: </asp:TableCell>
                 <asp:TableCell> <asp:TextBox ID="txtTitle" runat="server" MaxLength="255"></asp:TextBox>  </asp:TableCell>
                 
           
                       <asp:TableCell>Org Abbr: </asp:TableCell>
                    <asp:TableCell> <asp:TextBox ID="txtOrgAbbr" runat="server" MaxLength="255"></asp:TextBox> </asp:TableCell>
                           <asp:TableCell>Active: </asp:TableCell>
              <asp:TableCell> <asp:CheckBox ID="chkActive" runat="server" /> </asp:TableCell>
                      </asp:TableRow>
  <asp:TableRow>
                                            <asp:TableCell  ></asp:TableCell> <asp:TableCell  ></asp:TableCell><asp:TableCell  ></asp:TableCell>
                       <asp:TableCell  > <asp:Button ID="btnAdd" runat="server" Text=" Edit Person Information " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Large" ForeColor="White" Width="240px" Height="50px" 
                                                 
ToolTip="Edit Person Information"  CausesValidation="true"  OnClick="btnSubmit_Click"  /></asp:TableCell>
       <asp:TableCell  ></asp:TableCell> <asp:TableCell  ></asp:TableCell> 
                </asp:TableRow>
                  <asp:TableRow BackColor="#999999">
<asp:TableCell>Roles: </asp:TableCell>
                     
          <asp:TableCell>   <asp:CheckBoxList ID="chkRoles"      runat="server" ></asp:CheckBoxList>

</asp:TableCell>
                     
             
                                                                       <asp:TableCell ></asp:TableCell>
                       <asp:TableCell  > <asp:Button ID="btnEditRoles" runat="server" Text=" Edit Person Roles " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="Large" ForeColor="White" Width="240px" Height="50px" 
ToolTip="Edit Person Roles"  CausesValidation="true"  OnClick="btnEditRoles_Click"  /></asp:TableCell>
                         <asp:TableCell  BackColor="White"  >
                              <asp:Label runat="server" forcolor="Green"  ID="lblDTRUser"    Font-Bold="true" Font-Size="Large" />
                         </asp:TableCell>
                         <asp:TableCell  BackColor="White">
                             <asp:Label runat="server"  forcolor="Green" ID="lblPIUSer"    Font-Bold="true" Font-Size="Large" />
                             
                         </asp:TableCell>
</asp:TableRow>
                   <asp:TableRow BackColor="white">
                         <asp:TableCell  >
                           
                         </asp:TableCell> 
                       <asp:TableCell  ></asp:TableCell><asp:TableCell  >
                             <asp:Label runat="server"  ID="lblConfirm"    Font-Bold="true" Font-Size="Large" />
                                                                                            </asp:TableCell>
                         <asp:TableCell  >
                                                        
                              </asp:TableCell>
 <asp:TableCell>

    
 </asp:TableCell> <asp:TableCell>

    
 </asp:TableCell>
                    </asp:TableRow>
             </asp:Table>
                                           
              </div>
      
</div>

</asp:Content>
 

