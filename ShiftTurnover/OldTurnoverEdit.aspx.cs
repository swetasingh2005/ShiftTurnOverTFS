using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;
using System.Collections;
using System.Data;
using System.Text;
using System.Drawing;

using IronPdf;

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
    public partial class OldTurnoverEdit : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            txtDueDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDueDate.Attributes["max"] = DateTime.Today.Date.ToString("yyyy-MM-dd");
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
             

            if (!Page.IsPostBack)
            {
                int ShiftID = getShiftID();
                if (ShiftID == 0)
                {
                    lblErrorMsg.Text = "There are no Shift TurnOver Report for Date:" + txtDueDate.Text;
                    pnlRpt.Visible = false;
                }
                else
                {
                    lblErrorMsg.Text = "";
                    LoadForm(ShiftID);
                    pnlRpt.Visible = true;
                    lblMShift.Text = "for Shift: " + Common.getShiftLabel(ShiftID);
                }

                     
               
            }//postback
        }//page load
        private void LoadForm(int ShiftID)
        {
           
            LoadPastEventsGrid(ShiftID);
            LoadMaximoGrids(ShiftID);
            LoadShiftWorkers(ShiftID);
            LoadStatus(ShiftID);
        }

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
            this.GrPast.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.GrPast_ItemDataBound);
        }
        #endregion
        private void LoadStatus(int _ShiftID)
        {
            try
            {
                DataModule _dm = new DataModule();

                _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
                DataSet ds = _dm.GetDataSet("SelectReportDetails");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            //lblShiftWorkersCorrect.Text = row["ChillerYN"].ToString();
                            lblShiftWorkersComment.Text = row["ShiftWorkerInfo"].ToString();

                            lblChillersCorrect.Text = row["ChillerYN"].ToString();
                            lblChillersComment.Text = row["ChillerComments"].ToString();

                            lblChillersPmpCorrect.Text = row["ChillerPumpYN"].ToString();
                            lblChillersPmpComment.Text = row["ChillerPumpComments"].ToString();

                            lblCTCorrect.Text = row["CoolingTowerYN"].ToString();
                            lblCTComment.Text = row["CoolingTowerComments"].ToString();

                            lblFreeCoCorrect.Text = row["FreeCoolingYN"].ToString();
                            lblFreeCoComment.Text = row["FreeCoolingComments"].ToString();

                            lblBoilersCorrect.Text = row["BoilerYN"].ToString();
                            lblBoilersComment.Text = row["BoilerComments"].ToString();

                            lblBlrPmpCorrect.Text = row["ROPumpYN"].ToString();
                            lblBlrPmpComment.Text = row["ROPumpComments"].ToString();

                            lblWOComplete.Text = row["WOComplete"].ToString();
                            lblWOCorrect.Text = row["WOCorrect"].ToString();

                            lblWOOther.Text = row["WOOther"].ToString();
                            lblCriAlsCorrect.Text = row["AlarmsYN"].ToString();
                            lblCriAlsComment.Text = row["AlarmsComments"].ToString();




                        }

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        private void LoadShiftWorkers(int _ShiftID)
        {
            try
            {
                 
                DataModule _dm = new DataModule();

                _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
                DataSet ds = _dm.GetDataSet("SelectShiftworkers");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            switch (row["RoleID"])
                            {
                                case 3: // Chief 
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblCrewChief.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblCrewChief.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }
                                    break;
                                case 4: // Chiller Operator 
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblChillerOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblChillerOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;

                                    }
                                    break;
                                case 5: // Boiler Operator   
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblBoilerOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblBoilerOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;

                                    }

                                    break;


                                case 6: // Aux Operator
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblAuxOperator.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblAuxOperator.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
                                            break;
                                    }

                                    break;
                                case 7: // Electrician
                                    switch (row["primaryYN"].ToString())
                                    {
                                        case "Y": // Primary 
                                            lblElectrician.Text = row["Worker Name"].ToString() + " (Primary)";
                                            break;
                                        case "N":
                                            lblElectrician.Text += " and " + row["Worker Name"].ToString() + " (Secondary)";
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
                ErrorHandler.LogErrorToDB((int)Session["personid"], "NewTurnover.LoadForm", "SelectShiftworkersbyrole", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
        }
        protected void LoadPastEventsGrid(int _ShiftID)
        {
            DataModule _dm = new DataModule();
             
            _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftLiveLog");

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    pnlPast.Visible = false;
                     
                }
                else
                {
                    lblPast.Visible = false;
                    pnlPast.Visible = true;
                    GrPast.DataSource = ds;
                    GrPast.DataBind();
                }
            }
        }

        private void GrPast_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DataSet _dv = (DataSet)GrPast.DataSource;
            DataColumnCollection _dc = _dv.Tables[0].Columns;

            if (ViewState["gridcols"] == null)
            {
                IEnumerator _colenum = _dv.Tables[0].Columns.GetEnumerator();
                Hashtable _htbl = new Hashtable();
                while (_colenum.MoveNext())
                {
                    String ColName = _colenum.Current.ToString();
                    _htbl.Add(ColName, _dv.Tables[0].Columns.IndexOf(ColName).ToString());
                }
                //Storing column index in viewstate
                ViewState["gridcols"] = _htbl;
            }

            //configure datagrid
            e.Item.Cells[_dc.IndexOf(_dc["personroleid"])].Visible = false;
        }

        protected DataSet LoadMaximoWOData(int i, int _ShiftID)
        {
            //  get From Maximo  @shiftstartdatetime      @shiftenddatetime 
            string StoreProc = "SelectMaximoMaintWO";
            DataSet ds1 = new DataSet();
              DataModule _dm = new DataModule();
            string strSt = "";
            string strEnd = "";
            if (_ShiftID > 0)
            {
                _dm.AddParameter("@shiftid", SqlDbType.Int, _ShiftID);
                DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                        strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                    }
                }
                _dm.AddParameter("@shiftenddatetime", SqlDbType.DateTime, Convert.ToDateTime(strEnd));
                if (i == 1) { StoreProc = "SelectMaximoMaintWO"; _dm.AddParameter("@shiftstartdatetime", SqlDbType.DateTime, Convert.ToDateTime(strSt)); }
                else { StoreProc = "SelectMaximoSchedWO"; }
                 ds1 = _dm.GetDataSet(StoreProc);
            }
            return ds1 ;

        }
        public void LoadMaximoGrids(int _ShiftID)
        {

            DataSet ds = LoadMaximoWOData(1, _ShiftID);
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (  ds.Tables[0].Rows.Count == 0)
                    {
                        pnlWO.Visible = false;
                        lblWO.Visible = true;
                    }
                    else
                    {
                        lblWO.Visible = false;
                        pnlWO.Visible = true;
                        grdWO.DataSource = ds;
                        grdWO.DataBind();
                    }
                }
                else
                {
                    pnlWO.Visible = false;
                    lblWO.Visible = true;
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.LoadMaximoGrids", "SelectMaximoSchedWO", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            finally
            {
                ds = null;
            }
              ds = LoadMaximoWOData(2, _ShiftID);
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if ( ds.Tables[0].Rows.Count == 0)
                    {
                        pnlSche.Visible = false;
                        lblSche.Visible = true;
                    }
                    else
                    {
                        lblSche.Visible = false;
                        pnlSche.Visible = true;
                        grdSche.DataSource = ds;
                        grdSche.DataBind();
                    }
                }
                else
                {
                    pnlSche.Visible = false;
                    lblSche.Visible = true;
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.getShiftID", "SelectMaximoSchedWO", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
            finally
            {
                ds = null;
            }
            ds = LoadMaximoWOData(2, _ShiftID);
            
         
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (   ds.Tables[0].Rows.Count == 0)
                    {
                        pnlCAlm.Visible = false;
                        lblCAlm.Visible = true;
                    }
                    else
                    {
                        lblCAlm.Visible = false;
                        pnlCAlm.Visible = true;
                        grdCAlm.DataSource = ds;
                        grdCAlm.DataBind();
                    }
                }
                else
                {
                    pnlCAlm.Visible = false;
                    lblCAlm.Visible = true;
                }
            }

            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.LoadMaximoGrids", "SelectMaximoSchedWO", ex);
                Response.Redirect("CustomErrorPage.aspx");

            }
            finally
            {
                ds = null;
            }
         
        }
      
       
        public static int getShiftID(DateTime Date, string shiftperiod)
        {
            DataSet ds;
            DataSet ds1;
            int ShiftID = 0;
            try
            {
                DataModule _dm = new DataModule();
                DataModule _dm1 = new DataModule();
                string shiftnum = "A";
                _dm.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(Date));
                _dm.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                ds = _dm.GetDataSet("SelectShiftNum");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 &&  ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        shiftnum = Convert.ToString(ds.Tables[0].Rows[0]["Shift Number"]);
                    }

                }
                _dm1.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(Date));
                _dm1.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                _dm1.AddParameter("@shiftnum", SqlDbType.VarChar, shiftnum);
                ds1 = _dm1.GetDataSet("SelectShiftID");
                if (ds1 != null)
                {
                    if (ds.Tables.Count > 0 &&  ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ShiftID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ShiftID"]);
                    }

                }

            }
            catch (System.Exception ex)
            {
                 
            }
            return ShiftID;
        }
        [WebMethod]
      
            public static List<PIData> GetChillerData(DateTime Date , string Period )
        {
            string strSt = "";
            string strEnd = "";
            
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 1);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetChillerPumpData(DateTime Date, string Period)
        {
            string strSt = "";
            string strEnd = "";
             
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 2);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetBlrData(DateTime Date, string Period)
        {
            string strSt = "";
            string strEnd = "";
             
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 5);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetFreeCoData(DateTime Date, string Period)
        {
            string strSt = "";
            string strEnd = "";
            
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }
            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 4);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetROPumpData(DateTime Date, string Period)
        {
            string strSt = "";
            string strEnd = "";
               
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 6);
            return dataList;
        }
        [WebMethod]
        public static List<PIData> GetCTData(DateTime Date, string Period)
        {
            string strSt = "";
            string strEnd = "";
           
            int shiftID = getShiftID(Date, Period);
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, shiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftDateTime");
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    strSt = row["Start Date"].ToString() + " " + row["Start Time"].ToString();
                    strEnd = row["End Date"].ToString() + " " + row["End Time"].ToString();
                }
            }

            AFTime start1 = new AFTime(strSt);
            AFTime end1 = new AFTime(strEnd);
            //AFTime end1 = new AFTime("*");
            AFTimeRange timeRange1 = new AFTimeRange(start1, end1);
            AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            List<PIData> dataList = new List<PIData>();
            dataList = CreateDataList(start1, end1, interval, 3);
            return dataList;
        }
        public static DataSet CreateEquipmentList(int i)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@equipmenttypeid", SqlDbType.Int, i);
            DataSet ds = _dm.GetDataSet("SelectEquipmentList");
            return ds;
        }
        public static string GetStatus(string Status)
        {
            string StatusString = "";
            if (Status.Contains("Data")) { return "No Data"; }
            else
            {

                switch (Status)
                {
                    case "0": //
                    case "Standby":
                        StatusString = "Standby";
                        break;
                    case "1": //
                    case "Online":
                        StatusString = "Running";
                        break;
                    case "2": //
                    case "Forced Out":
                        StatusString = "Forced Out";
                        break;
                    case "3": //
                    case "Planned Out":
                        StatusString = "Planned Out";
                        break;

                    default:
                        StatusString = "No Data";
                        break;
                }

            }
            return StatusString;
        }
        public static List<PIData> CreateDataList(DateTime starttime, DateTime endtime, AFTimeSpan interval, int Graphid)
        {
            //data for google charts
            List<PIData> datalist = new List<PIData>();
            DataModule dataModule = new DataModule();
            DataSet ds = CreateEquipmentList(Graphid);
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    
                    PISystems myPIsystems = new PISystems();
                    PISystem myPISystem = myPIsystems["ORF-COGENAF"];
                    AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Database1"];
                    AFTimeRange timeRange1 = new AFTimeRange(starttime, endtime);
                    AFElement myElement = myDatabase.Elements["CUP"];
                    string _element = row["Element"].ToString().Remove(row["Element"].ToString().IndexOf("|"));
                    AFAttribute myAttr = myElement.Elements[_element].Attributes[row["Attribute"].ToString()];

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
                    string _status = " ";

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
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                            _status = val.Value.ToString();
                            _starttime = val.Timestamp;
                        }
                        else if (i == valsCHL.Count - 1)
                        {
                            _endtime = val.Timestamp;
                            datalist.Add(new PIData(row["Name"].ToString(), GetStatus(_status), (long)(_starttime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, (long)(_endtime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds));
                        }

                        i++;
                    } // end foreach i loop
                }//end chiller dataset j loop
            }

            return datalist;
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            
            int ShiftID = getShiftID();
            if (ShiftID == 0)
            {
                lblErrorMsg.Text = "There are no Shift TurnOver Report for Date:" + txtDueDate.Text + " (" + ddlPeriod.SelectedItem.Text + ")";
                pnlRpt.Visible = false;
            }
            else
            {
                lblMShift.Text = "for Shift: " + Common.getShiftLabel(ShiftID);
                lblErrorMsg.Text = "";
                LoadForm(ShiftID);
                pnlRpt.Visible = true;
            }
        }

        

         
       
        protected   int getShiftID()
        {
            DataSet ds;
            DataSet ds1;
            int ShiftID = 0;
            try
            {
                 DataModule _dm = new DataModule();
                DataModule _dm1 = new DataModule();
                string shiftperiod = ddlPeriod.SelectedValue;
                string shiftnum = "A";
                 
                _dm.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(txtDueDate.Text));
                _dm.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                ds = _dm.GetDataSet("SelectShiftNum");
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        shiftnum =  Convert.ToString( ds.Tables[0].Rows[0]["Shift Number"]) ;
                    }
                    
                }
                _dm1.AddParameter("@shiftdate", SqlDbType.Date, Convert.ToDateTime(txtDueDate.Text));
                _dm1.AddParameter("@shiftperiod", SqlDbType.VarChar, shiftperiod);
                _dm1.AddParameter("@shiftnum", SqlDbType.VarChar, shiftnum);
                ds1 = _dm1.GetDataSet("SelectShiftID");
                if (ds1 != null)
                {
                    if (ds1.Tables[0] != null && ds1.Tables[0].Rows.Count > 0)
                    {
                        ShiftID = Convert.ToInt16(ds1.Tables[0].Rows[0]["ShiftID"]);
                    }

                }

            }
            catch (System.Exception ex)
            {
                ErrorHandler.LogErrorToDB((int)Session["personid"], "OldTurnover.getShiftID", "SelectShiftID", ex);
                Response.Redirect("CustomErrorPage.aspx");
            }
            return ShiftID;
        }
        protected void btnResetReport_Click(object sender, EventArgs e)
        {
            
            lblErrorMsg.Text = "";
            txtDueDate.Text = DateTime.Today.ToString("yyyy-MM-dd"); 
            ddlPeriod.SelectedValue = "D";
            int ShiftID = getShiftID();
            LoadForm(ShiftID);
           lblMShift.Text = "for Shift: " + Common.getShiftLabel(ShiftID);
        }

        protected void txtDueDate_TextChanged(object sender, EventArgs e)
        {
            DateTime selectedDatetime = Convert.ToDateTime(txtDueDate.Text);
            System.TimeSpan diffResult = selectedDatetime - DateTime.Today;
            if (diffResult.Days < 0) { ddlPeriod.Enabled = true; } else { ddlPeriod.Enabled = false; }
        }
    }
}