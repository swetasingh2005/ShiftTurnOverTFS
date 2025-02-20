using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using System.Web.Services;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;

using IronPdf;

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
using System.IO;

namespace ShiftTurnover
{
    public partial class OldTurnover : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            int CurrentShiftID = Common.getCurrentShift();
            if (Common.getShiftLabel(CurrentShiftID).Contains("D"))
            { ddlPeriod.SelectedValue = "D"; }
            else
            { ddlPeriod.SelectedValue = "N"; }
        }
        private string GetTopViewAlarmsCount(int shiftID, string criticalAlarmReport)
        {
            string alarmsCount = "0";
            int startIndex;
            int endIndex;
            string filePath = ConfigurationManager.AppSettings["CriticalAlarmsReportsPath"];
            string fileNameSearchPattern = null;
            string createdOn = "";
            string CurrentShift = Common.getShiftLabel(shiftID);
            if (CurrentShift.Length > 0)
            {
                CurrentShift = CurrentShift.Substring(0, 10);
                createdOn = Convert.ToDateTime(CurrentShift).ToString("yyyyMMdd");
            }
            if (Common.getShiftLabel(shiftID).Contains("Day"))
            { createdOn += "_05"; }
            else
            { createdOn += "_17"; }
            switch (criticalAlarmReport)
            {
                case "Auxiliary":
                    fileNameSearchPattern = "NIHCUP-Auxiliary*";
                    break;
                case "Boiler":
                    fileNameSearchPattern = "NIHCUP-BoilerPlant*";
                    break;
                case "Chiller":
                    fileNameSearchPattern = "NIHCUP-ChillerPlant*";
                    break;
                case "WaterTreatment":
                    fileNameSearchPattern = "NIHCUP-WaterTreatment*";
                    break;
                default:
                    alarmsCount = "0";
                    break;
            }
            if (!string.IsNullOrWhiteSpace(fileNameSearchPattern))
            {
                var fileName = (from n in Directory.GetFiles(filePath, fileNameSearchPattern)
                                where Path.GetFileName(n).Contains(createdOn)
                                select Path.GetFileName(n)).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    filePath = Path.Combine(filePath, fileName);
                }
            }
            else
            {
                return alarmsCount;
            }
            if (!File.Exists(filePath))
            {
                return alarmsCount;
            }
            else
            {
                var files = from line in File.ReadLines(filePath)
                            where line.Contains("Alarm count:")
                            select new
                            {
                                Line = line
                            };

                foreach (var f in files)
                {
                    startIndex = f.Line.IndexOf("Alarm count:") + 12;
                    endIndex = f.Line.IndexOf("<br></font></p>");
                    alarmsCount = f.Line.Substring(startIndex, endIndex - startIndex);
                }
            }
            return alarmsCount;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int ShiftID = getShiftID();
                LoadForm(ShiftID);
                //hrCheckAll.HRef = "CheckStatusAll.aspx?PID=" + ShiftID;
                TopViewAlarmAux.HRef = "LoadHtml_Old.aspx?report=Auxiliary&PID=" + ShiftID;
                TopViewAlarmWT.HRef = "LoadHtml_Old.aspx?report=WaterTreatment&PID=" + ShiftID;
                TopViewAlarmB.HRef = "LoadHtml_Old.aspx?report=Boiler&PID=" + ShiftID;
                TopViewAlarmC.HRef = "LoadHtml_Old.aspx?report=Chiller&PID=" + ShiftID;
                // string Admins = System.Configuration.ConfigurationManager.AppSettings["Admin"];
                //if (Session["ssoname"] != null)
                //{
                //    if (Admins.Contains((string)Session["ssoname"]))
                //    { btnNotSubmitted.Visible = true; }
                //    else { btnNotSubmitted.Visible = false; }
                //}
                AuxAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Auxiliary");
                WTAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "WaterTreatment");
                BoilerAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Boiler");
                ChillerAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Chiller");
            }//postback
        }//page load

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
            this.GrPast.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(GrPast_ItemCommand);
            this.GrPast.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrPast_ItemDataBound);
            this.GrActive.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrActive_ItemCommand);
            this.GrActive.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrActive_ItemDataBound);
        }
        #endregion

        private void LoadForm(int ShiftID)
        {
            
            LoadPastEventsGrid(ShiftID);
            LoadDocumentDataGrid(ShiftID);
            //LoadMaximoGrids(ShiftID);
            try
            {
                LoadPiCriticalAlarmData(ShiftID);
            }
            catch (Exception ex)
            { }
           
            LoadShiftWorkers(ShiftID);
            LoadStatus(ShiftID);
            if (ShiftID == 0)
            {
                lblErrorMsg.Text = "There is no Shift Turnover Report for " + Convert.ToDateTime(txtDate.Text).ToString("MM/dd/yyyy") + " " + ddlPeriod.SelectedItem.Text + " Shift";
                pnlRpt.Visible = false;
            }
            else
            {
                lblMShift.Text = Common.getShiftLabel(ShiftID);
                lblCreatedShift.Text = Common.getCurrentShiftCreatedInfo(ShiftID);
                string SubMsg = Common.getCurrentShiftSubmittedInfo(ShiftID);
                lblSubmittedShift.Text = SubMsg;
                if (SubMsg.Contains("Not Submitted")){
                    lblSubmittedShift.ForeColor = Color.Red;
                    if (Common.isCrewLeader(Convert.ToInt32(Session["personroleid"])))
                    { btnSubmitNow.Visible = true; }
                    else { btnSubmitNow.Visible = false; }
                }
                else { lblSubmittedShift.ForeColor = Color.Green;
                    btnSubmitNow.Visible = false;
                }
                
                pnlRpt.Visible = true;
                lblErrorMsg.Text = "";
            }
        }
        protected int SaveStatus( )
        {
            int ShiftID = 0; 
            try
            {
                ShiftID= getShiftID();
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, ShiftID);
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.ExecuteCommand("submitreport");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.SaveStatus", "submitreport", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
            return ShiftID;
        }
        protected void LoadPiCriticalAlarmData(int ShiftID)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                pnlCAlm.Visible = false;
                
            }
        }
        
        private void LoadStatus(int _ShiftID)
        {
            try
            {
                DataModule _dm = new DataModule();

        _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
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
            ErrorHandler.LogErrorToDB((int) Session["personid"], "OldTurnover.LoadStatus", "SelectReportDetails", ex);
            Response.Redirect("CustomErrorPage.aspx");
        }
    }
    private void ResetShiftWorkers( )
        {
            lblElectrician.Text = "";
            lblCrewChief.Text = "";
            lblChillerOperator.Text = "";
            lblBoilerOperator.Text = "";
            lblAuxOperator.Text = "";
            lblCogenOperator.Text = "";
            lblShiftWorkersComment.Text = "";
        }
        private void LoadShiftWorkers(int _ShiftID)
        {
            try
            {
                ResetShiftWorkers();
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
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.LoadShiftWorkers", "SelectShiftworkers", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void LoadPastEventsGrid(int _ShiftID)
        {
            DataModule _dm = new DataModule();
             
            _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftLiveLog_ShiftSup");

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    pnlPast.Visible = false;
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

        #region "Past Events Grid Events"
        private void GrPast_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            Common _cm = new Common();
            //get column index
            Hashtable _table = new Hashtable();
            _table = (Hashtable)ViewState["gridcols"];
            int ShiftID = getShiftID();
            int _livelogid = Convert.ToInt32(_table["Live Log ID"].ToString());
          
            switch (e.CommandName)
            {
                case "View":
                    {
                        int value = Convert.ToInt32(e.Item.Cells[_livelogid].Text.ToString());
                        LoadHistoryGrid(value, ShiftID);
                        LoadPastEventsGrid(ShiftID);
                        LoadDocumentDataGrid(ShiftID);
                        break;
                    }
            }
        }
        protected void LoadHistoryGrid(int LiveLogID,int ShiftID)
        {
            
            if (Session["shiftid"] != null)
            {
                ShiftID = Convert.ToInt32(Session["shiftid"]);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "newWindow", "PopupReco(" + LiveLogID + ", " + ShiftID + ");", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("javascript:PopupReco(" + value + ");", value));

        }
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
            e.Item.Cells[_dc.IndexOf(_dc["Edited"]) + 1].Visible = false;
            ImageButton btnView = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnView");
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblDeleted = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblDeleted");
                lblDeleted.CssClass = "alert";
                if (e.Item.Cells[_dc.IndexOf(_dc["Edited"]) + 1].Text.ToString() == "Y")//deleted
                {
                    btnView.Visible = true;
                }
                else
                {
                    btnView.Visible = false;
                }
                    if ( e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Text.ToString() == "1")//deleted
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

        protected void LoadDocumentDataGrid(int _ShiftID)
        {
            if (Session["shiftid"] != null)
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(_ShiftID));
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

        
        public static int getShiftID(DateTime Date, string shiftperiod)
        {
            DataSet ds;
            DataSet ds1;
            int ShiftID = 0;
            try
            {
                DataModule _dm = new DataModule();
                DataModule _dm1 = new DataModule();
                string shiftnum = "A";
                _dm.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(Date));
                _dm.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                ds = _dm.GetDataSet("SelectShiftNum");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 &&  ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        shiftnum = Convert.ToString(ds.Tables[0].Rows[0]["Shift Number"]);
                    }

                }
                _dm1.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(Date));
                _dm1.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                _dm1.AddParameter("@shiftnum", SqlDbType.VarChar, shiftnum);
                ds1 = _dm1.GetDataSet("SelectShiftID");
                if (ds1 != null)
                {
                    if (ds.Tables.Count > 0 &&  ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ShiftID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ShiftID"]);
                    }

                }

            }
            catch (System.Exception ex)
            {
                 
            }
            return ShiftID;
        }

       
        protected void btnLoadReport_Click(object sender, EventArgs e)
        {

            int ShiftID = getShiftID();
            //hrCheckAll.HRef = "CheckStatusAll.aspx?PID=" + ShiftID;
            TopViewAlarmAux.HRef = "LoadHtml_Old.aspx?report=Auxiliary&PID=" + ShiftID;
            TopViewAlarmWT.HRef = "LoadHtml_Old.aspx?report=WaterTreatment&PID=" + ShiftID;
            TopViewAlarmB.HRef = "LoadHtml_Old.aspx?report=Boiler&PID=" + ShiftID;
            TopViewAlarmC.HRef = "LoadHtml_Old.aspx?report=Chiller&PID=" + ShiftID;
            AuxAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Auxiliary");
            WTAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "WaterTreatment");
            BoilerAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Boiler");
            ChillerAlarmCount.Text = GetTopViewAlarmsCount(ShiftID, "Chiller");
            LoadForm(ShiftID);
             
        }
        protected   int getShiftID()
        {
            DataSet ds;
            DataSet ds1;
            int ShiftID = 0;
            try
            {
                 DataModule _dm = new DataModule();
                DataModule _dm1 = new DataModule();
                string shiftperiod = ddlPeriod.SelectedValue;
                string shiftnum = "A";
                 
                _dm.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(txtDate.Text));
                _dm.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                ds = _dm.GetDataSet("SelectShiftNum");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        shiftnum =  Convert.ToString( ds.Tables[0].Rows[0]["Shift Number"]) ;
                    }
                    
                }
                _dm1.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(txtDate.Text));
                _dm1.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                _dm1.AddParameter("@shiftnum", SqlDbType.VarChar, shiftnum);
                ds1 = _dm1.GetDataSet("SelectShiftID");
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
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.getShiftID", "SelectShiftNum/SelectShiftID", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            return ShiftID;
        }
        protected void btnResetReport_Click(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            btnSubmitNow.Visible = false;
            //ResetShiftWorkers();
            txtDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); 
            ddlPeriod.SelectedValue = "D";
            int ShiftID = getShiftID();

            LoadForm(ShiftID);
        }

        protected void btnResetReport_Click1(object sender, EventArgs e)
        {
            int ShiftID = getShiftID();
            if (ShiftID > 0)
            {
                string url = "PDF.aspx?shiftid=" + ShiftID;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);
            }
            
             
        }

        protected void btnNotSubmitted_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.open ('ReportNotSubmitted.aspx','_blank');</script>");
        }

        protected void btnSubmitNow_Click(object sender, EventArgs e)
        {
            int ShiftID=SaveStatus();
            string msg = "Shift TurnOver Report for the Selected Shift (" + Common.getShiftLabel(ShiftID) + ") has been successfully submitted.";
            lblErrorMsg.Text = msg;
            LoadStatus(ShiftID);
        }
    }
}