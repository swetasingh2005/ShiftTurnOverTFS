using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Web.SessionState;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Net;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

namespace ShiftTurnover.Components
{
    public class Common
    {
        public Common() { }
        
        public static int getCurrentShift()
        {
            DataSet ds;
            int CurrentShiftID = 0;
            DataModule dataModule = new DataModule();
            try
            {
                ds = dataModule.GetDataSet("SelectCurrentShift");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        CurrentShiftID = Convert.ToInt16(ds.Tables[0].Rows[0]["ShiftID"]);
                    }

                }

                //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.getShiftLabel", "SelectCurrentShift", ex);
            }
            return CurrentShiftID;
        }
        public static string getShiftLabel(int shiftID)
        {
            string ShiftLabel = "";
            DataSet ds;
            //Populate litShift
            DataModule dataModule = new DataModule();
            try
            {
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftID);
                ds = dataModule.GetDataSet("SelectShiftDateTime");

                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ShiftLabel = ds.Tables[0].Rows[0]["Shift"].ToString();
                    }

                }

                //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.getShiftLabel", "SelectShiftDateTime", ex);
            }
            finally
            {
                ds = null;
                dataModule = null;
            }
            return ShiftLabel;

        }
        public static bool isCurrentShiftSubmitted(int shiftid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            bool Submitted = false;
            try
            {
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                ds = dataModule.GetDataSet("selectcurrentreportstatustypeid");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["currentreportstatustypeid"].ToString().Equals("2")) { Submitted = true; }
                    }

                }

            }
            catch (System.Exception ex)
            {
                 
            }
            finally
            {
                ds = null;
            }
            return Submitted;
        }
        public static string getCurrentShiftSubmittedInfo(int shiftid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            string msg = "Not Submitted";
            try
            {
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                dataModule.AddParameter("@statustypeid", SqlDbType.Int, 2);
                ds = dataModule.GetDataSet("selectcurrentreportstatus");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                         
                            msg = ds.Tables[0].Rows[0]["Updated By"].ToString() + " at " + ds.Tables[0].Rows[0]["Action Date"].ToString();
                        
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                ds = null;
            }
            return msg;
        }
        public static string getCurrentShiftCreatedInfo(int shiftid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            string msg = "";
            try
            {
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                dataModule.AddParameter("@statustypeid", SqlDbType.Int, 1);
                ds = dataModule.GetDataSet("selectcurrentreportstatus");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        
                            msg =  ds.Tables[0].Rows[0]["Updated By"].ToString() + " at " + ds.Tables[0].Rows[0]["Action Date"].ToString();
                        
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                ds = null;
            }
            return msg;
        }
        public static string getCurrentShiftSubInfo(int shiftid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            string   Submitted = " is Not Submitted.";
            try
            {
                dataModule.AddParameter("@statustypeid", SqlDbType.Int, 2);
                dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                ds = dataModule.GetDataSet("selectcurrentreportstatus");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["currentreportstatustypeid"].ToString().Equals("2")) {
                            Submitted = " was Submitted By " +  ds.Tables[0].Rows[0]["Updated By"].ToString() + " On " + ds.Tables[0].Rows[0]["Action Date"].ToString();
                        }
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
            finally
            {
                ds = null;
            }
            return Submitted;
        }
        public static bool CheckForDuplicate(string description, int shiftID,int personroleid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            bool CheckForDuplicate = false;int qty = 0;
            try
            {
                dataModule.AddParameter("@description", SqlDbType.VarChar, description);
                dataModule.AddParameter("@shiftID", SqlDbType.Int, shiftID);
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                ds = dataModule.GetDataSet("CheckForDuplicateRow");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        qty = Convert.ToInt16(ds.Tables[0].Rows[0]["qty"]);
                        if (qty> 0) { CheckForDuplicate = true; }
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.CheckForDuplicate", "CheckForDuplicate", ex);
                return CheckForDuplicate;
            }
            finally
            {
                ds = null;
            }
            return CheckForDuplicate;
        }
        public static bool isCrewLeader(int personroleid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            bool CrewLeader = false;
            try
            {
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                ds = dataModule.GetDataSet("CheckIsCrewLeader");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["IsCrewLeader"].ToString().Equals("Y")) { CrewLeader = true; }
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.isCrewLeader", "CheckIsCrewLeader", ex);
                return CrewLeader;
            }
            finally
            {
                ds = null;
            }
            return CrewLeader;
        }
        public static bool isShiftSupervisor(int personroleid)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            bool CrewLeader = false;
            try
            {
                dataModule.AddParameter("@personroleid", SqlDbType.Int, personroleid);
                ds = dataModule.GetDataSet("CheckIsShiftSupervisor");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["IsShiftSupervisor"].ToString().Equals("Y")) { CrewLeader = true; }
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.isCrewLeader", "CheckIsCrewLeader", ex);
                return CrewLeader;
            }
            finally
            {
                ds = null;
            }
            return CrewLeader;
        }
        public int GetPersonRoleID(int _reportid)
        {
            int _personroleID = 0;
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSet("selectreportdetails");

                if (ds != null)
                {
                    _personroleID = (int)ds.Tables[0].Rows[0]["personroleid"];
                }

                return _personroleID;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetPersonRoleID", "selectreportdetails", ex);
                return _personroleID;
            }

        }

        //not sure if this works with function inline
        public int GetCurrentReportStatusTypeID(int _reportid)
        {
            int _currentreportstatustypeid = 0;
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSetInLineQuery("select dbo.getcurrentreportstatustypeid(_reportid)");

                if (ds != null)
                {
                    _currentreportstatustypeid = (int)ds.Tables[0].Rows[0][0];
                }

                return _currentreportstatustypeid;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetCurrentReportStatusTypeID", "dbo.getcurrentreportstatustypeid", ex);
                return _currentreportstatustypeid;
            }

        }

        public DateTime GetCoverageFromDate(int _reportid)
        {
            DateTime _coveragefromdate = DateTime.Now.AddYears(-1000);
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSet("selectreportdetails");

                if (ds != null)
                {
                    //string _facilityname = ds.Tables[0].Rows[0]["Facility Name"].ToString();
                    _coveragefromdate = DateTime.Parse(ds.Tables[0].Rows[0]["Coverage From Date"].ToString());
                    //DateTime _coveragedate = DateTime.Parse(_coveragefromdate).ToString("yyyyMMdd");
                    //txtPIfile.Text = _coveragefromdate.Year.ToString() + _coveragefromdate.Month.ToString().PadLeft(2, '0') + _coveragefromdate.Day.ToString().PadLeft(2, '0') + "_" + _facilityname + "_PI.csv";
                }

                    return _coveragefromdate;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetCoverageFromDate", "selectreportdetails", ex);
                return _coveragefromdate;
            }
            
        }

        public DateTime GetCoverageToDate(int _reportid)
        {
            DateTime _coveragetodate = DateTime.Now.AddYears(-1000);
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSet("selectreportdetails");

                if (ds != null)
                {
                    _coveragetodate = DateTime.Parse(ds.Tables[0].Rows[0]["Coverage To Date"].ToString());
               }
                    return _coveragetodate;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetCoverageToDate", "selectreportdetails", ex);
                return _coveragetodate;
            }

        }

        public String GetFacilityName(int _reportid)
        {
            String _facilityname = "";
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSet("selectreportdetails");

                if (ds != null)
                {
                    _facilityname = ds.Tables[0].Rows[0]["Facility Name"].ToString();
                }

                    return _facilityname;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetFacilityName", "selectreportdetails", ex);
                return _facilityname;
            }

        }

        public int GetFacilityID(int _reportid)
        {
            int _facilityID = 0;
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@reportid", SqlDbType.Int, _reportid);

                DataSet ds = _dm.GetDataSet("selectreportdetails");

                if (ds != null)
                {
                    _facilityID = (int)ds.Tables[0].Rows[0]["facilityid"];
                }

                return _facilityID;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetFacilityID", "selectreportdetails", ex);
                return _facilityID;
            }

        }

        public String GetLocation(int _deviceid)
        {
            String _location = "";
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@deviceid", SqlDbType.Int, _deviceid);

                DataSet ds = _dm.GetDataSet("selectlocation");

                if (ds != null)
                {
                    _location = ds.Tables[0].Rows[0]["location"].ToString();
                }

                return _location;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, "Common.GetLocation", "selectlocation", ex);
                return _location;
            }

        }

        public bool IsPreviousFinding(int cur_personroleid, int cur_reportid, int _incidentid)
        {//return ture if incident is not in current reportid.
            try
            {
                DataSet ds;
                DataModule _dm = new DataModule();
                bool IsPreviousFinding;

                _dm.AddParameter("@incidentid", SqlDbType.Int, _incidentid);

                ds = _dm.GetDataSet("selectreportfromincident");

                if (ds != null && ds.Tables[0].Rows.Count > 0 )
                {
                    if ( cur_reportid == (int)ds.Tables[0].Rows[0]["reportid"])
                        IsPreviousFinding = false;
                    else
                        IsPreviousFinding = true;
                }
                else
                {
                    IsPreviousFinding = true;
                }

                return IsPreviousFinding;
            }
            catch (Exception ex)
            {
                //log error
                ErrorHandler.LogErrorToDB(cur_personroleid, cur_reportid, "Common.IsPreviousFinding", "selectreportfromincident", ex);
                return true;
            }
        }

        public string GetDefaultPageByRole(int _Personroleid)
        {
            try
            {
                DataModule dataModule = new DataModule();
                dataModule.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);

                DataSet ds = dataModule.GetDataSet("selectdefaultpage");

                return ds.Tables[0].Rows[0]["aspxpagenname"].ToString();
            }
            catch (System.Exception ex)
            {
                //log error
                ErrorHandler.LogErrorToDB(_Personroleid, 0, "Common.GetDefaultPage", "selectdefaultpage", ex);

                return "";
            }
        } //end GetDefaultPageByRole

        public string GetNavBar(int _ElementID, int _Personroleid, int _ReportID, int _Level)
        {
            string strPageName = "";
            string strImageFileName = "";
            string strImageAltTag = "";
            string strImageHeight = "";
            string strImageWidth = "";
            string strNavBarTxt = "";

            DataModule dmElement = new DataModule();
            DataSet dsElement = new DataSet();
            dmElement.AddParameter("@elementid", SqlDbType.Int, _ElementID);
            dmElement.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);
            if (_Level == 0)
            {
                dmElement.AddParameter("@level", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@level", SqlDbType.Int, _Level);
            }
            if (_ReportID == 0)
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, _ReportID);
            }

            try
            {
                dsElement = dmElement.GetDataSet("selectelementstructureall");
            }
            catch (System.Exception e)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetNavBar", "selectelementstructureall", e);
                return "";
            }

            if (dsElement.Tables[0] != null && dsElement.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow oRow in dsElement.Tables[0].Rows)
                {
                    strPageName = oRow["aspxpagename"].ToString();
                    strImageFileName = oRow["imagefilename"].ToString();
                    strImageAltTag = oRow["imagealttag"].ToString();
                    strImageHeight = oRow["imageheight"].ToString();
                    strImageWidth = oRow["imagewidth"].ToString();

                    strNavBarTxt += "<a href='" + strPageName + "'>" +
                                    "<img src='" + strImageFileName + "'" +
                                    " alt='" + strImageAltTag + "'" +
                                    " height='" + strImageHeight + "'" +
                                    " width='" + strImageWidth + "'></a>";
                }
                return strNavBarTxt;
            }
            else
            {
                return "";
            }

        } //end GetNavBar

        public string GetSubNavBar(int _ElementID, int _Personroleid, int _ReportID, int _NIHuid)
        {
            string strPageName = "";
            string strImageFileName = "";
            string strImageAltTag = "";
            string strImageHeight = "";
            string strImageWidth = "";
            string strDisplayLabel = "";
            string selectiontypeid = "";
            string Status = "";
            string strSubNavBarTxt = "";

            DataModule dmElement = new DataModule();
            DataSet dsElement = new DataSet();
            dmElement.AddParameter("@elementid", SqlDbType.Int, _ElementID);
            dmElement.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);
            dmElement.AddParameter("@level", SqlDbType.Int, 2);

            if (_ReportID == 0)
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, _ReportID);
            }

            if (_NIHuid == 0)
            {
                dmElement.AddParameter("@nihuid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@nihuid", SqlDbType.Int, _NIHuid);
            }

            try
            {
                dsElement = dmElement.GetDataSet("selectelementstructureall");
            }
            catch (System.Exception e)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetSubNavBar", "selectelementstructureall", e);
                return "";
            }

            if (dsElement.Tables[0] != null && dsElement.Tables[0].Rows.Count > 0)
            {
                strSubNavBarTxt = "<ul>\n";
                foreach (DataRow oRow in dsElement.Tables[0].Rows)
                {
                    Status = "";
                    strPageName = oRow["aspxpagename"].ToString();
                    strImageFileName = oRow["imagefilename"].ToString();
                    strImageAltTag = oRow["imagealttag"].ToString();
                    strImageHeight = oRow["imageheight"].ToString();
                    strImageWidth = oRow["imagewidth"].ToString();
                    strDisplayLabel = oRow["displaylabel"].ToString();
                    selectiontypeid = oRow["selectiontypeid"].ToString();

                    switch (selectiontypeid)
                    {
                        case "1":
                            Status = "current";
                            break;
                        case "2":
                            break;
                    }

                    if (Status != "")
                        strSubNavBarTxt += "\t<li class='current'><a href='" + strPageName + "'>" + strDisplayLabel + "</a></li>\n";
                    else
                    {
                        strSubNavBarTxt += "\t<li><a href='" + strPageName + "'>" + strDisplayLabel + "</a></li>\n";
                    }

                }
                strSubNavBarTxt += "</ul>\n";
                return strSubNavBarTxt;
            }
            else
            {
                return "";
            }

        } //end GetSubNavBar

        public string GetWizardBar(int _ElementID, int _Personroleid, int _ReportID, string _PersonRole)
        {
            string strPageName = "";
            string strImageFileName = "";
            string strImageAltTag = "";
            string strImageHeight = "";
            string strImageWidth = "";
            string strWizardTxt = "";

            DataModule dmElement = new DataModule();
            DataSet dsElement = new DataSet();
            dmElement.AddParameter("@elementid", SqlDbType.Int, _ElementID);
            dmElement.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);
            dmElement.AddParameter("@level", SqlDbType.Int, 3);

            if (_ReportID == 0)
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, _ReportID);
            }

            try
            {
                dsElement = dmElement.GetDataSet("selectelementstructureall");
            }
            catch (System.Exception e)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetWizard", "selectelementstructureall", e);
                return "";
            }

            if (dsElement.Tables[0] != null && dsElement.Tables[0].Rows.Count > 0)
            {
                bool bHasWrapUp = false;
                bool bHasDispatch = false;
                bool isSubFolder = false;
                //foreach (DataRow oRow in ds.Tables[0].Rows)
                foreach (DataRow oRow in dsElement.Tables[0].Rows)
                {

                    if (oRow["elementname"].ToString().ToLower() == "sendreport" ||
                        oRow["elementname"].ToString().ToLower() == "wrapup520" ||
                        oRow["elementname"].ToString().ToLower() == "submitreport521" ||
                        oRow["elementname"].ToString().ToLower() == "sendreport450" ||
                        oRow["elementname"].ToString().ToLower() == "sendreport2854" ||
                        oRow["elementname"].ToString().ToLower() == "sendreport2803" ||
                        oRow["elementname"].ToString().ToLower() == "wrapup7171")
                    {
                        bHasWrapUp = true;
                    }



                    if (_PersonRole == "Assistant" && oRow["elementname"].ToString().ToLower() == "submitreport521")
                    {
                        bHasWrapUp = false;
                    }


                    else
                    {



                        if (oRow["elementname"].ToString().ToLower() == "dispatchreport" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch520" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch521" ||
                           oRow["elementname"].ToString().ToLower() == "dispatchreport450" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch2854" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch2803" ||
                           oRow["elementname"].ToString().ToLower() == "dispatch7171")
                        {
                            bHasDispatch = true;
                        }

                        strPageName = oRow["aspxpagename"].ToString();
                        strImageFileName = oRow["imagefilename"].ToString();
                        strImageAltTag = oRow["imagealttag"].ToString();
                        strImageHeight = oRow["imageheight"].ToString();
                        strImageWidth = oRow["imagewidth"].ToString();
                        if (strImageFileName.IndexOf("../") != -1)
                        {
                            isSubFolder = true;
                        }

                        //strWizardTxt += "<a href='../" + strPageName + "'>" +
                        //    "<img src='../" + strImageFileName + "'" +
                        //    " alt='" + strImageAltTag + "'" +
                        //    " height='" + strImageHeight + "'" +
                        //    " width='" + strImageWidth + "'></a>";

                        strWizardTxt += "<a href='" + strPageName + "'>" +
                           "<img src='" + strImageFileName + "'" +
                           " alt='" + strImageAltTag + "'" +
                           " height='" + strImageHeight + "'" +
                           " width='" + strImageWidth + "'></a>";
                    }
                }

                if (bHasWrapUp == false && (_PersonRole == "Filer" || _PersonRole == "Assistant"))
                {
                    if (isSubFolder == false)
                    {
                        strWizardTxt += "<img src='images/wizbutton_sendreport_disabled.gif' alt='Send Report'>";
                    }
                    else
                    {
                        strWizardTxt += "<img src='../images/wizbutton_sendreport_disabled.gif' alt='Send Report'>";
                    }

                }
                if (bHasDispatch == false && (_PersonRole.IndexOf("Reviewer") != -1 || _PersonRole.IndexOf("Certifier") != -1 || _PersonRole.IndexOf("Auditor") != -1))
                {
                    if (isSubFolder == false)
                    {
                        strWizardTxt += "<img src='images/wizbutton_dispatchreport_disabled.gif' alt='Dispatch Report'>";
                    }
                    else
                    {
                        strWizardTxt += "<img src='../images/wizbutton_dispatchreport_disabled.gif' alt='Dispatch Report'>";
                    }

                }
                return strWizardTxt;
            }
            else
            {
                return "";
            }

        } //end GetWizard

        public string GetInstruction(int _ElementID, int _Personroleid, int _ReportID)
        {
            string strInstructionTxt = "";

            DataModule dmElement = new DataModule();
            DataSet dsElement = new DataSet();
            dmElement.AddParameter("@elementid", SqlDbType.Int, _ElementID);
            dmElement.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);

            if (_ReportID == 0)
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, _ReportID);
            }

            try
            {
                dsElement = dmElement.GetDataSet("selectelementinstruction");
            }
            catch (System.Exception e)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetInstruction", "selectelementinstruction", e);
                return "";
            }

            if (dsElement.Tables[0] != null && dsElement.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow oRow in dsElement.Tables[0].Rows)
                {
                    strInstructionTxt = oRow["instructiontext"].ToString();

                }
                return strInstructionTxt;
            }
            else
            {
                return "";
            }

        } //end GetInstruction

        public string GetSideBar(int _ElementID, int _Personroleid, int _ReportID)
        {
            string strSideBarTxt = "";
            string strQuestionID = "";
            string strQuestionText = "";
            string strDirectory = "";

            DataModule dmElement = new DataModule();
            DataSet dsElement = new DataSet();
            dmElement.AddParameter("@elementid", SqlDbType.Int, _ElementID);
            dmElement.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);

            if (_ReportID == 0)
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, System.DBNull.Value);
            }
            else
            {
                dmElement.AddParameter("@reportid", SqlDbType.Int, _ReportID);
            }

            try
            {
                dsElement = dmElement.GetDataSet("selectelementquestion");
            }
            catch (System.Exception e)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetSideBar", "selectelementquestion", e);
                return "";
            }

            if (dsElement.Tables[0] != null && dsElement.Tables[0].Rows.Count > 0)
            {
                strSideBarTxt += "<div id='commonquestions'><p id ='header'><span>Common Questions</span></p><ul>";

                foreach (DataRow oRow in dsElement.Tables[0].Rows)
                {
                    strQuestionText = oRow["questiontext"].ToString();
                    strQuestionID = oRow["questionid"].ToString();
                    strDirectory = oRow["elementdirectory"].ToString();

                    if (strDirectory == "")
                    {
                        strSideBarTxt += "<li><a href='AnswerPopup.aspx?qid=" + strQuestionID + "' target='_blank'>" + strQuestionText + "</a></li>";
                    }
                    else
                    {
                        strSideBarTxt += "<li><a href='../AnswerPopup.aspx?qid=" + strQuestionID + "' target='_blank'>" + strQuestionText + "</a></li>";
                    }

                }

                strSideBarTxt += "</ul></div>";

                return strSideBarTxt;
            }
            else
            {
                return "";
            }

        } //end GetSideBar

        public void ExportDataViewToExcel(string FileNameStr, DataView dv1)
        {
            //Extract DataView from GridView (without paging) and call this function
            // creating Excel Application 
            //Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Excel.Application app = new Excel.Application();

            // creating new WorkBook within Excel application 
            //Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);

            // creating new Excelsheet in workbook 
            //Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            Excel.Worksheet worksheet = null;

            Excel.Range oRange;

            // see the excel sheet behind the program 
            app.Visible = true;
            app.DisplayAlerts = false;

            // get the reference of first sheet. By default its name is Sheet1. 
            // store its reference to worksheet 
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;

            // changing the name of active sheet

            worksheet.Name = "Exported from datagrid";

            // storing header part in Excel 
            for (int i = 1; i < dv1.Table.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dv1.Table.Columns[i - 1].ColumnName;
            }
            // storing Each row and column value to excel sheet 
            //for (int i = 0; i < dv1.Table.Rows.Count - 1; i++)
            int _i = 0;
            int _j = 0;
            foreach (DataRow row in dv1.Table.Rows)
            {
                //for (int j = 0; j < dv1.Table.Columns.Count; j++)
                foreach (DataColumn column in dv1.Table.Columns)
                {
                    if (row[column].ToString() != "&nbsp;")
                    worksheet.Cells[_i + 2, _j + 1] = row[column].ToString();

                    _j++;
                }
                _j = 0;
                _i++;
            }
            //Below is for autofit columns
            Excel.Range c1 = worksheet.Cells[1, 1];
            Excel.Range c2 = worksheet.Cells[_i, dv1.Table.Columns.Count];
            oRange = worksheet.get_Range(c1, c2);
            oRange.EntireColumn.AutoFit();
            oRange.WrapText = true;
            oRange.ColumnWidth = 20;

            // save the application 
            //workbook.SaveAs(FileNameStr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application 
            //app.Quit();
            worksheet = null;
            oRange = null;
            workbook = null;
            app = null;
        }

        public void ExportDataGridToExcel(string FileNameStr, DataGrid Gr)
        {
            // creating Excel Application 
            //Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Excel.Application app = new Excel.Application();

            // creating new WorkBook within Excel application 
            //Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);

            // creating new Excelsheet in workbook 
            //Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            Excel.Worksheet worksheet = null;

            Excel.Range oRange;

            // see the excel sheet behind the program 
            app.Visible = true;
            app.DisplayAlerts = false;

            // get the reference of first sheet. By default its name is Sheet1. 
            // store its reference to worksheet 
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;

            // changing the name of active sheet

            worksheet.Name = "Exported from datagrid";

            // storing header part in Excel 
            for (int i = 1; i < Gr.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = Gr.Columns[i - 1].HeaderText;
            }
            // storing Each row and column value to excel sheet 
            for (int i = 0; i < Gr.Items.Count - 1; i++)
            {
                for (int j = 0; j < Gr.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = Gr.Items[i].Cells[j].ToString();
                }
            }
            // save the application 
            //workbook.SaveAs(FileNameStr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            // Exit from the application 
            //app.Quit();
            workbook = null;
            app = null;
        }

        public string GetReportTypeID(int _ReportID, int _Personroleid)
        {
            try
            {
                DataModule dataModule = new DataModule();
                dataModule.AddParameter("@reportid", SqlDbType.Int, _ReportID);

                DataSet ds = dataModule.GetDataSet("selectreporttype");

                return ds.Tables[0].Rows[0]["reporttypeid"].ToString();
            }
            catch (System.Exception ex)
            {
                //log error
                ErrorHandler.LogErrorToDB(_Personroleid, _ReportID, "Common.GetReportTypeID", "selectreporttype", ex);
                return "";
            }
        }

        public static DataSet getStateList()
        {
            //Populate state controls
            DataModule dataModule = new DataModule();
            return dataModule.GetDataSet("SelectStateTypeList");
        }
             
        public DataSet GetStateList()
        {
            //Populate state controls
            DataModule dataModule = new DataModule();
            return dataModule.GetDataSet("SelectStateTypeList");
        }

        public DataSet getState(int stateTypeID)
        {
            DataModule dm = new DataModule();
            dm.AddParameter("@stateTypeID", SqlDbType.Int, stateTypeID);
            return dm.GetDataSetFromCommand("SELECT * FROM statetype WHERE statetypeid = @stateTypeID");
        }

        #region FileUpload

        public void AddAttachment(int personroleid, int reportID, string AttachmentID, string attachmentName,
            string attachmentDescription, int attachmentContentTypeID)
        {
            DataModule dm = new DataModule();
            dm.AddParameter("@action", SqlDbType.VarChar, "insert");
            dm.AddParameter("@personroleid", SqlDbType.Int, personroleid);
            dm.AddParameter("@reportid", SqlDbType.Int, reportID);

            dm.AddParameter("@itemid", SqlDbType.Int, null);
            dm.AddParameter("@attachmentid", SqlDbType.Int, null);

            dm.AddParameter("@attachmenttypeid", SqlDbType.Int, 1); //electronic

            dm.AddParameter("@attachmentname", SqlDbType.VarChar, attachmentName);
            dm.AddParameter("@attachmentdescription", SqlDbType.VarChar, attachmentDescription);

            dm.AddParameter("@attachmentcontenttypeid", SqlDbType.Int, attachmentContentTypeID); //letter of invitation

            DataSet ds = dm.GetDataSet("edititemattachment520");
        }


        public void DeleteAttachment(int personroleid, int reportID, int attachmentID,
            string _NMSAttachmentID, string attachmentName, string attachmentDescription,
            int attachmentContentTypeID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
            _dm.AddParameter("@personroleid", SqlDbType.Int, personroleid);
            _dm.AddParameter("@reportid", SqlDbType.Int, reportID);
            _dm.AddParameter("@attachmentid", SqlDbType.Int, attachmentID);
            _dm.AddParameter("@attachmenttypeid", SqlDbType.Int, 1); //electronic

            _dm.AddParameter("@attachmentname", SqlDbType.VarChar, attachmentName);
            _dm.AddParameter("@attachmentdescription", SqlDbType.VarChar, attachmentDescription);

            _dm.AddParameter("@attachmentcontenttypeid", SqlDbType.Int, attachmentContentTypeID);

            DataSet ds = _dm.GetDataSet("edititemattachment520");
        }

        public DataSet GetReportAttachments(int reportID)
        {
            DataModule dm = new DataModule();
            dm.AddParameter("@reportid", SqlDbType.Int, reportID);
            DataSet ds = dm.GetDataSet("selectreportattachments");
            return ds;
        }

        
        #endregion

        public void LoadCommentLengendList(Literal litItemStatusType, bool bSubFolder)
        {
            DataModule _dm = new DataModule();
            DataSet ds = _dm.GetDataSet("selectcommentlegendlist");

            if (ds.Tables[0].Rows[0] != null)
            {
                litItemStatusType.Text += "<table border='0' align='center' cellpadding='0' cellspacing='15'><tr><td><strong>Entry Status Legend:</strong></td>";
                foreach (DataRow _oRow in ds.Tables[0].Rows)
                {
                    if (bSubFolder == true)
                    {
                        litItemStatusType.Text += "<td>" + _oRow["comment"].ToString().Replace("images", "../images") + "</td>";
                    }
                    else
                    {
                        litItemStatusType.Text += "<td>" + _oRow["comment"].ToString() + "</td>";
                    }
                }
                litItemStatusType.Text += "</tr></table>";
            }
        }

    } //end class Common
}
