<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_Acknowledgement.aspx.cs" Inherits="ShiftTurnover.NewTurnover_Acknowledgement" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />

</asp:Content>


<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">



    <div class="row" style="margin-top: 20px; border: solid 0px black;">
        <asp:Table ID="Table5" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="860">
            <asp:TableRow>
                <asp:TableCell>
                    <h1 style="color: #9A0000; font-weight: bold; width: 860px;">Acknowledgement for Previous Shift's Live Log Entries  
                        
                                                <br />
                        <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label>
                    </h1>
                </asp:TableCell>

            </asp:TableRow>

        </asp:Table>
    </div>
    <div class="container">

        <div class="row" style="margin-top: 10px; border: solid 0px black;">
            <asp:Table ID="tblPastEvents" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <fieldset class="fs-border">
                            <legend class="fs-border">Live Log Entries for 
                                <asp:Label runat="server" ID="lblPShift" ForeColor="Black"></asp:Label></legend>
                            <asp:Table ID="tblPastLog" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="lblPast" runat="server">There are no live log entries for the previous shift.</asp:Label>
                                        <asp:Panel ID="pnlPast" runat="server">
                                            <asp:Table ID="tblPast" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <asp:DataGrid ID="GrPast" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                            BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                            <HeaderStyle CssClass="columnhead" HorizontalAlign="Center"></HeaderStyle>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Deleted" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblDeleted" runat="server" Visible="false">Deleted</asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </asp:TableCell>
                                                </asp:TableRow>

                                            </asp:Table>
                                            <asp:Table ID="tblActive" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        Uploaded Documents
                                                    </asp:TableCell>
                                                </asp:TableRow>
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
        </div>

        <div class="row" style="margin-top: 10px; border: solid 0px black;">
            <asp:Table ID="tblAcknowledge" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                <asp:TableRow>
                    <asp:TableCell>
                        <label style="text-align: left; font-size: large; color: green;">
                            <asp:CheckBox ID="chkAch" runat="server" CssClass="ChkBoxClass" />
                            I acknowledge reviewing previous shift's live log entries. 
                        </label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>


        <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 5px; padding-right: 5px; padding-top: 35px;">
            <asp:Button ID="btnBack" runat="server" Text=" Save And Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back" OnClick="btnBack_Click" />
            &nbsp;&nbsp;  
            <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save" OnClick="btnSave_Click" />
            &nbsp;&nbsp;     
            <asp:Button ID="btnNext" runat="server" Text=" Save And Next" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go to Next tab" OnClick="btnNext_Click1" />
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