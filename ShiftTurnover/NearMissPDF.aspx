<%@ Page Title="" Language="C#" MasterPageFile="~/PrintPDF.Master" AutoEventWireup="true" CodeBehind="NearMissPDF.aspx.cs" Inherits="ShiftTurnover.NearMissPDF" %>
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
            var name = document.getElementById('<%= txtResolveComments.ClientID %>').value.trim();
            var chkResolve = document.getElementById('<%= chkResolve.ClientID %>');
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
<asp:Content ID="Content2" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_title" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_main" runat="server">
                          <div class="container-fluid" >
      <h1> Security Observation Report Form</h1>
       
         <asp:Panel ID="pnlSubmission" runat="server" Visible="true">
                         <div class="form-group">
         
      </div>
      <div class="form-group" >
        <label for="lblReporterName" > Reporter Name:</label>


        <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblReporterName" runat="server" CssClass="form-control" />
                                    
      </div>
               <div class="form-group" >
    <label for="lblContactInfo" > Contact Information: </label>


      <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblContactInfo" runat="server" CssClass="form-control" />
                               
  </div>
               <div class="form-group">
    <label for="lblLocation"> Location of Near Miss: </label>
      <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblLocation" runat="server" CssClass="form-control" />
      
  </div>
      <div class="form-group">
        <label for="lblIncidentDateTime"> Date/Time of Incident:       </label>
          <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblIncidentDateTime" runat="server" CssClass="form-control"
                       />
            
      </div>
              <div class="form-group">
   <label for="lblDescription"> Please describe in detail the potential incident/hazard/concern that was witnessed: </label>
     <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblDescription" runat="server"   
                CssClass="form-control description-label"   />
      
 </div>
      

      


    

      <div class="form-group">
        <label for="lblTask"> Please explain how the issue was resolved or the recommended actions to resolve:</label>
        <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblTask" runat="server" Rows="5" CssClass="form-control description-label"  />
      </div>

     

      <div class="form-group">
        <label for="lblWitnesses"> Additional information, witnesses, etc.:</label>
          <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblWitnesses" runat="server" Rows="5" CssClass="form-control description-label" />
      </div>
             <div class="form-group">
  <label for="lblNearMiss"> Near Miss?:</label>
    <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblNearMiss" runat="server"   CssClass="form-control description-label" />
</div>

          <div class="form-group">
     <label for="lblAttachment"> Attachment:</label>
       <asp:Label  ForeColor="Green" Font-Bold="true" ID="lblAttachment" runat="server" CssClass="form-control" />
            <asp:Image ID="aspImage" runat="server" AlternateText="No Image"  ClientIDMode="Static"    />
      
            
 
   </div>
           <div class="form-group" style="border: 1px solid black; padding-left:20px; padding-right:20px; padding-top:20px; padding-bottom:20px; background-color:azure;">  
                <div class="form-group">
                 <asp:Label ID="lblResult" runat="server" CssClass="alert" Font-Bold="true" Font-Size="Larger" /></div>
      <div class="form-group">
        <label for="lblResolved"> Was the incident resolved? :  </label>
                      <asp:CheckBox ID="chkResolve" runat="server" CssClass="bigCheckBox"  TextAlign="Left" />
      </div>
                    <div class="form-group">
  <label for="txtResolveComments"> Resolve Comments:</label>
     <asp:TextBox ID="txtResolveComments" runat="server" TextMode="MultiLine"
         CssClass="form-control" Rows="5" />
                        
</div>
             
                    <div class="form-group">
  <label for="txtResolveCommentsBy">Resolved By: </label>
     <asp:TextBox ID="txtResolveCommentsBy" runat="server"  ForeColor="Green" Font-Bold="true"
         CssClass="form-control"   />
                        
</div>
                  
               </div>  
          
                                 <asp:Table ID="tblButton" runat="server" 
CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0"   
     BorderWidth="1"  HorizontalAlign="Left">
                                     <asp:TableRow BackColor="antiquewhite">
             
                                        
                                       
                       <asp:TableCell HorizontalAlign="Left">
                                                           <asp:Button ID="btnDownload" runat="server" CssClass="form-control" OnClientClick="printPage(); return false;"  Text="Print this Form" BackColor="#b3112c" ForeColor="White"   Width="280"
OnClick="btnDownload_Click" /> 
                                                                                                                            
                                                      </asp:TableCell>
                                                                     <asp:TableCell>
                                 <asp:Button ID="btnResolve" runat="server" CssClass="form-control" Text="Resolve" BackColor="#b3112c" ForeColor="White"   Width="280" OnClientClick="return validateForm()"
OnClick="btnResolve_Click" /> 
                            </asp:TableCell>
                                                                   <asp:TableCell HorizontalAlign="Left">
                                                                       <asp:Button ID="btnClose" runat="server" Text="Go Back" CssClass="form-control"  OnClick="btnClose_Click" />
                                                                       <%--<asp:Button ID="btnClose" runat="server" Text="Close and Refresh Parent" CssClass="form-control" OnClientClick="closeAndRefreshParent(); return false;" />--%>
                                                                   </asp:TableCell>
</asp:TableRow>
    
                 
                                     </asp:Table>
               
             </asp:Panel>
                      
                          
      
    </div>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>

