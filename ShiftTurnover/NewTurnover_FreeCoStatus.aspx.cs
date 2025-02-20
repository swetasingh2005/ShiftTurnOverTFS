using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Drawing;
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

namespace ShiftTurnover
{
    public partial class NewTurnover_FreeCoStatus : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
             
        }


        protected void Page_Load(object sender, EventArgs e)
        {
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
                int CurrentShiftID = Common.getCurrentShift();
                MakePageReadOnly(CurrentShiftID);
                lblMShift.Text = "Current Shift: " + Common.getShiftLabel(CurrentShiftID);

                DateTime dtCurrent = DateTime.Now;
                DateTime dt630am = Convert.ToDateTime("06:30:00 AM");
                DateTime dt630pm = Convert.ToDateTime("06:30:00 PM");
                DateTime dt6am = Convert.ToDateTime("06:00:00 AM");
                DateTime dt6pm = Convert.ToDateTime("06:00:00 PM");
                int intComp630am = DateTime.Compare(dtCurrent, dt630am); //if now<630am then <0, if now=630am then =0, if now>630am then >0
                int intComp630pm = DateTime.Compare(dtCurrent, dt630pm);
                int intComp6pm = DateTime.Compare(dtCurrent, dt6pm);
                int intComp6am = DateTime.Compare(dtCurrent, dt6am);

                if (intComp6pm > 0 && intComp630pm < 0)
                {
                    lblMShift.Text += "<br />NOTE: You are Viewing/Editing Previous Shift. New Shift will be available at 6.30 pm.";
                    lblMShift.ForeColor = Color.Red;
                }
                if (intComp6am > 0 && intComp630am < 0)
                {
                    lblMShift.Text += "<br />NOTE: You are Viewing/Editing Previous Shift. New Shift will be available at 6.30 am.";
                    lblMShift.ForeColor = Color.Red;
                }

                LoadStatus();
            }//postback
        }
        protected void LoadStatus()
        {
            try
            {
                int shiftid = Convert.ToInt16(ViewState["ShiftID"]);
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                DataSet ds = _dm.GetDataSet("SelectReportDetails");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            rdoYN.SelectedValue = row["FreeCoolingYN"].ToString();
                            txtComment.Text = row["FreeCoolingComments"].ToString();

                        }
                    }
                }
                ShowHideComment(rdoYN.SelectedValue);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_FreeCoStatus.LoadStatus", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        private void MakePageReadOnly(int CurrentShiftID)
        {
            if (Common.isCurrentShiftSubmitted(CurrentShiftID))
            {
                btnSave.Enabled = false;
                btnNext.Enabled = false;
                btnPre.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnNext.Enabled = true;
                btnPre.Enabled = true;
            }
        }

        [WebMethod (EnableSession = true)]
        public static List<PIData> GetData()
        {
            string strSt = "";
            string strEnd = "";
            DataModule _dm = new DataModule();
            DataSet ds = _dm.GetDataSet("SelectCurrentShift");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
            //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            List<PIData> dataList = new List<PIData>();
            if (HttpContext.Current.Session["DS4"] == null)
            {
                dataList = CreateDataList(start1, end1, interval, 4);
            }
            else
            {
                dataList =(List <PIData>)HttpContext.Current.Session["DS4"];
            }
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
            if (Status.Contains("Data")) { return "No Data"; } else {

                switch (Status)
                {
                    case "0": //
                    case "Standby":
                        StatusString = "Standby";
                        break;
                    case "1" : //
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

        protected void rdoFreeCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideComment(rdoYN.SelectedValue);

        }
        protected void ShowHideComment(string YNVal)
        {
            if (YNVal.Equals("No")) { txtComment.Visible = true; lblComment.Visible = true; rfvComment.Enabled = true; }
            else { txtComment.Visible = false; lblComment.Visible = false; rfvComment.Enabled = false; }
        }
        protected void SaveStatus()
        {
            try
            {
                string Comments = "";
                if (rdoYN.SelectedValue.Equals("No")) { Comments = txtComment.Text; }
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("@FreeCoolingYN", SqlDbType.VarChar, rdoYN.SelectedValue);
                _dm.AddParameter("@FreeCoolingComments", SqlDbType.VarChar, Comments);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(18, "NewTurnOver_FreeCoStatus.SaveStatus", "AddEditReportInfo", ex);

            }
        }
        protected void SetTab(string Status)
        {
            switch (Status)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover_CTStatus.aspx");
                    break;
                case "btnSave":

                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_BlrStatus.aspx");
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
                SaveStatus();
                 SetTab(btn.ID);
            }
        }
        
    }
}