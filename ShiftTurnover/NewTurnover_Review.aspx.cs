using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;

using System.Web.Services;
using System.Collections;
using System.Data;
using System.Text;

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

using System.Net;

using ShiftTurnover.Components;
using IronPdf;

namespace ShiftTurnover
{
    public partial class NewTurnover_Review : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            btnNext.Attributes.Add("OnClick", "return confirmBox()");
        }
        private void MakePageReadOnly(int CurrentShiftID)
        {
            if (Common.isCurrentShiftSubmitted(CurrentShiftID))
            {
                lblTimeRemain.Text = "  ";
                 
                litSDK.Text = "Thank You For Submitting the Shift Turnover " + Common.getShiftLabel(CurrentShiftID) + "  Report!";
            }
            else
            {
                lblTimeRemain.Text = "Submit Button will be available ONLY FOR THE CREW LEADER AND SHIFT SUPERVISOR when all required fields are filled.";
                
                
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
                //hrCheckAll.HRef= "CheckStatusAll.aspx?PID=" + _ShiftID;  
            }//end if

            if (!Page.IsPostBack)
            {
                LoadForm();
            }//postback
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
            this.GrActive.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrActive_ItemCommand);
            this.GrActive.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrActive_ItemDataBound);
        }
        #endregion

        private void LoadForm()
        {
            LoadPastEventsGrid();
            LoadDocumentDataGrid();
            //LoadLOTODataGrid();
           
                LoadShiftWorkers();
                LoadStatus();

                int shiftid = 0;
                shiftid = Convert.ToInt32(Session["shiftid"]);
                lblMShift.Text = "Current Shift: " + Common.getShiftLabel(shiftid);

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

            if (!Common.isCurrentShiftSubmitted(shiftid) &&
                    ValidateShiftTurnoverForm()
                    && Common.isCrewLeader(Convert.ToInt32(Session["personroleid"])))
            { btnNext.Enabled = true; }
                else { btnNext.Enabled = false; }
                MakePageReadOnly(shiftid);
            }


        private void LoadStatus()
        {
            try
            {
                int shiftid = 0;
                shiftid = Convert.ToInt32(Session["shiftid"]);
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
                            { lblPrevAch.Text = "Yes"; lblPrevAch.ForeColor = Color.Green; }
                            else { lblPrevAch.Text = "No"; lblPrevAch.ForeColor = Color.Red; }
                            lblShiftWorkersComment.Text = row["ShiftWorkerInfo"].ToString();
                            lblCriAlsCorrect.Text = row["AlarmsYN"].ToString();
                            lblCriAlsComment.Text = row["AlarmsComments"].ToString();
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
        private void LoadShiftWorkers()
        {
            try
            {
                int shiftid = 0;
                shiftid = Convert.ToInt32(Session["shiftid"]);
                DataModule _dm = new DataModule();

                _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                DataSet ds = _dm.GetDataSet("SelectShiftworkers");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            switch (row["RoleID"])
                            {

                                case 10: // Shift Supervisor 
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblShiftSup.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblShiftSup.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }
                                    break;
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

                                case 8: // Cogen Operator
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblCogenOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblCogenOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
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
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadShiftWorkers", "SelectShiftworkers", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void LoadPastEventsGrid()
        {
            try
            {
                DataModule _dm = new DataModule();
                try
                {


                    int shiftid = Convert.ToInt32(Session["shiftid"]);
                    _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                    DataSet ds = _dm.GetDataSet("SelectShiftLiveLog");

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            pnlPast.Visible = false;
                            //lblPast.Text = "";
                            lblPast.Visible = true;
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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Review.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
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

        protected void LoadDocumentDataGrid()
        {
            if (Session["shiftid"] != null)
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                DataSet ds = _dm.GetDataSet("selectattachmentslist");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tblActive.Visible = true;
                        GrActive.DataSource = ds;
                        GrActive.DataBind();

                    }
                    else
                    {
                        tblActive.Visible = false;
                    }
                }
            }
            else
            {
                tblActive.Visible = false;
            }
        }

        #region "Attachments"

        private void ShowAttachment(string AttachmentID)
        {

            if (string.IsNullOrWhiteSpace(AttachmentID))
            {
                //can not display
                return;
            }

            if (AttachmentID != "" && AttachmentID != null)
            {
                DataModule dm2 = new DataModule();
                dm2.AddParameter("@attachmentid", SqlDbType.Int, System.Convert.ToInt32(AttachmentID));
                DataSet ds2 = dm2.GetDataSet("selectattachmentdetails");

                string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();

                string strFileLocation = ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + '\\' + strStoredFileName;

                //byte[] aData;
                WebClient req = new WebClient();
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = ds2.Tables[0].Rows[0]["ContentType"].ToString();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + ds2.Tables[0].Rows[0]["OriginalFileName"].ToString());
                byte[] aData = req.DownloadData(strFileLocation);
                Response.BinaryWrite(aData);
                //Response.TransmitFile(strFileLocation);
                //Response.Flush();
                Response.End();

                //following works from url
                //string strSaveFileAsPath = "attachmentsGSS/" + ds2.Tables[0].Rows[0]["StoredFileName"].ToString();
                //HttpContext.Current.Response.Redirect(strSaveFileAsPath, false); //gss added false

            }
        }

        #endregion

        #region "Document Grid Events"
        private void GrActive_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DataSet _dv = (DataSet)GrActive.DataSource;
            DataColumnCollection _dc = _dv.Tables[0].Columns;

            if (ViewState["gridcols2"] == null)
            {
                IEnumerator _colenum = _dv.Tables[0].Columns.GetEnumerator();
                Hashtable _htbl = new Hashtable();
                while (_colenum.MoveNext())
                {
                    String ColName = _colenum.Current.ToString();
                    _htbl.Add(ColName, _dv.Tables[0].Columns.IndexOf(ColName).ToString());
                }
                //Storing column index in viewstate
                ViewState["gridcols2"] = _htbl;
            }

            //configure datagrid
            e.Item.Cells[_dc.IndexOf(_dc["UploadPersonRoleID"]) + 1].Visible = false;

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
            //Template Column
            e.Item.Cells[0].Width = new Unit(110, UnitType.Pixel);
            e.Item.Cells.AddAt(e.Item.Cells.Count, e.Item.Cells[0]);
        }

        private void GrActive_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            Common _cm = new Common();
            //get column index
            Hashtable _table = new Hashtable();
            _table = (Hashtable)ViewState["gridcols2"];

            int _attachmentid = Convert.ToInt32(_table["Attachment ID"].ToString());

            switch (e.CommandName)
            {
                case "View":
                    {
                        if (e.Item.Cells[_attachmentid].Text == "&nbsp;")
                        {
                            return;
                        }
                        else
                        {
                            ShowAttachment(e.Item.Cells[_attachmentid].Text);
                        }

                        break;
                    }
            }
        }

        #endregion
      
        //protected DataSet LoadMaximoWOData(int i)
        //{
        //    //  get From Maximo  @shiftstartdatetime      @shiftenddatetime 
        //    string StoreProc = "SelectMaximoMaintWO";

        //    DataModule _dm = new DataModule();
        //    string strSt = "";
        //    string strEnd = "";
        //    int _ShiftID = 0;
        //    _ShiftID = Convert.ToInt16(Session["shiftid"].ToString());
        //    _dm.AddParameter("@shiftid", SqlDbType.Int,_ShiftID);
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
        //public void LoadMaximoGrids()
        //{

        //    DataSet ds = LoadMaximoWOData(1);
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
        //    ds = LoadMaximoWOData(2);
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




        //}

        //protected void LoadLOTODataGrid()
        //{
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectMaximoLOTOIndex");
        //    try
        //    {
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                pnlLOTO.Visible = false;

        //            }
        //            else
        //            {
        //                lblLOTO.Visible = false;
        //                pnlLOTO.Visible = true;
        //                grLOTO.DataSource = ds;
        //                grLOTO.DataBind();
        //            }
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Review.LoadLOTODataGrid", "SelectMaximoLOTOIndex", ex);
        //        Response.Redirect("CustomErrorPage.aspx");
        //    }
        //    finally
        //    {
        //        ds = null;
        //    }


        //}

        //[WebMethod]
        //public static List<PIData> GetChillerData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 1);
        //    return dataList;
        //}
        //[WebMethod]
        //public static List<PIData> GetChillerPumpData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 2);
        //    return dataList;
        //}
        //[WebMethod]
        //public static List<PIData> GetBlrData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 5);
        //    return dataList;
        //}
        //[WebMethod]
        //public static List<PIData> GetFreeCoData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 4);
        //    return dataList;
        //}
        //[WebMethod]
        //public static List<PIData> GetROPumpData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 6);
        //    return dataList;
        //}
        //[WebMethod]
        //public static List<PIData> GetCTData()
        //{
        //    string strSt = "";
        //    string strEnd = "";
        //    DataModule _dm = new DataModule();
        //    DataSet ds = _dm.GetDataSet("SelectCurrentShift");
        //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
        //            strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
        //        }
        //    }
        //    //   strSt = DateTime.Now.ToString("MM/dd/yyyy") + " " + "06:00";
        //    //  strEnd = DateTime.Now.ToString("MM/dd/yyyy") + " " + "18:00";

        //    AFTime start1 = new AFTime(strSt);
        //    AFTime end1 = new AFTime(strEnd);
        //    //AFTime end1 = new AFTime("*");
        //    AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
        //    //AFTimeSpan interval = new AFTimeSpan(minutes: 30);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
        //    //trying 5 min interval
        //    int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval"]);
        //    AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
        //    List<PIData> dataList = new List<PIData>();
        //    dataList = CreateDataList(start1, end1, interval, 3);
        //    return dataList;
        //}
        
        
        
        protected void btnPre_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_CriticalAlm.aspx");
        }

        private string LoadReportDetails()
        {
            StringBuilder sb = new StringBuilder();
            string errorText = "";
            try
            {
                DataModule _dm = new DataModule();
                DataSet ds;
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                ds = _dm.GetDataSet("SelectReportDetails");
                bool isValid = true;
                sb.Append("  ");
                sb.Append(" <ui>");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (string.IsNullOrEmpty(row["ReadPrevLiveLog"].ToString()))
                            {
                                sb.Append("<li>Acknowledge Previous Shift's Live Log entries on Acknowledgement Tab</li>");
                                sb.AppendLine();
                                isValid = false;
                            }
                            if (string.IsNullOrEmpty(row["ShiftWorkerInfo"].ToString()))
                            {
                                sb.Append("<li> Shift Worker Info on ShiftWorker Tab </li>");
                                sb.AppendLine();
                                isValid = false;
                            }
                            
                            if (string.IsNullOrEmpty(row["AlarmsYN"].ToString()))
                            {
                                sb.Append("<li>Response on Alarms->Critical Alarms Tab</li>");
                                sb.AppendLine();
                                isValid = false;
                            }
                           

                        }
                    }
                    else
                    {
                         
                       sb.Append("<li> Shift Worker Info on ShiftWorker Tab </li>");
                       sb.AppendLine();
                       sb.Append("<li> Response on  Previous Shift's Live Log entries on Acknowledgement Tab </li>");
                       sb.Append("<li>Response on Alarms->Critical Alarms Tab</li>");
                       sb.AppendLine();
                       isValid = false;
                        
                    }
                }
                else
                {
                    sb.Append("<li> Shift Worker Info on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    sb.Append("<li> Response on  Previous Shift's Live Log entries on Acknowledgement Tab </li>");
                    sb.Append("<li>Response on Alarms->Critical Alarms Tab</li>");
                    sb.AppendLine();
                    isValid = false;
                }
                sb.Append("</ui>");
                if (isValid) { sb.Clear(); }
                errorText = sb.ToString();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                sb = null;

            }
            return errorText;
        }
        private string LoadShiftworkersbyrole()
        {
            StringBuilder sb = new StringBuilder();
            bool isValid = true;
            string errorText = "";
            sb.Append(" <ui>");
            try
            {
                DataModule _dm = new DataModule();
                DataSet ds;
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 3);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append(" <li>Crew Leader(s) on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    isValid = false;
                }
                ds.Clear();

                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 4);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append("<li> Chiller  Operator(s) on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    isValid = false;
                }
                ds.Clear();

                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 5);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append("<li> Boiler Operator(s) on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    isValid = false;
                }
                ds.Clear();

                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 6);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append("<li> Auxiliary Operator(s) on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    isValid = false;
                }
                ds.Clear();

                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 8);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append("<li> Cogen Operator(s) on ShiftWorker Tab </li>");
                    sb.AppendLine();
                    isValid = false;
                }
                ds.Clear();

                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@PrimaryYN", SqlDbType.VarChar, "Y");
                _dm.AddParameter("@roleid", SqlDbType.Int, 7);
                ds = _dm.GetDataSet("SelectShiftworkersbyrole");
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    sb.Append("<li> Shift Electrician(s) on ShiftWorker Tab  </li>");
                    sb.AppendLine();
                    isValid = false;
                }

                sb.Append("</ui>");
                if (isValid) { sb.Clear(); }
                errorText= sb.ToString();
            }
            catch (Exception ex)
            {
                 
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.LoadForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                sb = null;

            }
            return errorText;
              
        }
        protected bool ValidateShiftTurnoverForm()
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string shiftWorkes = "";
            string ReportDetails = "";
            bool validated = false;
            try
            {
                
                shiftWorkes = LoadShiftworkersbyrole();
                ReportDetails = LoadReportDetails();
                if (shiftWorkes.Length > 0)
                {
                    sb.Append(LoadShiftworkersbyrole());
                    sb.AppendLine();
                }
                if (ReportDetails.Length > 0)
                {
                    sb.Append(LoadReportDetails());
                    sb.AppendLine();
                }


                if (sb.ToString().Length == 0)
                {
                    //litSDK.Text = "<span style='color:Green'> Are you sure you want to Submit this Shift Turnover Report? After Submitting you won't be able to edit this report. </span>";
                    validated = true;
                }
                else
                {
                    sb1.Append("<span style='color:red'> Following Fields are Required:");
                    sb1.AppendLine();
                    litSDK.Text = sb1.ToString() + sb.ToString() + "</span> ";
                   
                }
            }
            
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int) Session["personid"], "NewTurnover_Review.ValidateShiftTurnoverForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                sb1 = null;
                sb = null;
            }
            return validated;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            
            SaveStatus();
            string msg = " ";
            int CurrentShiftID = Common.getCurrentShift();
            msg = "Shift TurnOver Report for Current Shift (" + Common.getShiftLabel(CurrentShiftID) + ") has been successfully submitted.";
            DisplayAJAXMessageWithRedirect(this, msg, "MainPage.aspx");
            
        }
        protected void SaveStatus()
        {
            try
            {
               
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.ExecuteCommand("submitreport");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.SaveStatus", "submitreport", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
        protected   void DisplayAJAXMessageWithRedirect(System.Web.UI.Control page, string msg, string url)
        {
            string myScript = string.Format("alert('{0}');window.location='" + url + "';", msg.Replace("'", "").Replace("\"", "").Replace("\\", "").Replace("\r", "").Replace("\n", ""));
            ScriptManager.RegisterStartupScript(page, page.GetType(), "MyScript", myScript, true);
        }
    }
}