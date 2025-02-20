using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class EquipmentStatusAuto : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadShiftLabel();
        }

        private void LoadShiftLabel()
        {
            DataSet ds;
            //Populate litShift
            DataModule dataModule = new DataModule();
            try
            {
                ds = dataModule.GetDataSet("SelectCurrentShift");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {

                        lblMShift.Text = "Current Shift:<strong> " + ds.Tables[0].Rows[0]["Current Shift"].ToString() + "</strong>";
                        //Save ShiftID to session
                        Session["shiftid"] = ds.Tables[0].Rows[0]["ShiftID"].ToString();
                    }
                    else
                    {
                        Session["shiftid"] = 5232;
                    }
                }

                //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(1000, 0, "EquipmentStatusAuto.LoadShiftLabel", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                SubmitToSQLDatabase();
                
            }
        }

        private void LoadEQStatus(int ShiftID)
        {
            try
            {
                DataTable ds = PIEQData.GetShiftEQStatusSQL(ShiftID);
                grdED.DataSource=ds;
                grdED.DataBind();
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(1000, 0, "EquipmentStatusAuto.LoadEQStatus.GetShiftEQStatusSQL", "GetShiftStatusChangePI", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
        }
        protected void InsertTagID(int EQTagID, int ShiftID)
        {
            try
            {
            DataSet ds; 
            DataModule dataModule = new DataModule();
            dataModule.AddParameter("@shiftid", SqlDbType.Int, ShiftID);
            dataModule.AddParameter("@EQTagID", SqlDbType.Int, EQTagID);
            dataModule.AddParameter("@UpdatedBy", SqlDbType.VarChar,  "Automatic Task");
            dataModule.AddParameter("@Comments", SqlDbType.Int, null);
            ds = dataModule.GetDataSet("InsertShiftEQStatus");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(1000, 0, "EquipmentStatusAuto.InsertTagID", "InsertShiftEQStatus", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
        }
        protected void SubmitToSQLDatabase()
        {
            int ShiftID = 0;
            try
            {
                DataModule dataModule = new DataModule();
                DataSet ds1 = dataModule.GetDataSet("SelectCurrentShift");
                if (ds1 != null)
                {
                    if (ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ShiftID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ShiftID"]);

                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(1000, 0, "EquipmentStatusAuto.SubmitToSQLDatabase", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
            if (ShiftID > 0)
            {
                try
                {
                    DataTable ds = PIEQData.GetShiftStatusChangePI();
                    int EQTagID = 0;
                    foreach (DataRow row in ds.Rows)
                    {
                        String cellTagText = row[0].ToString();
                        if (cellTagText.Length > 0)
                        {
                            EQTagID = Convert.ToInt16(cellTagText);
                            InsertTagID(EQTagID, ShiftID);
                        }
                    }
                }
               
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(1000, 0, "EquipmentStatusAuto.SubmitToSQLDatabase", "GetShiftStatusChangePI", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
                LoadEQStatus(ShiftID);
        }
        }
            protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SubmitToSQLDatabase();
        }
    }
}