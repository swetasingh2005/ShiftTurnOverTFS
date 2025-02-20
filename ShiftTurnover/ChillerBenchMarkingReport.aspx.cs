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
    public partial class ChillerBenchMarkingReport : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
       

        protected void Page_Load(object sender, EventArgs e)
        {

            litBack.Text = "<a href='Reports.aspx'>⯇ Back to Reports Page</a>";
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["SID"]))
                {
                    int SystemID = Convert.ToInt16(Request.QueryString["SID"]);
                    if (SystemID == 1) { lblTitle.Text = "Chiller Alarm Benchmarking Analysis and Results"; }
                    else if (SystemID == 2) { lblTitle.Text = "Boiler Alarm Benchmarking Analysis and Results"; }
                    else if (SystemID == 3) { lblTitle.Text = "Water Treatment Alarm Benchmarking Analysis and Results"; }
                    else if (SystemID == 4) { lblTitle.Text = "Auxillary Alarm Benchmarking Analysis and Results"; }
                    LoadAllValue(SystemID);
                    LoadAllTop10(SystemID); 
                    LoadStaleGrid(SystemID);
                    LoadChatteringGrid(SystemID);
                    LoadFleetingGrid(SystemID);
                }
                

            }//postback
        }
        protected void LoadAllTop10(int SID)
        {
            DataTable ds = GridAllTop10(SID);
            if (ds != null && ds.Rows.Count > 0)
            {
                grdTop10.DataSource = ds;
                grdTop10.DataBind();
            }
        }
        protected DataTable GridAllTop10(int SystemID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
            DataSet ds = _dm.GetDataSet("SelectTop10Alarm");
            return ds.Tables[0];
        }
        protected void LoadChatteringGrid(int SID)
        {
            DataTable ds = GetChatteringData(SID);
            if (ds != null && ds.Rows.Count > 0)
            {
                grChattering.DataSource = ds;
                grChattering.DataBind();
                lblChattering.Text = " Total Number of Chattering Alarms:" + ds.Rows.Count;
                lblChattering1.Text =""+ ds.Rows.Count;
            }
            else
            {
                lblChattering.Text = "There are no Chattering Alarms.";
                lblChattering1.Text = "" + 0;
            }
        }
        protected void LoadFleetingGrid(int SID)
        {
            DataTable ds = GetFleetingData(SID);
            if (ds != null && ds.Rows.Count > 0)
            {
                grFleeting.DataSource = ds;
                grFleeting.DataBind();
                lblFleeting.Text = " Total Number of Fleeting Alarms:" + ds.Rows.Count;
                lblFleeting1.Text = " " + ds.Rows.Count;
            }
            else
            {
                lblFleeting.Text = "There are no Fleeting Alarms.";
                lblFleeting1.Text = " " + 0;
            }
        }
        protected DataTable GetFleetingData(int SystemID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
            DataSet ds = _dm.GetDataSet("SelectFleetingAlarms");
            return ds.Tables[0];
        }
        protected DataTable GetChatteringData(int SystemID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
            DataSet ds = _dm.GetDataSet("SelectChatteringAlarms");
            return ds.Tables[0];
        }
        protected DataTable GetStaleData(int SystemID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
            DataSet ds = _dm.GetDataSet("SelectStaleAlarms");
            return ds.Tables[0];
        }
        protected void LoadStaleGrid(int SID)
        {
            DataTable ds = GetStaleData(SID);
            if (ds != null && ds.Rows.Count > 0)
            {
                grStale.DataSource = ds;
                grStale.DataBind();
                lblStaleNumber.Text = " Total Number of Stale Alarms:" + ds.Rows.Count;
                float Avg = ds.Rows.Count / 30;
                lbl18_3.Text = Convert.ToString(Avg);
            }
            else
            {
                lblStaleNumber.Text = "There are no Stale Alarms.";
            }
            }
        protected void LoadAllValue(int SID)
        {
            DataTable ds = GetKPIData(SID,1);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                     
                    lblTotalAlarm1.Text = row["KPIValue"].ToString();
                    lblTotalAlarm2.Text = row["KPIValue"].ToString();
                    lblTotalAlarm3.Text = row["KPIValue"].ToString();
                }

            }
            ds = null;
            ds = GetKPIData(SID, 2);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID1.Text = row["KPIValue"].ToString();
                    
                }
            }
            ds = null;
            ds = GetKPIData(SID, 3);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID3.Text = row["KPIValue"].ToString() + "%";
                    lbl3_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 4);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID4.Text = row["KPIValue"].ToString();
                  //  lbl4_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = GetKPIData(SID, 5);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID5.Text = row["KPIValue"].ToString();
                    //lbl5_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 6);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID6.Text = row["KPIValue"].ToString() + "%";
                    lbl6_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 7);
            string low = "";
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    low = row["KPIValue"].ToString();

                }
            }
            ds = GetKPIData(SID, 8);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    low += " ("  +  row["KPIValue"].ToString() + "%)";

                }
            }
            lblKPIID7.Text = low;
            string medium = "";
            ds = GetKPIData(SID, 9);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    medium = row["KPIValue"].ToString();

                }
            }
            ds = null;
            ds = GetKPIData(SID, 10);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    medium += " (" + row["KPIValue"].ToString() + "%)";

                }
            }
            lblKPIID9.Text = medium;
            ds = null;
            string high = "";
            ds = GetKPIData(SID, 11);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    high = row["KPIValue"].ToString();

                }
            }
            ds = null;
            ds = GetKPIData(SID, 12);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    high += " (" + row["KPIValue"].ToString() + "%)";

                }
            }
            lblPriority_1.Text = "Low/Warning:80% </br>" + "Medium:15%</br>" + "High:5%" ;
            lblPriority_3.Text = "Low/Warning:" + low + " </br>" + "Medium:" + medium + " </br>" + "High:" + high;
            lblKPIID11.Text = high;
            ds = null;
            ds = GetKPIData(SID, 13);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID13.Text = row["KPIValue"].ToString();
                    lbl13_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 14);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID14.Text = row["KPIValue"].ToString();
                    //lbl14_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 15);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID15.Text = row["KPIValue"].ToString();
                   // lbl15_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 18);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID18.Text = row["KPIValue"].ToString();
                   // lbl16_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 19);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID19.Text = row["KPIValue"].ToString();
                    lbl19_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 20);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID20.Text = row["KPIValue"].ToString();
                    //lbl20_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 21);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID21.Text = row["KPIValue"].ToString();
                    lbl21_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 21);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID21.Text = row["KPIValue"].ToString();
                   // lbl22_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 22);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID22.Text = row["KPIValue"].ToString();
                    //lbl22_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 23);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID23.Text = row["KPIValue"].ToString();
                   // lbl23_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 24);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID24.Text = row["KPIValue"].ToString() + "%";
                    lbl24_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 25);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID25.Text = row["KPIValue"].ToString();
                    lbl25_3.Text = row["KPIValue"].ToString();
                }
            }

            ds = null;
            ds = GetKPIData(SID, 26);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID26.Text = row["KPIValue"].ToString();
                    lbl26_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 13);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID27.Text = row["KPIValue"].ToString();
                    //lbl27_3.Text = row["KPIValue"].ToString();
                }
            }
            ds = null;
            ds = GetKPIData(SID, 28);
            if (ds != null && ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    lblKPIID28.Text = row["KPIValue"].ToString();
                    lbl28_3.Text = row["KPIValue"].ToString();
                }
            }
        }
            
          
        protected DataTable GetKPIData(int SystemID,int KPIID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@Alarm_KPIID", SqlDbType.Int, KPIID);
            _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
            _dm.AddParameter("@Month", SqlDbType.VarChar, 7);
            _dm.AddParameter("@Year", SqlDbType.VarChar, 2022);
            DataSet ds = _dm.GetDataSet("getAlarmKPIValue");
            return ds.Tables[0];
        }

        protected void LoadRptNotSubmittedDataGrid()
        {
             
 
          

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
           
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            string script = "var windowObject = window.self; windowObject.opener = window.self; windowObject.close();";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Close Window", script, true);
        }
    }
}