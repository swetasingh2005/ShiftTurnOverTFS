<%@ Page Title="Manage CUP Documents" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="CUPDocumentUpload.aspx.cs" Inherits="ShiftTurnover.CUPDocumentUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">
        function confirmBox() {
            return window.confirm("The selected document will be deleted permanently! Are you sure you want to delete this document?");
        }
    </script>
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblReport" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="750" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Upload Document</legend>
                    <asp:Label runat="server" ID="lblStatus" Text="" CssClass="alert" />
                    <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                      <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top" ColumnSpan="3">
                                <asp:Label Font-Bold="true" runat="server">Select Folder:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                               <asp:DropDownList ID="ddlFolder" TabIndex="2" Width="150px" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Boiler" Value="2" ></asp:ListItem>
                    <asp:ListItem Text="Chiller" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Water Treatment" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Auxillary" Value="4"></asp:ListItem>
                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
                                 
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Bottom">
                                
                            </asp:TableCell>
                        </asp:TableRow>
                       
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top" ColumnSpan="3">
                                <asp:Label Font-Bold="true" runat="server">File to Upload:<img height="12" alt="Required" src="Images/icon_required.gif" width="13" /></asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="fileUpload" ErrorMessage="Please choose a file to upload" runat="server" CssClass="alert"></asp:RequiredFieldValidator>
                                <br />
                                <asp:FileUpload ID="fileUpload" TabIndex="14" AllowMultiple="false" CssClass="form-control" runat="server" Font-Bold="true" />
                            </asp:TableCell>
                            <asp:TableCell VerticalAlign="Bottom">
                                <asp:Button ID="btnUpload" TabIndex="15" runat="server" CssClass="form-control" Text="Upload" OnClick="btnUpload_Click" BackColor="#b3112c" ForeColor="White" />
                            </asp:TableCell>
                        </asp:TableRow>
                        
                    </asp:Table>

                </fieldset>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="Table2" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="750" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <fieldset class="fs-border">
                    <legend class="fs-border">Uploaded Documents</legend>
                    <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="600">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="lblActive" runat="server">There are no documents uploaded for the CUP.</asp:Label>
                                <asp:Panel ID="pnlActive" runat="server">
                                    <asp:Table ID="tblActive" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <asp:DataGrid ID="GrActive" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                   AutoGenerateColumns="false"  BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                    <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Actions">
                                                            <ItemTemplate>
                                                                <span class="buttonColumn">
                                                                    <asp:ImageButton ID="btnView" runat="server" CausesValidation="false" CommandName="View" ImageUrl="Images/actionbutton_view_off.gif"
                                                                        AlternateText="View This Document"></asp:ImageButton>
                                                                   
                                                                    <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"  Visible="false" ImageUrl ="Images/actionbutton_edit_off.gif"
                                                                        AlternateText="Edit This Document"></asp:ImageButton>
                                                                       
                                                                    <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" ImageUrl="Images/actionbutton_delete_off.gif"
                                                                        AlternateText="Delete This Document"></asp:ImageButton>
                                                                </span>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                    <asp:BoundColumn DataField="File" HeaderText="File"></asp:BoundColumn>
                                                         <asp:BoundColumn DataField="FolderLocation" HeaderText="Folder"></asp:BoundColumn>
                                                          <asp:BoundColumn DataField="Folder" Visible="false" HeaderText="FolderLocation"></asp:BoundColumn>
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
</asp:Content>


