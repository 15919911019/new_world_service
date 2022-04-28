using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Public.DbHelper
{
    /// <summary>
    /// 数据库抽象函数
    /// </summary>
    public abstract class AbsDatabaseClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual int ExecuteSql(string sql)
        {
            return 0;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public virtual int Insert<T>(T t, string tabName)
        {
            return 0;
        }

        public virtual int Insert<T>(List<T> t, string tabName)
        {
            return 0;
        }

        public virtual (bool, string) ExistInsert<T>(T t, string tabName, string existSql)
        {
            return (true, string.Empty);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete(string tabName, string where)
        {
            return 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete(string tabName, string recordId = null, string where = null)
        {
            return 0;
        }

        public virtual DataTable QueryDt(string dataSql)
        {
            return null;
        }

        public virtual DataSet QueryDs(string dataSql)
        {
            return null;
        }

        public virtual List<T> Query<T>(string dataSql) where T : new()
        {
            return null;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="countSql"></param>
        /// <param name="dataSql"></param>
        /// <returns></returns>
        public virtual (int, DataTable) Query(string countSql, string dataSql)
        {
            return (0, null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="countSql"></param>
        /// <param name="dataSql"></param>
        /// <returns></returns>
        public virtual (int, List<T>) Query<T>(string countSql, string dataSql) where T : new()
        {
            return (0, null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual (int,DataTable) Query(string tabName, int pageIndex, int pageSize, string where = null)
        {
            return (0, null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tabName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual (int, List<T>) Query<T>(string tabName, int pageIndex, int pageSize, string where = null) where T : new()
        {
            return (0, null);
        }

        public virtual (int, string) Update<T>(string tabName, string recordID, T t)
        {
            return (0, string.Empty);
        }

        public virtual (int, string) Update<T>(string tabName, List<T> data)
        {
            return (0, string.Empty);
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual bool Exists(string sql)
        {
            return false;
        }
    }
}
