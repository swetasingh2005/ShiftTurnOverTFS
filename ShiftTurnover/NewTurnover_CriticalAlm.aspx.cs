using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;
using System.Drawing;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShiftTurnover.Components;

namespace ShiftTurnover
{
    public partial class NewTurnover_CriticalAlm : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
           
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
                //LoadSmartGrid();
              
            }//postback
        }
        protected void LoadPiCriticalAlarmData()
        {
            try
            {
                //string strSt = "01/01/2019" + " " + "06:00";
                //string strEnd = "06/01/2019" + " " + "18:00";
                string strSt = "";
                string strEnd = "";
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                        strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                    }
                }
                PISystems myPIsystems = new PISystems();
                PISystem myPISystem = myPIsystems["ORF-COGENAF"];
                AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
                AFTime start1 = new AFTime(strSt);
                AFTime end1 = new AFTime(strEnd);
                //var query = "Template:'Alarms_Operations_Cogen_Tier_1' Start:>='" + start1 + "' End:<='" + end1 + "'";
                //var query = "Template:'Alarms_Operations_Blr_Tier_Test' Start:>='" + start1 + "'";  //End:<='" + end1 + "'";
                var query = "categoryName:'Alarms_Tier_*' Start:>='" + start1 + "' Start:<='" + end1 + "'";  //End:<='" + end1 + "'";
                var stringSearch = new AFEventFrameSearch(myDatabase, "String Search", query);

                var results = stringSearch.FindEventFrames(0, false, 10);

                //litSDK.Text += "<br />&nbsp;<br />" + "Event Frames Found: <strong>" + stringSearch.GetTotalCount() + "</strong><table border=1 width='800'><tr bgcolor='#c0c0c0'><th style='text-align:center;padding: 6px'>Event</th><th style='text-align:center;padding: 6px'>Severity</th><th style='text-align:center;padding: 6px'>Description</th><th style='text-align:center;padding: 6px'>Start Time</th><th style='text-align:center;padding: 6px'>End Time</th><th style='text-align:center;padding: 6px'>Duration</th></tr>";
                var counter = 0;
                string strDuration = "";
                DateTime dtDuration = new DateTime(2019, 1, 1);
                foreach (var item in results)
                {
                    if(item.EndTime.ToString() == "12/31/9999 11:59:59 PM")
                    {
                        //TimeSpan ts = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt") - item.EndTime.ToString("MM/dd/yyyy HH:mm:ss tt");
                        strDuration = "";
                    }
                    else
                    {
                        strDuration = item.Duration.ToString().Replace("+", ":");
                    }
                    

                   // litSDK.Text += "<tr><td style='text-align:center;padding: 6px'>" + item.Name + "</td><td style='text-align:center;padding: 6px'>" + item.Severity + "</td><td style='text-align:center;padding: 6px'>" + item.Description + "</td><td style='text-align:center;padding: 6px;white-space: nowrap;'>" + item.StartTime + "</td><td style='text-align:center;padding: 6px;white-space: nowrap;'>" + item.EndTime.ToString().Replace("12/31/9999 11:59:59 PM", "") + "</td><td style='text-align:center;padding: 6px'>" + strDuration + "</td></tr>";
                    counter++;
                    if (counter > 99) break;
                }
                //litSDK.Text += "</table>";
            }
            catch (Exception ex)
            {
                //ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_CriticalAlm.LoadPiCriticalAlarmData", "SelectCurrentShift", ex);
                // Response.Redirect("CustomErrorPage.aspx");

            }
        }
        protected void LoadSmartGrid()
        {
             
            try
            {
                        //LoadPiCriticalAlarmData();
                        lblWO.Visible = false;
                        pnlWO.Visible = true;
                   
                
            }

            catch (Exception ex)
            {
                //ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_CriticalAlm.LoadPiCriticalAlarmData", "SelectCurrentShift", ex);
                // Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
               
            }


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                string btnID = Convert.ToString(btn.ID);
                SaveStatus();
                  SetTab(btnID);

            }
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
                            rdoAlarmsYN.SelectedValue = row["AlarmsYN"].ToString();
                            txtAlarmsComments.Text = row["AlarmsComments"].ToString();
                        }
                        ShowHideComment(rdoAlarmsYN.SelectedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_CriticalAlm.LoadStatus", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        public void SaveStatus( )
        {
            try
            {
                DataModule _dm = new DataModule();
                string Comments = "";
                if (rdoAlarmsYN.SelectedValue.Equals("No")) { Comments = txtAlarmsComments.Text; }
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("@AlarmsYN", SqlDbType.VarChar, rdoAlarmsYN.SelectedValue);
                _dm.AddParameter("@AlarmsComments", SqlDbType.VarChar, Comments);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_CriticalAlm.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
        public void SetTab(string btn)
        {


            switch (btn)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover.aspx");
                   // Response.Redirect("NewTurnover_LOTO.aspx");
                    break;
                case "btnSave":
                    Response.Redirect("NewTurnover_CriticalAlm.aspx");
                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_Review.aspx");
                    break;
                default:

                    break;
            }


        }
        protected void ShowHideComment(string YNVal)
        {
            if (YNVal.Equals("No")) { tdAlarmsComments.Visible = true; rfvAlarmsComments.Enabled = true; }
            else { tdAlarmsComments.Visible = false; rfvAlarmsComments.Enabled = false; }
        }
        protected void rdoAlarmsYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideComment(rdoAlarmsYN.SelectedValue);
        }
    }
}