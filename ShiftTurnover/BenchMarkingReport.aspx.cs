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
    public partial class BenchMarkingReport : System.Web.UI.Page
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
            DataTable ds = GetData();

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
        protected void UpdateKPIs(int SystemID, int KPIID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "insert");
                _dm.AddParameter("@Alarm_KPIID", SqlDbType.Int, KPIID);
                _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
                _dm.AddParameter("@Month", SqlDbType.VarChar, 7);
                _dm.AddParameter("@Year", SqlDbType.VarChar, 2022);
      
                _dm.ExecuteCommand("editAlarmKPIValue");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["SID"]))
            {
                int TagID = Convert.ToInt16(Request.QueryString["SID"]);
                //UpdateKPIs(TagID, 1);
                //UpdateKPIs(TagID, 2);
                //UpdateKPIs(TagID, 3);
                //UpdateKPIs(TagID, 4);
                //UpdateKPIs(TagID, 5);
                //UpdateKPIs(TagID, 6);
                //UpdateKPIs(TagID, 7);
                //UpdateKPIs(TagID, 8);

                //UpdateKPIs(TagID, 9);
                //UpdateKPIs(TagID, 10);
                //UpdateKPIs(TagID, 11);
                //UpdateKPIs(TagID, 12);
                //UpdateKPIs(TagID, 15);
                //UpdateKPIs(TagID, 16);
                //UpdateKPIs(TagID, 17);
                //UpdateKPIs(TagID, 18);

                //UpdateKPIs(TagID, 19);
                //UpdateKPIs(TagID, 20);
                //UpdateKPIs(TagID, 21);
                //UpdateKPIs(TagID, 25);
                CalculateKPI(TagID, 13);
            }
        }
        protected DataRow GetCounter(DataTable dt)
        {
             
            DataRow First0Row = dt.Rows.Cast<DataRow>().Where(r => Convert.ToString(r["Val2"]).Equals("0")).FirstOrDefault();
            return First0Row;
        }
        protected void UpdateKPI(int SystemID, int KPIID, float Value)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@action", SqlDbType.VarChar, "insert");
                _dm.AddParameter("@Alarm_KPIID", SqlDbType.Int, KPIID);
                _dm.AddParameter("@SystemID", SqlDbType.VarChar, SystemID);
                _dm.AddParameter("@Month", SqlDbType.VarChar, 7);
                _dm.AddParameter("@Year", SqlDbType.VarChar, 2022);
                _dm.AddParameter("@KPIValue", SqlDbType.VarChar, Value);
                _dm.ExecuteCommand("insetKPIValue");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_WO.SaveStatus", "AddEditReportInfo", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
        }
            protected void CalculateKPI(int SystemID,int KPIID)
        {

            DataSet ds = AlarmKPICalculation.getAlarm10MinPeriod(SystemID);
            int First1ID = 0; int First0ID = 0; int Diffrence = 0; DateTime StartDate= DateTime.Now; DateTime EndDate;
            DataRow[] dDel; DataRow First1Row; int Counter = 1; int Count = ds.Tables[0].Rows.Count; DataRow First0Row;
            TimeSpan ts; int diff = 0;
            while (Count > 0)
            {
                Count = ds.Tables[0].Rows.Count;
                First1Row = ds.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Val2"]) > 0).FirstOrDefault();
                if (First1Row != null)
                {
                    First1ID = Convert.ToInt16(First1Row["Alarm_10MinID"]);
                    StartDate = Convert.ToDateTime(First1Row["Date_Range"]);
                    dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First1ID, "");
                    foreach (DataRow r in dDel)
                        ds.Tables[0].Rows.Remove(r);
                    Count = ds.Tables[0].Rows.Count;
                }
                else
                {
                    Count = 0;
                }
                if (Count > 0)
                {
                    First0Row = GetCounter(ds.Tables[0]);
                    if (First0Row != null)
                    {
                        Counter = Counter + 1;
                        First0ID = Convert.ToInt16(First0Row["Alarm_10MinID"]);
                        EndDate = Convert.ToDateTime(First0Row["Date_Range"]);
                        dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First0ID, "");
                        foreach (DataRow r in dDel)
                            ds.Tables[0].Rows.Remove(r);
                        Count = ds.Tables[0].Rows.Count;
                        if (First0ID - First1ID > Diffrence)
                        {
                            Diffrence = First0ID - First1ID;
                            ts =   EndDate - StartDate;
                            diff = (int)ts.TotalMinutes;
                        }
                    }
                    else
                    {
                        Count = 0;
                    }
                }
            }
            lblCounter.Text =" Total is " +  Counter.ToString() + "  And Diffrence " + Diffrence + "  " + diff;
            UpdateKPI(SystemID, 13, Counter);
            UpdateKPI(SystemID, 26, Diffrence);
            UpdateKPI(SystemID, 14, diff);
        }
        protected void CalculateChattering(int SystemID, int KPIID)
        {

            DataSet ds = AlarmKPICalculation.getAlarm10MinPeriod(SystemID);
            int First1ID = 0; int First0ID = 0; int Diffrence = 0; DateTime StartDate = DateTime.Now; DateTime EndDate;
            DataRow[] dDel; DataRow First1Row; int Counter = 1; int Count = ds.Tables[0].Rows.Count; DataRow First0Row;
            TimeSpan ts; int diff = 0;
            while (Count > 0)
            {
                Count = ds.Tables[0].Rows.Count;
                First1Row = ds.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Val2"]) > 0).FirstOrDefault();
                if (First1Row != null)
                {
                    First1ID = Convert.ToInt16(First1Row["Alarm_10MinID"]);
                    StartDate = Convert.ToDateTime(First1Row["Date_Range"]);
                    dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First1ID, "");
                    foreach (DataRow r in dDel)
                        ds.Tables[0].Rows.Remove(r);
                    Count = ds.Tables[0].Rows.Count;
                }
                else
                {
                    Count = 0;
                }
                if (Count > 0)
                {
                    First0Row = GetCounter(ds.Tables[0]);
                    if (First0Row != null)
                    {
                        Counter = Counter + 1;
                        First0ID = Convert.ToInt16(First0Row["Alarm_10MinID"]);
                        EndDate = Convert.ToDateTime(First0Row["Date_Range"]);
                        dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First0ID, "");
                        foreach (DataRow r in dDel)
                            ds.Tables[0].Rows.Remove(r);
                        Count = ds.Tables[0].Rows.Count;
                        if (First0ID - First1ID > Diffrence)
                        {
                            Diffrence = First0ID - First1ID;
                            ts = EndDate - StartDate;
                            diff = (int)ts.TotalMinutes;
                        }
                    }
                    else
                    {
                        Count = 0;
                    }
                }
            }
            lblCounter.Text = " Total is " + Counter.ToString() + "  And Diffrence " + Diffrence + "  " + diff;
            UpdateKPI(SystemID, 13, Counter);
            UpdateKPI(SystemID, 26, Diffrence);
            UpdateKPI(SystemID, 14, diff);
        }
        protected void CalculateMaxConsecutive(int SystemID, int KPIID)
        {

            DataSet ds = AlarmKPICalculation.getAlarm10MinPeriod(SystemID);
            int First1ID = 0; int First0ID = 0; int Diffrence = 0; DateTime StartDate = DateTime.Now; DateTime EndDate;
            DataRow[] dDel; DataRow First1Row; int Counter = 1; int Count = ds.Tables[0].Rows.Count; DataRow First0Row;
            TimeSpan ts; int diff = 0;
            while (Count > 0)
            {
                Count = ds.Tables[0].Rows.Count;
                First1Row = ds.Tables[0].Rows.Cast<DataRow>().Where(r => Convert.ToInt16(r["Val2"]) > 0).FirstOrDefault();
                if (First1Row != null)
                {
                    First1ID = Convert.ToInt16(First1Row["Alarm_10MinID"]);
                    StartDate = Convert.ToDateTime(First1Row["Date_Range"]);
                    dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First1ID, "");
                    foreach (DataRow r in dDel)
                        ds.Tables[0].Rows.Remove(r);
                    Count = ds.Tables[0].Rows.Count;
                }
                else
                {
                    Count = 0;
                }
                if (Count > 0)
                {
                    First0Row = GetCounter(ds.Tables[0]);
                    if (First0Row != null)
                    {
                        Counter = Counter + 1;
                        First0ID = Convert.ToInt16(First0Row["Alarm_10MinID"]);
                        EndDate = Convert.ToDateTime(First0Row["Date_Range"]);
                        dDel = ds.Tables[0].Select("Alarm_10MinID <  " + First0ID, "");
                        foreach (DataRow r in dDel)
                            ds.Tables[0].Rows.Remove(r);
                        Count = ds.Tables[0].Rows.Count;
                        if (First0ID - First1ID > Diffrence)
                        {
                            Diffrence = First0ID - First1ID;
                            ts = EndDate - StartDate;
                            diff = (int)ts.TotalMinutes;
                        }
                    }
                    else
                    {
                        Count = 0;
                    }
                }
            }
            lblCounter.Text = " Total is " + Counter.ToString() + "  And Diffrence " + Diffrence + "  " + diff;
            UpdateKPI(SystemID, 13, Counter);
            UpdateKPI(SystemID, 14, diff);
        }
    }
}
 