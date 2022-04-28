using Public.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Public.DbHelper.Sql
{
    public class SqlClient : AbsDatabaseClient
    {
        private static string _connectionString;

        public SqlClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteSql(string sql)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override int Insert<T>(T t, string tableName)
        {
            try
            {
                if (t == null)
                    return 0;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var sql = InsertSQL<T>(t, tableName);
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"写入数据异常 tabname:{tableName}", ex);
                return 0;
            }
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override int Insert<T>(List<T> t, string tableName)
        {
            try
            {
                if (t == null)
                    return 0;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    StringBuilder sb = new StringBuilder();
                    t.ForEach(q => sb.AppendLine(InsertSQL<T>(q, tableName)));
                    var result = ExecuteSql(sb.ToString());
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"写入数据异常 tabname:{tableName}", ex);
                return 0;
            }
        }

        public override (bool, string) ExistInsert<T>(T t, string tableName, string existSql)
        {
            try
            {
                if (t == null)
                    return (false, "数据为空") ;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(existSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return (false, "数据已经存在");

                    var sql = InsertSQL<T>(t, tableName);
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        return (true, cmd.ExecuteNonQuery().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"写入数据异常 tabname:{tableName}", ex);
                return (false, ex.Message);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override int Delete(string tabName, string where)
        {
            try
            {
                var delete = $"delete {tabName} where {where}";
                var temp = this.ExecuteSql(delete);
                return temp;
            }
            catch (Exception ex)
            {
                Logger.Error($"删除数据异常 tabName:{tabName}", ex);
                return 0;
            }
        }

        public override int Delete(string tabName, string recordId = null, string where = null)
        {
            try
            {
                var delete = $"delete {tabName} where recordID = '{recordId}'";
                var temp = this.ExecuteSql(delete);
                return temp;
            }
            catch (Exception ex)
            {
                Logger.Error($"删除数据异常 tabName:{tabName}", ex);
                return 0;
            }
        }

        /// <summary>
        /// 查询 返回DataSet
        /// </summary>
        /// <param name="dataSql"></param>
        /// <returns></returns>
        public override DataTable QueryDt(string dataSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return ds?.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询异常 SQL:{dataSql}", ex);
                return null;
            }
        }

        public override DataSet QueryDs(string dataSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询异常 SQL:{dataSql}", ex);
                return null;
            }
        }

        public override List<T> Query<T>(string dataSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return Tools.Tool.DataTableToList<T>(ds.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常 sql：{dataSql}", ex);
                return null;
            }
        }

        /// <summary>
        /// 查询 返回数据条数和DataTable
        /// </summary>
        /// <param name="countSql"></param>
        /// <param name="dataSql"></param>
        /// <returns></returns>
        public override (int, DataTable) Query(string countSql, string dataSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    SqlCommand cmd = new SqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return (count, dt);
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"查询数据异常", ex);
                return (0, null);
            }
        }

        public override (int, List<T>) Query<T>(string countSql, string dataSql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    SqlCommand cmd = new SqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return (count, Tools.Tool.DataTableToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常", ex);
                return (0, null);
            }
        }

        public override (int, DataTable) Query(string tabName, int pageIndex, int pageSize, string where = null)
        {
            try
            {
                where = string.IsNullOrEmpty(where) ? " 1=1 " : where;
                var countSql = $"select count(0) from {tabName} where {where}";
                var dataSql = MakeSkipSql(tabName, pageIndex, pageSize, where);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    SqlCommand cmd = new SqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return (count, dt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常", ex);
                return (0, null);
            }
        }

        public override (int, List<T>) Query<T>(string tabName, int pageIndex, int pageSize, string where = null)
        {
            try
            {
                where = string.IsNullOrEmpty(where) ? " 1=1 " : where;
                var countSql = $"select count(0) from {tabName} where {where}";
                var dataSql = MakeSkipSql(tabName, pageIndex, pageSize, where);

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    SqlCommand cmd = new SqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    SqlDataAdapter adapter = new SqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return (count, Tools.Tool.DataTableToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常", ex);
                return (0, null);
            }
        }

        public override (int, string) Update<T>(string tabName, string recordID, T t)
        {
            var update = string.Empty; ;
            try
            {
                update = UpdateSQL<T>(t, tabName, recordID);
                var result = ExecuteSql(update);
                return (result, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error($"更新数据异常 sql：{update}", ex);
                return (0, ex.Message);
            }
        }

        /// <summary>
        /// 判读数据是否存在
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override bool Exists(string sql)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    return false;
                else
                    return true;
            }
        }


        #region 私有函数
        /// <summary>
        /// 构造分页查询语句
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        private string MakeSkipSql(string tabName, int pageIndex, int pageSize, string where, string orderby = null)
        {
            var sql = $"select * from " +
                    $"( " +
                    $" select *, row_number() over(order by {(string.IsNullOrEmpty(orderby) ? "createtime desc" : orderby)}) row_idx from {tabName} where {where} " +
                    $") tab " +
                    $"where tab.row_idx between {((pageIndex - 1) * pageSize) + 1} and {pageSize + 1}";

            return sql;
        }

        private static string InsertSQL<T>(T t, string tabName)
        {
            var props = typeof(T).GetProperties();

            StringBuilder sbCol = new StringBuilder();
            StringBuilder sbVal = new StringBuilder();

            foreach (PropertyInfo pi in props)
            {
                var name = pi.Name;
                var ty = pi.PropertyType;
                var val = pi.GetValue(t, null);
                sbCol.Append(name).Append(",");
                sbVal.Append(GetValue(val, ty, true)).Append(",");
            }

            return $"insert into {tabName} " +
                $"({sbCol.ToString().TrimEnd(',')}) " +
                $"values " +
                $"({sbVal.ToString().TrimEnd(',')})";
        }

        private static string UpdateSQL<T>(T t, string tabName, string recordID)
        {
            var props = typeof(T).GetProperties();

            StringBuilder sb = new StringBuilder();
            foreach (PropertyInfo pi in props)
            {
                var name = pi.Name;
                var ty = pi.PropertyType;
                var val = pi.GetValue(t, null);
                sb.Append($"{name} = {GetValue(val, ty)}").Append(",");
            }

            return $"update {tabName} set {sb.ToString().Trim(',')} " +
                $"where recordID = '{recordID}'";
        }

        private static string GetValue(object val, Type ty, bool isOrcl = false)
        {
            var value = val?.ToString();
            var type = ty.ToString().ToLower().Trim().TrimEnd(']');

            if (string.IsNullOrEmpty(value) == true)
                return "''";

            if (type.EndsWith("int32") || type.EndsWith("long") ||
                type.EndsWith("float") || type.EndsWith("double") || type.EndsWith("decimal"))
                return value;
            else if (type.EndsWith("boolean"))
                return value.ToLower() == "true" ? "1" : "0";
            else if (type.EndsWith("string") || type.EndsWith("datetime"))
                return $"'{value}'";
            else  //enum 处理
            {
                string result = null;
                try
                {
                    var en = Enum.IsDefined(ty, val);
                    result = ((int)val).ToString();
                }
                catch (Exception ex)
                {
                    result = "''";
                }
                return result;
            }
        }


        #endregion
    }
}
