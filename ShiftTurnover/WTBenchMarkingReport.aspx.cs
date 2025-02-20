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

namespace ShiftTurnover
{
    public partial class WTBenchMarkingReport : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
       

        protected void Page_Load(object sender, EventArgs e)
        {

            litBack.Text = "<a href='Reports.aspx'>⯇ Back to Reports Page</a>";
            if (!Page.IsPostBack)
            {
                
               // LoadSmartGridForList();
               // LoadRptNotSubmittedDataGrid();

            }//postback
        }
        protected void LoadSmartGridForList()
        {
           
            DataTable ds = GetData();
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();
            

            if (ds != null)
            {
                
                if (ds.Rows.Count == 0)
                {
                     
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>ReportID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Shift</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Created <br />By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Created <br /> Date</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Submitted <br />by</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Crew <br /> Leader</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Chiller <br /> Operator</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Boiler <br /> Operator</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Auxiliary <br /> Operator</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Shift <br />Electrician	</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Cogen <br /> Operator</th>");


                    strHtml.Append("</tr></thead><tbody>");


                    try
                    {

                        foreach (DataRow row in ds.Rows)
                        {

                            strBody.Append("<tr>");



                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["ReportID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Shift"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["CreatedBy"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["CreateDate"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Submittedby"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["CrewLeader"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["ChillerOperator"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["BoilerOperator"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["AuxiliaryOperator"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["ShiftElectrician"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["CogenOperator"].ToString() + "</td>");

                            strBody.Append(" </tr>");

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "ReportNotSubmitted.LoadRptNotSubmittedDataGrid", "SelectNotSubmittedShift", ex);
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
                
            }

        }
        protected DataTable GetData()
        {
                     DataModule _dm = new DataModule();
            DataSet ds = _dm.GetDataSet("SelectNotSubmittedShift");
            return ds.Tables[0];
        }

        protected void LoadRptNotSubmittedDataGrid()
        {
            DataTable ds  = GetData();
 
            try
            {
                if (ds != null)
            {
                if (ds.Rows.Count > 0)
                {
                          
                             //   grLOTO.DataSource = ds;
                            //    grLOTO.DataBind();

                            
                         
                    }
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "ReportNotSubmitted.LoadRptNotSubmittedDataGrid", "SelectNotSubmittedShift", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
            }


        }

        protected void ExportDataSetToExcel(DataTable dt)
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
                "attachment;filename=LiveLogResult.xls");

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
    }
}