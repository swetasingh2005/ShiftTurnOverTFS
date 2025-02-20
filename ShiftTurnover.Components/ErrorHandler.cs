using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ShiftTurnover.Components
{
    public class ErrorHandler
    {
        public ErrorHandler() { }

        public static void LogErrorToDB(int _PersonRoleID, int _ReportID, string _MethodName, string _StoredProcName, Exception _Ex)
        {

            string _ConnectionString;
            string b = System.Configuration.ConfigurationManager.AppSettings["sqlConn"];
            ShiftTurnover.Components.Encryption com = new ShiftTurnover.Components.Encryption();
            _ConnectionString = com.Decrypt(b);
            com = null;

            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand sqlcmd = new SqlCommand("editerrorlog", conn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@action", "insert");
            sqlcmd.Parameters.AddWithValue("@personroleid", _PersonRoleID);
            if (_ReportID != 0)
            {
                sqlcmd.Parameters.AddWithValue("@reportid", _ReportID);
            }
            sqlcmd.Parameters.AddWithValue("@aspprocedurename", _MethodName);
            if (_StoredProcName != "")
            {
                sqlcmd.Parameters.AddWithValue("@sqlprocedurename", _StoredProcName);
            }
            sqlcmd.Parameters.AddWithValue("@errormessage", _Ex.Message);
            sqlcmd.Parameters.AddWithValue("@errortrace", _Ex.StackTrace);

            conn.Open();
            sqlcmd.ExecuteNonQuery();
            conn.Close();
            conn = null;

        }
        public static void LogErrorToDB(int _PersonRoleID, int _ReportID, string _MethodName, string _StoredProcName, string _Ex)
        {

            string _ConnectionString;
            string b = System.Configuration.ConfigurationManager.AppSettings["sqlConn"];
            ShiftTurnover.Components.Encryption com = new ShiftTurnover.Components.Encryption();
            _ConnectionString = com.Decrypt(b);
            com = null;

            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand sqlcmd = new SqlCommand("editerrorlog", conn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@action", "insert");
            sqlcmd.Parameters.AddWithValue("@personroleid", _PersonRoleID);
            if (_ReportID != 0)
            {
                sqlcmd.Parameters.AddWithValue("@reportid", _ReportID);
            }
            sqlcmd.Parameters.AddWithValue("@aspprocedurename", _MethodName);
            if (_StoredProcName != "")
            {
                sqlcmd.Parameters.AddWithValue("@sqlprocedurename", _StoredProcName);
            }
            sqlcmd.Parameters.AddWithValue("@errormessage", _Ex);
            sqlcmd.Parameters.AddWithValue("@errortrace", _Ex);

            conn.Open();
            sqlcmd.ExecuteNonQuery();
            conn.Close();
            conn = null;

        }
        public static void LogErrorToDB(int _PersonRoleID, string _MethodName, string _StoredProcName, Exception _Ex)
        {

            string _ConnectionString;
            string b = System.Configuration.ConfigurationManager.AppSettings["sqlConn"];
            ShiftTurnover.Components.Encryption com = new ShiftTurnover.Components.Encryption();
            _ConnectionString = com.Decrypt(b);
            com = null;

            SqlConnection conn = new SqlConnection(_ConnectionString);
            SqlCommand sqlcmd = new SqlCommand("editerrorlog", conn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@action", "insert");
            sqlcmd.Parameters.AddWithValue("@personroleid", _PersonRoleID);
            sqlcmd.Parameters.AddWithValue("@reportid", System.DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@aspprocedurename", _MethodName);
            if (_StoredProcName != "")
            {
                sqlcmd.Parameters.AddWithValue("@sqlprocedurename", _StoredProcName);
            }
            sqlcmd.Parameters.AddWithValue("@errormessage", _Ex.Message);
            sqlcmd.Parameters.AddWithValue("@errortrace", _Ex.StackTrace);

            conn.Open();
            sqlcmd.ExecuteNonQuery();
            conn.Close();
            conn = null;

        } //end class ErrorHandler
    }
}
