<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="AnswerPopup.aspx.cs" Inherits="ShiftTurnover.AnswerPopup" %>
<%@ MasterType  virtualPath="~/ShiftTurnover.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content13" ContentPlaceHolderID="cph_error" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_banner" runat="server">
</asp:Content>
<asp:Content ID="Content14" ContentPlaceHolderID="cph_main" runat="server">
    <div id="questionPopUp">
				<p><strong><asp:Literal id="Literal1" runat="server"></asp:Literal></strong></p>
				<p><asp:Literal id="Literal2" runat="server"></asp:Literal></p>
				<p>
                    <asp:Button ID="Button1" runat="server" Text="Print this page" OnClientClick="javascript:window.print()" TabIndex="1" />
				</p>
        </div>
        <div class="closewindow"><a href="javascript:window.close();" tabindex="2">Close this window</a></div>
</asp:Content>
<asp:Content ID="Content15" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>

