using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;

using ShiftTurnover.Components;

namespace ShiftTurnover
{
    
    public partial class Default : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session.Abandon();
                //uncomment below after single signon is turned on

                CheckForSingleSignOn(); //single signon will automatically skip the login page and go to roles
            }
           
        }

        private void CheckForSingleSignOn()
        {
            SSO sso = new SSO();

            if (sso.SsoIsTurnedOn() == true)//single sign on is available
            {
                string strUserName = sso.GetSMUserName();
                ProcessLogin(strUserName);
            }
        }

        private void ProcessLogin(string _UserID)
        {


            DataModule _dm = new DataModule();
            _dm.AddParameter("@ssoname", SqlDbType.VarChar, _UserID, 100);

            DataSet ds = _dm.GetDataSet("getlogin"); //authorization - has to exist in database
            string strPersonRoleID = "";

            if (ds != null && ds.Tables.Count > 0)
            //if (IsTableHasRow(ds, 0))
            // if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows.Count == 0)//no role is found, login failed
                {
                    pnlLogin.Visible = false;
                    lblErrMsg.Text = "<ul><li>" + System.Configuration.ConfigurationManager.AppSettings["LoginMessage"] + "</li></ul>";
                    lblErrMsg.Visible = true;
                }
                else if (ds.Tables[0].Rows.Count == 1)//found one role, go to default page for the role
                {

                    strPersonRoleID = ds.Tables[0].Rows[0]["personroleid"].ToString();

                    Session["personroleid"] = Convert.ToInt32(strPersonRoleID);
                    Session["ssoname"] = _UserID;
                    getUserRole((int)Session["personroleid"]);
                    //Common _cm = new Common();
                    //string _DefaultPage = _cm.GetDefaultPageByRole((int)Session["personroleid"]);
                    //string _DefaultPage = "MainPage.aspx";
                    string _DefaultPage = "";
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
                            _DefaultPage = "Admin.aspx";
                            break;
                        default:
                            _DefaultPage = ""; //CustomErrorPage.aspx?
                            break;
                    }
                    if (_DefaultPage != "")
                    {
                        Response.Redirect(_DefaultPage);
                        //Response.Redirect("ReportList.aspx");
                    }
                    else
                    {
                        Response.Redirect("CustomErrorPage.aspx");
                    }

                }
                else
                {//multiple roles are found, go to changerole page	

                    Session["ssoname"] = _UserID;
                    Response.Redirect("ChangeRole.aspx?id=" + _UserID);
                }
            }
            else //wrong loginid
            {
                pnlLogin.Visible = false;
                lblErrMsg.Text = "<ul><li>" + System.Configuration.ConfigurationManager.AppSettings["LoginMessage"] + "</li></ul>";
                lblErrMsg.Visible = true;
            }
        }

        private void getUserRole(int _PersonRoleID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@PersonRoleID", SqlDbType.Int, _PersonRoleID);

                DataSet ds = _dm.GetDataSet("selectpersonrole");

                if (ds != null)
                {
                    Session["personrole"] = ds.Tables[0].Rows[0]["role"].ToString();
                    Session["personname"] = ds.Tables[0].Rows[0]["displayname"].ToString();
                    Session["roledisplayname"] = ds.Tables[0].Rows[0]["role"].ToString();
                    Session["roleid"] = ds.Tables[0].Rows[0]["roleid"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(_PersonRoleID, "Default.getUserRole", "selectpersonrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }


        protected void btnLogin_Click(object sender, System.EventArgs e)
        {


            //Session.Abandon();
            if (txtPassword.Text == System.Configuration.ConfigurationManager.AppSettings["TempPass"]) //check if temp password is correct
            {
                ProcessLogin(txtUserID.Text); //check if userid is correct
            }
            else
            {//temp password is wrong

                pnlLogin.Visible = false;
                lblErrMsg.Text = "<ul><li>" + System.Configuration.ConfigurationManager.AppSettings["LoginMessage"] + "</li></ul>";
                lblErrMsg.Visible = true;
            }
        }
        
    }

    public class PIData

    {

        public string ChillerName;
        public string ChillerStatus;
        public string Color;
        public long StartTime;
        public long EndTime;

        public PIData(string chillername, string chillerstatus,string color, long starttime, long endtime)

        {

            ChillerName = chillername;
            ChillerStatus = chillerstatus;
            Color = color;
            StartTime = starttime;
            EndTime = endtime;

        }
        public PIData(string chillername, string chillerstatus,  long starttime, long endtime)

        {

            ChillerName = chillername;
            ChillerStatus = chillerstatus;
        
            StartTime = starttime;
            EndTime = endtime;

        }
    }
}