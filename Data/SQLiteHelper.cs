/*
 * SQLiteHelper.cs
 * SQLite连接
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
namespace WTTools.Data
{
    public class SQLiteHelper : IDbHelper
    {

        public SQLiteHelper()
        {
            this.ConnectionString = DBHelper.ConnectionString;
        }

        public string ConnectionString { get; set; }

        /// <summary>
        /// 组织Command Parameters字符串
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        private IDbDataParameter[] AddParameters(Hashtable ht)
        {
            IDbDataParameter[] parameterArray = new IDbDataParameter[ht.Count];
            IDictionaryEnumerator enumerator = ht.GetEnumerator();
            for(int i=0;enumerator.MoveNext();i++)
            {
                parameterArray[i] = new SQLiteParameter(enumerator.Key.ToString(), enumerator.Value);
            }
            return parameterArray;
        }


        public DbDataReader ExecReader(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
                connectionstring = this.ConnectionString;
            return this.ExecReader(connectionstring, sql, CommandType.Text, cmdParameters);
        }

        public DbDataReader ExecReader(string connectionstring, string sql, CommandType cmdType)
        {
            DbDataReader reader = null;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");

            SQLiteConnection conn = new SQLiteConnection(connectionstring);
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteTransaction trans = null;
            this.PrepareCommand(cmd, conn, ref trans, false, cmdType, sql,new IDbDataParameter[0]);
            try
            {
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return reader;
        }

        public DbDataReader ExecReader(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters)
        {
            DbDataReader reader = null;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");
            using (SQLiteConnection conn = new SQLiteConnection(connectionstring))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    SQLiteTransaction trans = null;
                    this.PrepareCommand(cmd, conn, ref trans, false, cmdType, sql, cmdParameters);
                    try
                    {
                        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    return reader;
                }
            }

        }

        public object ExecScalar(string sql, Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            return this.ExecScalar(this.ConnectionString, sql, CommandType.Text, cmdParameters);
        }

        public object ExecScalar(string connectionstring, SQLiteCommand cmd)
        {
            object obj = 0;
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new Exception("connectionstring");
            }
            using (SQLiteConnection conn = new SQLiteConnection(connectionstring))
            {
                SQLiteTransaction trans = null;
                this.PrepareCommand(cmd, conn, ref trans, true, cmd.CommandType, cmd.CommandText,new IDbDataParameter[0]);
                try
                {
                    obj = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }                
            }
            return obj;
        }

        public object ExecScalar(string connectionstring, string sql, Hashtable ht)
        {
            IDbDataParameter[] cmdParameters = this.AddParameters(ht);
            return this.ExecScalar(connectionstring, sql, CommandType.Text, cmdParameters);
        }

        public object ExecScalar(string connectionstring, string sql, CommandType cmdType)
        {
            object obj = 0;
            if (string.IsNullOrEmpty(connectionstring))
            {
                throw new Exception("connectionstring");
            }
            if(string.IsNullOrEmpty(sql))
            {
                throw new Exception("sql");
            }
            using (SQLiteConnection conn = new SQLiteConnection(connectionstring))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                SQLiteTransaction trans = null;
                this.PrepareCommand(cmd, conn, ref trans, true, cmdType, sql, new IDbDataParameter[0]);
                try
                {
                    obj = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return obj;
        }

        public object ExecScalar(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters)
        {
            object obj = 0;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");
            using (SQLiteConnection conn = new SQLiteConnection(connectionstring))
            {
                SQLiteCommand cmd = new SQLiteCommand();
                SQLiteTransaction trans = null;
                this.PrepareCommand(cmd, conn, ref trans, true, cmdType, sql, cmdParameters);
                try
                {
                    obj = cmd.ExecuteScalar();
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            return obj;
        }

        public int ExecSql(List<SqlCmdAndParams> lstCmdAndParams)
        {
            int num = 0;
            
            using(SQLiteCommand cmd = new SQLiteCommand())
            {
                using (SQLiteConnection conn = new SQLiteConnection())
                {
                    SQLiteTransaction trans = null;
                    try
                    {
                        foreach(SqlCmdAndParams @params in lstCmdAndParams)
                        {
                            this.PrepareCommand(cmd, conn, ref trans, true, CommandType.Text, @params.CommandText, @params.CommandParameters);
                            num += cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return num;
        }

        public int ExecSql(string sql, Hashtable ht)
        {
            return this.ExecSql(null, sql, ht);
        }

        public int ExecSql(string connectionstring, string sql, Hashtable ht)
        {
            IDbDataParameter[] dataParams = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
                connectionstring = this.ConnectionString;
            return this.ExecSql(connectionstring, sql, CommandType.Text, dataParams);
        }

        public int ExecSql(string connectionstring, string sql, System.Data.CommandType cmdType, params System.Data.IDbDataParameter[] cmdParameters)
        {
            int num = 0;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");

            using(SQLiteConnection conn = new SQLiteConnection())
            {
                using(SQLiteCommand cmd = new SQLiteCommand())
                {
                    SQLiteTransaction trans = null;
                    this.PrepareCommand(cmd, conn, ref trans, true, cmdType, sql, cmdParameters);
                    try
                    {
                        num = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch(Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return num;
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
            int num = 0;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");
            object obj = this.ExecScalar(connectionstring, sql, CommandType.Text);
            if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                num = 0;
            else
                num = int.Parse(obj.ToString());
            if(num==0)
                return false;            
            return true;
        }

        public bool Exists(string connectionstring, string sql, System.Collections.Hashtable ht)
        {
            int num = 0;
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");
            IDbDataParameter[] dataParams = this.AddParameters(ht);
            object obj = this.ExecScalar(connectionstring, sql, CommandType.Text, dataParams);
            if (obj == null || obj == DBNull.Value)
                num = 0;
            else
                num = int.Parse(obj.ToString());
            if (num == 0)
                return false;
            return true;
        }

        public System.Data.DataTable GetPageList(string connectionstring, string tableName, string primaryKey, string showColumns, string where, string orderBy, int pageSize, int pageIndex, out int recordCount)
        {
            DataTable dt = new DataTable();
            recordCount = Convert.ToInt32(this.ExecScalar(connectionstring, "Select COUNT(1) from " + tableName + (string.IsNullOrEmpty(where) ? "" : " where " + where), CommandType.Text));
            
            int num = (pageIndex - 1) * pageSize;
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(string.Format("SELECT {0} FROM {1}",showColumns,tableName));
            if (!string.IsNullOrEmpty(where))
                sbSql.Append(string.Format(" Where {0}",where));
            if (!string.IsNullOrEmpty(orderBy))
                sbSql.Append(string.Format(" Order By {0}", orderBy));
            if(pageSize>1)
                sbSql.Append(string.Format(" LIMIT {0}",pageSize));
            if(num>0)
                sbSql.Append(string.Format(" OFFSET {0}", num));
            string sql = sbSql.ToString();
            using(DbDataReader reader = this.ExecReader(connectionstring,sql,CommandType.Text))
            {
                if (reader != null)
                    dt.Load(reader);
            }
            return dt;
        }

        public DataTable Query(string sql, Hashtable ht)
        {
            return this.Query(null, sql, ht);
        }

        public DataTable Query(string connectionstring, string sql, CommandType cmdType)
        {
            return this.Query(connectionstring, sql, cmdType, new IDbDataParameter[0]);
        }

        public DataTable Query(string connectionstring, string sql, Hashtable ht)
        {
            IDbDataParameter[] dataParams = this.AddParameters(ht);
            if (string.IsNullOrEmpty(connectionstring))
                connectionstring = this.ConnectionString;
            return this.Query(connectionstring, sql, CommandType.Text, dataParams);
        }

        public DataTable Query(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters)
        {
            if (string.IsNullOrEmpty(connectionstring))
                throw new Exception("connectionstring");
            if (string.IsNullOrEmpty(sql))
                throw new Exception("sql");
            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection())
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    SQLiteTransaction trans = null;
                    this.PrepareCommand(cmd, conn, ref trans, false, cmdType, sql, cmdParameters);
                    try
                    {
                        new SQLiteDataAdapter(cmd).Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return dt;
        }

        public void Reset()
        {
            using(SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                using(SQLiteCommand cmd = new SQLiteCommand())
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.Parameters.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "vacuum";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 准备好数据库指令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="useTrans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParamters"></param>
        private void PrepareCommand(SQLiteCommand cmd,SQLiteConnection conn,ref SQLiteTransaction trans,bool useTrans,CommandType cmdType,string cmdText,params IDbDataParameter[] cmdParamters)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if(useTrans)
            {
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParamters != null)
            {
                foreach(var parameter in cmdParamters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}
