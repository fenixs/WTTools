/*
 * SQLHelper.cs
 * SqlServer操作类
 * 20150427went
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Sql;
namespace WTTools.Data
{
    public class SQLHelper : IDbHelper
    {

        public SQLHelper()
        {
            this.ConnectionString = DBHelper.ConnectionString;
        }

        public string ConnectionString { get; set; }


        private IDbDataParameter[] AddParameters(Hashtable ht)
        {
            IDbDataParameter[] parameterArray = new IDbDataParameter[ht.Count];
            IDictionaryEnumerator enumerator = ht.GetEnumerator();
            for (int i = 0; enumerator.MoveNext(); i++)
            {
                parameterArray[i] = new SqlParameter(enumerator.Key.ToString(), enumerator.Value);
            }
            return parameterArray;
        }


        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, ref SqlTransaction trans, bool useTrans, CommandType cmdType, string cmdText, params IDbDataParameter[] cmdParameters)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (useTrans)
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParameters != null)
            {
                foreach (IDbDataParameter parameter in cmdParameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public System.Data.Common.DbDataReader ExecReader(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
            {
                connectionstring = this.ConnectionString;
            }
            return this.ExecReader(connectionstring, sql, CommandType.Text, cmdParameters);
        }

        public System.Data.Common.DbDataReader ExecReader(string connectionstring, string sql, System.Data.CommandType cmdType)
        {
            DbDataReader reader = null;
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new ArgumentNullException("connectionString");
            }
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, false, cmdType, sql, new IDbDataParameter[0]);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                    return reader;
                }
            }
        }

        public System.Data.Common.DbDataReader ExecReader(string connectionstring, string sql, System.Data.CommandType cmdType, params System.Data.IDbDataParameter[] cmdParameters)
        {
            DbDataReader reader = null;
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, false, cmdType, sql, cmdParameters);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                    return reader;
                }
            }
        }

        public object ExecScalar(string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            return this.ExecScalar(this.ConnectionString, sql, CommandType.Text, cmdParameters);
        }

        public object ExecScalar(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            return this.ExecScalar(connectionstring, sql, CommandType.Text, cmdParameters);
        }

        public object ExecScalar(string connectionstring, string sql, System.Data.CommandType cmdType)
        {
            object obj2 = 0;
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlCommand command = new SqlCommand())
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, true, cmdType, sql, new IDbDataParameter[0]);
                    try
                    {
                        obj2 = command.ExecuteScalar();
                        trans.Commit();
                    }
                    catch (Exception exception)
                    {
                        trans.Rollback();
                        throw exception;
                    }
                    return obj2;
                }
            }
        }

        public object ExecScalar(string connectionstring, string sql, System.Data.CommandType cmdType, params System.Data.IDbDataParameter[] cmdParameters)
        {
            object obj2 = 0;
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlCommand command = new SqlCommand())
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, true, cmdType, sql, cmdParameters);
                    try
                    {
                        obj2 = command.ExecuteScalar();
                        trans.Commit();
                    }
                    catch (Exception exception)
                    {
                        trans.Rollback();
                        throw exception;
                    }
                    return obj2;
                }
            }
        }

        public int ExecSql(List<SqlCmdAndParams> lstCmdAndParams)
        {
            int num = 0;
            using (SqlCommand command = new SqlCommand())
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    SqlTransaction trans = null;
                    try
                    {
                        foreach (SqlCmdAndParams @params in lstCmdAndParams)
                        {
                            this.PrepareCommand(command, connection, ref trans, true, CommandType.Text, @params.CommandText, @params.CommandParameters);
                            num += command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    return num;
                }
            }
        }

        public int ExecSql(string sql, System.Collections.Hashtable ht)
        {
            return this.ExecSql(null, sql, ht);
        }

        public int ExecSql(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
            {
                connectionstring = this.ConnectionString;
            }
            return this.ExecSql(this.ConnectionString, sql, CommandType.Text, cmdParameters);
        }

        public int ExecSql(string connectionstring, string sql, System.Data.CommandType cmdType, params System.Data.IDbDataParameter[] cmdParameters)
        {
            int num = 0;
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlCommand command = new SqlCommand())
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, true, cmdType, sql, cmdParameters);
                    try
                    {
                        num = command.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception exception)
                    {
                        trans.Rollback();
                        throw exception;
                    }
                    return num;
                }
            }
        }

        public bool Exists(string sql)
        {
            return this.Exists(null, sql);
        }

        public bool Exists(string sql, System.Collections.Hashtable ht)
        {
            return this.Exists(null, sql, ht);
        }

        public bool Exists(string connectionstring, string sql)
        {
            int num;
            if (string.IsNullOrEmpty(connectionstring))
            {
                connectionstring = this.ConnectionString;
            }
            object objA = this.ExecScalar(connectionstring, sql, CommandType.Text);
            if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(objA.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public bool Exists(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            int num;
            if (string.IsNullOrEmpty(connectionstring))
            {
                connectionstring = this.ConnectionString;
            }
            object objA = this.ExecScalar(connectionstring, sql, ht);
            if (object.Equals(objA, null) || object.Equals(objA, DBNull.Value))
            {
                num = 0;
            }
            else
            {
                num = int.Parse(objA.ToString());
            }
            if (num == 0)
            {
                return false;
            }
            return true;
        }

        public System.Data.DataTable GetPageList(string connectionstring, string tableName, string primaryKey, string showColumns, string where, string orderBy, int pageSize, int pageIndex, out int recordCount)
        {
            DataTable table;
            new DataTable();
            recordCount = Convert.ToInt32(this.ExecScalar(connectionstring, "SELECT COUNT(1) FROM " + tableName + " " + where, CommandType.Text));
            StringBuilder builder = new StringBuilder();
            recordCount = 0;
            try
            {
                int num = (pageIndex - 1) * pageSize;
                int num2 = pageIndex * pageSize;
                builder.Append("Select * From (Select ROW_NUMBER() Over (Order By " + orderBy);
                builder.Append(string.Concat(new object[] { ") As rowNum, * From tableName ) As N Where rowNum > ", num, " And rowNum <= ", num2 }));
                recordCount = Convert.ToInt32(this.ExecScalar(connectionstring, "Select Count(1) From tableName Where " + where, CommandType.Text));
                table = this.Query(connectionstring, builder.ToString(), CommandType.Text);
            }
            catch (Exception exception)
            {
                throw new Exception("", exception);
            }
            return table;
        }

        public System.Data.DataTable Query(string sql, System.Collections.Hashtable ht)
        {
            return this.Query(this.ConnectionString, sql, ht);
        }

        public System.Data.DataTable Query(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
            {
                connectionstring = this.ConnectionString;
            }
            return this.Query(connectionstring, sql, CommandType.Text, cmdParameters);
        }

        public System.Data.DataTable Query(string connectionstring, string sql, System.Data.CommandType cmdType)
        {
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, false, cmdType, sql, new IDbDataParameter[0]);
                    try
                    {
                        new SqlDataAdapter(command).Fill(dataTable);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                    return dataTable;
                }
            }
        }

        public System.Data.DataTable Query(string connectionstring, string sql, System.Data.CommandType cmdType, params System.Data.IDbDataParameter[] cmdParameters)
        {
            if ((connectionstring == null) || (connectionstring.Length == 0))
            {
                throw new ArgumentNullException("connectionString");
            }
            if ((sql == null) || (sql.Length == 0))
            {
                throw new ArgumentNullException("sql");
            }
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    SqlTransaction trans = null;
                    this.PrepareCommand(command, connection, ref trans, false, cmdType, sql, cmdParameters);
                    try
                    {
                        new SqlDataAdapter(command).Fill(dataTable);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                    return dataTable;
                }
            }
        }

        public void Reset()
        {
            
        }
    }
}
