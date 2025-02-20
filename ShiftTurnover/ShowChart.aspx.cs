using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using ShiftTurnover.Components;
using System.Data;


namespace ShiftTurnover
{
    public partial class ShowChart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                int LID = 0;
                int SID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["LID"]))
                {
                    LID = Convert.ToInt32(Request.QueryString["LID"]);
                    
                    if (!String.IsNullOrEmpty(Request.QueryString["SID"]))
                    {
                        SID = Convert.ToInt32(Request.QueryString["SID"]);
                        lblShiftID.Text ="Workflow History For Shift= " + Common.getShiftLabel(SID);   ;
                    }
                        LoadHistoryGrid(LID);
                }

            }

        }
        protected void LoadHistoryGrid(int LiveLogID)
        {
            DataModule _dm = new DataModule();
            try
            {
               
                _dm.AddParameter("@LiveLogID", SqlDbType.Int, LiveLogID);
                DataSet ds = _dm.GetDataSet("SelectShiftLiveLogHistory");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                       
                    }
                    else
                    {
                        GrPast.DataSource = ds;
                        GrPast.DataBind();
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                _dm = null;

            }
        }
    }
}