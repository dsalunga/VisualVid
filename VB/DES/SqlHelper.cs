using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

//using DES.Utility;

namespace DES{

	/// <summary>
	/// The SqlHelper class is intended to encapsulate high performance, 
	/// scalable best practices for common uses of SqlClient.
	/// </summary>
	public abstract class SqlHelper {
		
		//Database connection strings
		public static readonly string CONN_STRING = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
		//public static readonly string CONN_STRING_DTC_INV = ConnectionInfo.DecryptDBConnectionString(ConfigurationManager.AppSettings["SQLConnString2"]);		
		//public static readonly string CONN_STRING_DTC_ORDERS = ConnectionInfo.DecryptDBConnectionString(ConfigurationManager.AppSettings["SQLConnString3"]);
		
		// Hashtable to store cached parameters
		private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {

			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connString)) {
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{

			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms) 
		{

			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
				int val = cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) against an existing database connection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="conn">an existing database connection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {

			SqlCommand cmd = new SqlCommand();

			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="trans">an existing sql transaction</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>an int representing the number of rows affected by the command</returns>
		public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {
			SqlCommand cmd = new SqlCommand();
			PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms);
			int val = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>A SqlDataReader containing the results</returns>
		public static SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(connString);

			// we use a try/catch here because if the method throws an exception we want to 
			// close the connection throw code, because no datareader will exist, hence the 
			// commandBehaviour.CloseConnection will not work
			try {
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return rdr;
			}catch {
				conn.Close();
				throw;
			}
		}

		public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(CONN_STRING);

			// we use a try/catch here because if the method throws an exception we want to 
			// close the connection throw code, because no datareader will exist, hence the 
			// commandBehaviour.CloseConnection will not work
			try 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return rdr;
			}
			catch 
			{
				conn.Close();
				throw;
			}
		}

		public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(CONN_STRING);

			// we use a try/catch here because if the method throws an exception we want to 
			// close the connection throw code, because no datareader will exist, hence the 
			// commandBehaviour.CloseConnection will not work
			try 
			{
				PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
				SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				cmd.Parameters.Clear();
				return rdr;
			}
			catch 
			{
				conn.Close();
				throw;
			}
		}

		public static DataSet ExecuteDataSet(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connString)) 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				DataSet ds = new DataSet();
				da.Fill(ds);

				cmd.Parameters.Clear();
				return ds;
			}
		}

		public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				DataSet ds = new DataSet();
				da.Fill(ds);

				cmd.Parameters.Clear();
				return ds;
			}
		}

		public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				DataSet ds = new DataSet();
				da.Fill(ds);

				cmd.Parameters.Clear();
				return ds;
			}
		}

		/*
		/// <summary>
		/// Outputs a dataset with the specified command, using the provided parameters 
		/// </summary>
		/// <param name="cmdText">command name / stored procedure</param>
		/// <param name="dataSetName">name of dataset</param>
		/// <param name="cmdParms">command parameters</param>
		/// <returns></returns>
		public static DataSet ExecuteDataSet(string cmdText, string dataSetName, string tableName, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
				SqlDataAdapter da = new SqlDataAdapter(cmd);

				DataSet ds = new DataSet(dataSetName);
				da.Fill(ds, tableName);

				cmd.Parameters.Clear();
				return ds;
			}
		}
		*/

		public static SqlDataAdapter ExecuteDataAdapter(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();
			SqlConnection conn = new SqlConnection(connString);

			// we use a try/catch here because if the method throws an exception we want to 
			// close the connection throw code, because no datareader will exist, hence the 
			// commandBehaviour.CloseConnection will not work
			try 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				SqlCommandBuilder builder = new SqlCommandBuilder(da);
				da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

				return da;
			}
			catch
			{
				conn.Close();
				throw;
			}
		}
		
		/// <summary>
		/// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="connectionString">a valid connection string for a SqlConnection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
		public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(connString)) {
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				object val = cmd.ExecuteScalar();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
				object val = cmd.ExecuteScalar();
				cmd.Parameters.Clear();
				return val;
			}
		}

		public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParms) 
		{
			SqlCommand cmd = new SqlCommand();

			using (SqlConnection conn = new SqlConnection(CONN_STRING)) 
			{
				PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
				object val = cmd.ExecuteScalar();
				cmd.Parameters.Clear();
				return val;
			}
		}

		/// <summary>
		/// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
		/// using the provided parameters.
		/// </summary>
		/// <remarks>
		/// e.g.:  
		///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
		/// </remarks>
		/// <param name="conn">an existing database connection</param>
		/// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
		/// <param name="commandText">the stored procedure name or T-SQL command</param>
		/// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
		/// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
		public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms) {
			
			SqlCommand cmd = new SqlCommand();

			PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
			object val = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return val;
		}

		/// <summary>
		/// add parameter array to the cache
		/// </summary>
		/// <param name="cacheKey">Key to the parameter cache</param>
		/// <param name="cmdParms">an array of SqlParamters to be cached</param>
		public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms) {
			parmCache[cacheKey] = cmdParms;
		}

		/// <summary>
		/// Retrieve cached parameters
		/// </summary>
		/// <param name="cacheKey">key used to lookup parameters</param>
		/// <returns>Cached SqlParamters array</returns>
		public static SqlParameter[] GetCachedParameters(string cacheKey) {
			SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
			
			if (cachedParms == null)
				return null;
			
			SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

			for (int i = 0, j = cachedParms.Length; i < j; i++)
				clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

			return clonedParms;
		}

		/// <summary>
		/// Prepare a command for execution
		/// </summary>
		/// <param name="cmd">SqlCommand object</param>
		/// <param name="conn">SqlConnection object</param>
		/// <param name="trans">SqlTransaction object</param>
		/// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
		/// <param name="cmdText">Command text, e.g. Select * from Products</param>
		/// <param name="cmdParms">SqlParameters to use in the command</param>
		private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms) {

			if (conn.State != ConnectionState.Open)
				conn.Open();

			cmd.Connection = conn;
			cmd.CommandText = cmdText;

			if (trans != null)
				cmd.Transaction = trans;

			cmd.CommandType = cmdType;

			if (cmdParms != null) {
				foreach (SqlParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}
	}
}