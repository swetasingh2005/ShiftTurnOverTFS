using DotNet.Highcharts;
using Microsoft.Office.Interop.Excel;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace ShiftTurnover
{
    public partial class NearMissSecurity : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

           // txtIncidentDateTime.Attributes["min"] = DateTime.Now.AddDays(-90).ToString("yyyy-MM-ddTHH:mm");

          //  txtIncidentDateTime.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["personrole"] != null)
            {
                if (!Page.IsPostBack)
                {
                    lblGuide.Text = "~/Documents/NearMissSafetyForm_UserManuel.docx";
                    LoadForm();

                }//postback
                else
                {
                    lblResult.Visible = false;
                }

            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        private void LoadForm()
        {
            if (Session["personname"] != null)
            {
                txtReporterName.Text = (string)Session["personname"];
            }
        }
            private void ClearForm()
        {
            txtReporterName.Text = "";
            txtIncidentDateTime.Text = "";
           
            
            txtLocation.Text = "";
            txtTask.Text = "";
            txtDescription.Text = "";
            txtWitnesses.Text = "";
            
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
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
                string reporterName = this.txtReporterName.Text.Trim();
                string incidentDateTimeStr = txtIncidentDateTime.Text.Trim();
                string contactInfo = "";
                if (txtContactInfo.Text.Length > 0) {
                    contactInfo= txtContactInfo.Text.Trim();
                }
                string location = txtLocation.Text.Trim();
                string task = txtTask.Text.Trim();
                string description = txtDescription.Text.Trim();
                string witnesses = txtWitnesses.Text.Trim();
                string NearMiss = rblNearMiss.SelectedValue ;
                
                DateTime incidentDateTime;
                if (!DateTime.TryParse(incidentDateTimeStr, out incidentDateTime))
                {
                    lblResult.Text = "Please enter a valid date/time.";
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    return;
                }
               
                
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                dataModule.AddParameter("@ContactInfo", SqlDbType.VarChar, contactInfo);
                dataModule.AddParameter("@IncidentDateTime", SqlDbType.DateTime, incidentDateTimeStr);
                if (this.rdoResolved.SelectedValue == "")
                {
                    dataModule.AddParameter("@Resolved", SqlDbType.Int, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@Resolved", SqlDbType.Int, rdoResolved.SelectedValue);
                }
                dataModule.AddParameter("@Location", SqlDbType.VarChar, location);
                dataModule.AddParameter("@Task", SqlDbType.VarChar, task);
                dataModule.AddParameter("@Description", SqlDbType.VarChar, description);
                dataModule.AddParameter("@AdditionalInfo", SqlDbType.VarChar, witnesses);
                if (this.rblNearMiss.SelectedValue == "")
                {
                    dataModule.AddParameter("@NearMiss", SqlDbType.VarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@NearMiss", SqlDbType.VarChar, NearMiss);
                }
                ds = dataModule.GetDataSet("InsertNearMissForm");
                int _NearMissSecurityID = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _NearMissSecurityID = Convert.ToInt32(row["NearMissSecurityID"]);
                    }
                }

                
                if (fileUpload.HasFile)
                {

                    UploadDoc(Convert.ToInt32(_NearMissSecurityID));
                }
                
                SendEmailTCUP(_NearMissSecurityID);
               
            lblResult.ForeColor = System.Drawing.Color.Green;
            lblResult.Text =
              "Thank you, " + reporterName +
              ". Your safety observation report has been submitted and email sent to CUP-Safety ( CUP-Safety@mail.nih.gov ) successfully.";
                lblResult.Visible = true;

               pnlConfirmation.Visible = true;
               pnlSubmission.Visible = false;

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(Session["shiftid"]), "MainPage.btnUpload_Click", " ", ex);
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
        protected void SavePDF(int ID)
        {
             
              Response.Redirect("NearMissPDF.aspx?ID=" + ID);
            
        }
       
        private void SendEmailTCUP(int ID)
        {
            string Subject = System.Configuration.ConfigurationManager.AppSettings["NearMissSubject"];
            
            Subject += " " + txtReporterName.Text;
            StringBuilder Body = new StringBuilder();
            string reporterName = this.txtReporterName.Text.Trim();
            string incidentDateTimeStr = txtIncidentDateTime.Text.Trim();
            string contactInfo = "";
            if (txtContactInfo.Text.Length > 0)
            {
                contactInfo = txtContactInfo.Text.Trim();
            }
            string location = txtLocation.Text.Trim();
            string task = txtTask.Text.Trim();
            string description = txtDescription.Text.Trim();
            string witnesses = txtWitnesses.Text.Trim();
            string Resolved = ""; string NearMiss = "";
            if (this.rblNearMiss.SelectedValue == "")
            {
                
            }
            else
            {
                NearMiss = rblNearMiss.SelectedItem.Text;
            }
            if (this.rdoResolved.SelectedValue == "")
            {

            }
            else
            {
                Resolved = rdoResolved.SelectedItem.Text;
            }
            string strFileName = "No Attachement";
            if ((this.fileUpload.PostedFile != null) && (this.fileUpload.PostedFile.ContentLength > 0))
            {
                //get the file name
                 strFileName = System.IO.Path.GetFileName(this.fileUpload.PostedFile.FileName);
            }
            string email = System.Configuration.ConfigurationManager.AppSettings["CUPSafety"];
            Body.Append("DTR CUP Team, " + " <br><br>");
            Body.Append(txtReporterName.Text + " have submitted a new Safety Observation Form ");
          

            Body.Append("<br><br><strong>---- Here are the details----</strong><br><br>");
            Body.AppendLine("<strong> Form ID: </strong>  " + ID );
            Body.AppendLine("<br><strong>1. Reporter Name: </strong>  " + reporterName);
            Body.AppendLine("<br><strong>2. Contact Info:   </strong> " + contactInfo);
            Body.AppendLine("<br><strong>3. Date/Time of Incident:  </strong> " +  incidentDateTimeStr);
            Body.AppendLine("<br><strong>4. Location:   </strong> " + location);
            Body.AppendLine("<br><strong>5. Please describe in detail the potential incident/hazard/concern that was witnessed:s: </strong>  " + description);
            Body.AppendLine("<br><strong>6. Resolved ?: </strong> " + Resolved);
            Body.AppendLine("<br><strong>7. Please explain how the issue was resolved or the recommended actions to resolve: </strong>  " + task);
            Body.AppendLine("<br><strong>8. Additional information, witnesses, etc.: </strong>  " + witnesses);
            Body.AppendLine("<br><strong>9. Near Miss?: </strong>  " + NearMiss);
            Body.AppendLine("<br><br><strong> Attachement:</strong>  " + strFileName);
            Body.AppendLine("<br><strong>Submitted Date: </strong> " + String.Format("{0:MM/dd/yyyy}", System.DateTime.Now.Date) + "<br><br><br>");

            Body.AppendLine("Regards," + "<br>");
            
            Body.Append(" <br>");

            if (email.Length > 0)
            {

                ADGroupHandler._SendEmail(email, Subject, Body.ToString());
            }
        }
       
        
        protected void SendEmail()
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            fileUpload.Dispose();
            lnkUploadedDoc.Text = "";
            ClearForm();
        }

        protected void lnkUploadedDoc_Click(object sender, EventArgs e)
        {

        }

        protected void imgDeleteDoc_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            SavePDF(1);
        }
    }
}