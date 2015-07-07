/*
 * 文件:IDbHelper.cs
 * 功能描述:数据库接口
 * went20150427
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Collections;
namespace WTTools.Data
{
    /// <summary>
    /// 数据库接口
    /// </summary>
    public interface IDbHelper
    {
        DbDataReader ExecReader(string connectionstring, string sql, Hashtable ht);
        DbDataReader ExecReader(string connectionstring, string sql, CommandType cmdType);

        DbDataReader ExecReader(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters);

        object ExecScalar(string sql, Hashtable ht);
        object ExecScalar(string connectionstring, string sql, Hashtable ht);
        object ExecScalar(string connectionstring, string sql, CommandType cmdType);
        object ExecScalar(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters);

        int ExecSql(List<SqlCmdAndParams> lstCmdAndParams);
        int ExecSql(string sql, Hashtable ht);
        int ExecSql(string connectionstring, string sql, Hashtable ht);
        int ExecSql(string connectionstring, string sql, CommandType cmdType,params IDbDataParameter[] cmdParameters);

        bool Exists(string sql);
        bool Exists(string sql, Hashtable ht);
        bool Exists(string connectionstring, string sql);
        bool Exists(string connectionstring, string sql, Hashtable ht);

        DataTable GetPageList(string connectionstring, string tableName, string primaryKey, string showColumns, string where, string orderBy, int pageSize, int pageIndex, out int recordCount);

        DataTable Query(string sql, Hashtable ht);
        DataTable Query(string connectionstring, string sql, CommandType cmdType);
        DataTable Query(string connectionstring, string sql, Hashtable ht);
        DataTable Query(string connectionstring, string sql, CommandType cmdType, params IDbDataParameter[] cmdParameters);

        void Reset();
    }


    /// <summary>
    /// 多个Sql语句
    /// </summary>
    public class SqlCmdAndParams
    {
        public DbParameter[] CommandParameters;
        public string CommandText;
        public CommandType CommandType;
        public Hashtable Parameters;
    }
}
