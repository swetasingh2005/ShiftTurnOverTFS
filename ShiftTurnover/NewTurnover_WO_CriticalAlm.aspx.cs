using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;

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
    public partial class NewTurnover_WO_CriticalAlm : System.Web.UI.Page
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
                
                LoadStatus();
                LoadSmartGrid();
              
            }//postback
        }
        protected void LoadMaximoWOData()
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
                var query = "Template:'Alarms_Operations_Cogen_Tier_1' Start:>='" + start1 + "' End:<='" + end1 + "'";
                var stringSearch = new AFEventFrameSearch(myDatabase, "String Search", query);

                litSDK.Text += "<p>Found " + stringSearch.GetTotalCount() + " Event Frames.</p>";
                var results = stringSearch.FindEventFrames(0, false, 10);

                litSDK.Text += "<br />&nbsp;<br /><table border=1 width='800'><tr><th>Event</th><th>Severity</th><th>Description</th><th>Start Time</th><th>End Time</th><th>Duration</th></tr>";
                var counter = 0;
                string strDuration = "";
                DateTime dtDuration = new DateTime(2019, 1, 1);
                foreach (var item in results)
                {
                    strDuration = item.Duration.ToString().Replace("+", ",");

                    litSDK.Text += "<tr><td>" + item.Name + "</td><td>" + item.Severity + "</td><td>" + item.Description + "</td><td>" + item.StartTime + "</td><td>" + item.EndTime + "</td><td>" + strDuration + "</td></tr>";
                    counter++;
                    if (counter > 99) break;
                }
                litSDK.Text += "</table>";
            }
            catch (Exception ex)
            {
                //ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_CriticalAlm.LoadMaximoWOData", "SelectCurrentShift", ex);
               // Response.Redirect("CustomErrorPage.aspx");

            }
        }
        protected void LoadSmartGrid()
        {
             
            try
            {
                       LoadMaximoWOData();
                        lblWO.Visible = false;
                        pnlWO.Visible = true;
                   
                
            }

            catch (Exception ex)
            {
                //ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_CriticalAlm.LoadMaximoWOData", "SelectCurrentShift", ex);
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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_CriticalAlm.LoadStatus", "SelectReportDetails", ex);
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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_CriticalAlm.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
        public void SetTab(string btn)
        {


            switch (btn)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover_WO_MaintSchedWO.aspx");
                    break;
                case "btnSave":
                    Response.Redirect("NewTurnover_WO_CriticalAlm.aspx");
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