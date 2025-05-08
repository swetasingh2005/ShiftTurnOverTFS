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
    public partial class SecurityLog : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            txtDateTime.Attributes["min"] = DateTime.Now.AddHours(-12).ToString("yyyy-MM-ddTHH:mm");

            txtDateTime.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["personrole"] != null)
            {
                if (!Page.IsPostBack)
                {
                    if (Request["update"] == "success")
                    {

                        lblConfirm.Text = "The Security Log has been successfully submitted.";
                        lblConfirm.Visible = true;
                    }
                    if (Request["update"] == "delete")
                    {

                        lblConfirm.Text = "The Security Log has been successfully deleted.";
                        lblConfirm.Visible = true;
                    }
                    if (Request["update"] == "edit")
                    {

                        lblConfirm.Text = "The Security Log has been successfully edited.";
                        lblConfirm.Visible = true;
                    }
                    txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

                    LoadPastEventsGrid();
                    lblStatus.Text = "";

                }//postback
                else
                {
                    lblConfirm.Visible = false;
                }

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        } //page load

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //

        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.GrPast.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(GrPast_ItemCommand);
        //    this.GrPast.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrPast_ItemDataBound);
        //    //this.GrActive.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrActive_ItemCommand);
        //    //this.GrActive.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrActive_ItemDataBound);
        //}
        #endregion




        protected void LoadPastEventsGrid()
        {
            DataModule _dm = new DataModule();
            try
            {
                //int CEAttributeID = 0;
                //if(drpAttachmentType.SelectedIndex > 0)
                //{
                //    CEAttributeID = Convert.ToInt16(drpAttachmentType.SelectedValue);
                //}
                // _dm.AddParameter("@CEAttributeID", SqlDbType.Int, CEAttributeID);
                _dm.AddParameter("@StartDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                _dm.AddParameter("@EndDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text).AddDays(-14));
                DataSet ds = _dm.GetDataSet("SelectShiftSecurityLog");

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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLogPage.LoadPastEventsGrid", "SelectShiftSecurityLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                _dm = null;

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
                _dm.GetDataSet("EditSecurityAttachment");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_AttachmentID), "SecurityLog.DeleteAttachment", "EditSecurityAttachment", ex);
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
                    DataSet ds2 = dm2.GetDataSet("selectstoredSecurityfilename");

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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(AttachmentID), "SecurityLog.DeleteAttachmentFile", "selectstoredSecurityfilename", ex);
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
                }
            }

            ////set values from the form
            //string attachmentName = txtName.Text;

            ////determine if attachment exists already

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
                DataModule dm = new DataModule();
                dm.AddParameter("@action", SqlDbType.VarChar, "insert");

                dm.AddParameter("@uploadpersonroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                dm.AddParameter("@LiveLogID", SqlDbType.Int, LiveLogID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);


                dm.AddParameter("@description", SqlDbType.VarChar, DBNull.Value);

                DataSet ds = dm.GetDataSet("EditSecurityAttachment");
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
                DataSet ds2 = dm2.GetDataSet("selectSecurityattachmentdetails");

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

        }

        #region "Document Grid Events"

        #endregion
        private void LoadLiveLog(string _LiveLogID)
        {
            try
            {
                lblLiveLogID.Text = _LiveLogID;
                DataSet ds;
                DataModule _dm = new DataModule();
                _dm.AddParameter("@SecurityLogID", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                ds = _dm.GetDataSet("SelectSecurityLog");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        txtEvent.Text = row["Description"].ToString();
                        lblLiveLogID1.Text = "Update Security Log ID: ";
                        if (!row["Service Request"].ToString().Equals("N"))
                        {
                            chkSvcReq.Checked = true;
                        }
                        int _attid = 0;
                        if (row["AttachmentID"] != null)
                        {

                            try
                            {
                                _attid = Int32.Parse(row["AttachmentID"].ToString());

                            }
                            catch (FormatException)
                            {

                            }


                        }
                        if (_attid > 0)
                        {
                            lnkUploadedDoc.Text = row["Attachments"].ToString();
                            lnkUploadedDoc.CommandArgument = Convert.ToString(_attid);
                            imgDeleteDoc.CommandArgument = Convert.ToString(_attid);
                            imgDeleteDoc.Visible = true;
                        }
                        else
                        {
                            lnkUploadedDoc.Text = "";
                            imgDeleteDoc.Visible = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "SecurityLog.LoadLiveLog", "SelectEnggLog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }

        private void DeleteLiveLog(string _LiveLogID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@personroleid", SqlDbType.Int, (int)Session["personroleid"]);
                _dm.AddParameter("@livelogid", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                _dm.GetDataSet("DeleteSecuritylog");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "HGVLog.DeleteLiveLog", "DeleteHGVlog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        protected string ReplaceDescriptionInvalidChar(string desc)
        {
            if (desc.Contains("#")) { return desc.Replace("#", "No. "); }
            return desc;
        }
        protected void EditLiveLog(int _LiveLogID)
        {
            try

            {
                DataSet ds;
                DataModule dataModule = new DataModule();
                int personroleid = 0;
                if (Session["personroleid"] != null)
                {
                    personroleid = (int)Session["personroleid"];
                }
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                string EventDesc = txtEvent.Text;
                dataModule.AddParameter("@description", SqlDbType.NVarChar, EventDesc);

                if (_LiveLogID > 0)
                {
                    dataModule.AddParameter("@SecurityLogID", SqlDbType.Int, _LiveLogID);
                }
                ds = dataModule.GetDataSet("UpdateSecurityLog");
                if (fileUpload.HasFile)
                {
                    LinkButton btn = (LinkButton)(lnkUploadedDoc);
                    if (btn != null)
                    {
                        int _attid = 0;
                        string _attachmentid = btn.CommandArgument;
                        try
                        {
                            _attid = Int32.Parse(_attachmentid);

                        }
                        catch (FormatException)
                        {

                        }
                        if (_attid > 0)
                        {
                            DeleteAttachmentFile(Convert.ToString(_attid));
                            DeleteAttachment(Convert.ToString(_attid));
                        }
                        UploadDoc(Convert.ToInt32(_LiveLogID));
                    }


                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }

            Response.Redirect("SecurityLog.aspx?update=edit");
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
        protected void InsertLiveLog()
        {
            try

            {

                DataSet ds;
                string RestAPIStatus = ""; string ServiceRequestID = "";
                DataModule dataModule = new DataModule();
                int personroleid = 0;
                if (Session["personroleid"] != null)
                {
                    personroleid = (int)Session["personroleid"];
                }
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
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
                   // dataModule.AddParameter("@servicerequestID", SqlDbType.VarChar, ServiceRequestID);
                }
                else
                {
                   // dataModule.AddParameter("@servicerequestID", SqlDbType.VarChar, System.DBNull.Value);
                    dataModule.AddParameter("@servicerequest", SqlDbType.VarChar, System.DBNull.Value);
                }
                string EventDesc = ServiceRequestID + ":  " +  txtEvent.Text ;
                dataModule.AddParameter("@description", SqlDbType.NVarChar, EventDesc);

                ds = dataModule.GetDataSet("InsertSecurityLog");
                int _LivelogID = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _LivelogID = Convert.ToInt32(row["SecurityLogID"]);
                    }
                }
                if (fileUpload.HasFile)
                {

                    UploadDoc(Convert.ToInt32(_LivelogID));
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }

            Response.Redirect("SecurityLog.aspx?update=success");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string mode = lblLiveLogID1.Text;
            int _LiveLogID = 0;
            if (lblLiveLogID.Text.Length > 0)
            {
                _LiveLogID = Convert.ToInt16(lblLiveLogID.Text);
            }
            if (mode.Contains("Update") && _LiveLogID > 0)
            {
                EditLiveLog(_LiveLogID);
            }
            else { InsertLiveLog(); }

        }
        protected void UploadDoc(int _LiveLogID)
        {


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

                        string strAttachmentID = UploadAttachment(_LiveLogID);
                        AddAttachment(strAttachmentID, _LiveLogID);



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

        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            txtEvent.Text = "";
            fileUpload.Dispose();
            lnkUploadedDoc.Text = "";
            imgDeleteDoc.Visible = false;
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fileUpload.Dispose();
        }

        protected void GrPast_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            string _attachmentid = "";
            int personroleid = 0;
            string lbldeleted = "No";
            string Admins = System.Configuration.ConfigurationManager.AppSettings["Admin"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDoc = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkDoc");
                ImageButton btnDelete = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDelete");
                ImageButton btnEdit = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnEdit");
                lbldeleted = Convert.ToString(e.Item.Cells[5].Text);
                personroleid = Convert.ToInt16(e.Item.Cells[9].Text);
                _attachmentid = Convert.ToString(e.Item.Cells[8].Text);
                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("OnClick", "return confirmBox2()");
                }
                if (!_attachmentid.Equals("&nbsp;") && lbldeleted.Equals("No"))
                { lnkDoc.Text = "View Document"; }
                if (lbldeleted.Equals("No") && (personroleid == (int)Session["personroleid"]
                    || Admins.Contains((string)Session["ssoname"])))//same user
                {
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                }
                else
                {
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                }
            }


        }

        protected void GrPast_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            Common _cm = new Common();
            //get column index
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                string _livelogid = Convert.ToString(e.Item.Cells[1].Text);
                string _attachmentid = ""; int _attid = 0;
                _attachmentid = Convert.ToString(e.Item.Cells[7].Text);
                try
                {
                    _attid = Int32.Parse(_attachmentid);

                }
                catch (FormatException)
                {

                }
                switch (e.CommandName)
                {
                    case "View":
                        {

                            ShowAttachment(_attachmentid);

                            break;
                        }
                    case "Delete":
                        {
                            if (_attid > 0)
                            {
                                DeleteAttachmentFile(_attachmentid);
                                DeleteAttachment(_attachmentid);
                            }
                            DeleteLiveLog(_livelogid);
                            Response.Redirect("SecurityLog.aspx?update=delete");

                            break;
                        }
                    case "Edit":
                        {
                            LoadLiveLog(_livelogid);
                            trDoc.Visible = true;
                            //LoadDocumentDataGrid(Convert.ToInt32(_livelogid));
                            break;
                        }
                }
            }
        }

        protected void lnkUploadedDoc_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string AttachmentID = btn.CommandArgument;
            ShowAttachment(AttachmentID);
        }



        protected void imgDeleteDoc_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)(sender);
            string AttachmentID = btn.CommandArgument;
            DeleteAttachmentFile(AttachmentID);
            DeleteAttachment(AttachmentID);
            lnkUploadedDoc.Text = "";
            imgDeleteDoc.Visible = false;
            LoadPastEventsGrid();
        }
    }
}