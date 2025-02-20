using ShiftTurnover.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ShiftTurnover
{
    public class AlarmKPICalculation
    {
        public static DataSet getAlarm10MinPeriod(int SystemID)
        {
            
            DataSet ds; 
            DataModule dataModule = new DataModule();
            dataModule.AddParameter("@systemid", SqlDbType.Int, SystemID);
            ds = dataModule.GetDataSet("SelectAlarm10MinPeriod");
            return ds;

        }
    }
}