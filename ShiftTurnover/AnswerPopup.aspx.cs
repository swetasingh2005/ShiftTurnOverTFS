using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using ShiftTurnover.Components;

namespace ShiftTurnover
{
    public partial class AnswerPopup : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
                TableCell mplblColumn = (TableCell)Master.FindControl("narrowcolumn");
                if (mplblColumn != null)
                {
                    mplblColumn.Visible = false;
                }         
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string strQuestionID = Request["qid"].ToString();

            DataModule dataModule = new DataModule();
            dataModule.AddParameter("@elementid", SqlDbType.Int, System.DBNull.Value);
            dataModule.AddParameter("@questionid", SqlDbType.Int, Convert.ToInt32(strQuestionID));
            DataSet ds = dataModule.GetDataSet("selectelementquestion");

            if (ds != null)
            {
                this.Literal1.Text = "<strong>" + ds.Tables[0].Rows[0]["questiontext"].ToString() + "</strong>";
                this.Literal2.Text = ds.Tables[0].Rows[0]["answertext"].ToString();
            }
        }
    }
}