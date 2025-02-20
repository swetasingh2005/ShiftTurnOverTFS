using System;
using System.Collections;
using System.Data;	
using System.Data.SqlClient;
using ShiftTurnover.Components;
using System.Web;	

namespace ShiftTurnover.DL
{
	/// <summary>
	/// Summary description for GSSDataModule.
	/// </summary>
	public class GSSDataModule
	{

		private string _ConnectionString;
		private ArrayList _ParameterList = new ArrayList();

		public GSSDataModule()
		{
			//
			// TODO: Add constructor logic here
			//
            string b = System.Web.Configuration.WebConfigurationManager.AppSettings["sqlConn"];
			Encryption com = new Encryption();
			_ConnectionString = com.Decrypt(b);
			com = null;
		}

		public DataSet GetDataSet(string StoredProcedureName)
		{
		
			DataSet ds = new DataSet();
			SqlConnection conn = new SqlConnection(_ConnectionString);
			SqlCommand sqlcmd = new SqlCommand(StoredProcedureName, conn);
			
			try
			{	
				sqlcmd.CommandType = CommandType.StoredProcedure;

				for(int i = 0; i < _ParameterList.Count; i++)
				{
					Parameter pm = (Parameter)_ParameterList[i];
                    sqlcmd.Parameters.AddWithValue(pm.Name, pm.Value);
				}
			
				SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
				da.Fill(ds);
			}
			catch(Exception ex)
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


		public DataSet GetDataSetFromCommand(string sqlCommandString)
		{
		
			DataSet ds = new DataSet();
			SqlConnection conn = new SqlConnection(_ConnectionString);
			SqlCommand sqlcmd = new SqlCommand(sqlCommandString, conn);
			
			try
			{	
				sqlcmd.CommandType = CommandType.Text;

				for(int i = 0; i < _ParameterList.Count; i++)
				{
					Parameter pm = (Parameter)_ParameterList[i];
                    sqlcmd.Parameters.AddWithValue(pm.Name, pm.Value);
				}
			
				SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
				da.Fill(ds);
			}
			catch(Exception ex)
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

			SqlConnection conn = new SqlConnection(_ConnectionString);
			SqlCommand sqlcmd = new SqlCommand(StoredProcedureName, conn);
			sqlcmd.CommandType = CommandType.StoredProcedure;
			
			try
			{
				for(int i = 0; i < _ParameterList.Count; i++)
				{
					Parameter pm = (Parameter)_ParameterList[i];
                    sqlcmd.Parameters.AddWithValue(pm.Name, pm.Value);
				}
				conn.Open();
				sqlcmd.ExecuteNonQuery();
			}
			catch(Exception ex)
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

		public void AddParameter(string parameterName, object parameterValue)
		{
			Parameter pm = new Parameter(parameterName, parameterValue);
			_ParameterList.Add(pm);
			pm = null;
		}

		/// <summary>
		/// A class definition for parameters.
		/// </summary>
		protected class Parameter
		{

			public Parameter()
			{
			}
			public Parameter(string Name, object Value)
			{
				_Name = Name;
				_Value = Value;
			}

			private string _Name;
			private object _Value;

			public string Name
			{
				get { return _Name; }
				set { _Name = value; }
			}

			public object Value
			{
				get { return _Value; }
				set { _Value = value; }
			}

		}

	}
}
