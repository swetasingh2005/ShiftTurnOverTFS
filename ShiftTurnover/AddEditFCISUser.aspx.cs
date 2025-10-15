using iTextSharp.text.pdf.codec;
using Microsoft.Office.Interop.Excel;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class AddEditFCISUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                litBack.Text = "<a href='ManageFCISUsers.aspx'>⯇ Back to Users List Page</a>";
                LoadRoles();
                string strUserID = "";int ID = 0;
                if (Request["ID"] != null)//from default.aspx
                {
                    strUserID = Request["id"].ToString();
                    ID=Convert.ToInt16(((string)Request["ID"]).ToString());
                    LoadPerson(ID);
                    LoadRoles(ID);
                }
               

            } //Not Postback
        }
        protected bool ValidateDTRUser(string userid)
        {
            if (!Components.ADGroupHandler.IsAuthenticated(userid))
            {
                return false;
            }
            else { return true; }
        }
        protected bool ValidateADGroup(string userid)
        {
            if (!Components.ADGroupHandler.IsUserInPIUserADGrp(userid))
            {
                return false;
            }
            else { return true; }
        }
        private void EditPerson(int ID)
        {
            DataModuleFCIS dataModule = new DataModuleFCIS();
            if (ID == 0) { dataModule.AddParameter("@action", SqlDbType.VarChar, "insert"); }
            else { dataModule.AddParameter("@action", SqlDbType.VarChar, "update"); }
            dataModule.AddParameter("@personid", SqlDbType.Int, ID);
            try {
                if (txtNIHID.Text == "")
                {
                    dataModule.AddParameter("@nihid", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@nihid", SqlDbType.NVarChar, txtNIHID.Text);
                }
                if (txtUserID.Text == "")
                {
                    dataModule.AddParameter("@userid", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@userid", SqlDbType.NVarChar, txtUserID.Text);
                }
                if (txtLastName.Text == "")
                {
                    dataModule.AddParameter("@lastname", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@lastname", SqlDbType.NVarChar, txtLastName.Text);
                }
                if (txtFirstName.Text == "")
                {
                    dataModule.AddParameter("@firstname", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@firstname", SqlDbType.NVarChar, txtFirstName.Text);
                }
                if (txtMName.Text == "")
                {
                    dataModule.AddParameter("@miname", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@miname", SqlDbType.NVarChar, txtMName.Text);
                }
            
               
                if (txtEmail.Text == "")
                {
                    dataModule.AddParameter("@email", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@email", SqlDbType.NVarChar, txtEmail.Text);
                }
                if (txtPhone.Text == "")
                {
                    dataModule.AddParameter("@phone", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@phone", SqlDbType.NVarChar, txtPhone.Text);
                }
                if (txtIC.Text == "")
                {
                    dataModule.AddParameter("@ic", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@ic", SqlDbType.NVarChar, txtIC.Text);
                }
                if (txtOrg.Text == "")
                {
                    dataModule.AddParameter("@org", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@org", SqlDbType.NVarChar, txtOrg.Text);
                }
                if (txtOrgAbbr.Text == "")
                {
                    dataModule.AddParameter("@orgAbbr", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@orgAbbr", SqlDbType.NVarChar, txtOrgAbbr.Text);
                }
                if (txtTitle.Text == "")
                {
                    dataModule.AddParameter("@title", SqlDbType.NVarChar, DBNull.Value);
                }
                else
                {
                    dataModule.AddParameter("@title", SqlDbType.NVarChar, txtTitle.Text);
                }
                if (chkActive.Checked)
                {
                    dataModule.AddParameter("@active", SqlDbType.Int, 1);
                }
                else
                {
                    dataModule.AddParameter("@active", SqlDbType.NVarChar, 0);
                }
                dataModule.AddParameter("@comments", SqlDbType.NVarChar, "");
                DataSet _ds = dataModule.GetDataSet("editPerson");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(Convert.ToInt32(ViewState["IncidentID"]), "AddIncident.EditIncident", "editincident", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
}
        private void EditRoles(int ID)
        {
            DataModuleFCIS dataModule = new DataModuleFCIS();
            dataModule.AddParameter("@action", SqlDbType.VarChar, "update");
            dataModule.AddParameter("@personid", SqlDbType.Int, ID);
            try
            {
                foreach (System.Web.UI.WebControls.ListItem item in chkRoles.Items)
                {
                    if (item.Selected)
                    {
                        dataModule.AddParameter("@personid", SqlDbType.Int, ID);
                        DataSet _ds = dataModule.GetDataSet("editPersonRoles");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(Convert.ToInt32(ViewState["IncidentID"]), "AddIncident.EditIncident", "editincident", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        private void LoadPerson(int ID)
        {
            DataModuleFCIS _dm = new DataModuleFCIS();
            _dm.AddParameter("@ID", SqlDbType.Int, ID);string Status = "InActive";
            DataSet ds = _dm.GetDataSet("SelectPersonByID");
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    if (ds.Tables[0].Rows[0]["LastName"] != null && ds.Tables[0].Rows[0]["LastName"].ToString().Length > 0)
                    { txtLastName.Text = ds.Tables[0].Rows[0]["LastName"].ToString(); }
                    if (ds.Tables[0].Rows[0]["FirstName"] != null && ds.Tables[0].Rows[0]["FirstName"].ToString().Length > 0)
                    { txtFirstName.Text = ds.Tables[0].Rows[0]["FirstName"].ToString(); }
                    if (ds.Tables[0].Rows[0]["MIName"] != null && ds.Tables[0].Rows[0]["MIName"].ToString().Length > 0)
                    { txtMName.Text = ds.Tables[0].Rows[0]["MIName"].ToString(); }
                    if (ds.Tables[0].Rows[0]["UserID"] != null && ds.Tables[0].Rows[0]["UserID"].ToString().Length > 0)
                    { txtUserID.Text = ds.Tables[0].Rows[0]["UserID"].ToString(); }
                    if (ds.Tables[0].Rows[0]["NIHID"] != null && ds.Tables[0].Rows[0]["NIHID"].ToString().Length > 0)
                    { txtNIHID.Text = ds.Tables[0].Rows[0]["NIHID"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Email"] != null && ds.Tables[0].Rows[0]["Email"].ToString().Length > 0)
                    { txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Phone"] != null && ds.Tables[0].Rows[0]["Phone"].ToString().Length > 0)
                    { txtPhone.Text = ds.Tables[0].Rows[0]["Phone"].ToString(); }
                    if (ds.Tables[0].Rows[0]["IC"] != null && ds.Tables[0].Rows[0]["IC"].ToString().Length > 0)
                    { txtIC.Text = ds.Tables[0].Rows[0]["IC"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Org"] != null && ds.Tables[0].Rows[0]["Org"].ToString().Length > 0)
                    { txtOrg.Text = ds.Tables[0].Rows[0]["Org"].ToString(); }
                    if (ds.Tables[0].Rows[0]["OrgAbbr"] != null && ds.Tables[0].Rows[0]["OrgAbbr"].ToString().Length > 0)
                    { txtOrgAbbr.Text = ds.Tables[0].Rows[0]["OrgAbbr"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Title"] != null && ds.Tables[0].Rows[0]["Title"].ToString().Length > 0)
                    { txtTitle.Text = ds.Tables[0].Rows[0]["Title"].ToString(); }
                    if (ds.Tables[0].Rows[0]["Active"] != null && ds.Tables[0].Rows[0]["Active"].ToString().Length > 0)
                    {
                        if (ds.Tables[0].Rows[0]["Active"].ToString().Equals("1"))
                        {
                            chkActive.Checked = true; Status = "Active ";
                        }
                        
                    }
                    if (ValidateDTRUser(txtUserID.Text)){ lblDTRUser.Text = "Valid DTR User"; }else { lblDTRUser.Text = "Not in DTR  "; }
                    if (ValidateADGroup(txtUserID.Text)) { lblPIUSer.Text = " 'PIUsers' AD Group User"; } else { lblPIUSer.Text = "Not in 'PIUsers' AD Group "; }
                    lblTitle.Text="Edit " + Status + "   User    " + txtLastName.Text + ", " + txtFirstName.Text + "(" + ID  + ")";
                }
                else { lblTitle.Text = "Add New User"; }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "AddEditUser.EditPerson", "SelectPersonByID", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        } //LoadRoles
        private void LoadRoles()
        {
            DataModuleFCIS _dm = new DataModuleFCIS();
            DataSet ds = new DataSet();
            _dm.AddParameter("@ID", SqlDbType.Int, 0);
            ds = _dm.GetDataSet("SelectRoles");

            if (ds != null && ds.Tables[0] != null)
            {
               
                chkRoles.DataSource = ds;
                chkRoles.DataTextField = "role";
                chkRoles.DataValueField = "roleid";
                chkRoles.DataBind();
            }

        } //LoadRoles
        private void LoadRoles(int ID)
        {
            DataModuleFCIS _dm = new DataModuleFCIS();

            _dm.AddParameter("@ID", SqlDbType.Int, ID, 0);

            DataSet ds = new DataSet();
            ds = _dm.GetDataSet("SelectRoles");

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (System.Web.UI.WebControls.ListItem item in chkRoles.Items)
                    {
                        string text = item.Text;
                        string value = item.Value;
                        DataRow[] rows = ds.Tables[0].Select("roleid='" + value + "'");
                        if (rows.Length > 0)
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        } //LoadRoles
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageFCISUsers.aspx");
        }
        
        protected bool ValidateSTOUser(string userid)
        {
            bool UserExists = false;
            DataModuleFCIS _dm = new DataModuleFCIS();
            _dm.AddParameter("@userid", SqlDbType.VarChar, userid);
            DataSet ds = _dm.GetDataSet("SelectPersonByUserID");
            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    UserExists=true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "AddEditUser.ValidateSTOUser", "SelectPersonByUserID", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            return UserExists;
        }

            protected void btnResolve_Click(object sender, EventArgs e)
        {

        }
       
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Request["ID"] != null)//from default.aspx
            {
                int ID = 0;
                ID=Convert.ToInt16(((string)Request["ID"]).ToString());

                if (ID > 0)
                {
                    EditPerson(ID);
                    lblConfirm.Text = "User  "+ txtUserID.Text + " successfully updated.";
                }
                else
                {
                    if (!ValidateADGroup(txtUserID.Text))
                    {
                        lblConfirm.Text = "User  "+ txtUserID.Text + " is not in PIUsers AD Group. Please open a ticket to add or contact DTR EFAM IT group for help.";
                    }
                    else if (ValidateSTOUser(txtUserID.Text))
                    {
                        lblConfirm.Text = "User  "+ txtUserID.Text + " already exists in Shift Turnover database. Please go back and select User from the list.";
                        
                    }
                    else
                    {
                        EditPerson(ID);
                        lblConfirm.Text = "User "+ txtUserID.Text + " successfully added to the Shift Turnover database.";
                    }
                }
            }
        }

        protected void btnEditRoles_Click(object sender, EventArgs e)
        {

        }
    }
}