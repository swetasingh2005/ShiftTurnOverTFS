using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;
using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ShiftTurnover
{
    public class PIEQData
    {
        public string Name;
        public string Status;

        public PIEQData(string name, string status)

        {
            Name = name;
            Status = status;
        }
        public static string SaveEquipStatusToPI(int ID, string Value)
        {
            //\\ORF-COGENAF\Cup_Main.Rev2\EQ_Status\CHLR\Chiller20System|PO_FO_Reason|Raw
            string RtrValue = "";string Attribute = "PO_FO_Reason";string _element = "";
            try
            {
                DataSet ds = CreateEquipmentList(ID);
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        PISystems myPIsystems = new PISystems();
                        PISystem myPISystem = myPIsystems["ORF-COGENAF"];
                        AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Cup_Main.Rev2"];
                        AFElement myElement = myDatabase.Elements["EQ_Status"];
                      
                        if (myElement != null && row["Element"].ToString().Contains("|") && Attribute.Length > 0)
                        {
                             _element = row["Element"].ToString().Remove(row["Element"].ToString().IndexOf("|"));
                        
                            try
                            {
                                AFAttribute myAttr = myElement.Elements[_element].Attributes[Attribute];
                                if (myAttr != null)
                                {
                                    if (myAttr.Attributes.Count > 0)
                                    {
                                        //AFAttribute myAttribute = myElement.Attributes[Attribute];
                                        AFAttribute myAttribute = myAttr.Attributes["Raw"];
                                        
                                        AFTime myTime = new AFTime(DateTime.Now); //could also use AFTime myTime = AFTime.Now;
                                        IList<AFValue> valuesToWrite = new List<AFValue>();
                                        AFValue afValueString = new AFValue(Value, myTime);
                                        afValueString.Attribute = myAttribute;
                                        valuesToWrite.Add(afValueString);
                                        AFListData.UpdateValues(valuesToWrite, AFUpdateOption.InsertNoCompression, AFBufferOption.BufferIfPossible);
                                        RtrValue = "Y";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                RtrValue = "Error Inserting " + _element + " " + ex.Message;

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                RtrValue = "Error Inserting " + _element + " " + ex.Message;

            }
            return RtrValue;
        }


            public static void InsertEQComment(int ShiftID , int UpdatedBy, int EQTagID, string comments)
        {
            DataSet ds;
            DataModule dataModule = new DataModule();
            dataModule.AddParameter("@ShiftID", SqlDbType.BigInt, ShiftID);
            dataModule.AddParameter("@UpdatedBy", SqlDbType.Int, UpdatedBy);
            dataModule.AddParameter("@EQTagID", SqlDbType.Int, EQTagID);
            dataModule.AddParameter("@comments", SqlDbType.Int, comments);
            ds = dataModule.GetDataSet("InsertShiftEQStatus");
        }
        public static DataTable GetShiftEQStatusSQL(int ShiftID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@shiftid", SqlDbType.Int, ShiftID);
            DataSet ds = _dm.GetDataSet("SelectShiftEQStatus");
            return ds.Tables[0];
        }
            public static DataTable GetShiftStatusChangePI()
        {
            string strSt = "";
            string strEnd = "";
            DataModule _dm = new DataModule();
            DataSet ds = _dm.GetDataSet("SelectCurrentShift");
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
            //AFTimeSpan interval = new AFTimeSpan(minutes: 60);//new AFTimeSpan(TimeSpan.FromDays(1)); //AFTimeSpan span = new AFTimeSpan(hours: 12, minutes: 30);
            //trying 5 min interval
            int _timespan = Convert.ToInt32(ConfigurationManager.AppSettings["AFTimeSpanInterval2"]);
            AFTimeSpan interval = new AFTimeSpan(minutes: _timespan);//changed to variable
            DataTable dataList = new DataTable();
            dataList = CreateDataList(start1, end1, interval);
              return dataList;
         //   Context.Response.Write("{ \"data\":" + DataTableToJSON(ds.Tables[0]) + "}");

        }
        public static DataSet CreateEquipmentList(int ID)
        {
            DataModule _dm = new DataModule();
            _dm.AddParameter("@EQTagID", SqlDbType.Int, ID);
            DataSet ds = _dm.GetDataSet("SelectEQTagList");
            return ds;
        }
        public static DataTable CreateDataList(DateTime starttime, DateTime endtime, AFTimeSpan interval)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            DataModule dataModule = new DataModule();
            DataSet ds = CreateEquipmentList(0);
            string Attribute = "";
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PISystems myPIsystems = new PISystems();
                    PISystem myPISystem = myPIsystems["ORF-COGENAF"];
                    AFDatabase myDatabase = myPIsystems["ORF-COGENAF"].Databases["Cup_Main.Rev2"];
                    AFTimeRange timeRange1 = new AFTimeRange(starttime, endtime);
                    AFElement myElement = myDatabase.Elements["EQ_Status"];
                    if (row["Attribute"] != null)
                    {
                        Attribute = row["Attribute"].ToString();
                    }
                    if (myElement != null && row["Element"].ToString().Contains("|") && Attribute.Length>0)
                    {
                        string _element = row["Element"].ToString().Remove(row["Element"].ToString().IndexOf("|"));
                        
                        try
                        {
                            AFAttribute myAttr = myElement.Elements[_element].Attributes[Attribute];
                            if (myAttr != null)
                            {
                                AFValues valsCHL = myAttr.Data.InterpolatedValues(
                                         timeRange: timeRange1,
                                         interval: interval,//for interpolated values
                                                            //boundaryType: AFBoundaryType.Inside, //only for RecordedValues
                                         desiredUOM: null,
                                         filterExpression: null,
                                         includeFilteredValues: false);

                                int i = 0;
                                foreach (AFValue val in valsCHL)
                                {
                                    if (val.Value.ToString().ToLower() == "active")
                                    {
                                        dt.Rows.Add(row["EQTagID"].ToString(), row["Parent"].ToString(), val.Value.ToString());
                                        break;
                                    }
                                    i++;
                                } // end foreach i loop
                            }//end chiller dataset j loop
                        }
                        catch (Exception ex)
                        {
                            string errr = ex.Message + " Attribute: " + Attribute + "  Element" + _element;
                          //  ErrorHandler.LogErrorToDB(1001, "NewTurnover_Acknowledgement.LoadStatus", "SelectReportDetails", ex);
                           
                        }
                    }
                }
            }

            return dt;
        }
    }
}