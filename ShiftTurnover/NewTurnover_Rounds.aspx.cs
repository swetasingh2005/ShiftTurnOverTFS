using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

using ShiftTurnover.Components;
using RestSharp;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.Xml;
using System.IO;

namespace ShiftTurnover
{
    public partial class NewTurnover_Rounds : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

        }
        private void MakePageReadOnly(int CurrentShiftID)
        {
            if (Common.isCurrentShiftSubmitted(CurrentShiftID))
            {
                //btnSave.Enabled = false;
                //btnNext.Enabled = false;
                //btnPre.Enabled = false;
            }
            else
            {
                //btnSave.Enabled = true;
                //btnNext.Enabled = true;
                //btnPre.Enabled = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["shiftid"] == null)
            {
                //Response.Redirect("");
                Session["shiftid"] = "0";
            }
            else
            {
                string _ShiftID = "";
                _ShiftID = Session["shiftid"].ToString();
                ViewState["ShiftID"] = _ShiftID;
            }//end if

            if (!Page.IsPostBack)
            {
                int CurrentShiftID = Common.getCurrentShift();
                //MakePageReadOnly(CurrentShiftID);
                lblMShift.Text = "Current Shift: " + Common.getShiftLabel(CurrentShiftID);

                DateTime dtCurrent = DateTime.Now;
                DateTime dt630am = Convert.ToDateTime("06:30:00 AM");
                DateTime dt630pm = Convert.ToDateTime("06:30:00 PM");
                DateTime dt6am = Convert.ToDateTime("06:00:00 AM");
                DateTime dt6pm = Convert.ToDateTime("06:00:00 PM");
                int intComp630am = DateTime.Compare(dtCurrent, dt630am); //if now<630am then <0, if now=630am then =0, if now>630am then >0
                int intComp630pm = DateTime.Compare(dtCurrent, dt630pm);
                int intComp6pm = DateTime.Compare(dtCurrent, dt6pm);
                int intComp6am = DateTime.Compare(dtCurrent, dt6am);

                if (intComp6pm > 0 && intComp630pm < 0)
                {
                    lblMShift.Text += "<br />NOTE: You are Viewing/Editing Previous Shift. New Shift will be available at 6.30 pm.";
                    lblMShift.ForeColor = Color.Red;
                }
                if (intComp6am > 0 && intComp630am < 0)
                {
                    lblMShift.Text += "<br />NOTE: You are Viewing/Editing Previous Shift. New Shift will be available at 6.30 am.";
                    lblMShift.ForeColor = Color.Red;
                }

                LoadLOTODataGrid();

            }//postback
        }
        protected string CreateServiceRequest1()
        {
            string content = "";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string MaxServer = ConfigurationManager.AppSettings["MaxServer"];
            string Cookie = ConfigurationManager.AppSettings["Cookie"];
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            var client = new RestClient("https://nihefam.usgov.maximo.com/maxrest/oslc/os/MXASSETMETER?lean=1&tlrange=-4h&tlattribute=assetmeter.lastreadingdate&oslc.select=assetnum,description,assetmeter.lastreadingdate,assetmeter.lastreading,assetmeter.metername,assetmeter.meter.description&_format=xml");
             client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("apikey", "61ir8aljid765uoj1iaadsq8dft6rkk8lfuatfki");
            request.AddHeader("Cookie", "JSESSIONID=0000KlVoMajAqeJrXGRD32Yui8e:1ff38hv6t");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);
            content = response.Content;
            content = content.Substring(content.IndexOf("<MXASSETMETERSet>"));
            content = content.Substring(0, content.IndexOf("</MXASSETMETERSet>") + 18);

            return content;
        }
        protected string CreateServiceRequest()
        {
            string content = "";
             
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string MaxServer = ConfigurationManager.AppSettings["MaxServer"];
            string Cookie = ConfigurationManager.AppSettings["Cookie"];
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
           
                 
            var client = new RestClient(MaxServer + "/maxrest/oslc/os/MXWOTRACK?lean=1&_format=xml" +
                "&oslc.select=wonum,nih_lototracking.lockboxnum,nih_lototracking.lotonum,tagoutenabled.tagoutdescription," +
                "nih_lototracking.statuschangedby,nih_lototracking.statuschangedbydate&oslc.where=status=\"LOTO\" and nih_lototracking.status in [\"LOTO\",\"TEST\"]");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("apikey", "61ir8aljid765uoj1iaadsq8dft6rkk8lfuatfki");
            request.AddHeader("Cookie", "JSESSIONID=0000KlVoMajAqeJrXGRD32Yui8e:1ff38hv6t");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);
            // Console.WriteLine(response.Content);

            content = response.Content ;
             content = content.Substring(content.IndexOf("<MXWOTRACKSet>"));  
            content = content.Substring(0, content.IndexOf("</MXWOTRACKSet>") + 15);
            
            return content;

        }
         

        protected void LoadLOTODataGrid()
        {
            string ds  = CreateServiceRequest1();

            
               //DataSet ds = _dm.GetDataSet("SelectMaximoLOTOIndex");
               StringReader theReader = new StringReader(ds);
            DataSet theDataSet = new DataSet();  
            theDataSet.ReadXml(theReader);
            try
            {
                if (theDataSet != null)
            {
                if (theDataSet.Tables[0].Rows.Count > 0)
                {
                       
                       
                                lblLOTO.Visible = false;
                                pnlLOTO.Visible = true;

                                grLOTO.DataSource = theDataSet.Tables[0];
                                grLOTO.DataBind();

                 }
                 else
                 {

                        lblLOTO.Visible = true;
                        pnlLOTO.Visible = false;
                    }
                         
                     
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_LOTO.LoadLOTODataGrid", "SelectMaximoLOTOIndex", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
            }


        }
       
        public static string GetUtcFormattedDate(DateTime date)
        {
            return date.ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt");
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                string btnID = Convert.ToString(btn.ID);
                //SaveStatus();
                SetTab(btnID);
            }
        }

        public void SetTab(string btn)
        {
            switch (btn)
            {
                case "btnBack": //
                    //Response.Redirect("NewTurnover_ROBlrStatus.aspx");
                    break;
                //case "btnSave":
                //    Response.Redirect("NewTurnover_WO.aspx");
                //    break;
                case "btnNext":
                   // Response.Redirect("NewTurnover_CriticalAlm.aspx");
                    break;
                default:

                    break;
            }
        }

        protected void grLOTO_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblDate = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblDate");
                string getValue = Convert.ToString(e.Item.Cells[5].Text);
                if (getValue.Length > 0)
                {
                    DateTime dt = Convert.ToDateTime(getValue);
                    e.Item.Cells[4].Text = GetUtcFormattedDate(dt);
                }
            }
             
        }
    }
}