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
    public partial class GSSP : System.Web.UI.MasterPage
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
                    pnlFirstMenu.Visible = true;

                    string url = HttpContext.Current.Request.Url.AbsoluteUri;
                    SetFirstMenu(url);
                    if (url.Contains("NewTurnover") || url.Contains("Main"))
                    {
                        SetSecondMenu(url);
                        if (url.Contains("_CriticalAlm"))
                        {
                            pnlsecondMenu.Visible = true;
                            pnlthirdMenu.Visible = false;
                            pnlthirdAlmMenu.Visible = true;
                            SetThirdAlmMenu(url);
                        }
                        else
                        {
                            pnlthirdMenu.Visible = false;
                            //pnlthirdWOMenu.Visible = false;
                            pnlthirdAlmMenu.Visible = false;
                        }
                    }
                    else if (url.Contains("Log") || url.Contains("NearMiss"))
                    {
                        pnlsecondMenu.Visible = false;
                        pnlthirdMenu.Visible = true;
                        pnlthirdAlmMenu.Visible = false;
                        SetThirdMenu(url);
                    }
                    else
                    {
                        pnlsecondMenu.Visible = false;
                    }

                    //if (url.Contains("SubmissionReport.aspx"))
                    if (new[] { "Manager" }.Contains(Session["personrole"].ToString()))
                    {
                        pnlsecondMenu.Visible = false;
                        pnlthirdMenu.Visible = false;
                        pnlFirstMenu.Visible = false;
                        //pnlthirdWOMenu.Visible = false;
                    }


                }

            }
            else
            {
                pnlsecondMenu.Visible = false;
                pnlthirdMenu.Visible = false;
                pnlFirstMenu.Visible = false;

            }

        }
        protected void SetFirstMenu(string url)
        {
            
            string btn = "MainPage";
            if (url.Contains("EquipmentStatus")) { btn = "EquipmentStatus"; }
            else if (url.Contains("NewTurnover")) { btn = "NewTurnover"; }
            else if (url.Contains("OldTurnover")) { btn = "OldTurnover"; }
            else if (url.Contains("CheckStatus")) { btn = "CheckStatus"; }
            else if (url.Contains("LiveLogSearch")) { btn = "LiveLogSearch"; }
            else if (url.Contains("EngineeringLog")) { btn = "EngineeringLog"; }
            else if (url.Contains("HVGLog")) { btn = "HVGLog"; }
            else if (url.Contains("FuelLog")) { btn = "FuelLog"; }
            else if (url.Contains("SecurityLog")) { btn = "SecurityLog"; }
            else if (url.Contains("NearMiss")) { btn = "NearMiss"; }
            else if (url.Contains("WaterTreatment")) { btn = "WaterTreatment"; }
            else { btn = "MainPage"; }

            switch (btn)
            {
                case "CheckStatus":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    pnlFirstMenu.Visible = false;
                    break;

                case "EquipmentStatus":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "active";
                    // aLiveLogSearch.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "inactive";

                    break;
                case "NewTurnover":
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "active";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "inactive";

                    pnlsecondMenu.Visible = true;
                    pnlthirdMenu.Visible = false;
                    break;
                case "MainPage":
                    aLiveLog.Attributes["class"] = "active";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "inactive";

                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    break;
                case "OldTurnover":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    // aLiveLogSearch.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "inactive";

                    break;
                case "LiveLogSearch":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";

                    break;
                case "SecurityLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";

                    break;
                case "NearMiss":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";

                    break;
                case "EngineeringLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";

                    break;
                case "HVGLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    //pnlthirdWOMenu.Visible = false;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //  aLiveLogSearch.Attributes["class"] = "inactive";

                    break;
                case "FuelLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    //pnlthirdWOMenu.Visible = false;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";
                    break;
                case "WaterTreatment":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = true;
                    aLiveLog.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    //aLiveLogSearch.Attributes["class"] = "inactive";

                    break;
                default:
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    break;
            }

        }
        protected void SetSecondMenu(string url)
        {
            string subMenubtn = "";
            if (url.Contains("NewTurnover.aspx")) { subMenubtn = "ShiftW"; }
            else if (url.Contains("CriticalAlm")) { subMenubtn = "CriticalAlm"; }
            else if (url.Contains("Main")) { subMenubtn = "Live"; }
            else if (url.Contains("LOTO")) { subMenubtn = "LOTO"; }
            else if (url.Contains("Review")) { subMenubtn = "Review"; }
            else if (url.Contains("Acknowledgement")) { subMenubtn = "Acknowledgement"; }
            else { subMenubtn = "Acknowledgement"; }
            switch (subMenubtn)
            {
                case "ShiftW":
                    btnShiftWorker.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnReview.BackColor = Color.Black;
                    
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Live":
                     
                    btnReview.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;

                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;


                case "CriticalAlm":
                    btnAlarms.BackColor = ColorTranslator.FromHtml("#b3112c");
                   
                    btnReview.BackColor = Color.Black;

                    btnShiftWorker.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Review":
                    btnReview.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnShiftWorker.BackColor = Color.Black;
                    
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Acknowledgement":
                    btnReview.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;
                     
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = ColorTranslator.FromHtml("#b3112c");
                    break;
                default:
                    break;


            }
        }
        protected void SetThirdMenu(string url)
        {
            string statusMenubtn = "";
            if (url.Contains("EngineeringLog")) { statusMenubtn = "btnEngg"; }
            else if (url.Contains("HVGLog")) { statusMenubtn = "btnHVG"; }
            else if (url.Contains("FuelLog")) { statusMenubtn = "btnFuel"; }
            else if (url.Contains("LiveLogSearch")) { statusMenubtn = "btnLogSearch"; }
            else if (url.Contains("WaterTreatment")) { statusMenubtn = "btnWaterTreatment"; }
            else if (url.Contains("SecurityLog")) { statusMenubtn = "btnSecurityLog"; }
            else if (url.Contains("NearMiss")) { statusMenubtn = "btnNearMiss"; }
            else { statusMenubtn = "btnEngg"; }
            pnlthirdMenu.Visible = true;
            switch (statusMenubtn)
            {
                case "btnNearMiss":
                    btnSecurityLog.BackColor = Color.Black;
                    btnWaterTreatment.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnLogSearch.BackColor = Color.Black;
                    btnEngg.BackColor = Color.Black;
                    btnNearMiss.BackColor = ColorTranslator.FromHtml("#b3112c");
                    break;
                case "btnSecurityLog":
                    btnSecurityLog.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnWaterTreatment.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnLogSearch.BackColor = Color.Black;
                    btnEngg.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                case "btnWaterTreatment":
                    btnWaterTreatment.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnHVG.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnLogSearch.BackColor = Color.Black;
                    btnEngg.BackColor = Color.Black;
                    btnSecurityLog.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                case "btnEngg":
                    btnEngg.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnHVG.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnLogSearch.BackColor = Color.Black;
                    btnWaterTreatment.BackColor = Color.Black;
                    btnSecurityLog.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                case "btnHVG":
                    btnEngg.BackColor = Color.Black;
                    btnHVG.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnFuel.BackColor = Color.Black;
                    btnLogSearch.BackColor = Color.Black;
                    btnWaterTreatment.BackColor = Color.Black;
                    btnSecurityLog.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                case "btnFuel":
                    btnEngg.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnFuel.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnLogSearch.BackColor = Color.Black;
                    btnWaterTreatment.BackColor = Color.Black;
                    btnSecurityLog.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                case "btnLogSearch":
                    btnEngg.BackColor = Color.Black;
                    btnHVG.BackColor = Color.Black;
                    btnFuel.BackColor = Color.Black;
                    btnWaterTreatment.BackColor = Color.Black;
                    btnLogSearch.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnSecurityLog.BackColor = Color.Black;
                    btnNearMiss.BackColor = Color.Black;
                    break;
                default:
                    break;
            }

        }

        protected void SetThirdAlmMenu(string url)
        {

            btnCriticalAlm.BackColor = ColorTranslator.FromHtml("#b3112c");

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
        protected void btnLiveLog_Click(object sender, EventArgs e)
        {

            Response.Redirect("MainPage.aspx");
        }

        protected void btnEquipmentStatus_Click(object sender, EventArgs e)
        {

            Response.Redirect("EquipmentStatus.aspx");
        }
        protected void btnNewTurnover_Click(object sender, EventArgs e)
        {

            Response.Redirect("NewTurnover_Acknowledgement.aspx");


        }
        protected void btnOldTurnover_Click(object sender, EventArgs e)
        {
            Response.Redirect("OldTurnover.aspx");
        }
        protected void btnShiftWorker_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover.aspx");
        }
        protected void btnStatus_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_ChlStatus.aspx");
        }
        //protected void btnWO_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("NewTurnover_WO.aspx");
        //}
        protected void btnReview_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_Review.aspx");
        }
        protected void btnChl_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_ChlStatus.aspx");
        }
        protected void btnChlPmp_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_ChlPmpStatus.aspx");
        }
        protected void btnCT_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_CTStatus.aspx");
        }
        protected void btnFreeCo_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_FreeCoStatus.aspx");
        }
        protected void btnBlr_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_BlrStatus.aspx");
        }
        protected void btnROPmp_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_ROBlrStatus.aspx");
        }
        protected void btnAlarms_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_CriticalAlm.aspx");
        }
        protected void btnCriticalAlm_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_CriticalAlm.aspx");
        }

        protected void btnAchn_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_Acknowledgement.aspx");
        }

        protected void btnLOTO_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_LOTO.aspx");
        }

        protected void btnLiveLog_Click1(object sender, EventArgs e)
        {
            Response.Redirect("MainPage.aspx");
        }

        protected void btnEngg_Click(object sender, EventArgs e)
        {
            Response.Redirect("EngineeringLog.aspx");
        }

        protected void btnHVG_Click(object sender, EventArgs e)
        {
            Response.Redirect("HVGLog.aspx");
        }

        protected void btnFuel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FuelLog.aspx");
        }

        protected void btnLogSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("LiveLogSearch.aspx");
        }

        protected void btnInCtr_Click(object sender, EventArgs e)
        {
            Response.Redirect("WaterTreatmentLog.aspx");
        }

        protected void btnWaterTreatment_Click(object sender, EventArgs e)
        {
            Response.Redirect("WaterTreatmentLog.aspx");
        }

        protected void btnSecurityLog_Click(object sender, EventArgs e)
        {
            Response.Redirect("SecurityLog.aspx");
        }

        protected void btnNearMiss_Click(object sender, EventArgs e)
        {
            Response.Redirect("NearMissSecurity.aspx");
        }
    }
}