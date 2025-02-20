using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class SubmissionReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litBack.Text = "<a href='Reports.aspx'>⯇ Back to Reports Page</a>";
            txtEndDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            txtStartDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");

            if (Session["personrole"] != null)
            {
                if (Session["personrole"].ToString() != "Manager")
                {
                    Response.Redirect("Default.aspx");
                }

                if (!Page.IsPostBack)
                {
                    txtStartDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
                    txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    //txtEDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                    //txtSDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                    string currentDate = DateTime.Today.ToShortDateString();
                    CompareValidator2.ValueToCompare = currentDate;
                    CompareValidator3.ValueToCompare = currentDate;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        } //Page_Load

        protected DataSet GetSubmissionReport()
        {
            DataModule dm = new DataModule();
            DataSet ds = new DataSet();
            try
            {
                dm.AddParameter("@startdate", SqlDbType.DateTime, Convert.ToDateTime(txtStartDate.Text));
                dm.AddParameter("@enddate", SqlDbType.DateTime, Convert.ToDateTime(txtEndDate.Text));
                ds = dm.GetDataSet("SelectSubmissionReport");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(Convert.ToInt32(Session["personroleid"]), 0, "SubmissionReport.GetSubmissionReport", "SelectSubmissionReport", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
                return ds;
        }

        protected void LoadSmartGridForList()
        {
            DataSet ds = GetSubmissionReport();

            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();

            lblDateRange.Text = txtStartDate.Text + " to " + txtEndDate.Text;

            if (ds != null && ds.Tables[0] != null)
            {
                lblCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table align='center' id=\'results\' >");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>ShiftID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Shift</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Created By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Submitted By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Submitted Date</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Crew Leader</th>");
                    strHtml.Append("</tr></thead><tbody>");

                    try
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            strBody.Append("<tr>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["ShiftID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["Shift"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["Created By"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["Submitted By"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["Submitted Date"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;'>" + row["Crew Leader"].ToString() + "</td>");
                            strBody.Append("</tr>");
                        }

                        netTable.InnerHtml = strHtml.ToString() + strBody.ToString() + "</tbody></table>";
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogErrorToDB(Convert.ToInt32(Session["personroleid"]), 0, "SubmissionReport.LoadSmartGridForList", "SelectSubmissionReport", ex);
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
            } //if ds!=null          
        }

        protected void ExportDataSetToExcel(DataTable dt)
        {
            HttpResponse response = HttpContext.Current.Response;

            // clean up the response.object
            response.Clear();
            response.Charset = "";

            // set the response mime type for excel application/octet-stream
            response.ContentType = "application/vnd.ms-excel";
            // response.ContentType = "application/octet-stream";
            // response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
            response.AddHeader("content-disposition",
                "attachment;filename=ShiftTurnoverReportsSubmissionResult.xls");

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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            pnlResult.Visible = true;
            LoadSmartGridForList();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            pnlResult.Visible = false;
            txtStartDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEndDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            netTable.InnerHtml = "";
            lblDateRange.Text = "";
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = GetSubmissionReport();
            if (ds != null)
            {
                ExportDataSetToExcel(ds.Tables[0]);
            }
        }

    }
}