using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;

namespace VisualVid.Core.Data;

/// <summary>
/// Migrated SqlHelper from the legacy DES library.
/// Uses Microsoft.Data.SqlClient for .NET 10 compatibility.
/// Connection string is injected rather than read from ConfigurationManager.
/// </summary>
public class SqlHelper
{
    private readonly string _connectionString;
    private static readonly Hashtable ParmCache = Hashtable.Synchronized(new Hashtable());

    public SqlHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(connString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    public int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    public int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, trans.Connection!, trans, cmdType, cmdText, cmdParms);
        int val = cmd.ExecuteNonQuery();
        cmd.Parameters.Clear();
        return val;
    }

    public SqlDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        var cmd = new SqlCommand();
        var conn = new SqlConnection(connString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        var cmd = new SqlCommand();
        var conn = new SqlConnection(_connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    public SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] cmdParms)
    {
        var cmd = new SqlCommand();
        var conn = new SqlConnection(_connectionString);
        try
        {
            PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
            var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return rdr;
        }
        catch
        {
            conn.Close();
            throw;
        }
    }

    public DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        using var da = new SqlDataAdapter(cmd);
        var ds = new DataSet();
        da.Fill(ds);
        cmd.Parameters.Clear();
        return ds;
    }

    public DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
        using var da = new SqlDataAdapter(cmd);
        var ds = new DataSet();
        da.Fill(ds);
        cmd.Parameters.Clear();
        return ds;
    }

    public object? ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(connString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        object? val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }

    public object? ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
        object? val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }

    public object? ExecuteScalar(string cmdText, params SqlParameter[] cmdParms)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand();
        PrepareCommand(cmd, conn, null, CommandType.StoredProcedure, cmdText, cmdParms);
        object? val = cmd.ExecuteScalar();
        cmd.Parameters.Clear();
        return val;
    }

    public static void CacheParameters(string cacheKey, params SqlParameter[] cmdParms)
    {
        ParmCache[cacheKey] = cmdParms;
    }

    public static SqlParameter[]? GetCachedParameters(string cacheKey)
    {
        var cachedParms = (SqlParameter[]?)ParmCache[cacheKey];
        if (cachedParms == null)
            return null;

        var clonedParms = new SqlParameter[cachedParms.Length];
        for (int i = 0; i < cachedParms.Length; i++)
            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

        return clonedParms;
    }

    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction? trans, CommandType cmdType, string cmdText, SqlParameter[]? cmdParms)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();

        cmd.Connection = conn;
        cmd.CommandText = cmdText;

        if (trans != null)
            cmd.Transaction = trans;

        cmd.CommandType = cmdType;

        if (cmdParms != null)
        {
            foreach (var parm in cmdParms)
                cmd.Parameters.Add(parm);
        }
    }
}
