using ShiftTurnover.Components;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace ShiftTurnover
{
    public partial class WaterTreatmentLog : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {

            txtDateTime.Attributes["min"] = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm");

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


                //if (Session["LiveLogID"] == null)
                //{
                //    //Response.Redirect("");
                //    Session["LiveLogID"] = "0";
                //}
                //else
                //{
                //    string _LiveLogID = "";
                //   _LiveLogID = Session["LiveLogID"].ToString();
                //    ViewState["LiveLogID"] = _LiveLogID;
                //    LoadDocumentDataGrid();
                //}//end if
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


        private void PopulateControls()
        {
            //DataSet ds;
            ////Populate litShift
            //DataModuleChem DataModuleChem = new DataModuleChem();
            //try
            //{
            //ds = DataModuleChem.GetDataSet("SelectCurrentShift");
            //if (ds != null)
            //{
            //    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //    {
            //            Label lblMasterStatus = (Label)Master.FindControl("lblMShift");
            //            lblMasterStatus.Text = "Current Shift:<strong> " +  ds.Tables[0].Rows[0]["Current Shift"].ToString() + "</strong>";
            //        //Save LiveLogID to session
            //        Session["LiveLogID"] = ds.Tables[0].Rows[0]["LiveLogID"].ToString();
            //    }
            //}

            //    //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            //}
            //catch (System.Exception ex)
            //{
            //    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.PopulateControls", "SelectCurrentShift", ex);
            //    Response.Redirect("CustomErrorPage.aspx");
            //}

            DataSet ds;
            DataModuleChem DataModuleChem = new DataModuleChem();
            //Populate Attachment Type dropdown
            ds = DataModuleChem.GetDataSet("selectattachmenttypelist");
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

        protected void LoadPastEventsGrid()
        {
            DataModuleChem _dm = new DataModuleChem();
            try
            {
                GrPast.DataSource = null;
                GrPast.DataBind();
                _dm.AddParameter("@StartDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                _dm.AddParameter("@EndDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text).AddDays(-14));
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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "MainPage.LoadPastEventsGrid", "SelectShiftLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                _dm = null;

            }
        }

        #region "Past Events Grid Events"
        private void DeleteAttachmentForLogEntry(int LiveLogID)
        {
            DataModuleChem _dm = new DataModuleChem();
            _dm.AddParameter("_LiveLogID", SqlDbType.Int, LiveLogID);
            DataSet ds = _dm.GetDataSet("selectattachmentslist");

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Attachment ID"].ToString()))
                        {
                            string AttchmentID = row["Attachment ID"].ToString();
                            DeleteAttachmentFile(AttchmentID);
                            DeleteAttachment(AttchmentID);

                        }
                    }
                }
            }
        }
        private void GrPast_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            Common _cm = new Common();
            //get column index
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                int _livelogid = Convert.ToInt16(e.Item.Cells[1].Text);
                //int _accesstypeid = Convert.ToInt32(_table["accesstypeid"].ToString());

                switch (e.CommandName)
                {
                    case "Delete":
                        {
                            DeleteAttachmentForLogEntry(_livelogid);
                            DeleteLiveLog(e.Item.Cells[1].Text);
                            LoadPastEventsGrid();

                            break;
                        }
                    case "Add":
                        {
                            trDocument.Visible = true;
                            txtDescription.Text = "";
                            lblLiveLogID1.Text = Convert.ToString(_livelogid);
                            lblNoOfDoc.Text = Convert.ToString(_livelogid);
                            LoadDocumentDataGrid(_livelogid);
                            break;
                        }
                }
            }
        }
        #endregion

        protected void LoadDocumentDataGrid(int _livelogid)
        {
            DataModuleChem _dm = new DataModuleChem();
            _dm.AddParameter("_LiveLogID", SqlDbType.Int, _livelogid);
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
                lblNoOfDoc.Text = Convert.ToString(ds.Tables[0].Rows.Count);
            }


        }

        #region "Attachments"

        private void DeleteAttachment(string _AttachmentID)
        {
            try
            {
                DataModuleChem _dm = new DataModuleChem();
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
                    DataModuleChem dm2 = new DataModuleChem();
                    dm2.AddParameter("@attachmentid", SqlDbType.Int, System.Convert.ToInt32(AttachmentID));
                    DataSet ds2 = dm2.GetDataSet("selectstoredfilename");

                    string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();

                    // string strFileLocation = ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + '\\' + strStoredFileName;
                    //  string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();
                    System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Documents/"));
                    string strFileLocation = di + "\\" + strStoredFileName;


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

        private string UploadAttachment(int LiveLogID)
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
                string strSaveLocation = Server.MapPath("~/Documents/ ") + "/" + strUploadFileName;
                // ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + "/" + strUploadFileName;
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
                    strAttachmentID = "error" + ex.Message;
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(Session["personroleid"]), "MainPage.btnUpload_Click", " ", ex);
                }
            }

            ////set values from the form
            //string attachmentName = txtName.Text;

            ////determine if attachment exists already
            LoadDocumentDataGrid(LiveLogID);
            LoadPastEventsGrid();
            return strAttachmentID;
        }

        private void AddAttachment(string _AttachmentID, int LiveLogID)
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
                //string _LiveLogID = "";
                //_LiveLogID = Session["LiveLogID"].ToString();
                string desc = "";
                if (txtDescription.Text.Length > 0)
                {
                    desc = txtDescription.Text;
                }
                DataModuleChem dm = new DataModuleChem();
                dm.AddParameter("@action", SqlDbType.VarChar, "insert");

                dm.AddParameter("@uploadpersonroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                dm.AddParameter("@LiveLogID", SqlDbType.Int, LiveLogID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);
                dm.AddParameter("@description", SqlDbType.VarChar, desc);

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
                DataModuleChem dm2 = new DataModuleChem();
                dm2.AddParameter("@attachmentid", SqlDbType.Int, System.Convert.ToInt32(AttachmentID));
                DataSet ds2 = dm2.GetDataSet("selectattachmentdetails");

                string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();
                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Documents/"));
                string strFileLocation = di + "\\" + strStoredFileName;
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
                Response.TransmitFile(strFileLocation);
                //Response.Flush();
                Response.End();

                //following works from url
                //  string strSaveFileAsPath = "Documents/" + ds2.Tables[0].Rows[0]["StoredFileName"].ToString();
                //  HttpContext.Current.Response.Redirect(strSaveFileAsPath, false); //gss added false

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
                        int _LiveLogID = 0;
                        if (lblLiveLogID1.Text.Length > 0)
                        {
                            _LiveLogID = Convert.ToInt16(lblLiveLogID1.Text);

                        }//end if

                        strAttachmentID = UploadAttachment(_LiveLogID);
                        AddAttachment(strAttachmentID, _LiveLogID);
                        //string filename = Path.GetFileName(fileUpload.FileName);
                        // fileUpload.SaveAs(Server.MapPath("~/Documents/ ") + filename);
                        lblStatus.Text = "Upload status: File uploaded!";
                        LoadDocumentDataGrid(_LiveLogID);
                        LoadPastEventsGrid();
                        txtDescription.Text = "";
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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(Session["LiveLogID"]), "MainPage.btnUpload_Click", " ", ex);
                }
            }
            // LoadDocumentDataGrid(_LiveLogID);
            LoadPastEventsGrid();
        }
        private void GrPast_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ImageButton btnDelete = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDeleteEntry");
                ImageButton btnDoc = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDoc");
                if (e.Item.Cells[3].Text.Equals("1"))
                {
                    btnDelete.Visible = false;
                    btnDoc.Visible = false;
                    //e.Item.Cells[2].Text = "Yes";
                    e.Item.Font.Strikeout = true;

                }
                else
                {
                    //e.Item.Cells[2].Text = "No";
                    e.Item.Font.Strikeout = false;
                    btnDelete.Visible = true;
                    btnDoc.Visible = true;
                }
                btnDelete.Attributes.Add("OnClick", "return confirmBox()");


            }

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
                        int _LiveLogID = 0;
                        if (lblLiveLogID1.Text.Length > 0)
                        {
                            _LiveLogID = Convert.ToInt16(lblLiveLogID1.Text);

                        }
                        LoadDocumentDataGrid(_LiveLogID);
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
                DataModuleChem _dm = new DataModuleChem();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                _dm.AddParameter("@livelogid", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                _dm.GetDataSet("editlivelog");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "LiveLog.DeleteLiveLog", "editlivelog", ex);
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
            var client = new RestClient(MaxServer + "/maxrest/oslc/script/CREATESR?reportedby=" + reportedby + "&subject=WaterTreatment-SR&description=" +
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

            try

            {

                DataSet ds;
                //UnComment when the Cloud is ready to go to Production
                string RestAPIStatus = ""; string EventDescription = "";
                if (chkSvcReq.Checked)
                {
                    RestAPIStatus = CreateCloudServiceRequest();
                    if (!RestAPIStatus.Contains("ticketid"))
                    {

                    }
                    else
                    {
                        EventDescription = Regex.Replace(RestAPIStatus, @"(\{|""|\})", "");
                    }
                }
                DataModuleChem DataModuleChem = new DataModuleChem();
                //int CEAttributeID = 0;
                //if (ddlAttribute.SelectedIndex > 0)
                //{ CEAttributeID = Convert.ToInt16(ddlAttribute.SelectedValue); }
                //DataModuleChem.AddParameter("@CEAttributeID", SqlDbType.Int, CEAttributeID);
                DataModuleChem.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                if (txtDateTime != null)
                {
                    DataModuleChem.AddParameter("@createdate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                }
                else
                {
                    DataModuleChem.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now);
                }

                if (chkSvcReq.Checked)
                {
                    DataModuleChem.AddParameter("@servicerequest", SqlDbType.VarChar, "Y");

                }
                else
                {
                    DataModuleChem.AddParameter("@servicerequest", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtEvent != null)
                {
                    //UnComment when the Cloud is ready to go to Production
                    string EventDesc = EventDescription + "   " + txtEvent.Text;
                    DataModuleChem.AddParameter("@description", SqlDbType.NVarChar, EventDesc);
                }
                else
                {
                    DataModuleChem.AddParameter("@description", SqlDbType.NVarChar, System.DBNull.Value);
                }
                ds = DataModuleChem.GetDataSet("InsertLiveLog");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Session["LiveLogID"] = row["LiveLogID"].ToString();

                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "LiveLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }

            Response.Redirect("WaterTreatmentLog.aspx?update=success");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChemEntryPage.aspx");
        }

        protected void drpAttachmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpAttachmentType.SelectedIndex > 0)
            {
                int TabID = 0;
                TabID = Convert.ToInt16(drpAttachmentType.SelectedValue);
                LoadSectionList(TabID);
            }
        }
        protected void LoadSectionList(int TabID)
        {
            DataSet ds;
            DataModuleChem DataModuleChem = new DataModuleChem();
            DataModuleChem.AddParameter("@CETabID", SqlDbType.Int, TabID);
            //Populate Attachment Type dropdown
            ds = DataModuleChem.GetDataSet("SelectSectionsList");
            if (ds != null)
            {
                ddlSection.DataSource = ds;
                ddlSection.DataTextField = "SectionName";
                ddlSection.DataValueField = "CESectionID";
                ddlSection.DataBind();
                ddlSection.Items.Insert(0, "-SELECT-");
            }
            ds = null;
        }
        protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSection.SelectedIndex > 0)
            {
                int AttrID = 0;
                AttrID = Convert.ToInt16(ddlSection.SelectedValue);
                LoadAttributeList(AttrID);
            }
        }
        protected void LoadAttributeList(int AttrID)
        {
            DataSet ds;
            DataModuleChem DataModuleChem = new DataModuleChem();
            DataModuleChem.AddParameter("@CESectionID", SqlDbType.Int, AttrID);
            //Populate Attachment Type dropdown
            ds = DataModuleChem.GetDataSet("SelectAttributesList");
            if (ds != null)
            {
                ddlAttribute.DataSource = ds;
                ddlAttribute.DataTextField = "AttrName";
                ddlAttribute.DataValueField = "CEAttributeID";
                ddlAttribute.DataBind();
                ddlAttribute.Items.Insert(0, "-SELECT-");
            }
            ds = null;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            trDocument.Visible = false;
        }

        protected void btnLiveLogSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("LiveLogSearch.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            chkSvcReq.Checked = false;
            fileUpload.Dispose();
            txtDescription.Text = "";
            trDocument.Visible = false;
        }
    }
}