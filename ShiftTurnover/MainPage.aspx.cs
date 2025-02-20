using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Configuration;

using ShiftTurnover.Components;
using RestSharp;
using System.Net.Security;
using System.Text.RegularExpressions;

namespace ShiftTurnover
{
    public partial class MainPage : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            LoadShiftLabel();
            if (!Common.isShiftSupervisor(Convert.ToInt32(Session["personroleid"])))
            {
                btnCancel.Visible = false;
            }
            else
            {
                btnCancel.Visible = true;
            }
            txtDateTime.Attributes["min"] = DateTime.Now.AddHours(-12).ToString("yyyy-MM-ddTHH:mm");
            txtDateTime.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["update"] == "success")
                {
                    lblConfirm.Text = "The Live Log has been successfully submitted.";
                    lblConfirm.Visible = true;
                }



                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                PopulateControls();
                LoadPastEventsGrid();
                lblStatus.Text = "";
                LoadDocumentDataGrid();

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
                    MakePageReadOnly(Convert.ToInt16(_ShiftID));
                }//end if
                ////configure buttons for roles
                //if (Session["personrole"].ToString().IndexOf("Submitter") != -1)
                //{//Submitter
                //    btnSubmit.Visible = true;
                //}
                //else
                //{//Reviewer/Admin
                //    btnSubmit.Visible = false;
                //}
            }//postback
            else
            {
                lblConfirm.Visible = false;
            }

        } //page load

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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MasterPage.LoadShiftLabel", "SelectCurrentShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
                dataModule = null;
            }
        }
        private void PopulateControls()
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            //Populate Attachment Type dropdown
            ds = dataModule.GetDataSet("selectattachmenttypelist");
            if (ds != null)
            {
                drpAttachmentType.DataSource = ds;
                drpAttachmentType.DataTextField = "attachmenttype";
                drpAttachmentType.DataValueField = "attachmenttypeid";
                drpAttachmentType.DataBind();
                drpAttachmentType.Items.Insert(0, "-SELECT-");
            }
            ds = null;

        }

        private void MakePageReadOnly(int CurrentShiftID)
        {
            if (Common.isCurrentShiftSubmitted(CurrentShiftID))
            {
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = true;
            }

        }
        protected void LoadLiveLog(string LiveLogID)
        {
            DataModule _dm = new DataModule();
            try
            {
                _dm.AddParameter("@LiveLogID", SqlDbType.BigInt, Convert.ToInt32(LiveLogID));
                DataSet ds = _dm.GetDataSet("SelectLiveLog");
                lblLiveLogID.Text = Convert.ToString(LiveLogID);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            txtEvent.Text = row["Description"].ToString();
                            if (!row["Service Request"].ToString().Equals("N"))
                                {
                                chkSvcReq.Checked = true;
                                }
                        }
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
        protected void LoadPastEventsGrid()
        {
            DataModule _dm = new DataModule();
            try
            {
                int shiftid = Convert.ToInt32(Session["shiftid"]);
                _dm.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                DataSet ds = _dm.GetDataSet("SelectShiftLiveLog_ShiftSup");

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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                _dm = null;

            }
        }

        #region "Past Events Grid Events"
        private void GrPast_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DataSet _dv = (DataSet)GrPast.DataSource;
            DataColumnCollection _dc = _dv.Tables[0].Columns;
            string _ShiftID = "";
            _ShiftID = Session["shiftid"].ToString();
           
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
                ImageButton btnDelete = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDelete");
                Label lblDeleted = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblDeleted");
                lblDeleted.CssClass = "alert";
                btnDelete.Attributes.Add("OnClick", "return confirmBox()");
                string deleted = e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Text.ToString();
                bool CurrShiftSubmitted = Common.isCurrentShiftSubmitted(Convert.ToInt16(_ShiftID));
                bool IsShiftSupervisor = Common.isShiftSupervisor(Convert.ToInt32(Session["personroleid"]));
                e.Item.Cells[_dc.IndexOf(_dc["Edited"]) ].Visible = false;
                //Adding Edit Button For Shift-Supervisors
                ImageButton btnEdit = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnEdit");
                ImageButton btnView = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnView");
                if (!CurrShiftSubmitted && IsShiftSupervisor && !deleted.Equals("1"))
                {
                    btnEdit.Visible = true;
                }
                else
                {
                    btnEdit.Visible = false;
                }

                if (e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Text.ToString() != "1"
                    && (IsShiftSupervisor || 
                    Convert.ToInt32(e.Item.Cells[_dc.IndexOf(_dc["personroleid"]) + 1].Text) == (int)Session["personroleid"] )
                     )//same user or Shift Supervisor
                {
                    btnDelete.Visible = true;
                }

                else
                {
                    btnDelete.Visible = false;

                    if (e.Item.Cells[_dc.IndexOf(_dc["Deleted"]) + 1].Text.ToString() == "1")
                    {
                        e.Item.Cells[_dc.IndexOf(_dc["Description"]) + 1].Style.Value = "text-decoration:line-through;";
                        lblDeleted.Visible = true;
                       
                    }
                }
                if (!CurrShiftSubmitted && IsShiftSupervisor
                    && e.Item.Cells[_dc.IndexOf(_dc["Edited"]) + 1].Text.ToString() != "N")
                {
                    btnView.Visible = true;
                }
                else
                {
                    btnView.Visible = false;
                }
                 
            }
            //Template Column
            e.Item.Cells[0].Width = new Unit(110, UnitType.Pixel);
            e.Item.Cells.AddAt(e.Item.Cells.Count, e.Item.Cells[0]);
            
        }

        private void GrPast_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            Common _cm = new Common();
            //get column index
            Hashtable _table = new Hashtable();
            _table = (Hashtable)ViewState["gridcols"];

            int _livelogid = Convert.ToInt32(_table["Live Log ID"].ToString());
            //int _accesstypeid = Convert.ToInt32(_table["accesstypeid"].ToString());

            switch (e.CommandName)
            {
                case "Delete":
                    {
                        DeleteLiveLog(e.Item.Cells[_livelogid].Text);
                        LoadPastEventsGrid();
                        LoadDocumentDataGrid();
                        break;
                    }
                case "Edit":
                    {
                        LoadLiveLog(e.Item.Cells[_livelogid].Text);
                        break;
                    }
                case "View":
                    {
                        int value = Convert.ToInt32(e.Item.Cells[_livelogid].Text.ToString());
                        LoadHistoryGrid(value);
                        LoadPastEventsGrid();
                        LoadDocumentDataGrid();
                        break;
                    }
            }
        }

        #endregion
        protected void LoadHistoryGrid(int LiveLogID)
        {
            int ShiftID = 0;
            if (Session["shiftid"] != null)
            {
                ShiftID = Convert.ToInt32(Session["shiftid"]);
            }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "newWindow", "PopupReco(" + LiveLogID + ", "+ ShiftID  + ");", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("javascript:PopupReco(" + value + ");", value));
            
        }
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
                        lblActive.Visible = false;
                        pnlActive.Visible = true;
                        GrActive.DataSource = ds;
                        GrActive.DataBind();

                    }
                    else
                    {
                        pnlActive.Visible = false;
                        lblStatus.Text = "";
                        lblActive.Visible = true;
                    }
                }
            }
            else
            {
                pnlActive.Visible = false;
                lblStatus.Text = "";
                lblActive.Visible = true;
            }
        }

        #region "Attachments"

        private void DeleteAttachment(string _AttachmentID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@attachmentid", SqlDbType.Int, Convert.ToInt32(_AttachmentID));
                _dm.GetDataSet("editattachment");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_AttachmentID), "MainPage.DeleteAttachment", "editattachment", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }

        private void DeleteAttachmentFile(string AttachmentID)
        {
            if (AttachmentID != "" && AttachmentID != null)
            {
                try
                {
                    DataModule dm2 = new DataModule();
                    dm2.AddParameter("@attachmentid", SqlDbType.Int, System.Convert.ToInt32(AttachmentID));
                    DataSet ds2 = dm2.GetDataSet("selectstoredfilename");

                    string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();

                    string strFileLocation = ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + '\\' + strStoredFileName;

                    if (System.IO.File.Exists(strFileLocation))
                    {
                        System.IO.File.Delete(strFileLocation);
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(AttachmentID), "MainPage.DeleteAttachmentFile", "selectstoredfilename", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
        }

        private string ValidateData()
        {
            string _ReturnValue = "";

            //get the extension
            string strExtension = System.IO.Path.GetExtension(this.fileUpload.PostedFile.FileName);
            //string[] allowedExtensions = new string[] { ".psd", ".png", ".jpg", ".jpeg", ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".gif", ".wpd", ".rtf", ".tif", ".tiff", ".bmp", ".csv", ".msg",".PNG", ".JPG", ".JPEG", ".PDF", ".DOC", ".DOCX", ".TXT", ".XLS", ".XLSX", ".PPT", ".PPTX", ".GIF", ".WPD", ".RTF", ".TIF", ".TIFF", ".BMP", ".CSV", ".MSG"};
            string[] allowedExtensions = ConfigurationManager.AppSettings["ArrayAllowedExtensions"].ToString().Split(',');
            if (!allowedExtensions.Contains(strExtension))
            {
                _ReturnValue += "<li>The extension " + strExtension + " is not an allowed extension for upload.</li>";
            }
            else
            {
                int FileLen = fileUpload.PostedFile.ContentLength;
                if (FileLen > int.Parse(ConfigurationManager.AppSettings["MaxFileSizeBytes"]))
                {
                    _ReturnValue += "<li>File size exceeds maximum allowable size of " + ConfigurationManager.AppSettings["MaxFileSize"].ToString() + "</li>";
                }
            }
            return _ReturnValue;
        }

        private string UploadAttachment()
        {
            string strAttachmentID = "";
            Guid g = Guid.NewGuid();
            strAttachmentID = System.Convert.ToString(g);

            if ((this.fileUpload.PostedFile != null) && (this.fileUpload.PostedFile.ContentLength > 0))
            {
                //get the file name only
                string strFileName = System.IO.Path.GetFileName(this.fileUpload.PostedFile.FileName);

                //get the extension
                string strExtension = System.IO.Path.GetExtension(this.fileUpload.PostedFile.FileName);

                //name the file using guid
                string strUploadFileName = strAttachmentID + strExtension;

                //save location
                string strSaveLocation = ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + "/" + strUploadFileName;
                //Replace when move to server
                //string strSaveLocation = System.Configuration.ConfigurationManager.AppSettings["AttachmentsFolder"] + strUploadFileName;

                ////get the size
                //int FileLen = fileAttachment.PostedFile.ContentLength;

                ////allocate the buffer
                //byte[] aData = new byte[FileLen];

                ////read uploaded file from the Stream
                //this.fileAttachment.PostedFile.InputStream.Read(aData, 0, FileLen);

                try
                {
                    this.fileUpload.PostedFile.SaveAs(strSaveLocation);
                }
                catch (Exception ex)
                {
                    Label mplblError = (Label)Master.FindControl("lblError");
                    if (mplblError != null)
                    {
                        mplblError.Text += "<li>" + ex.Message + "</li>";
                    }
                }
            }

            ////set values from the form
            //string attachmentName = txtName.Text;

            ////determine if attachment exists already
            LoadDocumentDataGrid();
            LoadPastEventsGrid();
            return strAttachmentID;
        }

        private void AddAttachment(string _AttachmentID, int _AttachmentTypeID)
        {
            if ((this.fileUpload.PostedFile != null) && (this.fileUpload.PostedFile.ContentLength > 0))
            {
                //get the file name
                string strFileName = System.IO.Path.GetFileName(this.fileUpload.PostedFile.FileName);

                //get the extension
                string strExtension = System.IO.Path.GetExtension(this.fileUpload.PostedFile.FileName);

                //name the uploaded file using guid
                string strUploadFileName = _AttachmentID + strExtension;

                //Content Type
                string strContentType = this.fileUpload.PostedFile.ContentType;

                DataModule dm = new DataModule();
                dm.AddParameter("@action", SqlDbType.VarChar, "insert");
                dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                dm.AddParameter("@uploadpersonroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                dm.AddParameter("@attachmenttypeid", SqlDbType.Int, _AttachmentTypeID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);

                if (txtDescription.Text == "")
                    dm.AddParameter("@description", SqlDbType.VarChar, DBNull.Value);
                else
                    dm.AddParameter("@description", SqlDbType.VarChar, txtDescription.Text.ToString());

                DataSet ds = dm.GetDataSet("editattachment");
            }//if posted file exists
        }

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

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strAttachmentID;

            if (fileUpload.HasFile)
            {
                try
                {
                    string strErrorMsg = ValidateData();

                    if (strErrorMsg != "")
                    {//validation failed
                        throw new Exception(strErrorMsg);
                    }
                    else
                    {
                        if (drpAttachmentType.SelectedValue == "-SELECT-")
                        {
                            lblStatus.Text = "Select Attachment Type";
                        }
                        else
                        {
                            int _AttachmentTypeID = Convert.ToInt32(drpAttachmentType.SelectedValue);
                            strAttachmentID = UploadAttachment();
                            AddAttachment(strAttachmentID, _AttachmentTypeID);
                            //string filename = Path.GetFileName(fileUpload.FileName);
                            //fileUpload.SaveAs(Server.MapPath("~/") + filename);
                            lblStatus.Text = "Upload status: File uploaded!";
                            LoadDocumentDataGrid();
                            LoadPastEventsGrid();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Table mpTableError = (Table)Master.FindControl("cph_error").FindControl("tblMasterError");
                    if (mpTableError != null)
                    {
                        mpTableError.Visible = true;
                    }

                    Label mplblError = (Label)Master.FindControl("lblError");
                    if (mplblError != null)
                    {
                        mplblError.Text += ex.Message;
                    }
                    lblStatus.Text = "Upload status: The file could not be uploaded.";
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(Session["shiftid"]), "MainPage.btnUpload_Click", " ", ex);
                }
            }
            LoadDocumentDataGrid();
            LoadPastEventsGrid();
        }

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

                ImageButton btnDelete = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDelete");

                btnDelete.Attributes.Add("OnClick", "return confirmBox2()");

                if (Convert.ToInt32(Session["personroleid"]) != Convert.ToInt32(e.Item.Cells[_dc.IndexOf(_dc["UploadPersonRoleID"]) + 1].Text))
                {
                    btnDelete.Visible = false;
                }
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
                case "Delete":
                    {
                        DeleteAttachmentFile(e.Item.Cells[_attachmentid].Text);
                        DeleteAttachment(e.Item.Cells[_attachmentid].Text);
                        LoadDocumentDataGrid();
                        LoadPastEventsGrid();
                        lblStatus.Text = "Status: File deleted!";
                        break;
                    }
            }
        }

        #endregion

        private void DeleteLiveLog(string _LiveLogID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                _dm.AddParameter("@livelogid", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                _dm.GetDataSet("editlivelog");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "MainPage.DeleteLiveLog", "editlivelog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        protected string ReplaceDescriptionInvalidChar(string desc)
        {
            if (desc.Contains("#")) { return desc.Replace("#", "No. "); }
            return desc;
        }
        protected string CreateCloudServiceRequest()
        {
            string content = "";
            string description = ReplaceDescriptionInvalidChar(txtEvent.Text);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string MaxServer = ConfigurationManager.AppSettings["MaxServer"];
            string Cookie = ConfigurationManager.AppSettings["Cookie"];
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            string reportedby = (string)Session["personname"];
            var client = new RestClient(MaxServer + "/maxrest/oslc/script/CREATESR?reportedby=" + reportedby + "&subject=STO-SR&description=" +
                description + "&siteid=BETHESDA&eo73dle6qir2vub1v4npc963cs3ckiklejv84ikh=eo73dle6qir2vub1v4npc963cs3ckiklejv84ikh&assetnum=AS100181");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", APIKey);
            request.AddHeader("Cookie", Cookie);
            //request.AddHeader("apikey", "eo73dle6qir2vub1v4npc963cs3ckiklejv84ikh");
            // request.AddHeader("Cookie", "JSESSIONID=0000v23MO8CV_FKKbYMp2rzMqCC:1frkl7dot");
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            content = response.Content;
            return content;
        }
        protected IRestResponse CreateServiceRequest()
        {
            //Encryption comn = new Encryption();
            string MaxServer = ConfigurationManager.AppSettings["MaxServer"];
            string Cookie = ConfigurationManager.AppSettings["Cookie"];
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            string reportedby = (string)Session["personname"];


            var client = new RestClient(MaxServer + "/maxrest/oslc/script/CREATESR?reportedby="
                + reportedby + "&subject=STO-SR&description=" + txtEvent.Text + "&siteid=BETHESDA&apikey=" +
                APIKey + "&assetnum=AS100310");
            client.Timeout = -1;
            client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest(Method.POST);
            request.AddHeader("apikey", APIKey);
            // request.AddHeader("Cookie", Cookie);
            // request.AddHeader("apikey", "61ir8aljid765uoj1iaadsq8dft6rkk8lfuatfki");
            // request.AddHeader("Cookie", "JSESSIONID=0000pwvMpdzSsFG4iLt_vNr6pn_:-1");
            IRestResponse response = client.Execute(request);



            return response;

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (lblLiveLogID.Text.Length == 0)
            {
                try
                {
                    DataSet ds;
                    string RestAPIStatus = ""; string ServiceRequestID = "";
                    string EventDescription = txtEvent.Text;
                    if (!Common.CheckForDuplicate(EventDescription, Convert.ToInt32(Session["shiftid"]), (int)Session["personroleid"]))
                    {
                        DataModule dataModule = new DataModule();
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                        if (txtDateTime != null)
                        {
                            dataModule.AddParameter("@createdate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                        }
                        else
                        {
                            dataModule.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now);
                        }
                        if (chkSvcReq.Checked)
                        {
                            dataModule.AddParameter("@servicerequest", SqlDbType.VarChar, "Y");
                            RestAPIStatus = CreateCloudServiceRequest();
                            ServiceRequestID = Regex.Replace(RestAPIStatus, @"(\{|""|\})", "");
                            dataModule.AddParameter("@servicerequestID", SqlDbType.VarChar, ServiceRequestID);
                        }
                        else
                        {
                                dataModule.AddParameter("@servicerequestID", SqlDbType.VarChar, System.DBNull.Value);
                                dataModule.AddParameter("@servicerequest", SqlDbType.VarChar, System.DBNull.Value);
                        }
                        if (txtEvent != null)
                        {
                            EventDescription =  txtEvent.Text;
                            dataModule.AddParameter("@description", SqlDbType.NVarChar, EventDescription);
                        }
                        else
                        {
                            dataModule.AddParameter("@description", SqlDbType.NVarChar, System.DBNull.Value);
                        }
                        ds = dataModule.GetDataSet("InsertLiveLog_1");
                    }

                }
                catch (System.Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.btnSubmit_Click", "InsertLiveLog", ex);
                    Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

                }

                
            }
            else //Update Live Log based on ID
            {
                try

                {
                    DataSet ds;
                    long LiveLogID = Convert.ToInt64(lblLiveLogID.Text);
                    DataModule dataModule = new DataModule();
                    dataModule.AddParameter("@LiveLogID", SqlDbType.BigInt, LiveLogID);
                    dataModule.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                    if (txtDateTime != null)
                    {
                        dataModule.AddParameter("@createdate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                    }
                    else
                    {
                        dataModule.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now);
                    }
                    if (txtEvent != null)
                    {
                        //UnComment when the Cloud is ready to go to Production
                        string EventDesc =  txtEvent.Text;
                        dataModule.AddParameter("@description", SqlDbType.NVarChar, EventDesc);
                    }
                    else
                    {
                        dataModule.AddParameter("@description", SqlDbType.NVarChar, System.DBNull.Value);
                    }
                    ds = dataModule.GetDataSet("UpdateLiveLog");
                    lblLiveLogID.Text = "";
                }
                catch (System.Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.btnSubmit_Click", "InsertLiveLog", ex);
                    Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

                }
            }
            Response.Redirect("MainPage.aspx?update=success");
        }
        
            protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            lblLiveLogID.Text = "";
            chkSvcReq.Checked = false;
            PopulateControls();
            LoadPastEventsGrid();
            lblStatus.Text = "";
            LoadDocumentDataGrid();
        }
    }
}