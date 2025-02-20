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
    public partial class FuelLog : System.Web.UI.Page
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

                    lblConfirm.Text = "The Fuel Log Record has been successfully submitted.";
                    lblConfirm.Visible = true;
                   
                    }
                if (Request["update"] == "delete")
                {
                    
                    lblConfirm.Text = "The Fuel Log Record has been successfully deleted.";
                    lblConfirm.Visible = true;
                }
                    if (Request["update"] == "edit")
                    {
                       
                        lblConfirm.Text = "The Fuel Log Record has been successfully edited.";
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
                _dm.AddParameter("@StartDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text));
                _dm.AddParameter("@EndDate", SqlDbType.DateTime, Convert.ToDateTime(txtDateTime.Text).AddDays(-14));
                DataSet ds = _dm.GetDataSet("SelectShiftFuelLog");

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
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "HVGLogPage.LoadPastEventsGrid", "SelectShiftHVGLog", ex);
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
                _dm.GetDataSet("EditFuelAttachment");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_AttachmentID), "FuelLog.DeleteAttachment", "EditHVGAttachment", ex);
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
                    DataSet ds2 = dm2.GetDataSet("selectstoredFuelfilename");

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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(AttachmentID), "FuelLog.DeleteAttachmentFile", "selectstoredHVGfilename", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
        }

        private string ValidateData(HttpPostedFile hp)
        {
            string _ReturnValue = "";

            //get the extension
            string strExtension = System.IO.Path.GetExtension(hp.FileName);
            //string[] allowedExtensions = new string[] { ".psd", ".png", ".jpg", ".jpeg", ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".gif", ".wpd", ".rtf", ".tif", ".tiff", ".bmp", ".csv", ".msg",".PNG", ".JPG", ".JPEG", ".PDF", ".DOC", ".DOCX", ".TXT", ".XLS", ".XLSX", ".PPT", ".PPTX", ".GIF", ".WPD", ".RTF", ".TIF", ".TIFF", ".BMP", ".CSV", ".MSG"};
            string[] allowedExtensions = ConfigurationManager.AppSettings["ArrayAllowedExtensions"].ToString().Split(',');
            if (!allowedExtensions.Contains(strExtension))
            {
                _ReturnValue += "<li>The extension " + strExtension + " is not an allowed extension for upload.</li>";
            }
            else
            {
                int FileLen = hp.ContentLength;
                if (FileLen > int.Parse(ConfigurationManager.AppSettings["MaxFileSizeBytes"]))
                {
                    _ReturnValue += "<li>File size exceeds maximum allowable size of " + ConfigurationManager.AppSettings["MaxFileSize"].ToString() + "</li>";
                }
            }
            return _ReturnValue;
        }
      
        private string UploadAttachment(int LiveLogID, HttpPostedFile hp)
        {
            string strAttachmentID = "";
            Guid g = Guid.NewGuid();
            strAttachmentID = System.Convert.ToString(g);

            if ((hp != null) && (hp.ContentLength > 0))
            {
                //get the file name only
                string strFileName = System.IO.Path.GetFileName(hp.FileName);

                //get the extension
                string strExtension = System.IO.Path.GetExtension(hp.FileName);

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
                    hp.SaveAs(strSaveLocation);
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
        private void AddTicket(string _AttachmentID, int LiveLogID)
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
                dm.AddParameter("@FuelLogID", SqlDbType.Int, LiveLogID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);
                dm.AddParameter("@attachmentid", SqlDbType.VarChar, DBNull.Value, 1000);
                dm.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now, 1000);
                dm.AddParameter("@description", SqlDbType.VarChar, "Ticket 1");
                try
                {
                    DataSet ds = dm.GetDataSet("EditFuelAttachment");

                }
                catch (Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 100, "FuelLog.AddAttachment", "EditFuelAttachment", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }//if posted file exists
        }
        private void AddTicket1(string _AttachmentID, int LiveLogID)
        {
            if ((this.fileUpload2.PostedFile != null) && (this.fileUpload2.PostedFile.ContentLength > 0))
            {
                //get the file name
                string strFileName = System.IO.Path.GetFileName(this.fileUpload2.PostedFile.FileName);

                //get the extension
                string strExtension = System.IO.Path.GetExtension(this.fileUpload2.PostedFile.FileName);

                //name the uploaded file using guid
                string strUploadFileName = _AttachmentID + strExtension;

                //Content Type
                string strContentType = this.fileUpload2.PostedFile.ContentType;
                //string _LiveLogID = "";
                //_LiveLogID = Session["LiveLogID"].ToString();
                DataModule dm = new DataModule();
                dm.AddParameter("@action", SqlDbType.VarChar, "insert");
                dm.AddParameter("@uploadpersonroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                dm.AddParameter("@FuelLogID", SqlDbType.Int, LiveLogID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);
                dm.AddParameter("@attachmentid", SqlDbType.VarChar, DBNull.Value, 1000);
                dm.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now, 1000);
                dm.AddParameter("@description", SqlDbType.VarChar, "Ticket 2");
                try
                {
                    DataSet ds = dm.GetDataSet("EditFuelAttachment");
                
                }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 100, "FuelLog.AddAttachment", "EditFuelAttachment", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }//if posted file exists
        }
        private void AddBillofLanding(string _AttachmentID, int LiveLogID)
        {
            if ((this.upBillOfLanding.PostedFile != null) && (this.upBillOfLanding.PostedFile.ContentLength > 0))
            {
                //get the file name
                string strFileName = System.IO.Path.GetFileName(this.upBillOfLanding.PostedFile.FileName);

                //get the extension
                string strExtension = System.IO.Path.GetExtension(this.upBillOfLanding.PostedFile.FileName);

                //name the uploaded file using guid
                string strUploadFileName = _AttachmentID + strExtension;

                //Content Type
                string strContentType = this.upBillOfLanding.PostedFile.ContentType;
                //string _LiveLogID = "";
                //_LiveLogID = Session["LiveLogID"].ToString();
                DataModule dm = new DataModule();
                dm.AddParameter("@action", SqlDbType.VarChar, "insert");
                dm.AddParameter("@uploadpersonroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                dm.AddParameter("@FuelLogID", SqlDbType.Int, LiveLogID);
                dm.AddParameter("@originalfilename", SqlDbType.VarChar, strFileName, 1000);
                dm.AddParameter("@storedfilename", SqlDbType.VarChar, strUploadFileName, 1000);
                dm.AddParameter("@contenttype", SqlDbType.VarChar, strContentType, 1000);
                dm.AddParameter("@attachmentid", SqlDbType.VarChar, DBNull.Value, 1000);
                dm.AddParameter("@createdate", SqlDbType.DateTime, DateTime.Now, 1000);
                dm.AddParameter("@description", SqlDbType.VarChar, "Bill Of Landing");
                try
                {
                    DataSet ds = dm.GetDataSet("EditFuelAttachment");

                }
                catch (Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 100, "FuelLog.AddAttachment", "EditFuelAttachment", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

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
                DataSet ds2 = dm2.GetDataSet("selectFuelattachmentdetails");

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
        #region "Document Grid Events"
     
        #endregion
        private void LoadLiveLog(string _LiveLogID)
        {
            try
            {
                lblLiveLogID.Text = _LiveLogID;
                DataSet ds; int _attid = 0; int _attid1 = 0; int _attidBill = 0;
                DataModule _dm = new DataModule();
                _dm.AddParameter("@FuelLogID", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                ds  = _dm.GetDataSet("SelectFuelLog");  
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                       txtGallon.Text= row["Gallons"].ToString();
                       txtBillNumber.Text = row["BillNumber"].ToString();
                       txtDriver.Text = row["Driver"].ToString();
                       txtOrderNumber.Text = row["OrderNumber"].ToString();
                       txtSupplier.Text = row["Supplier"].ToString();
                       lblLiveLogID1.Text = "Update Fuel Log ID: ";
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
                        if (row["AttachmentID1"] != null)
                        {
                            try
                            {
                                _attid1 = Int32.Parse(row["AttachmentID1"].ToString());

                            }
                            catch (FormatException)
                            {

                            }
                        }
                        if (row["AttachmentBillID"] != null)
                        {
                            try
                            {
                                _attidBill = Int32.Parse(row["AttachmentBillID"].ToString());

                            }
                            catch (FormatException)
                            {

                            }
                        }
                        if (_attid > 0)
                        {
                           // lnkUploadedDoc.Text = row["Attachments"].ToString();
                            lnkUploadedDoc.CommandArgument = Convert.ToString(_attid);
                            imgDeleteDoc.CommandArgument = Convert.ToString(_attid);
                            imgDeleteDoc.Visible = true;
                        }
                        else
                        {
                            lnkUploadedDoc.Text = "";
                            imgDeleteDoc.Visible = false;
                        }
                        if (_attid1 > 0)
                        {
                            // lnkUploadedDoc.Text = row["Attachments"].ToString();
                            lnkUploadedDoc1.CommandArgument = Convert.ToString(_attid1);
                            imgDeleteDoc1.CommandArgument = Convert.ToString(_attid1);
                            imgDeleteDoc1.Visible = true;
                        }
                        else
                        {
                            lnkUploadedDoc1.Text = "";
                            imgDeleteDoc1.Visible = false;
                        }
                        if (_attidBill > 0)
                        {
                            //lnkUploadedBillDoc.Text = row["Attachments"].ToString();
                            lnkUploadedBillDoc.CommandArgument = Convert.ToString(_attidBill);
                            imgDeleteBill.CommandArgument = Convert.ToString(_attidBill);
                            imgDeleteBill.Visible = true;
                        }
                        else
                        {
                            lnkUploadedBillDoc.Text = "";
                            imgDeleteBill.Visible = false;
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "FuelLog.LoadLiveLog", "SelectEnggLog", ex);
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
                _dm.AddParameter("@fuellogid", SqlDbType.Int, Convert.ToInt32(_LiveLogID));
                _dm.GetDataSet("DeleteFuellog");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_LiveLogID), "HGVLog.DeleteLiveLog", "DeleteHGVlog", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
     
        protected void EditLiveLog(int _LiveLogID)
        {
            try

            {
                DataSet ds;
                DataModule dataModule = new DataModule();
                int personroleid = 0;
           
                if (_LiveLogID > 0)
                {
                    dataModule.AddParameter("@FuelLogID", SqlDbType.Int, _LiveLogID);
                }
               
                if (Session["personroleid"] != null)
                {
                    personroleid = (int)Session["personroleid"];
                }
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                if (txtGallon != null)
                {
                    dataModule.AddParameter("@Gallons", SqlDbType.Float, Convert.ToDouble(txtGallon.Text));
                }
                else
                {
                    dataModule.AddParameter("@Gallons", SqlDbType.Float, System.DBNull.Value);
                }
                if (txtSupplier != null)
                {
                    dataModule.AddParameter("@Supplier", SqlDbType.VarChar, Convert.ToString(txtSupplier.Text));
                }
                else
                {
                    dataModule.AddParameter("@Supplier", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtDateTime != null)
                {
                    dataModule.AddParameter("@createdate", SqlDbType.VarChar, Convert.ToDateTime(txtDateTime.Text));
                }
                else
                {
                    dataModule.AddParameter("@createdate", SqlDbType.VarChar, DateTime.Now);
                }
                if (txtDriver != null)
                {
                    dataModule.AddParameter("@Driver", SqlDbType.VarChar, Convert.ToString(txtDriver.Text));
                }
                else
                {
                    dataModule.AddParameter("@Driver", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtBillNumber != null)
                {
                    dataModule.AddParameter("@BillNumber", SqlDbType.VarChar, Convert.ToString(txtBillNumber.Text));
                }
                else
                {
                    dataModule.AddParameter("@BillNumber", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtOrderNumber != null)
                {
                    dataModule.AddParameter("@OrderNumber", SqlDbType.VarChar, Convert.ToString(txtOrderNumber.Text));
                }
                else
                {
                    dataModule.AddParameter("@OrderNumber", SqlDbType.VarChar, System.DBNull.Value);
                }

                ds = dataModule.GetDataSet("UpdateFuelLog");
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

                    }
                }

                if (this.fileUpload2.HasFile)
                {
                    LinkButton btn1 = (LinkButton)(lnkUploadedDoc1);
                    if (btn1 != null)
                    {
                        int _attid1 = 0;
                        string _attachmentid1 = btn1.CommandArgument;
                        try
                        {
                            _attid1 = Int32.Parse(_attachmentid1);

                        }
                        catch (FormatException)
                        {

                        }
                        if (_attid1 > 0)
                        {
                            DeleteAttachmentFile(Convert.ToString(_attid1));
                            DeleteAttachment(Convert.ToString(_attid1));
                        }

                    }
                }

                if ( upBillOfLanding.HasFile)
                {
                        LinkButton btn1 = (LinkButton)(lnkUploadedBillDoc);
                        if (btn1 != null)
                        {
                            int _attid1 = 0;
                            string _attachmentid1 = btn1.CommandArgument;
                            try
                            {
                                _attid1 = Int32.Parse(_attachmentid1);

                            }
                            catch (FormatException)
                            {

                            }
                            if (_attid1 > 0)
                            {
                                DeleteAttachmentFile(Convert.ToString(_attid1));
                                DeleteAttachment(Convert.ToString(_attid1));
                            }
                        
                    }
                }
                UploadDoc(Convert.ToInt32(_LiveLogID));
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "FuelLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }

            Response.Redirect("FuelLog.aspx?update=edit");
        }
        protected void InsertLiveLog()
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
                if (txtGallon != null)
                {
                    dataModule.AddParameter("@Gallons", SqlDbType.Float, Convert.ToDouble(txtGallon.Text));
                }
                else
                {
                    dataModule.AddParameter("@Gallons", SqlDbType.Float, System.DBNull.Value);
                }
                if (txtSupplier != null)
                {
                    dataModule.AddParameter("@Supplier", SqlDbType.VarChar, Convert.ToString(txtSupplier.Text));
                }
                else
                {
                    dataModule.AddParameter("@Supplier", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtDateTime != null)
                {
                    dataModule.AddParameter("@createdate", SqlDbType.VarChar, Convert.ToDateTime(txtDateTime.Text));
                }
                else
                {
                    dataModule.AddParameter("@createdate", SqlDbType.VarChar, DateTime.Now);
                }
                if (txtDriver != null)
                {
                    dataModule.AddParameter("@Driver", SqlDbType.VarChar, Convert.ToString(txtDriver.Text));
                }
                else
                {
                    dataModule.AddParameter("@Driver", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtBillNumber != null)
                {
                    dataModule.AddParameter("@BillNumber", SqlDbType.VarChar, Convert.ToString(txtBillNumber.Text));
                }
                else
                {
                    dataModule.AddParameter("@BillNumber", SqlDbType.VarChar, System.DBNull.Value);
                }
                if (txtOrderNumber != null)
                {
                    dataModule.AddParameter("@OrderNumber", SqlDbType.VarChar, Convert.ToString(txtOrderNumber.Text));
                }
                else
                {
                    dataModule.AddParameter("@OrderNumber", SqlDbType.VarChar, System.DBNull.Value);
                }
                ds = dataModule.GetDataSet("InsertFuelLog");
                int _LivelogID = 0;
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        _LivelogID = Convert.ToInt32(row["FuelLogID"]);
                    }
                }
                if (fileUpload.HasFile || upBillOfLanding.HasFile || fileUpload2.HasFile)
                {
                    try
                    {
                        UploadDoc(Convert.ToInt32(_LivelogID));
                    }
                    catch (System.Exception ex)
                    {
                        ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "FuelLog.UploadDoc", "UploadDoc", ex);
                        Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "FuelLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }

            Response.Redirect("FuelLog.aspx?update=success");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string mode = lblLiveLogID1.Text;
            int _LiveLogID = 0;
            if (lblLiveLogID.Text.Length > 0)
            {
                _LiveLogID = Convert.ToInt16(lblLiveLogID.Text);
            }
            if (mode.Contains("Update") && _LiveLogID>0)
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
                    string strErrorMsg = ValidateData(fileUpload.PostedFile);
                    if (strErrorMsg != "")
                    {//validation failed
                        throw new Exception(strErrorMsg);
                    }
                    else
                    {
                         
                        string strAttachmentID = UploadAttachment(_LiveLogID, fileUpload.PostedFile);
                        AddTicket(strAttachmentID, _LiveLogID);
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
            if (fileUpload2.HasFile)
            {
                try
                {
                    string strErrorMsg = ValidateData(fileUpload2.PostedFile);
                    if (strErrorMsg != "")
                    {//validation failed
                        throw new Exception(strErrorMsg);
                    }
                    else
                    {

                        string strAttachmentID = UploadAttachment(_LiveLogID, fileUpload2.PostedFile);
                        AddTicket1(strAttachmentID, _LiveLogID);
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
            if (upBillOfLanding.HasFile)
            {
                try
                {
                    string strErrorMsg = ValidateData(upBillOfLanding.PostedFile);

                    if (strErrorMsg != "")
                    {//validation failed
                        throw new Exception(strErrorMsg);
                    }
                    else
                    {

                        string strAttachmentID = UploadAttachment(_LiveLogID,upBillOfLanding.PostedFile);
                        AddBillofLanding(strAttachmentID, _LiveLogID);
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
            txtOrderNumber.Text = "";
            txtGallon.Text = "";
            txtBillNumber.Text ="";
            txtDriver.Text = "";
            txtOrderNumber.Text = "";
            txtSupplier.Text = "";
            lblLiveLogID1.Text = "Update Fuel Log ID: ";
            fileUpload.Dispose();
            fileUpload2.Dispose();
            upBillOfLanding.Dispose();
            lnkUploadedDoc.Text = "";
            lnkUploadedDoc1.Text = "";
            imgDeleteDoc1.Visible = false;
            imgDeleteDoc.Visible = false;
            lnkUploadedBillDoc.Text = "";
            imgDeleteBill.Visible = false;
        }
 
         

        protected void btnCancel_Click(object sender, EventArgs e)
        {
             fileUpload.Dispose();
        }

        protected void GrPast_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

            string _attachmentid = "";string _attachmentbillid = "";
            int personroleid = 0; string _attachmentid1 = "";
            string lbldeleted = "No";
            string Admins = System.Configuration.ConfigurationManager.AppSettings["Admin"];
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDoc = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkDocTicket");
                LinkButton lnkDoc1 = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkDocTicket1");

                LinkButton lnkDocBillofLanding = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkDocBillofLanding");
                ImageButton btnDelete = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnDelete");
                ImageButton btnEdit = (System.Web.UI.WebControls.ImageButton)e.Item.FindControl("btnEdit");
                lbldeleted=Convert.ToString(e.Item.Cells[9].Text);
                personroleid = Convert.ToInt16(e.Item.Cells[14].Text);
                _attachmentid = Convert.ToString(e.Item.Cells[11].Text);
                _attachmentbillid = Convert.ToString(e.Item.Cells[12].Text);
                _attachmentid1 = Convert.ToString(e.Item.Cells[13].Text);
                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("OnClick", "return confirmBox2()");
                }
                if (!_attachmentid.Equals("&nbsp;") && lbldeleted.Equals("No"))
                { lnkDoc.Text = " Before Ticket ";  }
                if (!_attachmentid1.Equals("&nbsp;") && lbldeleted.Equals("No"))
                { lnkDoc1.Text = " After Ticket "; }
                if (!_attachmentbillid.Equals("&nbsp;") && lbldeleted.Equals("No"))
                { lnkDocBillofLanding.Text = " Bill of Lading"; }

                if (lbldeleted.Equals("No") &&   
                     Admins.Contains((string)Session["ssoname"]))//same user
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
                _attachmentid =Convert.ToString(e.Item.Cells[11].Text);
                string _attachmentid1 = ""; int _attid1 = 0;
                _attachmentid1 = Convert.ToString(e.Item.Cells[13].Text);
                string _attachmentbillid = ""; int _attbillid = 0; 
                _attachmentbillid = Convert.ToString(e.Item.Cells[12].Text);
                try
                {
                    _attid1 = Int32.Parse(_attachmentid1);
                   
                }
                catch (FormatException)
                {
                    
                }
                try
                {
                    _attid = Int32.Parse(_attachmentid);

                }
                catch (FormatException)
                {

                }
                try
                {
                    _attbillid = Int32.Parse(_attachmentbillid);

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
                    case "View1":
                        {

                            ShowAttachment(_attachmentid1);

                            break;
                        }
                    case "ViewBill":
                        {

                            ShowAttachment(_attachmentbillid);

                            break;
                        }
                    case "Delete":
                        {
                            if (_attid > 0)
                            {
                                DeleteAttachmentFile(_attachmentid);
                                DeleteAttachment(_attachmentid);
                            }
                            if (_attid1 > 0)
                            {
                                DeleteAttachmentFile(_attachmentid1);
                                DeleteAttachment(_attachmentid1);
                            }
                            if (_attbillid > 0)
                            {
                                DeleteAttachmentFile(_attachmentbillid);
                                DeleteAttachment(_attachmentbillid);
                            }
                            DeleteLiveLog(_livelogid);
                            Response.Redirect("FuelLog.aspx?update=delete");

                            break;
                        }
                    case "Edit":
                        { 
                            LoadLiveLog(_livelogid);
                            trDoc.Visible = true;
                           // LoadDocumentDataGrid(Convert.ToInt32(_livelogid));
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
        protected void btnCancelBillOfLanding_Click(object sender, EventArgs e)
        {
            upBillOfLanding.Dispose();
        }

        protected void lnkUploadedBillDoc_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string AttachmentID = btn.CommandArgument;
            ShowAttachment(AttachmentID);
        }

        protected void imgDeleteBill_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)(sender);
            string AttachmentID = btn.CommandArgument;
            DeleteAttachmentFile(AttachmentID);
            DeleteAttachment(AttachmentID);
            lnkUploadedBillDoc.Text = "";
            imgDeleteBill.Visible = false;
            LoadPastEventsGrid();
        }

        protected void btn1Cancel_Click(object sender, EventArgs e)
        {
            fileUpload2.Dispose();
        }

        protected void lnkUploadedDoc1_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string AttachmentID = btn.CommandArgument;
            ShowAttachment(AttachmentID);
        }

        protected void imgDeleteDoc1_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)(sender);
            string AttachmentID = btn.CommandArgument;
            DeleteAttachmentFile(AttachmentID);
            DeleteAttachment(AttachmentID);
            lnkUploadedDoc1.Text = "";
            imgDeleteDoc1.Visible = false;
            LoadPastEventsGrid();
        }
    }
}