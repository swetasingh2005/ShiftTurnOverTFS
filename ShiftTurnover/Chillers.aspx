<%@ Page Title="Chiller Statuses" Language="C#" MasterPageFile="~/ShiftTurnover.Master" AutoEventWireup="true" CodeBehind="Chillers.aspx.cs" Inherits="ShiftTurnover.Chillers" %>
<%@ MasterType VirtualPath="~/ShiftTurnover.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>


    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['timeline'] });
        //google.charts.setOnLoadCallback(drawChart);
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $.ajax({
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                url: 'Chillers.aspx/GetData',
                data: '{}',
                success:
                    function (response) {
                        drawChart(response.d);
                    }
            });
        })

        function drawChart(dataValues) {

            var container = document.getElementById('CHL');
            var chart = new google.visualization.Timeline(container);
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'string', id: 'ChillerName' });
            dataTable.addColumn({ type: 'string', id: 'ChillerStatus' });
            dataTable.addColumn({ type: 'date', id: 'StartTime' });
            dataTable.addColumn({ type: 'date', id: 'EndTime' });

            for (var i = 0; i < dataValues.length; i++) {
                dataTable.addRow([dataValues[i].ChillerName, dataValues[i].ChillerStatus, new Date(dataValues[i].StartTime), new Date(dataValues[i].EndTime)]);
            }

            //var options = {
            //    "title": "Chiller Statuses",
            //    chartArea: {
            //        // leave room for y-axis labels
            //        width: '94%'
            //    },
            //    legend: {
            //        position: 'labeled',
            //        labeledValueText: 'both'
            //    },
            //    width: '100%'
            //};
            //chart.draw(dataTable, options);
            chart.draw(dataTable);
        }
    </script>

    <link href="Content/extrastyles.css" rel="stylesheet" runat="server" />
</asp:Content>

<asp:Content ID="Content11" ContentPlaceHolderID="cph_ReportingPeriod" runat="server">
    <asp:Table ID="tblTop" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" Width="50%">
                <asp:Label runat="server" ID="lblShift" CssClass="lead"></asp:Label></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

<asp:Content ID="Content14" ContentPlaceHolderID="cph_instructions" runat="server">
    <div runat="server" id="divExisting" class="instructions"><strong>Chiller Status Instructions</strong>: Verify that the chiller statuses shown are correct and enter comments if data appears incorrect.</div>
    <p>
        &nbsp;<br />
    </p>
    <asp:Literal runat="server" ID="litTest" Visible="false"></asp:Literal>
</asp:Content>

<asp:Content ID="Content16" ContentPlaceHolderID="cph_main" runat="server">
    <asp:Table ID="tblStatus" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="800" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Panel runat="server" ID="pnlCHLStatus">
                    <fieldset class="fs-border">
                        <legend class="fs-border">Chiller Status</legend>
                        <asp:Table ID="tblCHLStatus" runat="server" CellPadding="0" CellSpacing="4" BorderWidth="0">
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="Bottom">
                                    <div><strong>Is the data correct?</strong></div>
                                    <asp:RadioButtonList runat="server" ID="rdoCHLStatus" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Yes" Text="&nbsp;Yes&nbsp;"></asp:ListItem>
                                        <asp:ListItem Value="No" Text="&nbsp;No&nbsp;"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:TableCell>
                                <asp:TableCell Width="25">
                                    &nbsp;
                                </asp:TableCell>

                                <asp:TableCell RowSpan="2" VerticalAlign="Middle" Width="500">
                                    <asp:Literal ID="litCHLChart" runat="server" Visible="false"> </asp:Literal>
                                    <div id="CHL" style="height: 600px"></div>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="Top">
                                    <div>Please enter a comment if the data appears incorrect:</div>
                                    <asp:TextBox ID="txtCHLComment" TabIndex="11" runat="server" Columns="45" Rows="6" TextMode="MultiLine"></asp:TextBox><br />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </fieldset>
                </asp:Panel>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblNavButtons" runat="server" CssClass="wiznavbuttons" CellPadding="0" CellSpacing="0" BorderWidth="0" Width="570px" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Button ID="btnBack" TabIndex="18" runat="server" CssClass="form-control" Text="⯇ Save and Go Back" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnBack_Click" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Button ID="btnCancel" TabIndex="20" runat="server" CssClass="form-control" Text="Cancel" BackColor="#b3112c" ForeColor="White" Width="100px" Font-Bold="false" OnClick="btnCancel_Click" CausesValidation="false" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Button ID="btnSave" TabIndex="19" runat="server" CssClass="form-control" Text="Save and Continue ⯈" BackColor="#b3112c" ForeColor="White" Width="180px" Font-Bold="false" OnClick="btnSave_Click" />
            </asp:TableCell>

        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="Content17" ContentPlaceHolderID="cph_footer" runat="server">
</asp:Content>

