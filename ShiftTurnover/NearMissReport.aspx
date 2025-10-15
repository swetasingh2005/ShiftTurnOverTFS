<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NearMissReport.aspx.cs"  Inherits="ShiftTurnover.NearMissReport" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Near-Miss Report Form</title>
  <style>
    body {
      font-family: Arial, sans-serif;
      margin: 20px;
      line-height: 1.5;
    }
    h1 {
      text-align: center;
    }
    .form-container {
      max-width: 800px;
      margin: 0 auto;
    }
    .form-group {
      margin-bottom: 12px;
    }
    .form-group label {
      display: block;
      font-weight: bold;
      margin-bottom: 4px;
    }
    .form-control {
      width: 100%;
      padding: 8px;
      box-sizing: border-box;
      font-size: 1rem;
    }
    .radio-list, .radio-list label {
      display: inline-block;
      margin-right: 16px;
      font-weight: normal;
    }
    .btn {
      padding: 10px 20px;
      font-size: 1rem;
    }
    #lblResult {
      margin-top: 20px;
      font-weight: bold;
      color: green;
    }
  </style>
</head>
<body>
  <form id="form1" runat="server">
    <div class="form-container">
      <h1>Near-Miss Report Form</h1>

      <div class="form-group">
        <label for="txtReporterName">1. Reporter Name:</label>


        <asp:TextBox ID="txtReporterName" runat="server" CssClass="form-control" />
      </div>

      <div class="form-group">
        <label for="txtIncidentDateTime">2. Date/Time of Incident:        </label>
        <asp:TextBox ID="txtIncidentDateTime" runat="server" CssClass="form-control"
                     TextMode="DateTimeLocal" />
      </div>

      <div class="form-group">
        <label>3. Immediate Severity of Incident:</label>
        <asp:RadioButtonList ID="rblImmediateSeverity" runat="server" CssClass="radio-list">
          <asp:ListItem Text="None" Value="None" />
          <asp:ListItem Text="Slight" Value="Slight" />
          <asp:ListItem Text="Moderate" Value="Moderate" />
          <asp:ListItem Text="Serious" Value="Serious" />
          <asp:ListItem Text="Catastrophic" Value="Catastrophic" />
        </asp:RadioButtonList>
      </div>

      <div class="form-group">
        <label>4. Potential Severity of Incident:</label>
        <asp:RadioButtonList ID="rblPotentialSeverity" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
          <asp:ListItem Text="1" Value="1" />
          <asp:ListItem Text="2" Value="2" />
          <asp:ListItem Text="3" Value="3" />
          <asp:ListItem Text="4" Value="4" />
          <asp:ListItem Text="5" Value="5" />
        </asp:RadioButtonList>
      </div>

      <div class="form-group">
        <label for="ddlDepartment">5. Department/Division:</label>
        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
          <asp:ListItem Text="-- Select --" Value="" />
          <asp:ListItem Text="Operations" Value="Operations" />
          <asp:ListItem Text="Maintenance" Value="Maintenance" />
          <asp:ListItem Text="Safety" Value="Safety" />
          <asp:ListItem Text="Quality" Value="Quality" />
          <asp:ListItem Text="HR" Value="HR" />
        </asp:DropDownList>
      </div>

      <div class="form-group">
        <label for="txtLocation">6. Location:</label>
        <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" />
      </div>

      <div class="form-group">
        <label for="txtTask">7. Task Being Performed:</label>
        <asp:TextBox ID="txtTask" runat="server" CssClass="form-control" />
      </div>

      <div class="form-group">
        <label for="txtDescription">8. Description of Near Miss:</label>
        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"
                     CssClass="form-control" Rows="5" />
      </div>

      <div class="form-group">
        <label for="txtWitnesses">9. Witnesses:</label>
        <asp:TextBox ID="txtWitnesses" runat="server" CssClass="form-control" />
      </div>

      <div class="form-group">
        <label for="txtImmediateActions">10. Immediate Actions Taken:</label>
        <asp:TextBox ID="txtImmediateActions" runat="server" TextMode="MultiLine"
                     CssClass="form-control" Rows="4" />
      </div>

      <asp:Button ID="btnSubmit" runat="server" CssClass="btn" Text="Submit"
                  OnClick="btnSubmit_Click" />

      <asp:Label ID="lblResult" runat="server" />
    </div>
  </form>
</body>
</html>
