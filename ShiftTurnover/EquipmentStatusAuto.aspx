<%@ Page Title="Equipment Status Change Automated Entry" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="EquipmentStatusAuto.aspx.cs" Inherits="ShiftTurnover.EquipmentStatusAuto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

<asp:Content ID="Content9" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
       
        <asp:TableRow>
              
            <asp:TableCell HorizontalAlign="left" Width="100%">
                 <asp:Label runat="server" ID="lblMShift"  Font-Size="Medium" ForeColor="Black"      ></asp:Label>
               <asp:Button ID="btnSubmit" TabIndex="14" runat="server" CssClass="form-control" ValidationGroup="vgLiveLog" CausesValidation="true"
                   Text="Acknowledge Equipment Status Change " BackColor="#b3112c" ForeColor="White" Width="480px" Font-Bold="false" OnClick="btnSubmit_Click" />
            </asp:TableCell>
            </asp:TableRow>
        <asp:TableRow>
              
            <asp:TableCell HorizontalAlign="left" Width="100%">
                <asp:DataGrid ID="grdED" runat="server" CssClass="listing" HorizontalAlign="Center"
                    UseAccessibleHeader="True" AutoGenerateColumns="true"
                                                            BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                            <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                            <Columns>
                                                              
                                                                </Columns>
                                                        </asp:DataGrid>
                </asp:TableCell>
            </asp:TableRow>
        
    </asp:Table>
</asp:Content>

 
