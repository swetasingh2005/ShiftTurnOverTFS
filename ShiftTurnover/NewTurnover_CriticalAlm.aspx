<%@ Page Title="" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="NewTurnover_CriticalAlm.aspx.cs" Inherits="ShiftTurnover.NewTurnover_CriticalAlm" %>

<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />--%>
    <link href="Content/jquery.dataTables.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="Scripts/jquery.dataTables.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" id="bootstrap-css" />
    <script>



        $(document).ready(function () {

            $('#example').DataTable({
                "aaSorting": [[0, "asc"]],
                "aLengthMenu": [[50, 100, 150, -1], [50, 100, 150, "All"]],
                "iDisplayLength": 50,
                "oLanguage": {
                    "sEmptyTable": "No alarms found with given parameters!!"
                }
            });

        });
    </script>
    <style type="text/css">
        /* unvisited link */
        a:link {
            color: black;
        }

        a:visited {
            color: black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <div class="container">

        <div class="row" style="margin-top: 85px;">
            <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-left  bhoechie-tab-container">
                <center>
                    <div class="row">

                        <div class="col-xs-12 col-sm-12 col-md-12 col-lg-12 text-center  bhoechie-tab-container" style="border: solid 0px Black;">
                            <%--  <div class="row" style="margin-top:20px; border:solid 0px black;">--%>
                            <h1>
                                <asp:Label ID="Label3" runat="server" ForeColor="#9A0000" Font-Bold="true" Text="Critical Alarms"></asp:Label>
                                <br />
                                <asp:Label runat="server" ID="lblMShift" Font-Size="Medium" ForeColor="Black"></asp:Label>
                                <br />
                            </h1>
                            <a class="btn btn-primary"  style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Auxiliary" target="iframe_criticalAlarms" role="button" aria-pressed="true">Auxiliary</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Boiler" target="iframe_criticalAlarms" role="button" aria-pressed="true">Boiler Plant</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=Chiller" target="iframe_criticalAlarms" role="button">Chiller Plant</a>
                            <a class="btn btn-primary" style="background-color:#b3112c; color:white; font-size:medium; font-weight:bold;"  href="LoadHtml.aspx?report=WaterTreatment" target="iframe_criticalAlarms" role="button">Water Treatment</a>
                            <br />
                            <br />
                            <asp:Label ID="lblWO" runat="server" ForeColor="Green"></asp:Label>
                            <asp:Panel ID="pnlWO" runat="server">
                                <iframe src="LoadHtml.aspx?report=Auxiliary" name="iframe_criticalAlarms" width="980" height="500"></iframe>
                            </asp:Panel>

                            <%--     <asp:Table ID="Table6" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <fieldset class="fs-border">
                                                    <legend class="fs-border"></legend>
                                                    <asp:Table ID="Table17" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="760">
                                                                                     <asp:TableRow>
                                                                                        <asp:TableCell>
                                                                                            <h1><asp:Label ID="lblHeader" runat="server"  ForeColor="#9A0000" Font-Bold="true" Text="Critical Alarms"></asp:Label>
                                            
                                                                   <br />
                                                  <asp:Label runat="server" ID="lblMShift"  Font-Size="Medium" ForeColor="Black"></asp:Label>
                                                                   </h1>
                                                                                            </asp:TableCell>
                                                                                         </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell>
                                                                <asp:Label ID="lblWO" runat="server" ForeColor="Green">There are no Critical Alarms for this shift.</asp:Label>
                                                                <asp:Panel ID="pnlWO" runat="server">
                                                                    <iframe src="LoadHtml.aspx" width="500" height="300"></iframe>
                                                                    <asp:Table ID="tblWO" runat="server" CellPadding="0" CellSpacing="0" Width="100%">
                                                                        <asp:TableRow>
                                                                            <asp:TableCell>
                                                                                <asp:Literal ID="litSDK" runat="server"></asp:Literal>--%>
                            <%-- <asp:DataGrid ID="grdWO" runat="server" CssClass="listing" HorizontalAlign="Center" UseAccessibleHeader="True"
                                                                                    BorderColor="Black" CellPadding="4" CellSpacing="0">
                                                                                    <HeaderStyle CssClass="columnhead"></HeaderStyle>
                                                                                </asp:DataGrid>--%>
                            <%--      </asp:TableCell>
                                                                        </asp:TableRow>
                                                                    </asp:Table>
                                                                </asp:Panel>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </fieldset>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>--%>
                            <%--                     </div>--%>
                            <div class="row" style="margin-top: 20px; border: solid 0px black;">
                                <asp:Table ID="Table1" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="Center" Width="800">
                                    <asp:TableRow>
                                        <asp:TableCell>
                                            <fieldset class="fs-border">
                                                <legend class="fs-border"></legend>
                                                <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" HorizontalAlign="left" Width="760">
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left">
                                                            <asp:Label ID="Label1" runat="server" Font-Bold="true">
                                                                   Are all critical alarms captured?
                                                            </asp:Label>
                                                            <img height="12" alt="Required" src="Images/icon_required.gif" width="13" />
                                                        </asp:TableCell>
                                                        <asp:TableCell>
                                                            <asp:RadioButtonList runat="server" ID="rdoAlarmsYN" RepeatDirection="Horizontal" AutoPostBack="true"
                                                                OnSelectedIndexChanged="rdoAlarmsYN_SelectedIndexChanged">
                                                                <asp:ListItem Value="Yes" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
                                                                <asp:ListItem Value="No" Text="&nbsp;No&nbsp;"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" ControlToValidate="rdoAlarmsYN"
                                                                ErrorMessage="Required" ID="rfvAlarmsYN" runat="server"></asp:RequiredFieldValidator>

                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow ID="tdAlarmsComments" runat="server" Visible="false">
                                                        <asp:TableCell HorizontalAlign="Left" Wrap="true">
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="true">Please enter a comment if the alarm data appears incorrect:</asp:Label>

                                                        </asp:TableCell>
                                                        <asp:TableCell Width="500px">
                                                            <asp:TextBox ID="txtAlarmsComments" TabIndex="11" runat="server" CssClass="form-control" Rows="6" TextMode="MultiLine"></asp:TextBox>
                                                            <asp:RequiredFieldValidator Display="Dynamic" CssClass="alert" Enabled="false" ControlToValidate="txtAlarmsComments"
                                                                ErrorMessage="Required" ID="rfvAlarmsComments" runat="server"></asp:RequiredFieldValidator>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </fieldset>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>


                            </div>


                            <br />


                            <!------ Tabs ---------->
                            <div class="list-group list-group-horizontal flex-lg-row bhoechie-tab-menu" style="padding-left: 35px; padding-right: 35px;">
                                <asp:Button ID="btnPre" runat="server" Text=" Save And Previous" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back " CausesValidation="true" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;  
                                <asp:Button ID="btnSave" runat="server" Text=" Save " CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" ToolTip="Save And Go Back " CausesValidation="true" OnClick="btnSave_Click" />
                                &nbsp;&nbsp;     
                                <asp:Button ID="btnNext" runat="server" Text=" Save And Next" CssClass="btn" BackColor="#b3112c" Font-Bold="true" Font-Size="X-Large" ForeColor="White" Width="100%" Height="50px" CausesValidation="true" ToolTip="Save And Go to Ground " OnClick="btnSave_Click" />
                            </div>

                        </div>
                    </div>

                </center>
            </div>

        </div>

    </div>

</asp:Content>


<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>
