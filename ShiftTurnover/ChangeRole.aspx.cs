using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ShiftTurnover.Components;

namespace ShiftTurnover
{
    public partial class ChangeRole : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TableCell mplblColumn = (TableCell)Master.FindControl("narrowcolumn");
            if (mplblColumn != null)
            {
                mplblColumn.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["reportid"] != null)
                {
                    Session["reportid"] = null;
                }

                string strUserID = "";
                if (Request["id"] != null)//from default.aspx
                {
                    //strUserID = Request["id"].ToString(); //GSS changed to avoid users changing id
                    strUserID = Session["ssoname"].ToString();
                    LoadRoles(strUserID);
                }
                else //from 'change role' link
                {
                    if (Session["personroleid"] == null)
                    {//there is an error when session time out.
                        Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        LoadRoles((int)Session["personroleid"]);
                        ListItem oItem = rdoRoles.Items.FindByValue(Session["personroleid"].ToString());
                        if (oItem != null) { oItem.Selected = true; }
                    }
                }

            } //Not Postback

        } //Page_Load

        private void LoadRoles(string _UserID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@ssoname", SqlDbType.VarChar, _UserID, 100);
            DataSet ds = new DataSet();
            ds = _dm.GetDataSet("getlogin");

            if (ds != null && ds.Tables[0] != null)
            {
                rdoRoles.DataSource = ds;
                rdoRoles.DataTextField = "personrole";
                rdoRoles.DataValueField = "personroleid";
                rdoRoles.DataBind();

                if (ds.Tables[0].Rows.Count == 0)//no role is found, login failed
                {
                    Response.Redirect("Default.aspx");
                }

                rdoRoles.Items[0].Selected = true;//check default role
            }

        } //LoadRoles

        private void LoadRoles(int _Personroleid)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@ssoname", SqlDbType.VarChar, System.DBNull.Value, 100);
            _dm.AddParameter("@personroleid", SqlDbType.Int, _Personroleid, 0);

            DataSet ds = new DataSet();
            ds = _dm.GetDataSet("getlogin");

            if (ds != null)
            {
                rdoRoles.DataSource = ds;
                rdoRoles.DataTextField = "personrole";
                rdoRoles.DataValueField = "personroleid";
                rdoRoles.DataBind();

                if (ds.Tables[0].Rows.Count == 0)//no role is found, login failed
                {
                    Response.Redirect("Default.aspx");
                }

                //rdoRoles.Items[0].Selected = true;//check default role
                ListItem lItem = rdoRoles.Items.FindByValue(_Personroleid.ToString());
                if (lItem != null) { lItem.Selected = true; }
            }
        } //LoadRoles

        private void getUserRole(int _Personroleid)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@personroleid", SqlDbType.Int, _Personroleid);

                DataSet ds = _dm.GetDataSet("selectpersonrole");

                if (ds != null)
                {
                    Session["personrole"] = ds.Tables[0].Rows[0]["role"].ToString();
                    Session["roleid"] = ds.Tables[0].Rows[0]["roleid"].ToString();
                    Session["personname"] = ds.Tables[0].Rows[0]["displayname"].ToString();
                   

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(_Personroleid, "ChangeRole.getUserRole", "selectpersonrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string _DefaultPage = "";
            Session["personroleid"] = null; 
            Session["personroleid"] = Convert.ToInt32(rdoRoles.SelectedValue);

            getUserRole(Convert.ToInt32(rdoRoles.SelectedValue));

            switch (Convert.ToInt32(Session["roleid"]))
            {
                case 1: // "Operator":
                    _DefaultPage = "MainPage.aspx";
                    break;
                //case "Assistant":
                //    _DefaultPage = "ReportList.aspx";
                //    break;
                case 9: // "Manager":
                    _DefaultPage = "Reports.aspx";
                    break;
                case 2: // "Admin":
                    _DefaultPage = "ManageUser.aspx";
                    break;
                default:
                    _DefaultPage = ""; //CustomErrorPage.aspx?
                    break;
            }

            //Common _cm = new Common();
            //string _DefaultPage = _cm.GetDefaultPageByRole((int)Session["personroleid"]);

            if (_DefaultPage != "")
            {
                Response.Redirect(_DefaultPage);
                ////Response.Redirect("ReportList.aspx");
            }
            else
            {
                Response.Redirect("CustomErrorPage.aspx");
            }

        } //btnsubmit_click

        protected void btnCancel_Click(object sender, EventArgs e)
        {
             Response.Redirect("Default.aspx");
        }
    }
}