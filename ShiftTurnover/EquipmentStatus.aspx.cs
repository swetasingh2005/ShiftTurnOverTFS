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
    public partial class EquipmentStatus : System.Web.UI.Page
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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MasterPage.LoadShiftLabel", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}