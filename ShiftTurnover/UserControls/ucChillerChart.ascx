<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucChillerChart.ascx.cs" Inherits="ShiftTurnover.UserControls.ucChillerChart" %>

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
                url: 'ChillerStatus.aspx/GetData',
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

            // Create DateFormat with a timezone offset of -4
            //var dateFormat = new google.visualization.DateFormat({formatType: 'long', timeZone: -4});

            // Format the first column
            //dateFormat.format(dataTable, 2);
            //dateFormat.format(dataTable, 3);

            var options = {
                "title": "Chiller Statuses",
                chartArea: {
                    // leave room for y-axis labels
                    width: '94%'
                },
                legend: {
                    position: 'labeled',
                    labeledValueText: 'both'
                },
                width: '100%'
            };
            chart.draw(dataTable, options);
        }
    </script>

 <div id="CHL" style="height:600px;" ></div>