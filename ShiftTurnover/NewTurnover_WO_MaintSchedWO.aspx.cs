using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ShiftTurnover
{
    public partial class NewTurnover_WO_MaintSchedWO : System.Web.UI.Page
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
                LoadSmartGrid();
              
            }//postback
        }
        protected DataSet LoadMaximoWOData()
        {
            //  get From Maximo  @shiftstartdatetime      @shiftenddatetime 
            DataModule _dm = new DataModule();
            string strSt = "";
            string strEnd = "";
            int _ShiftID = 0;
            _ShiftID = Convert.ToInt16(Session["shiftid"].ToString());
            _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            _dm.AddParameter("@shiftstartdatetime", SqlDbType.DateTime, Convert.ToDateTime(strSt));
            _dm.AddParameter("@shiftenddatetime", SqlDbType.DateTime, Convert.ToDateTime(strEnd));
            DataSet ds1 = _dm.GetDataSet("SelectMaximoMaintWO");
            return ds1;

        }
        protected void LoadSmartGrid()
        {
            DataSet ds = LoadMaximoWOData();
            try
            {
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        pnlWO.Visible = false;
                      
                    }
                    else
                    {
                        lblWO.Visible = false;
                        pnlWO.Visible = true;
                        grdWO.DataSource = ds;
                        grdWO.DataBind();
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_MaintSchedWO.LoadMaximoWOData", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
            finally
            {
                ds = null;
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
                            rdoWOOther.SelectedValue = row["WOOther"].ToString();
                          
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_MaintSchedWO.LoadStatus", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        public void SaveStatus( )
        {
            try
            {
                 
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("@WOScheduled", SqlDbType.VarChar, rdoWOOther.SelectedValue);
              
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_MaintSchedWO.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
        public void SetTab(string btn)
        {


            switch (btn)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover_WO.aspx");
                    break;
                case "btnSave":
                    Response.Redirect("NewTurnover_WO_MaintSchedWO.aspx");
                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_CriticalAlm.aspx");
                    break;
                default:

                    break;
            }


        }
        
    }
}