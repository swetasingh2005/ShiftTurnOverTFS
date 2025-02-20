using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Configuration;

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
using IronPdf;

namespace ShiftTurnover
{
    public partial class CheckStatusAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
               
                    int PID = 0;
                     
                    if (!String.IsNullOrEmpty(Request.QueryString["PID"]))
                    {
                        PID = Convert.ToInt32(Request.QueryString["PID"]);
                       
                         
                    }
               
            }

        }

        [WebMethod]
        public static List<PIData> GetChillerData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

            
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 1);
            return dataList;
        }

        [WebMethod]
        public static List<PIData> GetChillerPumpData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

            
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 2);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetBlrData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

            
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 5);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetFreeCoData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

            
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 4);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetROPumpData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

            
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 6);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetCTData(int shiftID)
        {
            string strSt = "";
            string strEnd = "";

             
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 3);
            return dataList;
        }
        public static DataSet CreateEquipmentList(int i)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@equipmenttypeid", SqlDbType.Int, i);
            DataSet ds = _dm.GetDataSet("SelectEquipmentList");
            return ds;
        }
        public static string GetStatus(string Status, int type)
        {
            string StatusString = "";
            if (Status.Contains("Data")) { return "No Data"; }
            else
            {
                if (type == 1)
                {

                    switch (Status)
                    {
                        case "0": //
                        case "Standby":
                            StatusString = "Standby";
                            break;
                        case "1": //
                        case "Online":
                            StatusString = "Running";
                            break;
                        case "2": //
                        case "Forced Out":
                            StatusString = "Forced Out";
                            break;
                        case "3": //
                        case "Planned Out":
                            StatusString = "Planned Out";
                            break;
                        case "4": //
                        case "Running (Turb)":
                            StatusString = "Running (Turb)";
                            break;
                        default:
                            StatusString = "No Data";
                            break;
                    }

                }
                else
                {

                    switch (Status)
                    {
                        case "0": //
                        case "Standby":
                            StatusString = "Standby";
                            break;
                        case "1": //
                        case "Online":
                            StatusString = "Running";
                            break;
                        case "2": //
                        case "Forced Out":
                            StatusString = "Forced Out";
                            break;
                        case "3": //
                        case "Planned Out":
                            StatusString = "Planned Out";
                            break;

                        default:
                            StatusString = "No Data";
                            break;
                    }

                }
            }
            return StatusString;
        }
        public static List<PIData> CreateDataList(DateTime starttime, DateTime endtime, AFTimeSpan interval, int Graphid)
        {
            //data for google charts
            List<PIData> datalist = new List<PIData>();
            DataModule dataModule = new DataModule();
            DataSet ds = CreateEquipmentList(Graphid);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //assign dataset values to array
                    //  PISystems myPIsystems = new PISystems();

                    //AFElement myElement = myDatabase.Elements["CUP"];
                    //AFAttribute myAttr = myElement.Elements["Chl\\" + row["Name"].ToString()].Attributes[row["Attribute"].ToString()];

                    PISystems myPIsystems = new PISystems();
                    PISystem myPISystem = myPIsystems["ORF-COGENAF"];
                    AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
                    AFTimeRange timeRange1 = new AFTimeRange(starttime, endtime);
                    AFElement myElement = myDatabase.Elements["CUP"];
                    string _element = row["Element"].ToString().Remove(row["Element"].ToString().IndexOf("|"));
                    AFAttribute myAttr = myElement.Elements[_element].Attributes[row["Attribute"].ToString()];

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
                    string _status = " ";

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
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status, Graphid), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            _status = val.Value.ToString();
                            _starttime = val.Timestamp;
                        }
                        else if (i == valsCHL.Count - 1)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status, Graphid), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                        }

                        i++;
                    } // end foreach i loop
                }//end chiller dataset j loop
            }

            return datalist;
        }
    }
}