using System;
using System.Web;


namespace ShiftTurnover
{
    public partial class NearMissReport : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            txtIncidentDateTime.Attributes["min"] = DateTime.Now.AddHours(-12).ToString("yyyy-MM-ddTHH:mm");

            txtIncidentDateTime.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // any initialization, e.g. populate dropdown if not postback
            if (!IsPostBack)
            {
                // ddlDepartment could be bound here if dynamic
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // collect all values
            string reporterName = this.txtReporterName.Text.Trim();
            string incidentDateTimeStr = txtIncidentDateTime.Text.Trim();
            string immediateSeverity = rblImmediateSeverity.SelectedValue;
            string potentialSeverity = rblPotentialSeverity.SelectedValue;
            string department = ddlDepartment.SelectedValue;
            string location = txtLocation.Text.Trim();
            string task = txtTask.Text.Trim();
            string description = txtDescription.Text.Trim();
            string witnesses = txtWitnesses.Text.Trim();
            string immediateActions = txtImmediateActions.Text.Trim();

            DateTime incidentDateTime;
            if (!DateTime.TryParse(incidentDateTimeStr, out incidentDateTime))
            {
                lblResult.Text = "Please enter a valid date/time.";
                lblResult.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // TODO: Save to database, send email, etc.
            // For demo, we'll just echo back:
            lblResult.ForeColor = System.Drawing.Color.Green;
            lblResult.Text =
              "Thank you, " + reporterName +
              ". Your near-miss report has been submitted successfully.";

            // Optionally clear the form
            // ClearForm();
        }

        private void ClearForm()
        {
            txtReporterName.Text = "";
            txtIncidentDateTime.Text = "";
            rblImmediateSeverity.ClearSelection();
            rblPotentialSeverity.ClearSelection();
            ddlDepartment.SelectedIndex = 0;
            txtLocation.Text = "";
            txtTask.Text = "";
            txtDescription.Text = "";
            txtWitnesses.Text = "";
            txtImmediateActions.Text = "";
        }
    }
}
