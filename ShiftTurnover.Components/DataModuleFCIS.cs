using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
//using Microsoft.ApplicationBlocks.Data;
using System.Configuration;
using System.Text;
using System.Web;

namespace ShiftTurnover.Components
{
	/// <summary>
	/// Summary description for DataModuleChem.
	/// </summary>
	/// 
	[Serializable()]
	public class DataModuleFCIS
	{
		#region Members
		private struct ParameterElement
		{
			public string ParamName;
			public SqlDbType ParamType;
			public object ParamValue;
			public int ParamSize;
			public ParameterDirection ParamDirection;
			ParameterElement(string name,SqlDbType type,object paramvalue,int size,ParameterDirection direction)
			{
				ParamName=name;
				ParamType=type;

				ParamValue=paramvalue;
				ParamSize=size;
				ParamDirection=direction;
			}
		}
		private SqlConnection cnn;
		private static string sqlstring;
		private ArrayList parstbl;

		#endregion


		public DataModuleFCIS()
		{
            string b = System.Web.Configuration.WebConfigurationManager.AppSettings["sqlconnFCIS"];
            Encryption comn = new Encryption();
          	SqlConnectionString = comn.Decrypt(b);
			parstbl=new ArrayList();
		}

		#region Method - GetDataSet
		//modified. Need to use command object to add timeout parameters.
        public DataSet GetDataSet(string StoredProcName)
        {
            //if (parstbl.Count>0)
            //{
            try
            {
                cnn = new SqlConnection(SqlConnectionString);
                cnn.Open();

                SqlCommand _Comm = new SqlCommand();
                _Comm.Connection = cnn;

                SqlParameter[] arParms = new SqlParameter[parstbl.Count];
                IEnumerator myenum = parstbl.GetEnumerator();
                ParameterElement paramelement;
                int i = 0;
                while (myenum.MoveNext())
                {
                    paramelement = (ParameterElement)myenum.Current;
                    //arParms[i] = new SqlParameter(paramelement.ParamName, paramelement.ParamType ); 
                    _Comm.Parameters.Add(paramelement.ParamName, paramelement.ParamType, paramelement.ParamSize);
                    _Comm.Parameters[paramelement.ParamName].Value = paramelement.ParamValue;
                    _Comm.Parameters[paramelement.ParamName].Direction = paramelement.ParamDirection;
                    //arParms[i].Value = paramelement.ParamValue;
                    //if (!paramelement.ParamSize.Equals(0)){arParms[i].Size=paramelement.ParamSize;}
                    //arParms[i].Direction=paramelement.ParamDirection;
                    i++;
                }

                //set command time out
                _Comm.CommandTimeout = Convert.ToInt16(System.Web.Configuration.WebConfigurationManager.AppSettings["CommandTimeOut"]);//25000;
                _Comm.CommandType = CommandType.StoredProcedure;
                _Comm.CommandText = StoredProcName;

                DeleteParameters();

                //return SqlHelper.ExecuteDataset(cnn, CommandType.StoredProcedure,StoredProcName, arParms);
                //run command and convert reader to dataset
                return convertDataReaderToDataSet(_Comm.ExecuteReader());

            }
            catch (Exception ex)
            {
                ErrorHandler.LogErrorToDB(100, 100, "DataModuleChem.GetDataSet", StoredProcName, ex);

                throw new Exception("Error in ShiftTurnover.Components.DataModuleChem.GetDataSet " + ex.Message, ex);
            }
            finally
            {
                if (cnn != null)
                {
                    try
                    {
                        cnn.Close();
                    }
                    catch (Exception e)
                    { }
                }
            }
        }
		
		# endregion	
		public DataSet GetDataSetInLineQuery(string inlinequery)
		{
			try
			{
				cnn = new SqlConnection(SqlConnectionString);
				cnn.Open();

				SqlCommand _Comm = new SqlCommand();
				_Comm.Connection = cnn;
				_Comm.CommandText=inlinequery;
				_Comm.CommandType=CommandType.Text;

				_Comm.CommandTimeout = Convert.ToInt16(System.Web.Configuration.WebConfigurationManager.AppSettings["CommandTimeOut"]);//25000;
				
				return convertDataReaderToDataSet(_Comm.ExecuteReader());
					
			}
			catch(Exception ex)
			{
				throw new Exception("Error in ShiftTurnover.Components.DataModuleChem.GetDataSetInLineQuery " + ex.Message,ex);
			}
			finally
			{
				cnn.Close();
			}			
		}



   


