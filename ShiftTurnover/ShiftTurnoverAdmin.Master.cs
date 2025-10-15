using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ShiftTurnover.Components;

namespace ShiftTurnover
{
    public partial class ShiftTurnoverAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["pageid"] != null)
            {
                narrowcolumn.Visible = true;
            }


            lblTitle.Text = Page.Title;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["personrole"] != null)
            {
                if (Request["popup"] != null)
                {
                    if (lblHideSideBar.Text == "1")
                    {
                        narrowcolumn.Visible = false;
                    }
                }
                if (!Page.IsPostBack)
                {
                    lblTitle.Text = Page.Title; //added again to display updated value, if any on content page
                    lblGuide.Text = "~/Documents/NearMissSafetyForm_UserManuel.docx";

                    if (Session["personname"] != null)
                    {
                        lblName.Text = (string)Session["personname"];
                    }
                    if (Session["personrole"] != null)
                    {
                        lblRole.Text = (string)Session["personrole"];
                    }


                }
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