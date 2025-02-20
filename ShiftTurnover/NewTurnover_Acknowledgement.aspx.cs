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
using System.Configuration;
using System.Net;

using ShiftTurnover.Components;
using IronPdf;

namespace ShiftTurnover
{
    public partial class NewTurnover_Acknowledgement : System.Web.UI.Page
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

                    }
                }

                //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Acknowledgement.LoadShiftLabel", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
                dataModule = null;
            }
        }
        private void MakePageReadOnly(int CurrentShiftID)
        {
             
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
                LoadForm();
                LoadStatus();
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
                
                int shiftid = 0;
                int PrevShiftid = 0;
                shiftid = Convert.ToInt32(Session["shiftid"]);
                PrevShiftid = getPrevShiftID();
                lblMShift.Text = "Current Shift: " + Common.getShiftLabel(shiftid);
                //lblPShift.Text = "Previous Shift: " + Common.getShiftLabel(PrevShiftid);
                //LoadPastEventsGrid(PrevShiftid);

            GetPreviousLiveLog();

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

            
                MakePageReadOnly(shiftid);
            }

        protected void LoadStatus()
        {
            try
            {
                int shiftid =  Convert.ToInt32(Session["shiftid"]);
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                DataSet ds = _dm.GetDataSet("SelectReportDetails");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if(!string.IsNullOrEmpty(row["ReadPrevLiveLog"].ToString()) &&  row["ReadPrevLiveLog"].ToString().Equals("Yes"))
                            { chkAch.Checked = true; }
                            else { chkAch.Checked = false; }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Acknowledgement.LoadStatus", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }

        protected void GetPreviousLiveLog()
        {
            int _PrevShiftID;
            try
            {
                DataModule _dm = new DataModule();
                DataSet ds = new DataSet();
                ds = _dm.GetDataSet("SelectPreviousShiftDetails");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if(ds.Tables[0].Rows[0]["ShiftID"] == DBNull.Value)
                        {
                            pnlPast.Visible = false;
                            //lblPast.Text = "";
                            lblPast.Visible = true;
                            tblActive.Visible = false;
                        }
                        else
                        {
                            _PrevShiftID = Convert.ToInt32(ds.Tables[0].Rows[0]["ShiftID"]);
                            LoadPastEventsGrid(_PrevShiftID);
                            LoadDocumentDataGrid(_PrevShiftID);
                        }

                        lblPShift.Text = "Previous Shift: " + ds.Tables[0].Rows[0]["Previous Shift"].ToString();
                    }

                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Acknowledgement.GetPreviousLiveLog", "SelectPreviousShiftDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }

        protected int getPrevShiftID()
        {
            DataSet ds;
            int ShiftID = 0;
            try
            {
                DataModule _dm = new DataModule();
                ds = _dm.GetDataSet("SelectPreviousShift");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ShiftID = Convert.ToInt32(ds.Tables[0].Rows[0]["ShiftID"]);
                    }
                }


            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Acknowledgement.getPrevShiftID", "SelectPreviousShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            return ShiftID;
        }
        protected void LoadPastEventsGrid(int PrevShiftid)
        {
            try
            {
                DataModule _dm = new DataModule();
                try
                {
                    _dm.AddParameter("@shiftid", SqlDbType.Int, PrevShiftid);
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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Acknowledgement.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }
                finally
                {
                    _dm = null;

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Acknowledgement.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
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

        protected void LoadDocumentDataGrid(int _PrevShiftID)
        {
            if (Session["shiftid"] != null)
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(_PrevShiftID));
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

        protected void SaveStatus()
        {
            try
            {
                string ReadPrevLiveLog = "";
                if (chkAch.Checked) { ReadPrevLiveLog = "Yes"; } else { ReadPrevLiveLog = ""; }
                    
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("@ReadPrevLiveLog", SqlDbType.VarChar, ReadPrevLiveLog);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(18, "NewTurnover_Acknowledgement.SaveStatus", "AddEditReportInfo", ex);

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SaveStatus();
            Response.Redirect("EquipmentStatus.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveStatus();
        }

        protected void btnNext_Click1(object sender, EventArgs e)
        {
            SaveStatus();
            Response.Redirect("NewTurnover.aspx");
        }
    }
}