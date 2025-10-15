using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class ManageFCISUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                lblConfirm.Text = " ";


                if (Request["update"] == "delete")
                {

                    int PID = 0;
                    if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                    {
                        PID = Convert.ToInt32(Request.QueryString["ID"]);

                        if (PID > 0)
                        {

                            DeleteUser(PID);
                        }

                    }
                    lblConfirm.Text = "The User ID (" + PID + ")  has been successfully archived.";
                    lblConfirm.Visible = true;
                }
                LoadSmartGrid();


            }//postback
        }
        private void DeleteUser(int ID)
        {
            
            try
            {

                DataModuleFCIS _dm = new DataModuleFCIS();
                _dm.AddParameter("@action", SqlDbType.VarChar, "delete");
                _dm.AddParameter("@personid", SqlDbType.Int, ID);
               
                _dm.GetDataSet("deleteuser");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(1200, ID, "Manageuser.DeleteUser" + ex.Message, "deleteuser", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        }
        protected System.Data.DataTable GetData()
        {
            DataModuleFCIS _dm = new DataModuleFCIS();
            DataSet ds = _dm.GetDataSet("SelectPersonRoleListALL");
            return ds.Tables[0];
        }

        protected void LoadSmartGrid()
        {
            System.Data.DataTable ds = GetData();
            int _attID = 0; int _ID = 0; string ViewURL = ""; string EditURL = "";
            StringBuilder strHtml = new StringBuilder();
            StringBuilder strBody = new StringBuilder();


            if (ds != null)
            {
                lblCount.Text = "Total: " + Convert.ToString(ds.Rows.Count);
                if (ds.Rows.Count == 0)
                {
                    netTable.InnerHtml = "";
                }
                else
                {

                    strHtml.Append("<table  align='center'   id=\'example\' >  ");
                    strHtml.Append("<thead><tr Width:100%; border:solid 1px black;>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Person ID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Display Name</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>User ID</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Roles</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Active</th>");
                    strHtml.Append("<th style='text-align:center; border:solid 1px black; color:white;  background-color:grey;'>Action</th>");
                    strHtml.Append("</tr></thead><tbody>");



                    try
                    {

                        foreach (DataRow row in ds.Rows)
                        {
                            if (row["Active"].ToString().Equals("No"))
                            { strBody.Append("<tr style='text-decoration: line-through; '>"); }
                            else { strBody.Append("<tr>"); }


                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>" + row["PersonID"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["displayname"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["userid"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["Roles"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ;  black;'>" + row["Active"].ToString() + "</td>");
                            strBody.Append("<td style='text-align:left; border:solid 1px ; black; '>");
                            string uEdit = ResolveUrl("~/Images/Modify.png");
                             
                            _attID = Convert.ToInt16(row["PersonID"].ToString());
                            string uDel = ResolveUrl("~/Images/archive.jpg");
                            if (_attID > 0)
                            {
                                ViewURL = "ManageFCISUsers.aspx?update=delete&";
                                strBody.Append("<a target='_self' href=\"" + ViewURL + "aid=" + _attID + "\"><img src=" + uDel + " alt='edit' width='30' height='30' title='Click here to deactivate the user '  /></a> ");
                            }
                            strBody.AppendLine();
                            EditURL = "AddEditFCISUser.aspx?";
                            strBody.Append("<a target='_self' href=\"" + EditURL + "ID=" + _attID + "\"><img src=" + uEdit + " alt='edit' width='30' height='30' title='Click here to Edit this User'  /></a> ");
                            strBody.Append("</td> </tr>");
                        }
                        netTable.InnerHtml = strHtml.ToString() + strBody.ToString() + "</tbody></table>";
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogErrorToDB((int)Session["personroleid"], "ManageFCISusers.LoadSmartGrid", "SelectPersonRoleListALL", ex);
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditUser.aspx?ID=0");
        }
    }
}