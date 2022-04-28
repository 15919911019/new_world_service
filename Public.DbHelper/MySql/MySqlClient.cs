using MySql.Data.MySqlClient;
using Public.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Public.DbHelper.MySql
{
    public class MySqlClient : AbsDatabaseClient
    {
        private static string _connectionString;
        private MySqlConnection _connect;

        public MySqlClient(string connectionString)
        {
            _connectionString = connectionString;

            _connect = new MySqlConnection(_connectionString);
            _connect.Open();
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override int ExecuteSql(string sql)
        {
            return MySqlHelper.ExecuteNonQuery(_connect, sql);
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

                var sql = InsertSQL<T>(t, tableName);
                return MySqlHelper.ExecuteNonQuery(_connect, sql);
            }
            catch (Exception ex)
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

                StringBuilder sb = new StringBuilder();
                t.ForEach(q => sb.AppendLine(InsertSQL<T>(q, tableName)));
                return MySqlHelper.ExecuteNonQuery(_connect, sb.ToString());
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
                    return (false, "数据为空");

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(existSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return (false, "数据已经存在");

                    var sql = InsertSQL<T>(t, tableName);
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        var count = cmd.ExecuteNonQuery().ToString();
                        conn.Close();
                        return (true, count);
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
                var delete = $"delete from {tabName} where {where}";
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
                var delete = $"delete from {tabName} where recordID = '{recordId}'";
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
        public override DataTable QueryDt(string sql)
        {
            try
            {
                DataTable dt = MySqlHelper.ExecuteDataset(_connect, sql)?.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Error($"查询异常 SQL:{sql}", ex);
                return null;
            }
        }

        public override DataSet QueryDs(string sql)
        {
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(_connect, sql);
                return ds;
            }
            catch (Exception ex)
            {
                Logger.Error($"查询异常 SQL:{sql}", ex);
                return null;
            }
        }

        public override List<T> Query<T>(string sql)
        {
            try
            {
                DataTable dt = MySqlHelper.ExecuteDataset(_connect, sql)?.Tables[0];
                return Tools.Tool.DataTableToList<T>(dt);
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常 sql：{sql}", ex);
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
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    MySqlCommand cmd = new MySqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        int.TryParse(obj.ToString(), out count);

                    if (count == 0)
                        return (0, null);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();
                    return (count, dt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"查询数据异常", ex);
                return (0, null);
            }
        }

        public override (int, List<T>) Query<T>(string countSql, string dataSql)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    MySqlCommand cmd = new MySqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();
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
                var countSql = $"select count(0) from `{tabName}` where {where}";
                var dataSql = MakeSkipSql(tabName, pageIndex, pageSize, where);

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    int count = 0;
                    MySqlCommand cmd = new MySqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (int)obj;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();
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

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    Int64 count = 0;
                    MySqlCommand cmd = new MySqlCommand(countSql, conn);
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        return (0, null);
                    else
                        count = (Int64)obj;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(dataSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();
                    return ((int)count, Tools.Tool.DataTableToList<T>(dt));
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
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object obj = cmd.ExecuteScalar();
                conn.Close();
                if (Object.Equals(obj, null) || Object.Equals(obj, System.DBNull.Value) || int.Parse(obj.ToString()) == 0)
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
            var sql = $"select * from {tabName} where {where} " +
                      $"order by '{orderby}'" +
                      $"limit {((pageIndex - 1) * pageSize)}, {pageSize}";

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
                sbCol.Append($"`{name}`").Append(",");
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
