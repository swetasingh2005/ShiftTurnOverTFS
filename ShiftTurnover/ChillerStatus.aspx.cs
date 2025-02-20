using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Services;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;

using ShiftTurnover.Components;


namespace ShiftTurnover
{
    public partial class ChillerStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PISystems myPIsystems = new PISystems();
            AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
            AFElement myElement = myDatabase.Elements["CUP"];
            AFAttribute myAttr = myElement.Elements["CHL\\CHL22"].Attributes["Status"];
            AFValues listStatusValues = new AFValues();
            listStatusValues.Add(new AFValue(myAttr, AFTime.Now));
            AFValue myAFValue = myAttr.GetValue();

            AFTime myTime = new AFTime("04/24/2019 06:00");
            AFValue myAFValue1 = myAttr.Data.RecordedValue(myTime, AFRetrievalMode.AtOrAfter, myAttr.DefaultUOM);

            //AFTime start1 = new AFTime("*-10m");
            //AFTime end1 = new AFTime("*");
            AFTime start1 = new AFTime("04/30/2019 01:00");
            AFTime end1 = new AFTime("04/30/2019 13:00");
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);

            AFValues valsCHL22 = myAttr.Data.InterpolatedValues(
                       timeRange: timeRange1,
                       interval: interval,//for interpolated values
                       //boundaryType: AFBoundaryType.Inside, //only for RecordedValues
                       desiredUOM: null,
                       filterExpression: null,
                       includeFilteredValues: false);

            //litTest.Text = myAFValue1.ToString();
            litTest.Text = "Print recorded values - CHL22<br />";

            ArrayList _afArray22_0, _afArray22_1, _afArray22_2, _afArray22_3, _afArray22_4, _afArray22_5;

            _afArray22_0 = new ArrayList();
            _afArray22_1 = new ArrayList();
            _afArray22_2 = new ArrayList();
            _afArray22_3 = new ArrayList();
            _afArray22_4 = new ArrayList();
            _afArray22_5 = new ArrayList();

            foreach (AFValue val in valsCHL22)
            {
                litTest.Text += "Timestamp (Local): " + val.Timestamp.LocalTime + "<br />";
                litTest.Text += "Value: " + val.Value + "<br />";//" " + val +  //?.UOM.Abbreviation + "<br />";
                if (Convert.ToInt32(val.Value) == 0) _afArray22_0.Add(val);
                if (Convert.ToInt32(val.Value) == 1) _afArray22_1.Add(val);
                if (Convert.ToInt32(val.Value) == 2) _afArray22_2.Add(val);
                if (Convert.ToInt32(val.Value) == 3) _afArray22_3.Add(val);
                if (Convert.ToInt32(val.Value) == 4) _afArray22_4.Add(val);
                if (Convert.ToInt32(val.Value) == 5) _afArray22_5.Add(val);
            }

            List<Series> mySeries = new List<Series>();
            List<Point> myPoints = new List<Point>();
            //myPoints.Add(new Point
            //{
            //    X = (detailRec.RecordTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
            //    Y = detailRec.TotalCount
            //});
            foreach (AFValue val in valsCHL22)
            {
                myPoints.Add(new Point
                {
                    Y = (val.Timestamp.LocalTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                    X = Convert.ToInt32(val.Value)
                });
                //mySeries.Add(new Series
                //{
                //    Name = distinctDistrict.Name,
                //    Data = new Data(myPoints.ToArray())
                //});
                var txtStatus = "";
                switch (Convert.ToInt32(val.Value))
                {
                    case 0:
                        txtStatus = "Standby";
                        break;
                    case 1:
                        txtStatus = "Running (Elec)";
                        break;
                    case 2:
                        txtStatus = "Forced Out";
                        break;
                    case 3:
                        txtStatus = "Planned Out";
                        break;
                    case 4:
                        txtStatus = "Running (Turb)";
                        break;
                    default:
                        txtStatus = "No Data";
                        break;
                }

                mySeries.Add(new Series
                {
                    Name = txtStatus,
                    Data = new Data(myPoints.ToArray())
                });
            }
                //.SetSeries(mySeries.Select(s => new Series
                // {
                //     Name = s.Name,
                //     Data = s.Data
                // }).ToArray())

                //litCHLChart.Text = "";
            //Render_CHLChart();

            //data for google charts
            //List<PIData> datalist = new List<PIData>();
            

            ////DateTime _timevalue = DateTime.Now;
            //DateTime _timevalue = myTime;
            //DateTime _starttime = myTime;
            //DateTime _endtime = myTime;
            //string _status = "5";

            //int i = 0;
            //foreach (AFValue val in valsCHL22)
            //{
            //   if(DateTime.Compare(val.Timestamp,myTime) == 0)
            //    {
            //        _starttime = val.Timestamp.LocalTime;
            //        _status = val.Value.ToString();
            //    }
            //   if(val.Value.ToString() !=_status)
            //    {
            //        _endtime = val.Timestamp.LocalTime;
            //        datalist.Add(new PIData("CHL22",_status, _starttime, _endtime));
            //        _status = val.Value.ToString();
            //        _starttime = val.Timestamp.LocalTime;
            //    }
            //    else if (i == valsCHL22.Count-1)
            //    {
            //        _endtime = val.Timestamp.LocalTime;
            //        datalist.Add(new PIData("CHL22", _status, _starttime, _endtime));
            //    }

            //    i++;
            //}

                DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart").InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Bar })
         .SetTitle(new DotNet.Highcharts.Options.Title
         {
             Text = "Chiller Statuses",
             X = 0
         })
         .SetSubtitle(new Subtitle
         {
             Text = "Source: PI",
             X = -20
         })
         .SetXAxis(new XAxis
         {
             //Categories = Months//Array generated from database
             //Categories = new[] { "CHL16", "CHL17", "CHL18", "CHL19", "CHL20", "CHL21", "CHL22", "CHL23", "CHL24", "CHL25", "CHL26", "CHL27" }
             Categories = new[] { "CHL22" }
         })
         .SetYAxis(new YAxis
         {
             //Type = AxisTypes.Datetime,
             //TickInterval = 7 * 24 * 3600 * 1000, // one week
             //TickWidth = 0,
             Type = AxisTypes.Datetime,
             //TickInterval = 24 * 3600 * 1000/ 16, // 1.5 hrs
             TickWidth = 0,
             //DateTimeLabelFormats = new DateTimeLabel { Hour = "%I %p",Minute = "%I:%M %p"},
             //DateTimeLabelFormats = new DateTimeLabel { Hour = "%a %e.%b" },
             Min = 0
         })

        .SetPlotOptions(new PlotOptions
        {
            Bar = new PlotOptionsBar
            {
                Stacking = Stackings.Normal
            }
        }
        )
         //.SetSeries(new[]
         //        {
         //new Series { Name = "Standby", Data = new Data(new object[] {_afArray22_0 })},
         //new Series { Name = "Running (Elec)", Data = new Data(new object[] {_afArray22_1 })},
         //new Series { Name = "Forced Out", Data = new Data(new object[] {_afArray22_2 })},
         //new Series { Name = "Planned Out", Data = new Data(new object[] {_afArray22_3 })},
         //new Series { Name = "Running (Turb)", Data = new Data(new object[] {_afArray22_4 })},
         //new Series { Name = "No Data", Data = new Data(new object[] {_afArray22_5 })}
         //new Series { Name = "Standby", Data = new Data(getAFData(valsCHL22,0))},
         //new Series { Name = "Running (Elec)", Data = new Data(getAFData(valsCHL22,1))},
         //new Series { Name = "Forced Out", Data = new Data(getAFData(valsCHL22,2))},
         //new Series { Name = "Planned Out", Data = new Data(getAFData(valsCHL22,3))},
         //new Series { Name = "Running (Turb)", Data = new Data(getAFData(valsCHL22,4))},
         //new Series { Name = "No Data", Data = new Data(getAFData(valsCHL22,5))},
         //new Series { Name = "Running", Data = new Data(getAFData(valsCHL22,1))},
         //new Series { Name = "Berlin", Data =new Data(getAFData(valsCHL22,2))},
         //new Series { Name = "London", Data = new Data(getAFData(valsCHL22,3))}
         //new Series { Data = new Data(new object[] {_afArray22}  ) }
         //    new Series { Data = new Data(new object[,]
         //{
         //{ new DateTime(2019, 4, 25,6,0,0), 0 },
         //{ new DateTime(2019, 4, 25,6,30,0), 0.6 },
         //{ new DateTime(2019, 4, 25,7,0,0), 0.7 },
         //{ new DateTime(2019, 4, 25,7,30,0), 0.8 },
         //{ new DateTime(2019, 4, 25,8,0,0), 0.6 },
         //{ new DateTime(2019, 4, 25,8,30,0), 0.6 },
         //{ new DateTime(2019, 4, 25,9,0,0), 0.67 },
         //{ new DateTime(2019, 4, 25,9,30,0), 0.81 },
         //{ new DateTime(2019, 4, 25,10,0,0), 0.78 },
         //{ new DateTime(2019, 4, 25,10,30,0), 0.98 },
         //{ new DateTime(2019, 4, 25,11,0,0), 1.84 },
         //{ new DateTime(2019, 4, 25,11,30,0), 1.80 },
         //{ new DateTime(2019, 4, 25,12,0,0), 1.80 },
         //{ new DateTime(2019, 4, 25,12,30,0), 1.92 },
         //{ new DateTime(2019, 4, 25,13,0,0), 2.49 },
         //{ new DateTime(2019, 4, 25,13,30,0), 2.79 },
         //{ new DateTime(2019, 4, 25,14,0,0), 2.73 },
         //{ new DateTime(2019, 4, 25,14,30,0), 2.61 },
         //{ new DateTime(2019, 4, 25,15,0,0), 2.76 },
         //{ new DateTime(2019, 4, 25,15,30,0), 2.82 },
         //{ new DateTime(2019, 4, 25,16,0,0), 2.8 },
         //{ new DateTime(2019, 4, 25,16,30,0), 2.1 },
         //{ new DateTime(2019, 4, 25,17,0,0), 1.1 },
         //{ new DateTime(2019, 4, 25,17,30,0), 0.25 },
         //{ new DateTime(2019, 4, 25,18,0,0), 0 }
         //}) }
         .SetSeries(mySeries.Select(s => new Series
         {
             Name = s.Name,
             Data = s.Data
         }).ToArray());
        //}
           // );



           //litCHLChart.Text = chart.ToHtmlString();

        }// end page load

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        [WebMethod]

        public static List<PIData> GetData()
        {
            //AFTime start1 = new AFTime("*-10m");
            //AFTime end1 = new AFTime("*");
            AFTime start1 = new AFTime("10/30/2019 01:00");
            AFTime end1 = new AFTime("10/30/2019 13:00");
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval);
            return dataList;
        }
        public static DataSet CreateChillers( )
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Chiller", typeof(string));
            dt.Rows.Add(new object[] { @"chl16" });
            dt.Rows.Add(new object[] { @"chl17" });
            dt.Rows.Add(new object[] { @"chl18" });
            dt.Rows.Add(new object[] { @"chl19" });
            dt.Rows.Add(new object[] { @"chl20" });
            dt.Rows.Add(new object[] { @"chl21" });
            dt.Rows.Add(new object[] { @"chl22" });
            dt.Rows.Add(new object[] { @"chl23" });
            dt.Rows.Add(new object[] { @"chl24" });
            dt.Rows.Add(new object[] { @"chl25" });
            dt.Rows.Add(new object[] { @"chl26" });
            dt.Rows.Add(new object[] { @"chl27" });
            ds.Tables.Add(dt);
            return ds;
        }
            public static List<PIData> CreateDataList(DateTime starttime, DateTime endtime, AFTimeSpan interval)
        {
            //data for google charts
            List<PIData> datalist = new List<PIData>();
            string[] arrCHL;
            DataModule dataModule = new DataModule();
            DataSet ds = CreateChillers();
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                //Empty array
                arrCHL = new string[ds.Tables[0].Rows.Count];
                //foreach (DataRow oRow in ds.Tables[0].Rows)
                //{
                //    Months.Add(oRow);
                //}
                //loopcounter
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    //assign dataset values to array
                    arrCHL[j] = ds.Tables[0].Rows[j]["Chiller"].ToString();

                    PISystems myPIsystems = new PISystems();
                    AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
                    AFElement myElement = myDatabase.Elements["CUP"];
                    AFAttribute myAttr =  myElement.Elements["Chl\\" + arrCHL[j].ToString()].Attributes["Status_String"];
                    AFTimeRange timeRange1 = new AFTimeRange(starttime, endtime);

                    AFValues valsCHL = myAttr.Data.InterpolatedValues(
                               timeRange: timeRange1,
                               interval: interval,//for interpolated values
                               //boundaryType: AFBoundaryType.Inside, //only for RecordedValues
                               desiredUOM: null,
                               filterExpression: null,
                               includeFilteredValues: false);

                    //DateTime _timevalue = DateTime.Now;
                    //val.Timestamp.LocalTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                    DateTime _timevalue = starttime;
                    DateTime _starttime = starttime;
                    DateTime _endtime = starttime;
                    string _status = "No Data";

                    int i = 0;
                    foreach (AFValue val in valsCHL)
                    {
                        if (DateTime.Compare(val.Timestamp, starttime) == 0)
                        {
                            _starttime = val.Timestamp;
                             _status = val.Value.ToString();
                        }
                        if (val.Value.ToString() != _status)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(arrCHL[j].ToString(), _status, (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            _status = val.Value.ToString();
                            _starttime = val.Timestamp;
                        }
                        else if (i == valsCHL.Count - 1)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(arrCHL[j].ToString(), _status, (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                        }

                        i++;
                    } // end foreach i loop
                }//end chiller dataset j loop
            }

            return datalist;
        }
        protected void Render_CHLChart()
        {
            string[] Months = new string[12];
            DataModule dataModule = new DataModule();
            DataSet ds = dataModule.GetDataSet("tmpSelectMonths");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                //Empty array
                string[] arrMonths = new string[ds.Tables[0].Rows.Count];
                //foreach (DataRow oRow in ds.Tables[0].Rows)
                //{
                //    Months.Add(oRow);
                //}
                //loopcounter
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //assign dataset values to array
                    arrMonths[i] = ds.Tables[0].Rows[i]["Month"].ToString();
                }
                Months = arrMonths;
            }
            ds = null;

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart").InitChart(new DotNet.Highcharts.Options.Chart { DefaultSeriesType = ChartTypes.Bar })
         .SetTitle(new DotNet.Highcharts.Options.Title
         {
             Text = "Chiller Statuses",
             X = 0
         })
         .SetSubtitle(new Subtitle
         {
             Text = "Source: PI",
             X = -20
         })
         .SetXAxis(new XAxis
         {
             //Categories = Months//Array generated from database
             Categories = new[] { "CHL16", "CHL17", "CHL18", "CHL19", "CHL20", "CHL21", "CHL22", "CHL23", "CHL24", "CHL25", "CHL26", "CHL27" }
         })
         .SetYAxis(new YAxis
         {
             //Type = AxisTypes.Datetime,
             //TickInterval = 7 * 24 * 3600 * 1000, // one week
             //TickWidth = 0,
             Type = AxisTypes.Datetime,
             TickInterval = 24 * 3600 * 1000 / 16, // 1.5 hrs
             TickWidth = 0,
             Min = 0
         })

        .SetPlotOptions(new PlotOptions
        {
            Bar = new PlotOptionsBar
            {
                Stacking = Stackings.Normal
            }
        }
        )
         .SetSeries(new[]
                 {
                    new Series { Name = "Standby", Data = new Data(getData(1))},
                    new Series { Name = "New York", Data = new Data(getData(2))},
                    new Series { Name = "Berlin", Data =new Data(getData(3))},
                    new Series { Name = "London", Data = new Data(getData(4))}
                }
             );

            //litCHLChart.Text = chart.ToHtmlString(); 
        } //end Render CHLChart

        protected Object[] getData(int CityID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@cityid", SqlDbType.Int, CityID);
            DataSet ds = _dm.GetDataSet("tmpSelectDataValues");

            if (ds != null && ds.Tables.Count > 0)
            {
                Int32 totalArraySize = ds.Tables[0].Rows.Count; ;
                Object[] XAxisData = new object[totalArraySize];
                Object[] YAxisServices = new object[totalArraySize];
                int J = 0;
                foreach (DataRow drRow in ds.Tables[0].Rows)
                {
                    YAxisServices[J] = new object[] { Convert.ToDouble(drRow[0].ToString()) };
                    J += 1;
                }
                return YAxisServices;
            }
            return null;
        }

        protected Object[] getAFData(AFValues afv, int val)
        {

            Int32 totalArraySize = afv.Count;
                Object[] XAxisData = new object[totalArraySize];
                Object[] YAxisServices = new object[totalArraySize];
                int J = 0;
                foreach (AFValue x in afv)
                {
                    YAxisServices[J] = new object[] { x.Timestamp.LocalTime };
                    J += 1;
                }
                return YAxisServices;
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

    }

    //public class PIData

    //{

    //    public string ChillerName;
    //    public string ChillerStatus;
    //    public long StartTime;
    //    public long EndTime;

    //    public PIData(string chillername, string chillerstatus, long starttime, long endtime)

    //    {

    //        ChillerName = chillername;
    //        ChillerStatus = chillerstatus;
    //        StartTime = starttime;
    //        EndTime = endtime;

    //    }

    //}
}