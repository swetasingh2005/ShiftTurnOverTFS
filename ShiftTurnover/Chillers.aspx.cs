using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Services;

using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.PI;
using OSIsoft.AF.Time;
using OSIsoft.AF.Data;
using OSIsoft.AF.EventFrame;
using OSIsoft.AF.Search;

using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Point = DotNet.Highcharts.Options.Point;

using ShiftTurnover.Components;

namespace ShiftTurnover
{
    public partial class Chillers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }// end page load

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion

        [WebMethod]

        public static List<PIData> GetData()
        {
            //AFTime start1 = new AFTime("*-10m");
            //AFTime end1 = new AFTime("*");
            AFTime start1 = new AFTime("10/15/2019 06:00");
            AFTime end1 = new AFTime("10/15/2019 18:00");
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval);
            return dataList;
        }

        public static List<PIData> CreateDataList(DateTime starttime, DateTime endtime, AFTimeSpan interval)
        {
            //data for google charts
            List<PIData> datalist = new List<PIData>();
            string[] arrCHL;
            DataModule dataModule = new DataModule();
            DataSet ds = dataModule.GetDataSet("tmpSelectChillers");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                //Empty array
                arrCHL = new string[ds.Tables[0].Rows.Count];
                //foreach (DataRow oRow in ds.Tables[0].Rows)
                //{
                //    Months.Add(oRow);
                //}
                //loopcounter
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    //assign dataset values to array
                    arrCHL[j] = ds.Tables[0].Rows[j]["Chiller"].ToString();

                    PISystems myPIsystems = new PISystems();
                    AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
                    AFElement myElement = myDatabase.Elements["CUP\\CHL"];
                    AFAttribute myAttr = myElement.Elements[arrCHL[j].ToString()].Attributes["Status_String"];
                    AFTimeRange timeRange1 = new AFTimeRange(starttime, endtime);

                    AFValues valsCHL = myAttr.Data.InterpolatedValues(
                               timeRange: timeRange1,
                               interval: interval,//for interpolated values
                                                  //boundaryType: AFBoundaryType.Inside, //only for RecordedValues
                               desiredUOM: null,
                               filterExpression: null,
                               includeFilteredValues: false);

                    //DateTime _timevalue = DateTime.Now;
                    //val.Timestamp.LocalTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                    DateTime _timevalue = starttime;
                    DateTime _starttime = starttime;
                    DateTime _endtime = starttime;
                    string _status = "No Data";

                    int i = 0;
                    foreach (AFValue val in valsCHL)
                    {
                        if (DateTime.Compare(val.Timestamp, starttime) == 0)
                        {
                            _starttime = val.Timestamp;
                            _status = val.Value.ToString();
                        }
                        if (val.Value.ToString() != _status)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(arrCHL[j].ToString(), _status, (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            _status = val.Value.ToString();
                            _starttime = val.Timestamp;
                        }
                        else if (i == valsCHL.Count - 1)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(arrCHL[j].ToString(), _status, (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                        }

                        i++;
                    } // end foreach i loop
                }//end chiller dataset j loop
            }

            return datalist;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}