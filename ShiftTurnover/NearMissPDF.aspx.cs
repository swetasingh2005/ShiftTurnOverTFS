using IronPdf;
using Microsoft.Office.Interop.Excel;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    int PID = Convert.ToInt32(Request.QueryString["ID"]);
                    LoadForm(PID);
                   
                }
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               

            }//postback
           
        }
       
        //protected void PrintPage()
        //{

        //    IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
        //    var pdfPrintOptions = new PdfPrintOptions()
        //    {
        //        MarginTop = 0,
        //        MarginBottom = 0,
        //        MarginLeft = 0,
        //        MarginRight = 0,
        //        Header = new HtmlHeaderFooter()
        //        {

        //            HtmlFragment = "<center><h1>DTR Architectural Design Review Checklist</h1> </center>",
        //            Height = 30,
        //            Spacing = 0,

        //            DrawDividerLine = false

        //        },
        //        Footer = new HtmlHeaderFooter()
        //        {
        //            Height = 30,
        //            HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
        //            Spacing = 0,
        //            DrawDividerLine = false

        //        },
        //        CssMediaType = PdfPrintOptions.PdfCssMediaType.Print
        //    };


        //    IronPdf.AspxToPdf.RenderThisPageAsPdf(IronPdf.AspxToPdf.FileBehavior.Attachment, "CheckList.pdf", pdfPrintOptions);


            
        //    string attachment = "attachment; filename=report.pdf";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    Response.ContentType = "application/pdf";
        //    StringWriter stw = new StringWriter();
        //    HtmlTextWriter htextw = new HtmlTextWriter(stw);
        //    htextw.AddStyleAttribute("font-size", "8pt");
        //    htextw.AddStyleAttribute("color", "Grey");

        //    PendingOrdersPanel.RenderControl(htextw); //Name of the Panel
        //    Document document = new Document();
        //    document = new Document(PageSize.A4, 5, 5, 15, 5);
        //    FontFactory.GetFont("Tahoma", 50, iTextSharp.text.BaseColor.BLUE);
        //    PdfWriter.GetInstance(document, Response.OutputStream);
        //    document.Open();

        //    StringReader str = new StringReader(stw.ToString());
        //    HTMLWorker htmlworker = new HTMLWorker(document);
        //    htmlworker.Parse(str);

        //    document.Close();
        //    Response.Write(document);
        //}
        protected void LoadForm(int ID)
        {
            DataModule _dm = new DataModule();  
            _dm.AddParameter("@ID", SqlDbType.Int, ID);
            DataSet ds = _dm.GetDataSet("selectnearmissreportbyid");
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                   
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
                        lblAttachment.Text = ds.Tables[0].Rows[0]["Attachments"].ToString();
                        string[] filePaths = Directory.GetFiles(Server.MapPath("~/Documents/"));
                        string fileName = Path.GetFileName(ds.Tables[0].Rows[0]["StoredFileName"].ToString());
                        aspImage.ImageUrl= "~/Documents/" + fileName;
                        aspImage.Visible = true;
                    }
                    else
                    {
                        lblAttachment.Text = "No Attachment";
                        aspImage.Visible = false;
                    } 
                   

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "RptViewer.FillDRC", "selectdrcdetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
           
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
                dataModule.AddParameter("@action", SqlDbType.VarChar, "update" );
                dataModule.AddParameter("@NearMissSecurityID", SqlDbType.Int, ID);
                if (chkResolve.Checked) { dataModule.AddParameter("@Resolved", SqlDbType.Int, 1); }
                else dataModule.AddParameter("@Resolved", SqlDbType.Int, 0);
                dataModule.AddParameter("@ResolvedComments", SqlDbType.VarChar, resolveComments);

                ds = dataModule.GetDataSet("editNearMissReport");
                
            }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
            lblResult.Text = "Near Miss form has been marked resolved. ";
            MakeReadOnly();
        }
        protected void MakeReadOnly( )
        {
            txtResolveComments.ReadOnly = true;
            btnResolve.Visible = false;
            chkResolve.Enabled = false;
        }
            protected void btnDownload_Click(object sender, EventArgs e)
        {

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("NearMissSubmittedReport.aspx");
        }
    }
}