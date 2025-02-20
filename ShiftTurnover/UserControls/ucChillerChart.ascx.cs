using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
using System.Data;

namespace ShiftTurnover.UserControls
{
    public partial class ucChillerChart : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
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
        public static DataSet CreateChillers()
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
                    AFAttribute myAttr = myElement.Elements["Chl\\" + arrCHL[j].ToString()].Attributes["Status_String"];
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
                            if (val.Value.ToString().Contains("Data was not available"))
                            { _status = "No Data"; }
                            else { _status = val.Value.ToString(); }
                        }
                        if (val.Value.ToString() != _status)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(arrCHL[j].ToString(), _status, (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            if (val.Value.ToString().Contains("Data was not available"))
                            { _status = "No Data"; }
                            else { _status = val.Value.ToString(); }
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
    
    }
}