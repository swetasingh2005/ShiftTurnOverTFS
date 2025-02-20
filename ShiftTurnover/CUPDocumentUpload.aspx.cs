using ShiftTurnover.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class CUPDocumentUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDocumentDataGrid();
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
            this.GrActive.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.GrActive_ItemCommand);
           // this.GrActive.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrActive_ItemDataBound);
        }
        #endregion

        #region "Document Grid Events"
        private void ShowAttachment(string strStoredFileName, string foldername)
        {

            if (string.IsNullOrWhiteSpace(strStoredFileName))
            {
                //can not display
                return;
            }

            if (strStoredFileName != "" && strStoredFileName != null)
            {
                string strFileLocation = foldername + '\\' + strStoredFileName;

                int ext = strStoredFileName.LastIndexOf(".");
                string contentType = strStoredFileName.Substring(ext);
                //byte[] aData;
                WebClient req = new WebClient();
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strStoredFileName + ""); ;
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
        private void GrActive_ItemCommand(object sender, DataGridCommandEventArgs e)
        {

            //string FolderLocation = ConfigurationManager.AppSettings["AttachmentsCUPFolder"].ToString();
            //Replace when move to server
            string FolderName = e.Item.Cells[3].Text;


            switch (e.CommandName)
            {
                case "View":
                    {
                        if (e.Item.Cells[1].Text == "&nbsp;")
                        {
                            return;
                        }
                        else
                        {
                            ShowAttachment(e.Item.Cells[1].Text, FolderName);
                        }

                        break;
                    }

            }
        }
        #endregion
        private void LoadDocument(int _attachmentid)
        {
            
        }
       
        protected void GetDocuments(object sender, EventArgs e)
        {
            
            LoadDocumentDataGrid();
        }

    

        private void DeleteAttachmentFile(string file)
        {
            if (file != "" && file != null)
            {
                try
                {
                     

                    string strFileLocation = ConfigurationManager.AppSettings["AttachmentsFolder"].ToString() + '\\' + file;

                    if (System.IO.File.Exists(strFileLocation))
                    {
                        System.IO.File.Delete(strFileLocation);
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 1001, "DeleteAttachmentFile.DeleteAttachmentFile", "selectstoredfilename", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
        }
        public void ProcessDirectory(string targetDirectory, ref DataTable dt)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            foreach (string fileName in fileEntries)
            {
                FileInfo fi = new FileInfo(fileName);
                dt.Rows.Add(fi.Name, targetDirectory, targetDirectory.Replace(@"\\ors-fs.ors.nih.gov\PubORF\ORF_DTR\", ""));
            }
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory, ref dt);
            }
        }
        protected void LoadDocumentDataGrid()
        { 
            string path = ConfigurationManager.AppSettings["AttachmentsCUPFolder"].ToString();
            DataTable dt = new DataTable("Mydata");
            dt.Columns.Add("File", typeof(string));
            dt.Columns.Add("Folder", typeof(string));
            dt.Columns.Add("FolderLocation", typeof(string));
            
            ProcessDirectory(path, ref dt);
          
            if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        lblActive.Visible = false;
                        pnlActive.Visible = true;
                        GrActive.DataSource = dt;
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
            return strAttachmentID;
        }

    
    
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
                                        strAttachmentID = UploadAttachment();
                                         
                                        string filename = Path.GetFileName(fileUpload.FileName);
                                        fileUpload.SaveAs(Server.MapPath("~/") + filename);
                                        lblStatus.Text = "Upload status: File uploaded!";
                                        LoadDocumentDataGrid();
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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(Session["reportid"]), "AddDocument.btnUpload_Click", " ", ex);
                }
            }
            LoadDocumentDataGrid();
        }

        protected void lnkDoc_Click(object sender, EventArgs e)
        {
            //if (lnkDoc.CommandArgument != null)
            //{
            //    string AttachmentID = Convert.ToString(lnkDoc.CommandArgument);
            //    ShowAttachment(AttachmentID);
            //}
        }
    }
}