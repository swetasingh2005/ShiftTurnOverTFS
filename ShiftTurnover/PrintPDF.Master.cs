using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class PrintPDF : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            lblTitle.Text = Page.Title;
        }

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

            if (ds != null && ds.Tables.Count > 0)
            //if (IsTableHasRow(ds, 0))
            // if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                if (ds.Tables[0].Rows.Count == 0)//no role is found, login failed
                {
                    Response.Redirect("~/Default.aspx");
                }
                else  //found one role, go to default page for the role
                {



                }

            }
            else //wrong loginid
            {

            }
        }


        private void sendToLogin()
        {
            Response.Redirect("~/Default.aspx");
        }

        private void sendToDefaultPage()
        {
            Common _cm = new Common();
            string _DefaultPage = _cm.GetDefaultPageByRole((int)Session["personroleid"]);
            if (_DefaultPage != "")
            {
                Response.Redirect(_DefaultPage);
            }
            else
            {
                Response.Redirect("~/CustomErrorPage.aspx");
            }
        }
    }
}