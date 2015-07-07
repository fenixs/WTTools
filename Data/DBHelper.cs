/*
 * DBHelper.cs
 * 数据库连接
 * 20150427Went
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTTools.Data
{
    class DBHelper
    {
        private static IDbHelper _helper;
        private static object _lock = new object();
        public static string ConnectionString = "";
        public static DataBaseType DatabaseType = DataBaseType.SQLite;

        /// <summary>
        /// 数据库连接实例
        /// </summary>
        public static IDbHelper Instance
        {
            get{
                if(_helper == null)
                {
                    GetHelper(ConnectionString);
                }
                return _helper;
            }
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="conn"></param>
        private static void GetHelper(string conn)
        {
            //判断是否需要新建连接
            if (ConnectionString != conn || _helper == null)
            {
                //如果原来的连接存在，释放
                if (_helper != null && !string.IsNullOrEmpty(conn))
                {
                    _helper = null;
                }
                if (_helper == null)
                {
                    lock (_lock)
                    {
                        if (_helper == null)
                        {
                            GetHelper(conn, DatabaseType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新建数据库连接
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sourceType"></param>
        private static IDbHelper GetHelper(string conn,DataBaseType sourceType)
        {
            if (string.IsNullOrEmpty(conn))
            {
                conn = ConnectionString;
            }
            switch (sourceType)
            {
                case DataBaseType.SQLite:
                    {
                        _helper = new SQLiteHelper();
                        break;
                    }
                case DataBaseType.SQLServer:
                    {
                        break;
                    }
                //default:
                //    {
                //        break;
                //    }
            }
            return _helper;
        }
    }

    public enum DataBaseType
    {
        SQLServer = 0,
        SQLite = 1
    }
}
