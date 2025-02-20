using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace ShiftTurnover
{
    public partial class CustomErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblErrorMsg.Text = "<li>An unhandled error occurred when you ran the application.  If this message keeps appearing, please contact the system administrator.</li>";
            }
        }
    }
}