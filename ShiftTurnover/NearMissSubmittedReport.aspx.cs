using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using ShiftTurnover.Components;
using RestSharp;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Web.DynamicData;
using System.Security.Cryptography;
using DotNet.Highcharts;


namespace ShiftTurnover
{
    public partial class NearMissSubmittedReport : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
       

        protected void Page_Load(object sender, EventArgs e)
        {

            litBack.Text = "<a href='Reports.aspx'>⯇ Back to Reports Page</a>";
            if (!Page.IsPostBack)
            {
                lblConfirm.Text = " ";
               
                lblGuide.Text = "~/Documents/NearMissSafetyForm_UserManuel.docx";
                if (Request["update"] == "delete")
                {
                    
                    int PID = 0;
                    if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        PID = Convert.ToInt32(Request.QueryString["ID"]);
                       
                        if (PID > 0)
                        {
                           
                             DeleteForm(PID);
                        }

                    }
                    lblConfirm.Text = "The Safety Observation Form ID ("+ PID +")  has been successfully archived.";
                    lblConfirm.Visible = true;
                }
                if (Request["update"] == "view")
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["aid"]))
                    {
                        string PID = Convert.ToString(Request.QueryString["aid"]);
                        ShowAttachment(PID);
                    }
                       
                }
                 
                LoadSmartGridForFuel();
               

            }//postback
        }
        protected void ResolveIssue(int ID,string comments)
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
                    dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                    dataModule.AddParameter("@action", SqlDbType.VarChar, "update");
                    dataModule.AddParameter("@NearMissSecurityID", SqlDbType.Int, ID);
                    dataModule.AddParameter("@Resolved", SqlDbType.Int, 1);
                    dataModule.AddParameter("@ResolvedComments", SqlDbType.VarChar, comments);
                    ds = dataModule.GetDataSet("editNearMissReport");
                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "SecurityLog.btnSubmit_Click", "InsertLiveLog", ex);
                Response.Redirect("CustomErrorPage.aspx?msg=" + ex.Message);

            }
            lblConfirm.Text = "Near Miss form has been marked resolved. ";
            LoadSmartGridForFuel();
        }
        protected void LoadSmartGridForFuel()
        {
            System.Data.DataTable ds = GetData();
             int _ID = 0;string ViewURL = ""; string EditURL = "";string DeleteURL = ""; 
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();
           
            
            if (ds != null)
            {
                lblCount.Text ="Total: " +  Convert.ToString(ds.Rows.Count);
                if (ds.Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;' visible='false'>Form ID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Incident <br />Incident Date</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Reported <br />By</th>");
                    
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Contact Info</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Location</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Task</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Description</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Additional Info	</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Near Miss? 	</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Resolved?</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Resolved Comment</th>");
             
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Attachments</th>");
                    //strHtml.Append("<th style='text-align:center; visible='false' border:solid 1px black; color:white;  background-color:grey;'>personroleid</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Action</th>");
                    strHtml.Append("</tr></thead><tbody>");



                    try
                    {

                        foreach (DataRow row in ds.Rows)
                        {
                            if (row["Deleted"].ToString().Equals("1"))
                            { strBody.Append("<tr style='text-decoration: line-through; '>"); }
                            else { strBody.Append("<tr>"); }
                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; ' visible='false'>  Form-" + row["NearMissSecurityID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + Convert.ToDateTime(row["IncidentDateTime"]).ToString("dd/MM/yyyy hh:mm tt") + "</td>");
                            
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["CreatedBy"].ToString() + "</td>");
                           
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["ContactInfo"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["Location"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>" + row["Task"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>" + row["Description"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>" + row["AdditionalInfo"].ToString() + "</td>");
                            if (row["NearMiss"].ToString().Equals("1"))
                            {
                                strBody.Append("<td style='text-align:left; border:solid 1px ; black; '> Yes </td>");
                            }
                            else
                            {
                                strBody.Append("<td style='text-align:left; border:solid 1px ; black; '> No </td>");
                            }
                            if (row["Resolved"].ToString().Equals("1"))
                            {
                                if (row["ResolvedComments"] != null && row["ResolvedComments"].ToString().Length > 0)
                                {
                                    strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>  <font color='green'> By " + row["ResolvedbBy"].ToString() + " at " + row["EditedDate"].ToString() + " </font> </td>");
                                    
                                }
                                else
                                {
                                    strBody.Append("<td style='text-align:left; border:solid 1px ;black;   '>  <font color='green'> By " + row["CreatedBy"].ToString() + " at " + row["EditedDate"].ToString() + " </font></td>");
                                }
                                
                            }
                            else { strBody.Append("<td style='text-align:left; border:solid 1px ; black;'> <font color='red'> No </font> </td>"); }

                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["ResolvedComments"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px; black;  '>" + row["Attachments"].ToString() + "</td>");
                     
                            //strBody.Append("<td style='text-align:left; visible='false' border:solid 1px black;  '>" + row["personroleid"].ToString() + "</td>");

                            //try
                            //{ 
                            //    _attID = Int32.Parse(Convert.ToString(row["AttachmentID"])); 
                            //}
                            //catch (FormatException) { }
                            try
                            { _ID = Int32.Parse(Convert.ToString(row["NearMissSecurityID"])); }
                            catch (FormatException) { }

                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>");
                            string uEdit = ResolveUrl("~/Images/Modify.png");
                            string uPreview = ResolveUrl("~/Images/Image.png");
                        
                            string uDel = ResolveUrl("~/Images/archive.jpg");
                            int _attID = row.Field<int?>("AttachmentID") ?? 0;
                            if (_attID > 0)
                            {
                                ViewURL= "NearMissSubmittedReport.aspx?update=view&"   ;
                                strBody.Append("<a target='_self' href=\"" + ViewURL + "aid=" + _attID + "\"><img src=" + uPreview + " alt='edit' width='30' height='30' title='Click here to view the attached Image'  /></a> ");
                            }

                            strBody.AppendLine();
                            if (_ID > 0)
                            {
                                 EditURL = "NearMissPDF.aspx?";
                                 strBody.Append("<a target='_self' href=\"" + EditURL + "ID=" + _ID + "\"><img src=" + uEdit + " alt='edit' width='30' height='30' title='Click here to Resolve this issue'  /></a> ");
                            }
                            DeleteURL = "NearMissSubmittedReport.aspx?update=delete";
                            if (row["Deleted"].ToString().Equals("1"))
                            {   }
                            else {
                                strBody.AppendLine();
                                strBody.Append("<a target='_self' href=\"" + DeleteURL + "&ID=" + _ID + "\"><img src=" + uDel + " alt='edit' OnClick='return confirmBox()' width='30' height='30' title='Click here to archive this form'  /></a> "); }

                            strBody.Append("</td> </tr>");
                        }
                        netTable.InnerHtml = strHtml.ToString() + strBody.ToString() + "</tbody></table>";
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogErrorToDB((int)Session["personroleid"], "ReviewList.LoadSmartGridForList", "selectdrclist", ex);
                        Response.Redirect("CustomErrorPage.aspx");
                    }
                    finally
                    {
                        strBody.Clear();
                        strHtml.Clear();
                    }
                }
            }
            else
            {
                netTable.InnerHtml = "";
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
                DataSet ds2 = dm2.GetDataSet("selectNearmissattachmentslist");

                string strStoredFileName = ds2.Tables[0].Rows[0]["StoredFileName"].ToString();
                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/Documents/"));
                string strFileLocation = di + "\\" + strStoredFileName;
                //byte[] aData;
                WebClient req = new WebClient();
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.Buffer = true;
                //Response.ContentType = ds2.Tables[0].Rows[0]["ContentType"].ToString();
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
        
        protected System.Data.DataTable GetData()
        {
            DataModule _dm = new DataModule();
            DataSet ds = _dm.GetDataSet("SelectNearMissForms");
            return ds.Tables[0];
        }

         

        protected void ExportDataSetToExcel(System.Data.DataTable dt)
        {


            System.Web.HttpResponse response = HttpContext.Current.Response;

            // first let's clean up the response.object
            response.Clear();
            response.Charset = "";

            // set the response mime type for excel   application/octet-stream
            response.ContentType = "application/vnd.ms-excel";
            // response.ContentType = "application/octet-stream";
            // response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            response.AddHeader("content-disposition",
                "attachment;filename=NearMissSecurityForms.xls");

            // create a string writer
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    GridView gd = new GridView();
                    gd.DataSource = dt;
                    gd.DataBind();
                    gd.RenderControl(htw);
                    response.Write(sw.ToString());
                    response.End();

                }
            }
        }



        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable ds = GetData();
            ExportDataSetToExcel(ds);
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            string script = "var windowObject = window.self; windowObject.opener = window.self; windowObject.close();";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Close Window", script, true);
        }
   //     protected void GrPast_ItemDataBound(object sender, DataGridItemEventArgs e)
        //{
            
        //    string _attachmentid = "";
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        LinkButton lnkDoc = (System.Web.UI.WebControls.LinkButton)e.Item.FindControl("lnkDoc");
        //        _attachmentid = Convert.ToString(e.Item.Cells[11].Text);
        //        if (!_attachmentid.Equals("&nbsp;"))
        //        { lnkDoc.Text = "View Document"; }else { lnkDoc.Text = " "; }
        //    }
        //}
        //private void DeleteAttachment(string _AttachmentID)
        //{
        //    try
        //    {
        //        DataModule _dm = new DataModule();
        //        _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
        //        _dm.AddParameter("@attachmentid", SqlDbType.Int, Convert.ToInt32(_AttachmentID));
        //        _dm.GetDataSet("EditNearMissAttachment");
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(_AttachmentID), "NearMissSubmittedReport.DeleteAttachment", "EditNearMissAttachment", ex);
        //        Response.Redirect("CustomErrorPage.aspx");
        //    }

        //}

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
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], Convert.ToInt32(AttachmentID), "NearMissSubmittedReport.DeleteAttachmentFile", "selectstorednearmissfilename", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
        }
        
        private void DeleteForm(int ID)
        {
            int personroleid = 0;
            if (Session["personroleid"] != null)
            {
                personroleid = (int)Session["personroleid"];
            }
            try
            {
              
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                _dm.AddParameter("@NearMissSecurityID", SqlDbType.Int, ID);
                _dm.GetDataSet("deleteNearMissReport");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(personroleid, ID, "NearMissSubmittedReport.DeleteForm" + ex.Message, "deleteNearMissReport", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        protected void btnDownload_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("NearMissPDF.aspx?ID=18");
        }

        
    }
}