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
    public partial class LoadHTML_Old : System.Web.UI.Page
    {
         
        protected void LoadReports()
        {
            if (Request.QueryString["PID"] != null)
            {
                int CurrentShiftID = Convert.ToInt16(Request.QueryString["PID"]);
                string criticalAlarmReport = Request.QueryString["report"];
                string filePath = ConfigurationManager.AppSettings["CriticalAlarmsReportsPath"];
                string fileNameSearchPattern = null;
                string createdOn = "";
                string CurrentShift = Common.getShiftLabel(CurrentShiftID);
                if (CurrentShift.Length > 0)
                {
                    CurrentShift = CurrentShift.Substring(0, 10);
                    createdOn = Convert.ToDateTime(CurrentShift).ToString("yyyyMMdd");
                }
                if (Common.getShiftLabel(CurrentShiftID).Contains("Day"))
                { createdOn += "_05"; }
                else
                { createdOn += "_17"; }
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
                    lblCriticalAlarmReport.Text = "Top View Alarms report does not exist for the selected shift.";
                    return;
                }
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
            }
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
                error += filePath + "  " + ex.Message;
                ErrorHandler.LogErrorToDB(1000, "LoadHtml.LoadForm", "LoadReports", ex);
                lblCriticalAlarmReport.Text = error;
            }
            finally
            {

            }
            

        }
    }
}