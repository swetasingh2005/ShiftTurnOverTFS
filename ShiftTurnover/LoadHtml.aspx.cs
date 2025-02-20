using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class LoadHtml : System.Web.UI.Page
    {
        protected void LoadReports()
        {
          
            string criticalAlarmReport = Request.QueryString["report"];
            string filePath = ConfigurationManager.AppSettings["CriticalAlarmsReportsPath"];
            string fileNameSearchPattern = null;
            string createdOn = DateTime.Now.ToString("yyyyMMdd");
            DateTime dtCurrent = DateTime.Now;
            DateTime dt630am = Convert.ToDateTime("06:30:00 AM");
            DateTime dt630pm = Convert.ToDateTime("06:30:00 PM");
            int intComp630am = DateTime.Compare(dtCurrent, dt630am); //if now<630am then <0, if now=630am then =0, if now>630am then >0
            int intComp630pm = DateTime.Compare(dtCurrent, dt630pm);


            if (intComp630am > 0 && intComp630pm < 0)
            {
                createdOn += "_05";
            }
            else
            {
                createdOn += "_17";
            }

            switch (criticalAlarmReport)
            {
                case "Auxiliary":
                    fileNameSearchPattern = "NIHCUP-Auxiliary*";
                    break;
                case "Boiler":
                    fileNameSearchPattern = "NIHCUP-BoilerPlant*";
                    break;
                case "Chiller":
                    fileNameSearchPattern = "NIHCUP-ChillerPlant*";
                    break;
                case "WaterTreatment":
                    fileNameSearchPattern = "NIHCUP-WaterTreatment*";
                    break;
                default:
                    lblCriticalAlarmReport.Text = "Missing or invalid report";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(fileNameSearchPattern))
            {
                var fileName = (from n in Directory.GetFiles(filePath, fileNameSearchPattern)
                                where Path.GetFileName(n).Contains(createdOn)
                                select Path.GetFileName(n)).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    filePath = Path.Combine(filePath, fileName);
                }
            }
            else
            {
                lblCriticalAlarmReport.Text = "Missing or invalid report";
                return;
            }
            if (!File.Exists(filePath))
            {
                lblCriticalAlarmReport.Text = "Top View Alarms report does not exist for the current shift.";
                return;
            }
            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();
        }
            protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadReports();
            }
             
            catch (Exception ex)
            {
                string error = "There was a problem loading the report at ";
                string filePath = ConfigurationManager.AppSettings["CriticalAlarmsReportsPath"];
                error += filePath + "  "  + ex.Message;
                ErrorHandler.LogErrorToDB(1000, "LoadHtml.LoadForm", "LoadReports", ex);
                lblCriticalAlarmReport.Text = error;
            }
            finally
            {

            }


        }
    }
    }
