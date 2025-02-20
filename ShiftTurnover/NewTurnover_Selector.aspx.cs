using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;

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
using System.Text;

namespace ShiftTurnover
{
    public partial class NewTurnover_Selector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                LoadCurrentShiftLabel();
            }//postback
        }
        private void LoadCurrentShiftLabel()
        {
            DataSet ds;
            int PreviousShiftID = 0;
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
                        PreviousShiftID = Convert.ToInt32(ds.Tables[0].Rows[0]["ShiftID"]) - 1;
                        LoadPreviousShiftLabel(PreviousShiftID);
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
        private void LoadPreviousShiftLabel(int shiftid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            try
            {
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                ds = dataModule.GetDataSet("SelectShiftDateTime");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        lblPShift.Text = "Previous Shift:<strong> " + ds.Tables[0].Rows[0]["Shift"].ToString() + "</strong>";
                    }

                }
               
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Selector.LoadPreviousShiftLabel", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
            }
           
        }
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_Review.aspx");
        }

        
    }
}