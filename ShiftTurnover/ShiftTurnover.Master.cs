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
                    if (url.Contains("NewTurnover"))
                    {
                        SetSecondMenu(url);
                        if (url.Contains("Status"))
                        {
                            pnlthirdMenu.Visible = true;
                            //pnlthirdWOMenu.Visible = false;
                            pnlthirdAlmMenu.Visible = false;
                            SetThirdMenu(url);
                        }
                        else if (url.Contains("_CriticalAlm"))

                        {
                            pnlsecondMenu.Visible = true;
                            pnlthirdMenu.Visible = false;
                            //pnlthirdWOMenu.Visible = false;
                            pnlthirdAlmMenu.Visible = true;
                            SetThirdAlmMenu(url);
                        }
                        //else if (url.Contains("_WO"))
                        //{
                        //    pnlthirdMenu.Visible = false;
                        //    //pnlthirdWOMenu.Visible = true;
                        //    pnlthirdAlmMenu.Visible = false;
                        //    pnlsecondMenu.Visible = true;
                        //    SetThirdWOMenu(url);
                        //}

                        else
                        {
                            pnlthirdMenu.Visible = false;
                            //pnlthirdWOMenu.Visible = false;
                            pnlthirdAlmMenu.Visible = false;
                        }
                    }
                    else
                    {
                        pnlsecondMenu.Visible = false;
                    }

                    //if (url.Contains("SubmissionReport.aspx"))
                    if (new[] {"Manager"}.Contains(Session["personrole"].ToString()))
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
                //pnlthirdWOMenu.Visible = false;

                //if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("Default.aspx"))
                //{
                //    Response.Redirect("Default.aspx");
                //}




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
            else if (url.Contains("WaterTreatmentLog")) { btn = "WaterTreatmentLog"; }
            else { btn = "MainPage"; }

            switch (btn)
            {
                case "CheckStatus":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    pnlFirstMenu.Visible = false;
                    break;
                case "MainPage":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "active";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    break;
                case "EquipmentStatus":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "active";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    break;
                case "NewTurnover":
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "active";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    pnlsecondMenu.Visible = true;
                    pnlthirdMenu.Visible = false;
                    break;

                case "OldTurnover":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    break;
                case "LiveLogSearch":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "active";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    break;
                case "EngineeringLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "active";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    break;
                case "HVGLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "active";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    break;
                case "FuelLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "active";
                    break;
                case "WaterTreatmentLog":
                    pnlsecondMenu.Visible = false;
                    pnlthirdMenu.Visible = false;
                    //pnlthirdWOMenu.Visible = false;
                    aMainPage.Attributes["class"] = "inactive";
                    aNewTurnover.Attributes["class"] = "inactive";
                    aOldTurnover.Attributes["class"] = "inactive";
                    aEngineeringLog.Attributes["class"] = "inactive";
                    aEquipmentStatus.Attributes["class"] = "inactive";
                    aLiveLogSearch.Attributes["class"] = "inactive";
                    aHVGLog.Attributes["class"] = "inactive";
                    aFuelLog.Attributes["class"] = "inactive";
                    aWaterTreatmentLog.Attributes["class"] = "active";
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
            if (url.Contains("Status")) { subMenubtn = "Status"; }
            else if (url.Contains("CriticalAlm")) { subMenubtn = "CriticalAlm"; }
            //else if (url.Contains("WO")) { subMenubtn = "WO"; }
            else if (url.Contains("LOTO")) { subMenubtn = "LOTO"; }
            else if (url.Contains("Review")) { subMenubtn = "Review"; }
            else if (url.Contains("Acknowledgement")) { subMenubtn = "Acknowledgement"; }
            else { subMenubtn = "ShiftW"; }
            switch (subMenubtn)
            {
                case "ShiftW":
                    btnShiftWorker.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnReview.BackColor = Color.Black;
                    btnStatus.BackColor = Color.Black;
                    btnLOTO.BackColor = Color.Black;
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Status":
                    btnStatus.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnReview.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;
                    btnLOTO.BackColor = Color.Black;
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                //case "WO":
                //    btnWO.BackColor = ColorTranslator.FromHtml("#b3112c");
                //    btnReview.BackColor = Color.Black;
                //    btnStatus.BackColor = Color.Black;
                //    btnShiftWorker.BackColor = Color.Black;
                //    btnAlarms.BackColor = Color.Black;
                //    btnAcknowledgement.BackColor = Color.Black;
                //    break;
                case "LOTO":
                    btnLOTO.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnReview.BackColor = Color.Black;
                    btnStatus.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "CriticalAlm":
                    btnAlarms.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnLOTO.BackColor = Color.Black;
                    btnReview.BackColor = Color.Black;
                    btnStatus.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Review":
                    btnReview.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnShiftWorker.BackColor = Color.Black;
                    btnStatus.BackColor = Color.Black;
                    btnLOTO.BackColor = Color.Black;
                    btnAlarms.BackColor = Color.Black;
                    btnAcknowledgement.BackColor = Color.Black;
                    break;
                case "Acknowledgement":
                    btnReview.BackColor = Color.Black;
                    btnShiftWorker.BackColor = Color.Black;
                    btnStatus.BackColor = Color.Black;
                    btnLOTO.BackColor = Color.Black;
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
            if (url.Contains("ChlStatus")) { statusMenubtn = "ChlStatus"; }
            else if (url.Contains("CTStatus")) { statusMenubtn = "CTStatus"; }
            else if (url.Contains("FreeCoStatus")) { statusMenubtn = "FreeCoStatus"; }
            else if (url.Contains("ROBlrStatus")) { statusMenubtn = "ROBlrStatus"; }
            else if (url.Contains("ChlPmpStatus")) { statusMenubtn = "ChlPmpStatus"; }
            else { statusMenubtn = "BlrStatus"; }
            switch (statusMenubtn)
            {
                case "ChlStatus":
                    btnChl.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnPump.BackColor = Color.Black;
                    btnCT.BackColor = Color.Black;
                    btnFreeCo.BackColor = Color.Black;
                    btnBlr.BackColor = Color.Black;
                    btnRO.BackColor = Color.Black;
                    break;
                case "ChlPmpStatus":
                    btnChl.BackColor = Color.Black;
                    btnPump.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnCT.BackColor = Color.Black;
                    btnFreeCo.BackColor = Color.Black;
                    btnBlr.BackColor = Color.Black;
                    btnRO.BackColor = Color.Black;
                    break;
                case "CTStatus":
                    btnChl.BackColor = Color.Black;
                    btnPump.BackColor = Color.Black;
                    btnCT.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnFreeCo.BackColor = Color.Black;
                    btnBlr.BackColor = Color.Black;
                    btnRO.BackColor = Color.Black;
                    break;
                case "FreeCoStatus":
                    btnChl.BackColor = Color.Black;
                    btnPump.BackColor = Color.Black;
                    btnCT.BackColor = Color.Black;
                    btnFreeCo.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnBlr.BackColor = Color.Black;
                    btnRO.BackColor = Color.Black;
                    break;
                case "ROBlrStatus":
                    btnChl.BackColor = Color.Black;
                    btnPump.BackColor = Color.Black;
                    btnCT.BackColor = Color.Black;
                    btnFreeCo.BackColor = Color.Black;
                    btnBlr.BackColor = Color.Black;
                    btnRO.BackColor = ColorTranslator.FromHtml("#b3112c");
                    break;

                case "BlrStatus":
                    btnBlr.BackColor = ColorTranslator.FromHtml("#b3112c");
                    btnPump.BackColor = Color.Black;
                    btnCT.BackColor = Color.Black;
                    btnFreeCo.BackColor = Color.Black;
                    btnChl.BackColor = Color.Black;
                    btnRO.BackColor = Color.Black;
                    break;
                default:
                    break;
            }

        }
        //protected void SetThirdWOMenu(string url)
        //{
        //    string statusMenubtn = "";
        //    if (url.Contains("NewTurnover_WO_MaintSchedWO")) { statusMenubtn = "NewTurnover_WO_MaintSchedWO"; }
        //    else if (url.Contains("NewTurnover_WO_WCWO")) { statusMenubtn = "NewTurnover_WO_WCWO"; }
        //    else { statusMenubtn = "NewTurnover_WO"; }
        //    switch (statusMenubtn)
        //    {
        //        case "NewTurnover_WO":
        //            //btnMaintWO.BackColor = ColorTranslator.FromHtml("#b3112c");
        //            //btnMaintSchedWO.BackColor = Color.Black;

        //            break;
        //        case "NewTurnover_WO_MaintSchedWO":
        //            //btnMaintWO.BackColor = Color.Black;
        //            //btnMaintSchedWO.BackColor = ColorTranslator.FromHtml("#b3112c");

        //            break;

        //        default:
        //            break;
        //    }

        //}
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
        //protected void btnMaintWO_Click(object sender, EventArgs e)
        //{
        //    Button btn = (Button)sender;
        //    string url = "NewTurnover_WO.aspx";
        //    if (btn != null)
        //    {
        //        switch (btn.ID)
        //        {
        //            case "btnMaintWO":
        //                url = "NewTurnover_WO.aspx";
        //                break;
        //            case "btnMaintSchedWO":
        //                url = "NewTurnover_WO_MaintSchedWO.aspx";
        //                break;


        //            case "btnWCWO":
        //                url = "NewTurnover_WO_WCWO.aspx";
        //                break;
        //            case "btnWChemAlam":
        //                url = "NewTurnover_WO_WChemAlam.aspx";
        //                break;
        //            default:
        //                break;
        //        }


        //    }
        //    Response.Redirect(url);

        //}
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
    }
}