using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace ShiftTurnover
{
    public partial class NewTurnover : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
           
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
                MakePageReadOnly(CurrentShiftID);
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

                for (int i = 3; i <= 11; i++)
                    {
                        LoadLookup(i);
                    }
                    LoadComment();
                    LoadForm();
                
            }//postback
        }

        private void MakePageReadOnly(int CurrentShiftID)
        {
            if (Common.isCurrentShiftSubmitted(CurrentShiftID))
            {
                btnSave.Enabled = false;
                btnNext.Enabled = false;
                btnBack.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                btnNext.Enabled = true;
                btnBack.Enabled = true;
            }
        }

        private void LoadShiftLabel()
        {
            if (Session["shiftid"] != null)
            {
                int shiftid = Convert.ToInt32(Session["shiftid"].ToString());
                DataSet ds;
                //Populate litShift
                DataModule dataModule = new DataModule();
                try
                {
                    dataModule.AddParameter("@shiftid", SqlDbType.Int, shiftid);
                    ds = dataModule.GetDataSet("SelectShiftDateTime");
                    if (ds != null)
                    {
                        if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                        {
                            lblMShift.Text = "Current Shift:<strong> " + ds.Tables[0].Rows[0]["Shift"].ToString() + "</strong>";
                        }

                    }

                    //litShift.Text = "Current Shift: " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                }
                catch (System.Exception ex)
                {
                    ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover_Review.LoadShiftLabel", "SelectShiftDateTime", ex);
                    Response.Redirect("CustomErrorPage.aspx");
                }

            }
        }

        protected void LoadComment()
        {
           
            try
            {
              
                DataModule _dm = new DataModule();
                DataSet ds;
            _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
            ds = _dm.GetDataSet("SelectReportDetails");
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                            txtShiftWorkerInfo.Text = row["ShiftWorkerInfo"].ToString();  
                           
                        }
                }
            }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadComment", "SelectReportDetails", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            
        }
        private void LoadForm()
        {
            try
            {
                DataModule _dm = new DataModule();
                DataSet ds;
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                ds = _dm.GetDataSet("SelectShiftworkers");
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            switch (row["RoleID"])
                            {
                                case 3: // Chief 
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlCrewChief.SelectedValue = row["personroleid"].ToString(); trSecCrewChief.Visible = false;  
                                            break;
                                        case "N":
                                            ddlSecCrewChief.SelectedValue = row["personroleid"].ToString(); trSecCrewChief.Visible = true;  btnCrewChief.Visible = false;
                                            break;
                                    }
                                    break;
                                case 4: // Chiller Operator 
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlChillerOperator.SelectedValue = row["personroleid"].ToString(); trSecChillerOperator.Visible = false;  
                                            break;
                                        case "N":
                                            ddlSecChillerOperator.SelectedValue = row["personroleid"].ToString(); trSecChillerOperator.Visible = true;  btnChillerOperator.Visible = false;
                                            break;

                                    }
                                    break;
                                case 5: // Boiler Operator   
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlBoilerOperator.SelectedValue = row["personroleid"].ToString(); trSecBoilerOperator.Visible = false;  
                                            break;
                                        case "N":
                                            ddlSecBoilerOperator.SelectedValue = row["personroleid"].ToString(); trSecBoilerOperator.Visible = true;  btnBoilerOperator.Visible = false;
                                            break;

                                    }

                                    break;


                                case 6: // Aux Operator
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlAuxOperator.SelectedValue = row["personroleid"].ToString(); trSecAuxOperator.Visible = false;  
                                            break;
                                        case "N":
                                            ddlSecAuxOperator.SelectedValue = row["personroleid"].ToString(); trSecAuxOperator.Visible = true; btnAuxOperator.Visible = false;
                                            break;
                                    }

                                    break;
                                case 7: // Electrician
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlElectrician.SelectedValue = row["personroleid"].ToString(); trSecElectrician.Visible = false;  
                                            break;
                                        case "N":
                                            ddlSecElectrician.SelectedValue = row["personroleid"].ToString(); trSecElectrician.Visible = true; btnElectrician.Visible = false;
                                            break;
                                    }

                                    break;

                                case 8: // Cogen Operator
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlCogenOperator.SelectedValue = row["personroleid"].ToString(); trSecCogenOperator.Visible = false;
                                            break;
                                        case "N":
                                            ddlSecCogenOperator.SelectedValue = row["personroleid"].ToString(); trSecCogenOperator.Visible = true; btnCogenOperator.Visible = false;
                                            break;
                                    }

                                    break;
                                case 10: // Shift Supervisor
                                    switch (row["PrimaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            ddlShiftSuper.SelectedValue = row["personroleid"].ToString(); trSecShiftSuper.Visible = false;
                                            break;
                                        case "N":
                                            ddlSecShiftSuper.SelectedValue = row["personroleid"].ToString(); trSecShiftSuper.Visible = true; btnShiftSuper.Visible = false;
                                            break;
                                    }

                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadForm", "SelectShiftworkers", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        private void LoadLookup(int RoleID)
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@roleid", SqlDbType.Int, RoleID);
                DataSet ds = _dm.GetDataSet("SelectPersonList");
                
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        switch (RoleID)
                        {
                            case 3: // Chief 
                                ddlCrewChief.DataSource = ds;
                                ddlCrewChief.DataTextField = "personname";
                                ddlCrewChief.DataValueField = "personroleid";
                                ddlCrewChief.DataBind();
                                ddlCrewChief.Items.Insert(0, "-SELECT-");
                                ddlCrewChief.Items.Add(new ListItem("Other (Name in Comments Below)","10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlCrewChief.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecCrewChief.DataSource = ds;
                                ddlSecCrewChief.DataTextField = "personname";
                                ddlSecCrewChief.DataValueField = "personroleid";
                                ddlSecCrewChief.DataBind();
                                ddlSecCrewChief.Items.Insert(0, "-SELECT-");
                                ddlSecCrewChief.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecCrewChief.SelectedValue = Session["personroleid"].ToString();
                                }
                                break;
                            case 5: // Boiler Operator 
                                ddlBoilerOperator.DataSource = ds;
                                ddlBoilerOperator.DataTextField = "personname";
                                ddlBoilerOperator.DataValueField = "personroleid";
                                ddlBoilerOperator.DataBind();
                                ddlBoilerOperator.Items.Insert(0, "-SELECT-");
                                ddlBoilerOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlBoilerOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecBoilerOperator.DataSource = ds;
                                ddlSecBoilerOperator.DataTextField = "personname";
                                ddlSecBoilerOperator.DataValueField = "personroleid";
                                ddlSecBoilerOperator.DataBind();
                                ddlSecBoilerOperator.Items.Insert(0, "-SELECT-");
                                ddlSecBoilerOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecBoilerOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                break;
                            case 6: // Aux Operator   
                                ddlAuxOperator.DataSource = ds;
                                ddlAuxOperator.DataTextField = "personname";
                                ddlAuxOperator.DataValueField = "personroleid";
                                ddlAuxOperator.DataBind();
                                ddlAuxOperator.Items.Insert(0, "-SELECT-");
                                ddlAuxOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlAuxOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecAuxOperator.DataSource = ds;
                                ddlSecAuxOperator.DataTextField = "personname";
                                ddlSecAuxOperator.DataValueField = "personroleid";
                                ddlSecAuxOperator.DataBind();
                                ddlSecAuxOperator.Items.Insert(0, "-SELECT-");
                                ddlSecAuxOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecAuxOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                break;


                            case 4: // Chiller Operator
                                ddlChillerOperator.DataSource = ds;
                                ddlChillerOperator.DataTextField = "personname";
                                ddlChillerOperator.DataValueField = "personroleid";
                                ddlChillerOperator.DataBind();
                                ddlChillerOperator.Items.Insert(0, "-SELECT-");
                                ddlChillerOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlChillerOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecChillerOperator.DataSource = ds;
                                ddlSecChillerOperator.DataTextField = "personname";
                                ddlSecChillerOperator.DataValueField = "personroleid";
                                ddlSecChillerOperator.DataBind();
                                ddlSecChillerOperator.Items.Insert(0, "-SELECT-");
                                ddlSecChillerOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecChillerOperator.SelectedValue = Session["personroleid"].ToString();
                                }

                                break;
                            case 7: // Electrician
                                ddlElectrician.DataSource = ds;
                                ddlElectrician.DataTextField = "personname";
                                ddlElectrician.DataValueField = "personroleid";
                                ddlElectrician.DataBind();
                                ddlElectrician.Items.Insert(0, "-SELECT-");
                                ddlElectrician.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlElectrician.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecElectrician.DataSource = ds;
                                ddlSecElectrician.DataTextField = "personname";
                                ddlSecElectrician.DataValueField = "personroleid";
                                ddlSecElectrician.DataBind();
                                ddlSecElectrician.Items.Insert(0, "-SELECT-");
                                ddlSecElectrician.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecElectrician.SelectedValue = Session["personroleid"].ToString();
                                }

                                break;

                            case 8: // Cogen Operator
                                ddlCogenOperator.DataSource = ds;
                                ddlCogenOperator.DataTextField = "personname";
                                ddlCogenOperator.DataValueField = "personroleid";
                                ddlCogenOperator.DataBind();
                                ddlCogenOperator.Items.Insert(0, "-SELECT-");
                                ddlCogenOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlCogenOperator.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecCogenOperator.DataSource = ds;
                                ddlSecCogenOperator.DataTextField = "personname";
                                ddlSecCogenOperator.DataValueField = "personroleid";
                                ddlSecCogenOperator.DataBind();
                                ddlSecCogenOperator.Items.Insert(0, "-SELECT-");
                                ddlSecCogenOperator.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecCogenOperator.SelectedValue = Session["personroleid"].ToString();
                                }

                                break;
                            case 10: // Shift Supervisor
                                ddlShiftSuper.DataSource = ds;
                                ddlShiftSuper.DataTextField = "personname";
                                ddlShiftSuper.DataValueField = "personroleid";
                                ddlShiftSuper.DataBind();
                                ddlShiftSuper.Items.Insert(0, "-SELECT-");
                                ddlShiftSuper.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlShiftSuper.SelectedValue = Session["personroleid"].ToString();
                                }
                                ddlSecShiftSuper.DataSource = ds;
                                ddlSecShiftSuper.DataTextField = "personname";
                                ddlSecShiftSuper.DataValueField = "personroleid";
                                ddlSecShiftSuper.DataBind();
                                ddlSecShiftSuper.Items.Insert(0, "-SELECT-");
                                ddlSecShiftSuper.Items.Add(new ListItem("Other (Name in Comments Below)", "10000"));
                                if (Session["personroleid"] != null)
                                {
                                    ddlSecShiftSuper.SelectedValue = Session["personroleid"].ToString();
                                }

                                break;

                            default:
                                break;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "NewTurnover.LoadLookup", "SelectPersonList", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn != null)
            {
                string btnID = Convert.ToString(btn.ID);
                SaveReportInfo();
                for (int i = 3; i <= 10; i++)
                {
                    SaveShiftWorkers(i);
                }
             
                SetTab(btnID);
            }
        }
        public void SaveReportInfo( )
        {
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                _dm.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt32(Session["personroleid"]));
                _dm.AddParameter("@ShiftWorkerInfo", SqlDbType.VarChar, txtShiftWorkerInfo.Text);
                _dm.ExecuteCommand("AddEditReportInfo");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(18, "NewTurnOver.SaveReportInfo", "AddEditReportInfo", ex);

            }
        }
        protected void SaveShiftWorkers(int RoleID )
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            try
            { 
            switch (RoleID)
            {
                case 3: // Chief 
                    if (!ddlCrewChief.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlCrewChief.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecCrewChief.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecCrewChief.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                       
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                       
                    break;
                case 4:
                    if (!ddlChillerOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlChillerOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecChillerOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecChillerOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int,0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                  
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    break;
                case 5:
                    if (!ddlBoilerOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlBoilerOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecBoilerOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecBoilerOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    break;

                    
                case 6:
                    if (!ddlAuxOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlAuxOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecAuxOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecAuxOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    break;

                case 7:
                    if (!ddlElectrician.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlElectrician.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecElectrician.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecElectrician.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    break;

                case 8:
                    if (!ddlCogenOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlCogenOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    if (!ddlSecCogenOperator.SelectedValue.Equals("-SELECT-"))
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecCogenOperator.SelectedValue));
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    else
                    {
                        dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                        dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                        dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                        ds = dataModule.GetDataSet("AddEditShiftWorkers");
                    }
                    break;
                    case 10:
                        if (!ddlShiftSuper.SelectedValue.Equals("-SELECT-"))
                        {
                            dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                            dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlShiftSuper.SelectedValue));
                            dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                            dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "Y");
                            ds = dataModule.GetDataSet("AddEditShiftWorkers");
                        }
                        if (!ddlSecShiftSuper.SelectedValue.Equals("-SELECT-"))
                        {
                            dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                            dataModule.AddParameter("@personroleid", SqlDbType.Int, Convert.ToInt16(ddlSecShiftSuper.SelectedValue));
                            dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                            dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                            ds = dataModule.GetDataSet("AddEditShiftWorkers");
                        }
                        else
                        {
                            dataModule.AddParameter("@shiftid", SqlDbType.Int, Convert.ToInt32(Session["shiftid"]));
                            dataModule.AddParameter("@personroleid", SqlDbType.Int, 0);
                            dataModule.AddParameter("@roleid", SqlDbType.Int, RoleID);
                            dataModule.AddParameter("@primaryYN", SqlDbType.VarChar, "N");
                            ds = dataModule.GetDataSet("AddEditShiftWorkers");
                        }
                        break;

                    default:

                    break;
            }
          
                
            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "NewTurnover.SaveShiftWorkers", "AddEditShiftWorkers", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void SetTab(string Status)
        {


            switch (Status)
            {
                case "btnBack": //
                    Response.Redirect("NewTurnover_Acknowledgement.aspx");
                    break;
                case "btnSave":
                     
                    break;
                case "btnNext":
                    Response.Redirect("NewTurnover_CriticalAlm.aspx");
                    break;
                default:

                    break;
            }


        }
        private void SetButtonMode(int mode, ImageButton btn)
        {
            //string uAdd = ResolveUrl("~/Images/Add.png");
            //string uDel = ResolveUrl("~/Images/Delete.png");
            //if (mode == 1)
            //{
               
            //    btn.CommandName = "Delete";
            //    btn.ImageUrl = uDel;
            //}
            //else
            //{
             
            //    btn.CommandName = "Add";
            //    btn.ImageUrl = uAdd;
                
            //}
        }
        protected void btnCrewChief_Click(object sender, ImageClickEventArgs e)
        {
            
                    trSecCrewChief.Visible = true;
                    
                    rfvSecCrewChief.Enabled = true;
                    btnCrewChief.Visible = false;
        }
        protected void btnSecCrewChiefDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecCrewChief.SelectedValue = "-SELECT-";
            trSecCrewChief.Visible = false;
         
            rfvSecCrewChief.Enabled = false;
            btnCrewChief.Visible = true;
        }
        protected void btnChillerOperator_Click(object sender, ImageClickEventArgs e)

        {
        
                    trSecChillerOperator.Visible = true;
                    rfvSecChillerOperator.Enabled = true;
                   
                    btnChillerOperator.Visible = false;

        }
        protected void btnChillerOperatorDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecChillerOperator.SelectedValue = "-SELECT-";
            trSecChillerOperator.Visible = false;
          
            rfvSecChillerOperator.Enabled = false;
            btnChillerOperator.Visible = true;
        }
        protected void btnBoilerOperator_Click(object sender, ImageClickEventArgs e)
        {
            
                    trSecBoilerOperator.Visible = true;
                    rfvSecBoilerOperator.Enabled = true;
                 
                    btnBoilerOperator.Visible = false;
       
        }
        protected void btnBoilerOperatorDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecBoilerOperator.SelectedValue = "-SELECT-";
            trSecBoilerOperator.Visible = false;
           
            rfvSecBoilerOperator.Enabled = false;
            btnBoilerOperator.Visible = true;
        }

        protected void btnAuxOperator_Click(object sender, ImageClickEventArgs e)
        {
            
                    trSecAuxOperator.Visible = true;
                  
                    rfvSecAuxOperator.Enabled = true;
                    btnAuxOperator.Visible = false;
        }

        protected void btnAuxOperatorDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecAuxOperator.SelectedValue = "-SELECT-";
            trSecAuxOperator.Visible = false;

            rfvSecAuxOperator.Enabled = false;
            btnAuxOperator.Visible = true;
        }

        protected void btnElectrician_Click(object sender, ImageClickEventArgs e)
        {
             
                    trSecElectrician.Visible = true;
                  
                    btnElectrician.Visible = false;
                    rfvSecElectrician.Enabled = true;
                 
        }

        protected void btnElectricianDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecElectrician.SelectedValue = "-SELECT-";
            trSecElectrician.Visible = false;
           
            rfvSecElectrician.Enabled = false;
            btnElectrician.Visible = true;
        }

        protected void btnCogenOperator_Click(object sender, ImageClickEventArgs e)
        {

            trSecCogenOperator.Visible = true;
            rfvSecCogenOperator.Enabled = true;

            btnCogenOperator.Visible = false;

        }
        protected void btnCogenOperatorDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecCogenOperator.SelectedValue = "-SELECT-";
            trSecCogenOperator.Visible = false;

            rfvSecCogenOperator.Enabled = false;
            btnCogenOperator.Visible = true;
        }

        protected void btnShiftSuper_Click(object sender, ImageClickEventArgs e)
        {
            trSecShiftSuper.Visible = true;
            rfvSecShiftSuper.Enabled = true;

            btnShiftSuper.Visible = false;
        }

        protected void btnSecShiftSuperDel_Click(object sender, ImageClickEventArgs e)
        {
            ddlSecShiftSuper.SelectedValue = "-SELECT-";
            trSecShiftSuper.Visible = false;

            rfvSecShiftSuper.Enabled = false;
            btnShiftSuper.Visible = true;
        }
    }
}