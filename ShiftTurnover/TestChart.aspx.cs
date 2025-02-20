using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Services;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;

 
using iTextSharp.text;
using iTextSharp.text.pdf;

using ShiftTurnover.Components;
using System.Configuration;
using RestSharp;
using System.Diagnostics;

namespace ShiftTurnover
{
    public partial class TestChart : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            DateTime Date = DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute)
                          .AddSeconds(-DateTime.Now.Second);
            txtEndDate.Text = Date.ToString("yyyy-MM-ddTHH:mm");
            txtStartDate.Text= Date.AddHours(-24).ToString("yyyy-MM-ddTHH:mm");
            txtMax.Text = "10000";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
             

        }// end page load
        protected void ExportDataSetToExcel()
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
                "attachment;filename=PIData.xls");

            // create a string writer
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {

                    grdResult.RenderControl(htw);
                    response.Write(sw.ToString());
                    response.End();
                }
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (grdResult.Items.Count > 0)
            { ExportDataSetToExcel(); }
            
        }
        protected void CreateServiceRequest()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start(); int count = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(DateTime));//column 1   
            dt.Columns.Add("Action", typeof(string));//column 1   
            dt.Columns.Add("Type", typeof(DateTime));//column 2
            dt.Columns.Add("Database", typeof(string));//column 3   
            dt.Columns.Add("Path", typeof(string));//column 4
            dt.Columns.Add("Name", typeof(string));//column 4
            dt.Columns.Add("User", typeof(string));//column 4
            dt.DefaultView.Sort = "Date asc";
            dt = dt.DefaultView.ToTable();
            PISystems myPIsystems = new PISystems();
            PISystem myPISystem = myPIsystems["ORF-COGENAF"];
            AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
            AFElement myElement = myDatabase.Elements["CUP"];
            OSIsoft.AF.Diagnostics.AFAuditTrail myAttr =
                new OSIsoft.AF.Diagnostics.AFAuditTrail(myPISystem);
            
            string strSt = Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-ddTHH:mm");
            int MaxCount = Convert.ToInt32(txtMax.Text);
            string strEnd = Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-ddTHH:mm");
            AFTime start1 = new AFTime(strEnd);
            AFTime end1 = new AFTime(strSt);

            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            if (myAttr != null)
            {
                dt  = myAttr.GetFirst(timeRange1, MaxCount);
               
                count=dt.Rows.Count;
                lblTotalList.Text = count.ToString() ;
                if (count > 0)
                {
                   
                    grdResult.DataSource = dt;
                    grdResult.DataBind();
                }
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            lblTotalList.Text = "Time taken to return " + count + " record is " + ts.Minutes + " Min and " + +ts.Seconds + " Sec"; ;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            CreateServiceRequest();
        }
    }

   
}