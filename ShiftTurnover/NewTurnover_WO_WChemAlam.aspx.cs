using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class NewTurnover_WO_WChemAlam : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadShiftLabel();
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
        private void LoadShiftLabel()
        {
            if (Session["shiftid"] == null)
            {
                int shiftid = Convert.ToInt32(Session["shiftid"].ToString());
                MakePageReadOnly(shiftid);
                DataSet ds;
                //Populate litShift
                DataModule dataModule = new DataModule();
                try
                {
                    dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                    ds = dataModule.GetDataSet("SelectShiftDateTime");
                    if (ds != null)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            lblMShift.Text = "Current Shift:<strong> " + ds.Tables[0].Rows[0]["Shift"].ToString() + "</strong>";
                        }

                    }

                    //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                }
                catch (System.Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Review.LoadShiftLabel", "SelectShiftDateTime", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

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
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_WO_WChemAlam.LoadSmartGrid", "SelectMaximoMaintWO", ex);
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
                            rdoAlarmsYN.SelectedValue = row["ChemAlarmsYN"].ToString();
                            txtAlarmsComments.Text = row["ChemAlarmsComments"].ToString();
                        }
                        ShowHideComment(rdoAlarmsYN.SelectedValue);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_WO_WChemAlam.LoadStatus", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        public void SaveStatus( )
        {
            try
            {
                string Comments = "";
                if (rdoAlarmsYN.SelectedValue.Equals("No")) { Comments = txtAlarmsComments.Text; }
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("ChemAlarmsYN", SqlDbType.VarChar, rdoAlarmsYN.SelectedValue);
                _dm.AddParameter("ChemAlarmsComments", SqlDbType.VarChar, Comments);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO_WChemAlam.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        public void SetTab(string btn)
        {


            switch (btn)
            {
                case "btnPre": //
                    Response.Redirect("NewTurnover_WO_WCWO.aspx");
                    break;
                case "btnSave":
                    Response.Redirect("NewTurnover_WO_WChemAlam.aspx");
                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_Review.aspx");
                    break;
                default:

                    break;
            }


        }
        protected void rdoAlarmsYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowHideComment(rdoAlarmsYN.SelectedValue);
        }
        protected void ShowHideComment(string YNVal)
        {
            if (YNVal.Equals("No")) { tdAlarmsComments.Visible = true; rfvAlarmsComments.Enabled = true; }
            else { tdAlarmsComments.Visible = false; rfvAlarmsComments.Enabled = false; }
        }

    }
}