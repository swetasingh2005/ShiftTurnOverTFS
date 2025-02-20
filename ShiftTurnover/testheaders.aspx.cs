using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ShiftTurnover
{
    public partial class testheaders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (string var in Request.ServerVariables)
            {
                Response.Write(var + " " + Request[var] + "<br>");
            }

            Response.Write(HttpContext.Current.Request.ServerVariables["ALL_RAW"]);
        }
    }
}