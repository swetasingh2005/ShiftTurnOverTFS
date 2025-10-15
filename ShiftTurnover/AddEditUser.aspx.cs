 
using iTextSharp.text.pdf.codec;
using iTextSharp.text.pdf.spatial.units;
using Microsoft.Office.Interop.Excel;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static ShiftTurnover.Components.ADGroupHandler;

namespace ShiftTurnover
{
    public partial class AddEditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                litBack.Text = "<a href='ManageUser.aspx'>⯇ Back to Users List Page</a>";
                LoadRoles();
                string strUserID = ""; int ID = 0;
                if (Request["ID"] != null)//from default.aspx
                {
                    strUserID = Request["id"].ToString();
                    hfPersonId.Value = strUserID;
                    ID = Convert.ToInt16(((string)Request["ID"]).ToString());
                    if (ID > 0)
                    {
                        LoadPerson(ID);
                        LoadRoles(ID);
                        btnCheckPIUser.Visible = false;
                        tblValidation.Visible = false;
                    }
                    else
                    {
                        lblTitle.Text = "Add New User";
                        btnCheckPIUser.Visible = true;
                        tblValidation.Visible = true;
                    }
                }

            } //Not Postback
        }
        protected bool ValidateADGroup(string userid)
        {
            if (!Components.ADGroupHandler.IsUserInPIUserADGrp(userid))
            {
                return false;
            }
            else { return true; }
        }

        protected bool ValidateDTRUser(string userid)
        {
            if (!Components.ADGroupHandler.IsAuthenticated(userid))
            {
                return false;
            }
            else { return true; }
        }
        protected void btnResolve_Click(object sender, EventArgs e)
        {

        }

        private void EditRoles()
        {
            int personId = Convert.ToInt32(hfPersonId.Value);
            DataModule dataModule = new DataModule();
           
            try
            {
                // Step 1: Insert roles that are checked but not already in DB
                foreach (ListItem item in chkRoles.Items)
                {
                    if (item.Selected)
                    {
                        int roleId = Convert.ToInt32(item.Value);  
                        dataModule.AddParameter("@personid", SqlDbType.Int, personId);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, roleId);
                        dataModule.AddParameter("@action", SqlDbType.VarChar, "update");
                        DataSet _ds = dataModule.GetDataSet("editpersonrole1");
                       
                    }
                }

                // Step 2: Delete roles that are unchecked
                foreach (ListItem item in chkRoles.Items)
                {
                    if (!item.Selected)
                    {
                        int roleId = Convert.ToInt32(item.Value);
                        dataModule.AddParameter("@personid", SqlDbType.Int, personId);
                        dataModule.AddParameter("@roleid", SqlDbType.Int, roleId);
                        dataModule.AddParameter("@action", SqlDbType.VarChar, "delete");
                        DataSet _ds = dataModule.GetDataSet("editpersonrole1");
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(Convert.ToInt32(ViewState["IncidentID"]), "AddIncident.EditIncident", "editincident", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        private void EditPerson(int ID)
        {
            DataModule dataModule = new DataModule();
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
            private void LoadPerson(int ID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@ID", SqlDbType.Int, ID);
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
                            chkActive.Checked = true;
                        }
                        
                    }
                    lblTitle.Text = "Edit User Information For: " + txtFirstName.Text + " " + txtLastName.Text;
                    
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], "AddEditUser.EditPerson", "SelectPersonByID", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }

        } //LoadRoles
        private void LoadRoles()
        {
            DataModule _dm = new DataModule();
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
            DataModule _dm = new DataModule();
            
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
            Response.Redirect("ManageUser.aspx");
        }
        
        protected bool ValidateSTOUser(string userid)
        {
            bool UserExists = false;
            DataModule _dm = new DataModule();
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

            
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Request["ID"] != null)
            {
                string msg = "User  " + txtUserID.Text ;
                int ID = 0;
                ID=Convert.ToInt16(((string)Request["ID"]).ToString());

                if (ID > 0)
                {
                    EditPerson(ID);
                    msg +=  " successfully updated.";
                }
                else
                {
                    if (!ValidateDTRUser(txtUserID.Text))
                    {
                        msg = " is not a DTR User.";
                    }
                    else if (!ValidateADGroup(txtUserID.Text))
                    {
                        msg  += "< /br>   not in PIUsers AD Group. Please open a <a href='https://myitsm.nih.gov/nih_sd?id=service_request_catalog' target='_blank'>ticket </a> to add or contact <a href='mailto:ORFDTREFAMIT@mail.nih.gov?subject=PIUsers group access'>  DTR EFAM IT";
                    }
                   
                    else if (ValidateSTOUser(txtUserID.Text))
                    {
                        msg += " already exists in Shift Turnover database. Please go back and select User from the list to edit User Information.";
                        
                    }
                    else
                    {
                        EditPerson(ID);
                        msg = " successfully added to the Shift Turnover database. Please assign role(s) now.";
                    }
                }
                lblConfirm.Text = msg;
            }
        }

        protected void btnEditRoles_Click(object sender, EventArgs e)
        {
            EditRoles();
        }
        protected void PopulateUserDetails(string userid)
        {
            DTMUsers u = new DTMUsers();
            u = GetADUsersInfo(userid);
            if (u != null)
            {
                txtFirstName.Text = u.FirstName;
                txtLastName.Text = u.LastName;
                txtEmail.Text = u.Email;
                txtPhone.Text = u.Phone;
            }
        }
        protected void ClearForm( )
        {
                txtFirstName.Text ="";
                txtLastName.Text = "";
                txtEmail.Text = "";
                txtPhone.Text ="";
        }
        protected void btnCheckPIUser_Click(object sender, EventArgs e)
        {
            
            StringBuilder sb = new StringBuilder();string msg = txtUserID.Text;
            sb.Append(txtUserID.Text); 
            if (!ValidateDTRUser(txtUserID.Text))
            {
                msg += " is not a valid NIH/DTR User.";
                sb.Append("<li> is not a valid NIH/DTR User.</li>");
                sb.AppendLine();
                lblDTRUser.ForeColor = System.Drawing.Color.Green;
            }
            else if (!ValidateADGroup(txtUserID.Text))
            {
                msg += " not in PIUsers AD Group.Please open a <a href='https://myitsm.nih.gov/nih_sd?id=service_request_catalog' target='_blank'>ticket </a> to add or contact <a href='mailto:ORFDTREFAMIT@mail.nih.gov?subject=PIUsers group access'>  DTR EFAM IT.";
                sb.Append("<li> not in PIUsers AD Group.</li>");
                sb.Append("  Please open a <a href='https://myitsm.nih.gov/nih_sd?id=service_request_catalog' target='_blank'>ticket </a> to add or contact <a href='mailto:ORFDTREFAMIT@mail.nih.gov?subject=PIUsers group access'>  DTR EFAM IT.");
                sb.AppendLine();
                lblPIUers.ForeColor = System.Drawing.Color.Green;
            }

            else if (ValidateSTOUser(txtUserID.Text))
            {
                msg += "already exists in Shift Turnover database.  Please go back and select User from the list to edit User Information.";
                sb.Append("<li>   already exists in Shift Turnover database.  Please go back and select User from the list to edit User Information.</li>");
                sb.AppendLine();
                lblSTOUser.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
               msg = "";
               tblValidation.Visible = false;
               sb.Clear();
                PopulateUserDetails(txtUserID.Text);
            }
            lblConfirm.Text = msg; 
        }
            
        }
    }
