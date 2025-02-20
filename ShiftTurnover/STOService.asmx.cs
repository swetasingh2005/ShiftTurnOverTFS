using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using ShiftTurnover.Components;
using ShiftTurnover.DataModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Services;
using DataTable = System.Data.DataTable;

namespace ShiftTurnover
{
    /// <summary>
    /// Summary description for STOService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class STOService : System.Web.Services.WebService
    {
        readonly Encryption Enc = new Encryption();
        readonly SqlConnection STOConn;

        public STOService()
        {
            STOConn = new SqlConnection(Enc.Decrypt(ConfigurationManager.AppSettings["sqlConn"]));
        }
        public string DataTableToJSON(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }
        [WebMethod(EnableSession = true)]
        public void GetShiftStatusChange()
        {
            int shiftId = Convert.ToInt32(Session["shiftid"]);
            List<ShiftEQ.Status> eqStatuses = new List<ShiftEQ.Status>();
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand("SelectShiftEQStatus", STOConn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@shiftid", shiftId);
            SqlDataAdapter da = new SqlDataAdapter
            {
                SelectCommand = cmd
            };
            da.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {                
                foreach (DataRow row in dt.Rows)
                {
                    ShiftEQ.Status status = new ShiftEQ.Status
                    {
                        EQTagID = (int)row["EQTagID"],
                        Tag = row["Tag"].ToString().Replace("\"", "\\"),
                        Comments = row["Comments"].ToString(),
                        Parent = row["Parent"].ToString().Replace("\"", "\\"),
                        AcknowledgedByName = row["AchnowledgeByN"].ToString(),
                        AcknowledgeDate = row["AchnowledgeDate"].ToString(),
                        AddedBy = row["Added By"].ToString(),
                        UpdatedDate = row["UpdatedDate"].ToString()
                    };
                    eqStatuses.Add(status);
                }
            }
            Context.Response.Write("{ \"data\":" + JsonConvert.SerializeObject(eqStatuses) + "}");
        }
        [WebMethod(EnableSession = true)]
        public void AddComment(string eqTagId, string comment)
        {
            try
            {
                int shiftId = Convert.ToInt32(Session["shiftid"]);
                int prId = Convert.ToInt32(Session["PersonRoleID"]);
                SqlCommand cmd = new SqlCommand("UpdateShiftEQStatusComment", STOConn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@shiftid", shiftId);
                cmd.Parameters.AddWithValue("@UpdatedBy", prId);
                cmd.Parameters.AddWithValue("@EQTagID", Convert.ToInt32(eqTagId));
                cmd.Parameters.AddWithValue("@Comments", comment);
                STOConn.Open();
                cmd.ExecuteNonQuery();
                STOConn.Close();
                PIEQData.SaveEquipStatusToPI(Convert.ToInt32(eqTagId), comment);
                Context.Response.Write("Success");
            }
            catch (Exception)
            {
                Context.Response.Write("Error");
            }
            
        }
         
       
    }
}
