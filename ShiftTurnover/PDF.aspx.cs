using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using System.Web.Services;
using System.Collections;
using System.Data;
using System.Text;

using IronPdf;
using OSIsoft.AF;
using OSIsoft.AF.Search;
using OSIsoft.AF.Time;
using ShiftTurnover.Components;


namespace ShiftTurnover
{
    public partial class PDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["shiftid"]))
            {
                int shiftid = Convert.ToInt32(Request.QueryString["shiftid"]);
                LoadStatus(shiftid);
                LoadShiftWorkers(shiftid);
                LoadPastEventsGrid(shiftid);
              //  LoadMaximoGrids(shiftid);
                try
                {
                   // LoadPiCriticalAlarmData(shiftid);
                }
                catch (Exception ex)
                { }
                lblMShift.Text = Common.getShiftLabel(shiftid);
                lblCreatedShift.Text = Common.getCurrentShiftCreatedInfo(shiftid);
                string SubMsg = Common.getCurrentShiftSubmittedInfo(shiftid);
                lblSubmittedShift.Text = SubMsg;
                if (SubMsg.Contains("Not Submitted"))
                {
                    lblSubmittedShift.ForeColor = Color.Red;
                }
                else { lblSubmittedShift.ForeColor = Color.Green; }
                PrintPage(shiftid);
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GrPast.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrPast_ItemDataBound);
        }
        #endregion
        protected void LoadPastEventsGrid(int shiftid)
        {
            try
            {
                DataModule _dm = new DataModule();
                try
                {
                    _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                    DataSet ds = _dm.GetDataSet("SelectShiftLiveLog");

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            pnlPast.Visible = false;
                            lblPast.Text = "";
                        }
                        else
                        {
                            lblPast.Visible = false;
                            pnlPast.Visible = true;
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
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }

        #region "Past Events Grid Events"
        private void GrPast_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DataSet _dv = (DataSet)GrPast.DataSource;
            DataColumnCollection _dc = _dv.Tables[0].Columns;

            if (ViewState["gridcols"] == null)
            {
                IEnumerator _colenum = _dv.Tables[0].Columns.GetEnumerator();
                Hashtable _htbl = new Hashtable();
                while (_colenum.MoveNext())
                {
                    String ColName = _colenum.Current.ToString();
                    _htbl.Add(ColName, _dv.Tables[0].Columns.IndexOf(ColName).ToString());
                }
                //Storing column index in viewstate
                ViewState["gridcols"] = _htbl;
            }

            //configure datagrid
            e.Item.Cells[_dc.IndexOf(_dc["personroleid"]) + 1].Visible = false;
            e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Visible = false;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblDeleted = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblDeleted");
                lblDeleted.CssClass = "alert";

                //btnDelete.Attributes.Add("OnClick", "return confirmBox()");

                if (e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Text.ToString() == "1")//deleted
                {
                    e.Item.Cells[_dc.IndexOf(_dc["Description"]) + 1].Style.Value = "text-decoration:line-through;";
                    lblDeleted.Visible = true;
                }
            }
            //Template Column
            e.Item.Cells[0].Width = new Unit(110, UnitType.Pixel);
            e.Item.Cells.AddAt(e.Item.Cells.Count, e.Item.Cells[0]);
        }

        #endregion
        protected void PrintPage(int shiftid)
        {
            string title = "";
            title = Common.getShiftLabel(shiftid);
            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            var pdfPrintOptions = new PdfPrintOptions()
            {
                MarginTop = 0,
                MarginBottom = 0,
                MarginLeft = 0,
                MarginRight = 0,
                Header = new HtmlHeaderFooter()
                {

                    HtmlFragment = "<div style='background-color:#b3112c;color:White; align-content:center'><center><h2 style='color:White;'>Shift Turnover Report for Shift: " + Common.getShiftLabel(shiftid) +"  </h1> </center></div>",
                    Height = 30,
                    Spacing = 0,
                     
                    DrawDividerLine = true

                },
                Footer = new HtmlHeaderFooter()
                {
                    Height = 30,
                    HtmlFragment = "<div style='background-color:#d2f5f7;color:Black;'><center><i>Page {page} of {total-pages}<i>      ( Report Printed On:" + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")  + " ) </center></div>",
                    Spacing = 0,
                    DrawDividerLine = false

                },
                CssMediaType = PdfPrintOptions.PdfCssMediaType.Print
            };
            
             IronPdf.AspxToPdf.RenderThisPageAsPdf(IronPdf.AspxToPdf.FileBehavior.Attachment, "ShiftTurnOverReport.pdf", pdfPrintOptions);
            

        }
        private void LoadShiftWorkers(int _ShiftID)
        {
            try
            {
                
                DataModule _dm = new DataModule();

                _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
                DataSet ds = _dm.GetDataSet("SelectShiftworkers");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                             
                            switch (row["RoleID"])
                            {
                                case 3: // Chief 
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblCrewChief.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblCrewChief.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }
                                    break;
                                case 4: // Chiller Operator 
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblChillerOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblChillerOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;

                                    }
                                    break;
                                case 5: // Boiler Operator   
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblBoilerOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblBoilerOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;

                                    }

                                    break;


                                case 6: // Aux Operator
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblAuxOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblAuxOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }

                                    break;
                                case 7: // Electrician
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblElectrician.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblElectrician.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        private void LoadStatus(int shiftid)
        {
            try
            {

                DataModule _dm = new DataModule();

                _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                DataSet ds = _dm.GetDataSet("SelectReportDetails");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(row["ReadPrevLiveLog"].ToString()) && row["ReadPrevLiveLog"].ToString().Equals("Yes"))
                            { lblPrevAch.Text = "Yes"; }
                            else { lblPrevAch.Text = "No"; }
                            lblShiftWorkersComment.Text = row["ShiftWorkerInfo"].ToString();
  
                         
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadForm", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        //protected void LoadPiCriticalAlarmData(int ShiftID)
        //{
        //    try
        //    {
        //        //string strSt = "01/01/2019" + " " + "06:00";
        //        //string strEnd = "06/01/2019" + " " + "18:00";
        //        string strSt = "";
        //        string strEnd = "";
        //        DataModule _dm = new DataModule();
        //        _dm.AddParameter("@shiftid", SqlDbType.Int, ShiftID);
        //        DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
        //        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in ds.Tables[0].Rows)
        //            {
        //                strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //                strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //            }
        //        }
        //        PISystems myPIsystems = new PISystems();
        //        PISystem myPISystem = myPIsystems["ORF-COGENAF"];
        //        AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
        //        AFTime start1 = new AFTime(strSt);
        //        AFTime end1 = new AFTime(strEnd);
        //        //var query = "Template:'Alarms_Operations_Cogen_Tier_1' Start:>='" + start1 + "' End:<='" + end1 + "'";
        //        //var query = "Template:'Alarms_Operations_Blr_Tier_Test' Start:>='" + start1 + "'";  //End:<='" + end1 + "'";
        //        var query = "categoryName:'Alarms_Tier_1' Start:>='" + start1 + "' Start:<='" + end1 + "'";  //End:<='" + end1 + "'";
        //        var stringSearch = new AFEventFrameSearch(myDatabase, "String Search", query);

        //        ltCriAlm.Text += "<p>Found " + stringSearch.GetTotalCount() + " Event Frames.</p>";
        //        var results = stringSearch.FindEventFrames(0, false, 10);

        //        ltCriAlm.Text += "<br />&nbsp;<br /><table border=1 width='800'><tr><th>Event</th><th>Severity</th><th>Description</th><th>Start Time</th><th>End Time</th><th>Duration</th></tr>";
        //        var counter = 0;
        //        string strDuration = "";
        //        DateTime dtDuration = new DateTime(2019, 1, 1);
        //        foreach (var item in results)
        //        {
        //            if (item.EndTime.ToString() == "12/31/9999 11:59:59 PM")
        //            {
        //                //TimeSpan ts = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss tt") - item.EndTime.ToString("MM/dd/yyyy HH:mm:ss tt");
        //                strDuration = "";
        //            }
        //            else
        //            {
        //                strDuration = item.Duration.ToString().Replace("+", ",");
        //            }


        //            ltCriAlm.Text += "<tr><td>" + item.Name + "</td><td>" + item.Severity + "</td><td>" + item.Description + "</td><td>" + item.StartTime + "</td><td>" + item.EndTime.ToString().Replace("12/31/9999 11:59:59 PM", "") + "</td><td>" + strDuration + "</td></tr>";
        //            counter++;
        //            if (counter > 99) break;
        //        }
        //        ltCriAlm.Text += "</table>";
        //        pnlCAlm.Visible = true;
        //        lblCAlm.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        pnlCAlm.Visible = false;
        //        lblCAlm.Visible = true;
        //    }
        //}

        //protected DataSet LoadMaximoWOData(int i,int _ShiftID)
        //{
        //    //  get From Maximo  @shiftstartdatetime      @shiftenddatetime 
        //    string StoreProc = "SelectMaximoMaintWO";

        //    DataModule _dm = new DataModule();
        //    string strSt = "";
        //    string strEnd = "";

        //    _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
        //    DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    _dm.AddParameter("@shiftenddatetime", SqlDbType.DateTime, Convert.ToDateTime(strEnd));
        //    if (i == 1) { StoreProc = "SelectMaximoMaintWO"; _dm.AddParameter("@shiftstartdatetime", SqlDbType.DateTime, Convert.ToDateTime(strSt)); }
        //    else { StoreProc = "SelectMaximoSchedWO"; }
        //    DataSet ds1 = _dm.GetDataSet(StoreProc);
        //    return ds1;

        //}
        //public void LoadMaximoGrids(int _ShiftID)
        //{

        //    DataSet ds = LoadMaximoWOData(1, _ShiftID);
        //    try
        //    {
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                pnlWO.Visible = false;
        //                lblWO.Visible = true;
        //            }
        //            else
        //            {
        //                lblWO.Visible = false;
        //                pnlWO.Visible = true;
        //                grdWO.DataSource = ds;
        //                grdWO.DataBind();
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadMaximoGrids", "SelectMaximoMaintWO", ex);
        //        Response.Redirect("CustomErrorPage.aspx");
        //    }
        //    finally
        //    {
        //        ds = null;
        //    }
        //    ds = LoadMaximoWOData(2, _ShiftID);
        //    try
        //    {
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                pnlSche.Visible = false;
        //                lblSche.Visible = true;
        //            }
        //            else
        //            {
        //                lblSche.Visible = false;
        //                pnlSche.Visible = true;
        //                grdSche.DataSource = ds;
        //                grdSche.DataBind();
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadMaximoGrids", "SelectMaximoMaintWO", ex);
        //        Response.Redirect("CustomErrorPage.aspx");

        //    }
        //    finally
        //    {
        //        ds = null;
        //    }

        //    ds = LoadMaximoWOData(2, _ShiftID);


        //}

    }
}