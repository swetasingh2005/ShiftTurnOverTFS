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
    public partial class AddEQReason : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadForm();
                if (Request["update"] != null)
                {
                    if (Request["update"] == "success")
                    {
                        lblConfirm.Text = "Your reason for Equipment Status Change have been successfully submitted.";
                        lblConfirm.Visible = true;
                       
                    }
                    else if (Request["update"] == "fail")
                    {

                        lblConfirm.Text = "There was an error submitting the reason to PI server.";
                        lblConfirm.Visible = true;
                    }
                    else
                    {
                        lblConfirm.Text = "";
                        lblConfirm.Visible = false;
                    }
                }

            }//end !IsPostBack

        }
        protected void LoadForm()
        {
            int _strEQTagID = Convert.ToInt32(Request["EQTagID"]);
            int _strShiftID = Convert.ToInt32(Session["ShiftID"]);
            DataModule dataModule = new DataModule();

            try
            {
                dataModule.AddParameter("@EQTagID", SqlDbType.Int, _strEQTagID);
                dataModule.AddParameter("@ShiftID", SqlDbType.Int, _strShiftID);
                DataSet ds = dataModule.GetDataSet("SelectEQCommentByID");

                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        Page.Title = "  " + ds.Tables[0].Rows[0]["Parent"].ToString() + " ";
                        lblParent.Text = "  " + ds.Tables[0].Rows[0]["Parent"].ToString() + " ";
                        lblReasonAddedBy.Text = ds.Tables[0].Rows[0]["Added By"].ToString();
                        lblReasonAddedDate.Text = ds.Tables[0].Rows[0]["UpdatedDate"].ToString();
                        txtReason.Text = ds.Tables[0].Rows[0]["Comments"].ToString();
                    }

                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personroleid"], 0, "ViewHistory.LoadPageTitle", "selectparameters", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void SaveEquipStatusToSQLAndPI()
        {
            string update = "";
           
                string EquipComments = "";
                int _strEQTagID = Convert.ToInt32(Request["EQTagID"]);
                int _strShiftID = Convert.ToInt32(Session["ShiftID"]);
                int _strUpdatedBy=Convert.ToInt32(Session["PersonRoleID"]);
            if (txtReason.Text.Length > 0)
                { EquipComments = Convert.ToString(txtReason.Text); }
            try
            {
                DataModule _dm = new DataModule();
                _dm.AddParameter("@shiftid", SqlDbType.Int, _strShiftID);
                _dm.AddParameter("@UpdatedBy", SqlDbType.Int, _strUpdatedBy);
                _dm.AddParameter("@EQTagID", SqlDbType.Int, _strEQTagID);
                _dm.AddParameter("@Comments", SqlDbType.VarChar, EquipComments);
                _dm.ExecuteCommand("UpdateShiftEQStatusComment");
                PIEQData.SaveEquipStatusToPI(_strEQTagID, EquipComments);
                update = "success";
            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover_Review.SaveStatus", "submitreport", ex);
                Response.Redirect("CustomErrorPage.aspx");
                update = "fail";

            }
            Response.Redirect("AddEQReason.aspx?EQTagID=" + _strEQTagID + "&update=" + update);
        }
        
  
        protected void Button1_Click(object sender, EventArgs e)
        {
            SaveEquipStatusToSQLAndPI();
           
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("NewTurnover_Review.aspx");
        }
    }
}