		//added: need to convert reader to dataset.
		public static DataSet convertDataReaderToDataSet(SqlDataReader reader)
		{
			DataSet dataSet = new DataSet();
            do
            {

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);
                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                    reader.NextResult();
                }
            } while (reader.HasRows);

            return dataSet;
		}

        public string getConnectionString() //For activeReports
        {
            string b = System.Configuration.ConfigurationManager.AppSettings["sqlconnFCIS"];
            Encryption comn = new Encryption ();
            string SqlConnectionString = comn.Decrypt(b);

            return SqlConnectionString;
        }
	

		#region Methods - Add, Delete Parameter
		public void AddParameter(string ParameterName,SqlDbType ParameterType,object ParameterValue)
		{

			ParameterElement paramelement;
			paramelement.ParamName=ParameterName;
			paramelement.ParamType=ParameterType;
			paramelement.ParamValue=ParameterValue;
			paramelement.ParamSize=0;
			paramelement.ParamDirection=ParameterDirection.Input;
			parstbl.Add(paramelement);
					
		}
		public void AddParameter(string ParameterName,SqlDbType ParameterType,object ParameterValue,int ParameterSize)//If Param size=0 than ignore size
		{

			ParameterElement paramelement;
			paramelement.ParamName=ParameterName;
			paramelement.ParamType=ParameterType;
			paramelement.ParamValue=ParameterValue;
			paramelement.ParamSize=ParameterSize;
			paramelement.ParamDirection=ParameterDirection.Input;
			parstbl.Add(paramelement);
					
		}
		public void AddParameter(string ParameterName,SqlDbType ParameterType,object ParameterValue,int ParameterSize,ParameterDirection ParameterDirectionInOut)//If Param size=0 than ignore size
		{

			ParameterElement paramelement;
			paramelement.ParamName=ParameterName;
			paramelement.ParamType=ParameterType;
			paramelement.ParamValue=ParameterValue;
			paramelement.ParamSize=ParameterSize;
			paramelement.ParamDirection=ParameterDirectionInOut;
			parstbl.Add(paramelement);
					
		}
		private void DeleteParameters()
		{
			parstbl.Clear();
		}
		#endregion

		public string SqlConnectionString
		{
			get{return sqlstring;}
			set{sqlstring=value;}
        }

        //added by gss
        public DataSet GetDataSetFromCommand(string sqlCommandString)
        {

            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand sqlcmd = new SqlCommand(sqlCommandString, conn);

            try
            {
                sqlcmd.CommandType = CommandType.Text;

                SqlParameter[] arParms = new SqlParameter[parstbl.Count];
                IEnumerator myenum = parstbl.GetEnumerator();
                ParameterElement paramelement;
                int i = 0;
                while (myenum.MoveNext())
                {
                    paramelement = (ParameterElement)myenum.Current;
                    sqlcmd.Parameters.Add(paramelement.ParamName, paramelement.ParamType, paramelement.ParamSize);
                    sqlcmd.Parameters[paramelement.ParamName].Value = paramelement.ParamValue;
                    sqlcmd.Parameters[paramelement.ParamName].Direction = paramelement.ParamDirection;
                    i++;
                }

                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlcmd = null;
                conn = null;
            }

            return ds;

        }

        public void ExecuteCommand(string StoredProcedureName)
        {

            SqlConnection conn = new SqlConnection(SqlConnectionString);
            SqlCommand sqlcmd = new SqlCommand(StoredProcedureName, conn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            try
            {

                SqlParameter[] arParms = new SqlParameter[parstbl.Count];
                IEnumerator myenum = parstbl.GetEnumerator();
                ParameterElement paramelement;
                int i = 0;
                while (myenum.MoveNext())
                {
                    paramelement = (ParameterElement)myenum.Current;
                    sqlcmd.Parameters.Add(paramelement.ParamName, paramelement.ParamType, paramelement.ParamSize);
                    sqlcmd.Parameters[paramelement.ParamName].Value = paramelement.ParamValue;
                    sqlcmd.Parameters[paramelement.ParamName].Direction = paramelement.ParamDirection;
                    i++;
                }
                conn.Open();
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                sqlcmd = null;
                conn = null;
            }

        }

    }
}