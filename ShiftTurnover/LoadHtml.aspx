<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnoverNoHeader.Master" AutoEventWireup="true" CodeBehind="LoadHtml.aspx.cs" Inherits="ShiftTurnover.LoadHtml" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_main" runat="server">
      <div>
        <asp:Label runat="server" ID="lblCriticalAlarmReport" Font-Size="Medium" Font-Bold="true" CssClass="" ForeColor="Red"></asp:Label>
    </div>
</asp:Content>
