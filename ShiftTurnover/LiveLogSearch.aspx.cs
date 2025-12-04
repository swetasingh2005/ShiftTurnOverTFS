using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace ShiftTurnover
{
    public partial class LiveLogSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtEDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            txtSDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");

            if (Session["personrole"] != null)
            {
                if (!Page.IsPostBack)
                {
                    txtSDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
                    txtEDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
                    //txtEDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                    //txtSDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
                    LoadReporter();
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
       
      
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateQuery())
            {
                pnlResult.Visible = true;
                if (ddlLogType.SelectedValue.Equals("3"))
                { LoadSmartGridForFuel(); }
                
                else
                { LoadSmartGridForList(); }
            }
            else
            {
                pnlResult.Visible = false;
            }
             
        }
        private void LoadReporter( )
        {
            try
            {
                DataModule _dm = new DataModule();
              
                DataSet ds = _dm.GetDataSet("SelectLiveLogPersonList");

                if (ds != null)
                {
                    ddlReporter.DataSource = ds;
                    ddlReporter.DataTextField = "personname";
                    ddlReporter.DataValueField = "personroleid";
                    ddlReporter.DataBind();
                    ddlReporter.Items.Insert(0, "All Reporters");
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "NewTurnover.LoadLookup", "SelectLiveLogPersonList", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected bool ValidateQuery()
        {
            bool rtn = true;
             
           if (ddlSearch1.SelectedIndex==0 && (txtSearch1.Text.Length>0 && txtSearch2.Text.Length > 0))
            {
                lblCondition.Text = "Required";
                rtn = false;
            }
           else
            {
                lblCondition.Text = "";
                rtn = true;
            }
        
          
             
            return rtn;
        }
        protected DataSet GetSearchResult()
        {
            StringBuilder sb = new StringBuilder();
            DataModule _dm = new DataModule();
            DataSet dt = new DataSet();
            string SR = ddlSR.SelectedValue;
            string Reporter = "  All  "; string DateRange = " From   " + txtSDate.Text + " To " + txtEDate.Text;
            int PersonRoleID = 0;
            DateTime Sdate = Convert.ToDateTime(DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd"));
            DateTime Edate = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
            if (txtEDate.Text.Length > 0)
            {
                Edate= Convert.ToDateTime(txtEDate.Text);
            }
            if (txtSDate.Text.Length > 0)
            {
                Sdate = Convert.ToDateTime(txtSDate.Text);
            }
            sb.Append(" select  personroleid,ShiftID,'Reported By' = dbo.getpersonname(dbo.getpersonid(personroleid), NULL) ,'Time of Event' = CreateDate,'Description' =  ISNULL(ServiceRequestID,' ')  + '  ' +  Description,'Service Request' = ServiceRequest , CASE WHEN Deleted = 1 THEN 'Yes' else 'No' End as 'Deleted', UpdatedDate from LiveLog");
            sb.Append("  where 1=1");
            
            if (txtSDate.Text.Length>0 && txtEDate.Text.Length > 0)
            {
                sb.Append(" And CreateDate between '" + Sdate + "'  and '" + Edate + "'");
            }
           
            if (ddlReporter.SelectedIndex > 0)
            {
                PersonRoleID = Convert.ToInt32(ddlReporter.SelectedValue);
                Reporter = ddlReporter.SelectedItem.Text;
                sb.Append(" and PersonRoleID =   " + PersonRoleID);
                
            }
            if (SR.Equals("true"))
            {
                sb.Append(" and (ServiceRequest  like '%y%' or ServiceRequest like '%true%' or ServiceRequest like '%sr%')  ");
            }
            else if (SR.Equals("false"))
            {
                sb.Append(" and (  ServiceRequest like '%false%'  )  ");
            }
            if (txtSearch1.Text.Length > 0)
            {
                sb.Append(" and ( ");
                sb.Append("  Lower([Description]) like '%" + txtSearch1.Text + "%'  ");
                if (ddlSearch1.SelectedIndex > 0 && txtSearch2.Text.Length > 0)
                {
                    sb.Append(ddlSearch1.SelectedItem.Text + "  Lower([Description]) like '%" + txtSearch2.Text + "%' ");
                }
                sb.Append(" )");
            }
            dt = _dm.GetDataSetInLineQuery(sb.ToString());
            return dt;
        }
        
        protected DataSet GetSearchResult(string table)
        {
            StringBuilder sb = new StringBuilder();
            DataModule _dm = new DataModule();
            DataModuleChem _dmChem = new DataModuleChem();
            DataSet dt = new DataSet();
            string SR = ddlSR.SelectedValue;
            string Reporter = "  All  "; string DateRange = " From   " + txtSDate.Text + " To " + txtEDate.Text;
            int PersonRoleID = 0;
            DateTime Sdate = Convert.ToDateTime(DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd"));
            DateTime Edate = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
            if (txtEDate.Text.Length > 0)
            {
                Edate = Convert.ToDateTime(txtEDate.Text).AddDays(1);
            }
            if (txtSDate.Text.Length > 0)
            {
                Sdate = Convert.ToDateTime(txtSDate.Text).Date;
            }
            sb.Append(" select  personroleid,'Reported By' = dbo.getpersonname(dbo.getpersonid(personroleid), NULL) ,'Time of Event' = CreateDate,Description , CASE WHEN Deleted = 1 THEN 'Yes' else 'No' End as 'Deleted' from " + table + "");
            //string sql = " select  personroleid,'Reported By' = dbo.getpersonname(dbo.getpersonid(personroleid), NULL) ,'Time of Event' = CreateDate,Description , CASE WHEN Deleted = 1 THEN 'Yes' else 'No' End as 'Deleted' from EngineeringLog";
            sb.Append("  where 1=1 ");

            if (txtSDate.Text.Length > 0 && txtEDate.Text.Length > 0)
            {
                sb.Append(" And CreateDate between '" + Sdate + "'  and '" + Edate + "'");
            }

            if (ddlReporter.SelectedIndex > 0)
            {
                PersonRoleID = Convert.ToInt32(ddlReporter.SelectedValue);
                Reporter = ddlReporter.SelectedItem.Text;
                sb.Append(" and PersonRoleID =   " + PersonRoleID);

            }
           
            if (txtSearch1.Text.Length > 0)
            {
                sb.Append(" and ( ");
                sb.Append("  Lower([Description]) like '%" + txtSearch1.Text + "%'  ");
                if (ddlSearch1.SelectedIndex > 0 && txtSearch2.Text.Length > 0)
                {
                    sb.Append(ddlSearch1.SelectedItem.Text + "  Lower([Description]) like '%" + txtSearch2.Text + "%' ");
                }
                sb.Append(" )");
            }
            if (ddlLogType.SelectedValue.Equals("4"))
            { dt = _dmChem.GetDataSetInLineQuery(sb.ToString()); }
            else { dt = _dm.GetDataSetInLineQuery(sb.ToString()); }
            
            return dt;
        }
        protected DataSet GetSearchResultFuelLog()
        {
            StringBuilder sb = new StringBuilder();
            DataModule _dm = new DataModule();
            DataSet dt = new DataSet();
            string SR = ddlSR.SelectedValue;
            string Reporter = "  All  "; string DateRange = " From   " + txtSDate.Text + " To " + txtEDate.Text;
            int PersonRoleID = 0;
            DateTime Sdate = Convert.ToDateTime(DateTime.Today.AddYears(-5).ToString("yyyy-MM-dd"));
            DateTime Edate = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-dd"));
            if (txtEDate.Text.Length > 0)
            {
                Edate = Convert.ToDateTime(txtEDate.Text);
            }
            if (txtSDate.Text.Length > 0)
            {
                Sdate = Convert.ToDateTime(txtSDate.Text);
            }
            sb.Append(" select FuelLogID, personroleid,'Reported By' = dbo.getpersonname(dbo.getpersonid(personroleid), NULL) ,'Time of Event' = CreateDate,Gallons, Supplier, Driver, BillNumber, OrderNumber  ");
            //string sql = " select  personroleid,'Reported By' = dbo.getpersonname(dbo.getpersonid(personroleid), NULL) ,'Time of Event' = CreateDate,Description , CASE WHEN Deleted = 1 THEN 'Yes' else 'No' End as 'Deleted' from EngineeringLog";
            sb.Append(",'AttachmentID' = (Select top 1 FuelLogAttachmentID from[dbo].[FuelLogAttachment] a where a.FuelLogID =l.FuelLogID and  a.DocDescription='Ticket 1' order by FuelLogAttachmentID desc)");
            sb.Append(",'AttachmentID1' = (Select top 1 FuelLogAttachmentID from[dbo].[FuelLogAttachment] a where a.FuelLogID =l.FuelLogID and  a.DocDescription='Ticket 2' order by FuelLogAttachmentID desc)");
            sb.Append(", 'AttachmentBillID' = (Select top 1   FuelLogAttachmentID from[dbo].[FuelLogAttachment] a where a.FuelLogID =l.FuelLogID and  a.DocDescription='Bill Of Landing' order by FuelLogAttachmentID desc)");
            sb.Append(" from  FuelLog l where 1=1 ");
            if (txtSDate.Text.Length > 0 && txtEDate.Text.Length > 0)
            {
                sb.Append(" And CreateDate between '" + Sdate.ToShortDateString() + "'  and '" + Edate.AddDays(1).ToShortDateString() + "'");
            }
            if (ddlReporter.SelectedIndex > 0)
            {
                PersonRoleID = Convert.ToInt32(ddlReporter.SelectedValue);
                Reporter = ddlReporter.SelectedItem.Text;
                sb.Append(" and PersonRoleID =   " + PersonRoleID);
            }

            if (txtSearch1.Text.Length > 0)
            {
                sb.Append(" and ( ");
                sb.Append("  Lower([Supplier]) like '%" + txtSearch1.Text + "%'  ");
                if (ddlSearch1.SelectedIndex > 0 && txtSearch2.Text.Length > 0)
                {
                    sb.Append(ddlSearch1.SelectedItem.Text + "  Lower([Supplier]) like '%" + txtSearch2.Text + "%' ");
                }
                sb.Append(" )");
                
            }
            dt = _dm.GetDataSetInLineQuery(sb.ToString());
            return dt;
        }
        
        protected void LoadSmartGridForFuel()
        {
            string keyword = ""; DataSet ds;
            if (txtSearch1.Text.Length > 0)
            {
                keyword = txtSearch1.Text;
            }
            ds = GetSearchResultFuelLog();
            
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();
            lblReporter.Text = ddlReporter.SelectedItem.Text;
            lblDateRange.Text = txtSDate.Text + " to " + txtEDate.Text;
            lblSR.Text = ddlSR.SelectedItem.Text;
            string Condition = ""; int _attID = 0;int _attID1 = 0; int _attBillID = 0;
            if (!ddlSearch1.SelectedValue.Equals("0"))

            { Condition = ddlSearch1.SelectedItem.Text; }
            lblKeywordlist.Text = txtSearch1.Text + " " + Condition + " " + txtSearch2.Text;


            if (ds != null)
            {
                lblCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>FuelLogID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Reported By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Time of Event</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Gallons</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Supplier</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Driver</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Bill Number</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Order Number</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Attchment(s)</th>");
                    strHtml.Append("</tr></thead><tbody>");


                    try
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            strBody.Append("<tr>");

                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["FuelLogID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Reported By"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Time of Event"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Gallons"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Supplier"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Driver"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["BillNumber"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["OrderNumber"].ToString() + "</td>");

                            try
                            { _attID = Int32.Parse(Convert.ToString(row["AttachmentID"])); }
                            catch (FormatException) { }
                            try
                            { _attID1 = Int32.Parse(Convert.ToString(row["AttachmentID1"])); }
                            catch (FormatException) { }
                            try
                            { _attBillID = Int32.Parse(Convert.ToString(row["AttachmentBillID"])); }
                            catch (FormatException) { }
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>");
                            if (_attID > 0)
                            { strBody.Append("<a  target='_blank' href=\"ViewAttachment.aspx?aid=" + _attID + "\">  Before Ticket </a>"); }
                            strBody.AppendLine();
                            if (_attID1 > 0)
                            { strBody.Append("<a  target='_blank' href=\"ViewAttachment.aspx?aid=" + _attID1 + "\">  After Ticket </a>"); }
                            strBody.AppendLine();
                            if (_attBillID > 0)
                            { strBody.Append("      "); strBody.Append("<a  target='_blank' href=\"ViewAttachment.aspx?aid=" + _attBillID + "\">   Bill Of Lading </a>"); }

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
        protected void LoadSmartGridForList()
        {
            string keyword = ""; DataSet ds;
            if (txtSearch1.Text.Length > 0)
            {
                keyword = txtSearch1.Text;
            }
            if (ddlLogType.SelectedIndex == 0)
            {
                 ds = GetSearchResult();
            }
            else if (ddlLogType.SelectedIndex == 1)
            {
                 ds = GetSearchResult(" EngineeringLog ");
            }
            else if (ddlLogType.SelectedIndex == 4)
            {
                ds = GetSearchResult(" LiveLog ");
            }
            else if (ddlLogType.SelectedIndex == 5)
            {
                ds = GetSearchResult(" SecurityLog ");
            }
            else
            {
                ds = GetSearchResult("HVGLog");
            }
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();
            lblReporter.Text = ddlReporter.SelectedItem.Text;
            lblDateRange.Text = txtSDate.Text + " to " + txtEDate.Text;
            lblSR.Text = ddlSR.SelectedItem.Text;
            string Condition = "";
            if (!ddlSearch1.SelectedValue.Equals("0"))

            { Condition = ddlSearch1.SelectedItem.Text; }
            lblKeywordlist.Text = txtSearch1.Text + " " +  Condition + " " + txtSearch2.Text;


            if (ds != null)
            {
                lblCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    if (ddlLogType.SelectedIndex == 0)
                    {
                        strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>ShiftID</th>");
                        strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Shift</th>");
                    }
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Reported By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Time of Event</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Description</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Deleted</th>");
                    if (ddlLogType.SelectedIndex == 0)
                    {
                        strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Service Request</th>");
                        strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Updated Date</th>");
                    }
                    strHtml.Append("</tr></thead><tbody>");


                    try
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["Deleted"].ToString().Equals("Yes")) 
                            { strBody.Append("<tr style='text-decoration:line-through;'>"); }
                            else { strBody.Append("<tr>"); }


                            if (ddlLogType.SelectedIndex == 0)
                            {
                                strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["ShiftID"].ToString() + "</td>");
                                strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + Common.getShiftLabel(Convert.ToInt16(row["ShiftID"])) + "</td>");
                            }
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Reported By"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Time of Event"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Description"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Deleted"].ToString() + "</td>");
                            if (ddlLogType.SelectedIndex == 0)
                            {
                                strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Service Request"].ToString() + "</td>");
                                strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["UpdatedDate"].ToString() + "</td>");

                            }
                            strBody.Append(" </tr>");

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
        protected void LoadSmartGridForListBYSP()
        {
            string keyword = "";
            DataModule _dm = new DataModule();
            string SR = ddlSR.SelectedValue;
            string Reporter = "  All  "; string DateRange = " From   " + txtSDate.Text + " To " + txtEDate.Text;
            int PersonRoleID = 0;
            if (ddlReporter.SelectedIndex>0)
            {
                PersonRoleID = Convert.ToInt32(ddlReporter.SelectedValue);
                Reporter = ddlReporter.SelectedItem.Text;
            }
             
            _dm.AddParameter("@searchtext", SqlDbType.VarChar, txtSearch1.Text.ToLower());
            _dm.AddParameter("@SDate", SqlDbType.DateTime, Convert.ToDateTime(txtSDate.Text));
            _dm.AddParameter("@EDate", SqlDbType.DateTime, Convert.ToDateTime(txtEDate.Text));
            _dm.AddParameter("@SR", SqlDbType.VarChar, SR);
            _dm.AddParameter("@PersonRoleID", SqlDbType.Int, PersonRoleID);
            DataSet ds = _dm.GetDataSet("checksearchtext");
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();
           
            if (ds != null)
            {
                lblCount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>ShiftID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Shift</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Reported By</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Time of Event</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Description</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Service Request</th>");
                    strHtml.Append("</tr></thead><tbody>");


                    try
                    {

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            strBody.Append("<tr>");
                            


                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["ShiftID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + Common.getShiftLabel(Convert.ToInt16(row["ShiftID"])) + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Reported By"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Time of Event"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Description"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px black;  '>" + row["Service Request"].ToString() + "</td>");


                               strBody.Append(" </tr>");

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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch2.Text = "";
            txtSearch1.Text = "";
            ddlSearch1.SelectedIndex = 0;
            lblCondition.Text = "";
            pnlResult.Visible = false;
            ddlSR.SelectedIndex = 0;
            lblError.Text = "";
            txtSDate.Text = DateTime.Today.AddYears(-1).ToString("yyyy-MM-dd");
            ddlReporter.SelectedIndex = 0;
            ddlSearch1.SelectedIndex = 0;
            ddlLogType.SelectedIndex = 0;
            txtEDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            netTable.InnerHtml = "";
            lblKeywordlist.Text = "";
            lblReporter.Text = "";
            lblSR.Text = "";
            lblDateRange.Text = "";
        }

       
        protected void ExportDataSetToExcel(DataTable dt)
        {


            HttpResponse response = HttpContext.Current.Response;

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

        protected void btnExport_Click(object sender, EventArgs e)
        {   if (ddlLogType.SelectedIndex == 0)
            {
                DataSet ds = GetSearchResult();
                if (ds != null)
                {
                    ExportDataSetToExcel(ds.Tables[0]);
                }
            }
            else if (ddlLogType.SelectedIndex == 1)
            {
                DataSet ds = GetSearchResult(" EngineeringLog ");
                if (ds != null)
                {
                    ExportDataSetToExcel(ds.Tables[0]);
                }
            }
            else if (ddlLogType.SelectedIndex == 3)
            {
                DataSet ds = GetSearchResultFuelLog();
                if (ds != null)
                {
                    ExportDataSetToExcel(ds.Tables[0]);
                }
            }
            else if (ddlLogType.SelectedIndex == 5)
            {
                DataSet ds = GetSearchResult(" SecurityLog ");
                if (ds != null)
                {
                    ExportDataSetToExcel(ds.Tables[0]);
                }
            }
            else
            {
                DataSet ds =  GetSearchResult(" HVGLog ");
                if (ds != null)
                {
                    ExportDataSetToExcel(ds.Tables[0]);
                }
            }
        }
         

        

     
        protected void ddlSearch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSearch1.SelectedValue.Equals("0"))
            { rfvSearch2.Enabled = false; }
            else { rfvSearch2.Enabled = true; }
        }
    }
}