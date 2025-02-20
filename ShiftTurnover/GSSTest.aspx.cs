using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using System.Text;
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

namespace ShiftTurnover
{
    public partial class GSSTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _serverName = ConfigurationManager.AppSettings["PIServer"].ToString();
            PISystems myPIsystems = new PISystems();
            PISystem myPISystem = myPIsystems[_serverName];
            AFDatabase myDatabase = myPIsystems[_serverName].Databases["Database1"];
            AFElement myElement = myDatabase.Elements["Chemical_Treatment\\Boiler_Plant\\City\\Operator_Rounds"];
            AFAttribute myAttribute = myElement.Attributes["Conductivity"];
            AFValue myAFValue = myAttribute.GetValue();

            //AFTime myTime = new AFTime("04/19/2019 15:55");
            AFTime myTime = new AFTime(DateTime.Now); //could also use AFTime myTime = AFTime.Now;
            AFValue myValue1 = myAttribute.Data.RecordedValue(myTime, AFRetrievalMode.AtOrAfter, myAttribute.DefaultUOM);
            AFValue myValue2 = myAttribute.Data.RecordedValue(myTime, AFRetrievalMode.AtOrBefore, myAttribute.DefaultUOM);
            AFValue myValue3 = myAttribute.Data.InterpolatedValue(myTime, myAttribute.DefaultUOM);

            litSDK.Text = "Version: " + myPISystem.ServerVersion + "<br />Connected user: " + myPISystem.CurrentUserName + "<br />Selected database: " + myDatabase.Name;
            litSDK.Text += "<br />Attribute path: " + myAttribute.GetPath() + "<br />Value: " + myAFValue + "<br />Timestamp: " + myAFValue.Timestamp + "<br />Float Value: " + String.Format("{0:0.##}", myAFValue);
            litSDK.Text += "<br />Input time: " + myTime.LocalTime + "<br />Value at time : " + myValue1.Timestamp + " is " + myValue1.Value + "<br />Value at time : " + myValue2.Timestamp + " is " + myValue2.Value + "<br />Value at time : " + myValue3.Timestamp + " is " + myValue3.Value;

            //string strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
            //string strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

            string strSt = "01/01/2019" + " " + "06:00";
            string strEnd = "06/01/2019" + " " + "18:00";

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);

            var query = "Template:'Alarms_Operations_Cogen_Tier_1' Start:>='" + start1 + "' End:<='" + end1 + "'";

            var stringSearch = new AFEventFrameSearch(myDatabase, "String Search", query);

            litSDK.Text += "<p>Found " + stringSearch.GetTotalCount() + " Event Frames.</p>";
            var results = stringSearch.FindEventFrames(0, false, 10);

            litSDK.Text += "<br />&nbsp;<br /><table border=1 width='800'><tr><th>Event</th><th>Severity</th><th>Description</th><th>Start Time</th><th>End Time</th><th>Duration</th></tr>";
            var counter = 0;
            string strDuration = "";
            DateTime dtDuration = new DateTime(2019,1,1);
            foreach (var item in results)
            {
                strDuration = item.Duration.ToString().Replace("+",",");

                litSDK.Text += "<tr><td>" + item.Name + "</td><td>" + item.Severity + "</td><td>" + item.Description + "</td><td>" + item.StartTime + "</td><td>" + item.EndTime + "</td><td>" + strDuration + "</td></tr>";
                counter++;
                if (counter > 99) break;
            }
            litSDK.Text += "</table>";

            if (Session["shiftid"] == null)
            {
                //Response.Redirect("");
                Session["shiftid"] = "0";
            }
            else
            {
                string _ShiftID = "";
                _ShiftID = Session["shiftid"].ToString();
                ViewState["ShiftID"] = _ShiftID;
            }//end if

            if (!Page.IsPostBack)
            {
                
            }//postback
        }
        [WebMethod]
        public static List<PIData> GetData()
        {
            //AFTime start1 = new AFTime("*-10m");
            //AFTime end1 = new AFTime("*");

            string strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
            string strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 1);
            return dataList;
        }

        public static DataSet CreateEquipmentList(int i)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@equipmenttypeid", SqlDbType.Int, i);
            DataSet ds = _dm.GetDataSet("SelectEquipmentList");
            return ds;
        }
        public static string GetStatus(string Status)
        {
            string StatusString = "";
            if (Status.Contains("Data")) { return "No Data"; }
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
                        StatusString = "Running (Elec)";
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
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            _status = val.Value.ToString();
                            _starttime = val.Timestamp;
                        }
                        else if (i == valsCHL.Count - 1)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                        }

                        i++;
                    } // end foreach i loop
                }//end chiller dataset j loop
            }

            return datalist;
        }
        protected void SetTab(string Status)
        {
            switch (Status)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover.aspx");
                    break;
                case "btnSave":

                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_ChlPmpStatus.aspx");
                    break;
                default:

                    break;
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Server.Execute("CheckStatus.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                string btnID = Convert.ToString(btn.ID);
                SetTab(btn.ID);
                //  SaveStatus(btnID);
            }
        }

        public void UpdateDataBase(string YNField, string YN, string CommentsField, string Comments)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter(YNField, SqlDbType.VarChar, YN);
                _dm.AddParameter(CommentsField, SqlDbType.VarChar, Comments);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(18, "NewTurnOver_Status.UpdateDataBase", "AddEditReportInfo", ex);

            }
        }

        public void SaveStatus(string Status)
        {

            // UpdateDataBase("@ChillerYN", rdoChiller.SelectedValue, "@ChillerComments", txtChiller.Text);

        }
    }
}