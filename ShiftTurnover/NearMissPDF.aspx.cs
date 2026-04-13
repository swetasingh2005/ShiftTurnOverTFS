using IronPdf;
using Microsoft.Office.Interop.Excel;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace ShiftTurnover
{
    public partial class NearMissPDF : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["personroleid"] != null)
            {

                if (!Page.IsPostBack)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        int PID = Convert.ToInt32(Request.QueryString["ID"]);
                        LoadForm(PID);

                    }


                }//postback
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
           
        }
        protected void LoadForm(int ID)
        {
            DataModule _dm = new DataModule();  
            _dm.AddParameter("@ID", SqlDbType.Int, ID);
            DataSet ds = _dm.GetDataSet("selectnearmissreportbyid");
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    lblID.Text =  Convert.ToString( ID) ;
                    if (ds.Tables[0].Rows[0]["CreatedBy"] != null && ds.Tables[0].Rows[0]["CreatedBy"].ToString().Length > 0)
                    { lblReporterName.Text = ds.Tables[0].Rows[0]["CreatedBy"].ToString(); }
                    if (ds.Tables[0].Rows[0]["IncidentDateTime"] != null && ds.Tables[0].Rows[0]["IncidentDateTime"].ToString().Length > 0)
                    { lblIncidentDateTime.Text = ds.Tables[0].Rows[0]["IncidentDateTime"].ToString(); }
                    if (ds.Tables[0].Rows[0]["ContactInfo"] != null && ds.Tables[0].Rows[0]["ContactInfo"].ToString().Length > 0)
                    { lblContactInfo.Text = ds.Tables[0].Rows[0]["ContactInfo"].ToString(); }
                    if (ds.Tables[0].Rows[0]["NearMiss"] != null && ds.Tables[0].Rows[0]["NearMiss"].ToString().Length > 0)
                    {
                        if (ds.Tables[0].Rows[0]["NearMiss"].ToString().Equals("1"))
                        {
                            lblNearMiss.Text ="Yes";
                        }
                        else
                        {
                            lblNearMiss.Text = "No";
                        }
                       
                    }
                    else { lblNearMiss.Text = "No"; }
                    if (ds.Tables[0].Rows[0]["Resolved"] != null && ds.Tables[0].Rows[0]["Resolved"].ToString().Length > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Resolved"].ToString().Equals("1"))
                        {
                            chkResolve.Checked = true;
                        }
                        if( ds.Tables[0].Rows[0]["Resolved"].ToString().Equals("1") )
                        {
                            if (ds.Tables[0].Rows[0]["ResolvedComments"] != null && ds.Tables[0].Rows[0]["ResolvedComments"].ToString().Length > 0)
                            {
                                txtResolveComments.Text = ds.Tables[0].Rows[0]["ResolvedComments"].ToString();
                                txtResolveCommentsBy.Text = ds.Tables[0].Rows[0]["ResolvedbBy"].ToString() + " at " + ds.Tables[0].Rows[0]["EditedDate"].ToString();
                            }
                            else
                            {
                                txtResolveComments.Text = "";
                                txtResolveCommentsBy.Text = ds.Tables[0].Rows[0]["CreatedBy"].ToString() + " at " + ds.Tables[0].Rows[0]["EditedDate"].ToString();
                            }
                            MakeReadOnly();
                        } 
                    }
                    if (ds.Tables[0].Rows[0]["Deleted"] != null && ds.Tables[0].Rows[0]["Deleted"].ToString().Length > 0)
                    {
                        
                        if (ds.Tables[0].Rows[0]["Deleted"].ToString().Equals("1"))
                        {
                            MakeReadOnly();
                        }
                    }
                            if (ds.Tables[0].Rows[0]["Location"] != null && ds.Tables[0].Rows[0]["Location"].ToString().Length > 0)
                    { lblLocation.Text = ds.Tables[0].Rows[0]["Location"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Task"] != null && ds.Tables[0].Rows[0]["Task"].ToString().Length > 0)
                    { lblTask.Text = ds.Tables[0].Rows[0]["Task"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Description"] != null && ds.Tables[0].Rows[0]["Description"].ToString().Length > 0)
                    { lblDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString(); }
                    if (ds.Tables[0].Rows[0]["AdditionalInfo"] != null && ds.Tables[0].Rows[0]["AdditionalInfo"].ToString().Length > 0)
                    { lblWitnesses.Text = ds.Tables[0].Rows[0]["AdditionalInfo"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Attachments"] != null && ds.Tables[0].Rows[0]["Attachments"].ToString().Length > 0)
                    {
                        string[] filePaths = Directory.GetFiles(Server.MapPath("~/Documents/"));
                        string fileName = Path.GetFileName(ds.Tables[0].Rows[0]["StoredFileName"].ToString());
                        string _attachmentid = ds.Tables[0].Rows[0]["AttachmentID"].ToString();
                        BindGrid(ID);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "NearMissPDF.LoadForm", "selectnearmissreportbyid", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
           
        }
        
        private void BindGrid(int ID)
        {
            try
            {
                DataModule _dm = new DataModule();
            _dm.AddParameter("@ID", SqlDbType.Int, ID);
            DataSet ds = _dm.GetDataSet("selectnearmissattachbyid");
          
            gvAttachments.DataSource = ds.Tables[0];
            gvAttachments.DataBind();
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int) Session["personroleid"], 0, "NearMiss.BindGrid", "selectnearmissattachbyid", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
}
        protected void MakeReadOnly( )
        {
            txtResolveComments.ReadOnly = true;
            btnResolve.Visible = false;
            chkResolve.Enabled = false;
            fileUpload.Enabled = false;
        }
        
        private void DeleteAttachment(string _AttachmentID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@attachmentid", SqlDbType.Int, Convert.ToInt32(_AttachmentID));
                _dm.GetDataSet("DeleteNearMissAttachment");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_AttachmentID), "NearMissPDF.DeleteAttachment", "EditNearMissAttachment", ex);
                Response.Redirect("CustomErrorPage.aspx");
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
        protected void UploadDoc(int _NearMissSecurityID)
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

                        string strAttachmentID = UploadAttachment(_NearMissSecurityID);
                        AddAttachment(strAttachmentID, _NearMissSecurityID);

                    }
                }
                catch (Exception ex)
                {

                    lblStatus.Text = "Upload status: The file could not be uploaded.";
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], "NearMissPDF.UploadDoc", " ", ex);
                }
            }

        }
        private string UploadAttachment(int _NearMissSecurityID)
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


                try
                {
                    this.fileUpload.PostedFile.SaveAs(strSaveLocation);
                }
                catch (Exception ex)
                {

                }
            }

            return strAttachmentID;
        }
        private void AddAttachment(string _AttachmentID, int _NearMissSecurityID)
        {
            try { 
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
                dm.AddParameter("@NearMissSecurityID", SqlDbType.Int, _NearMissSecurityID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);


                dm.AddParameter("@description", SqlDbType.VarChar, "Near Miss Security Attachment");

                DataSet ds = dm.GetDataSet("EditNearMissAttachment");
            }//if posted file exists
            }
            catch (Exception ex)
            {

                lblStatus.Text = "Upload status: The file could not be uploaded.";
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "NearMissPDF.AddAttachment", "EditNearMissAttachment", ex);
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
                    DataSet ds2 = dm2.GetDataSet("selectstorednearmissfilename");

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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(AttachmentID), "NearMissPDF.DeleteAttachmentFile", "File Deletion", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
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
                DataSet ds2 = dm2.GetDataSet("selectNearMissattachmentdetails");

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
        protected void gvAttachments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ViewDoc")
            {
                // Open document
                ShowAttachment(Convert.ToString(id));

            }
            else if (e.CommandName == "DeleteRow")
            {
                // Delete document
                DeleteAttachmentFile(Convert.ToString(id));
                DeleteAttachment(Convert.ToString(id));
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    int PID = Convert.ToInt32(Request.QueryString["ID"]);
                    BindGrid(PID);
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                int PID = Convert.ToInt32(Request.QueryString["ID"]);
                UploadDoc(PID);
                BindGrid(PID);
            }
           
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fileUpload.Dispose();
            
        }
        protected void btnResolve_Click(object sender, EventArgs e)
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
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    int ID = Convert.ToInt32(Request.QueryString["ID"]);
                    string resolveComments = txtResolveComments.Text.Trim();
                    dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                    dataModule.AddParameter("@action", SqlDbType.VarChar, "update");
                    dataModule.AddParameter("@NearMissSecurityID", SqlDbType.Int, ID);
                    if (chkResolve.Checked) { dataModule.AddParameter("@Resolved", SqlDbType.Int, 1); }
                    else dataModule.AddParameter("@Resolved", SqlDbType.Int, 0);
                    dataModule.AddParameter("@ResolvedComments", SqlDbType.VarChar, resolveComments);

                    ds = dataModule.GetDataSet("editNearMissReport");

                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NearMissPDF.btnResolve_Click", "editNearMissReport", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
            lblResult.Text = "Near Miss form has been marked resolved. ";
            MakeReadOnly();
        }
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("NearMissSubmittedReport.aspx");
        }
        protected void lnkDoc_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string AttachmentID = btn.CommandArgument;
            ShowAttachment(AttachmentID);
        }
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)(sender);
            string AttachmentID = btn.CommandArgument;
            DeleteAttachment(AttachmentID);
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                int PID = Convert.ToInt32(Request.QueryString["ID"]);
                BindGrid(PID);
            }
        }
    }
}