using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Public.Tools
{
    public class Tool
    {
        private const string SKey = "abc!@#...";

        #region json exchange obj

        /// <summary>
        /// 对象转换成json串 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }

        /// <summary> 
        /// 从一个Json串生成对象信息 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="jsonString"></param> 
        /// <returns></returns> 
        public static T JsonToObject<T>(string jsonString)
        {
            if (jsonString != "" || jsonString != null)
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return default(T);
            }
        }

        #endregion


        #region File

        /// <summary>
        /// 保存字符串到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="isAppend"></param>
        /// <returns></returns>
        public static bool WriteFile(string path, string content, bool isAppend = false)
        {
            try
            {
                var dir = Path.GetDirectoryName(path);

                if (string.IsNullOrWhiteSpace(dir) == false && Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                if (isAppend)
                {
                    content += Environment.NewLine;
                }

                FileStream stream = null;
                if (File.Exists(path) == false)
                    stream = File.Create(path);
                else
                    stream = File.Open(path, isAppend ? FileMode.Append : FileMode.Truncate);
                var sw = new StreamWriter(stream);
                sw.Write(content);
                sw.Close();
                stream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion


        #region DataTable 与 List 的相互转化

        public static DataTable ListToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// DataTable To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(
            DataTable dt,
            Dictionary<string, string> dictColumnEN_CN = null,
            Dictionary<object, object> dictValueRepleace = null) where T : new()
        {
            // 定义集合    
            List<T> ts = new List<T>();

            if (dt == null)
                return ts;

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    try
                    {
                        // 确定列名
                        if (dictColumnEN_CN != null && dictColumnEN_CN.Keys.Contains(pi.Name))
                            tempName = dictColumnEN_CN[pi.Name];
                        else
                            tempName = pi.Name;

                        if (dt.Columns.Contains(tempName))
                        {
                            // 判断此属性是否有Setter      
                            if (!pi.CanWrite) continue;

                            object value = dr[tempName];
                            var ty = pi.PropertyType.ToString().ToLower();

                            if (dictValueRepleace != null && dictValueRepleace.Keys.Contains(value) == true)
                                value = dictValueRepleace[value];

                            if (value != DBNull.Value)
                            {
                                if (ty.Contains("int") == true)
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                else if (ty.Contains("long") == true)
                                    pi.SetValue(t, long.Parse(value.ToString()), null);
                                else if (ty.Contains("decimal") == true)
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                else if (ty.Contains("float") == true)
                                    pi.SetValue(t, float.Parse(value.ToString()), null);
                                else if (ty.Contains("bool") == true)
                                    pi.SetValue(t, bool.Parse(value.ToString()), null);
                                else if (ty.Contains("date") == true)
                                    pi.SetValue(t, DateTime.Parse(value.ToString()), null);
                                else
                                    pi.SetValue(t, value.ToString(), null);
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
                ts.Add(t);
            }
            return ts;
        }

        #endregion


        #region XML to Entity

        public static T ParserXml<T>(string xmlString) where T : new()
        {
            XmlDocument xml = new XmlDocument();
            var ff = xmlString.Replace(" ", "").Replace("\\n", "").Replace("\"", "");
            xml.LoadXml(ff);
            XmlNode root = xml.SelectSingleNode("xml");
            XmlNodeList nodes = root.ChildNodes;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int j = 0; j < nodes.Count; j++)
            {
                var name = nodes[j].Name.Trim();
                var value = nodes[j].InnerText.Trim();
                if (dict.ContainsKey(name) == false)
                    dict.Add(name, value);
            }

            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (dict.ContainsKey(pi.Name))
                    pi.SetValue(t, dict[pi.Name], null);
            }

            return t;
        }


        #endregion


        #region 文件和二进制流之间的转化

        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
                return new byte[0];

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }

        /// <summary>
        /// 将byte数组转换为文件并保存到指定地址
        /// </summary>
        /// <param name="buff">byte数组</param>
        /// <param name="savepath">保存地址</param>
        public static void Bytes2File(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }

        #endregion


        #region 时间与长整型之间的转化
        /// <summary>
        /// 时间转换成长整形
        /// </summary>
        /// <param name="TheDate">时间</param>
        /// <returns></returns>
        public static long DatetimeToLong(DateTime TheDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = TheDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 长整形转换成时间
        /// </summary>
        /// <param name="milliTime">长整形</param>
        /// <returns></returns>
        public static DateTime LongToDatetime(long milliTime)
        {
            long timeTricks = new DateTime(1970, 1, 1).Ticks + milliTime * 10000 + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours * 3600 * (long)10000000;
            return new DateTime(timeTricks);
        }
        #endregion


        #region GetData from File
        /// <summary>
        /// 读取路径下的文件，根据扩展名
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        /// <param name="strExtension">扩展名</param>
        public static string GetFileFromPath(string strPath, string strExtension)
        {
            string strResult = "";
            string strFileName = "";
            DirectoryInfo dInfo = new DirectoryInfo(strPath);
            foreach (FileInfo fInfo in dInfo.GetFiles(string.Format("*.{}", strExtension)))
            {
                strFileName = dInfo.FullName;
                strResult = File.ReadAllText(strFileName);
            }
            return strResult;
        }
        #endregion


        #region Ftp <==> File
        /// <summary>
        /// 从Ftp上下载文件
        /// </summary>
        /// <param name="strIp">ip地址</param>
        /// <param name="strUser">用户名</param>
        /// <param name="strPassword">密码</param>
        /// <param name="strFileName">文件名</param>
        /// <returns>文件内容</returns>
        public static string DownloadFileFromFtp(string strIp, string strUser, string strPassword, string strFileName)
        {
            FtpWebRequest ftp;
            ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strIp + "/" + strFileName));
            //指定用户名和密码 
            ftp.Credentials = new NetworkCredential(strUser, strPassword);
            WebResponse wr = ftp.GetResponse();
            StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.Default);
            string strGet = "";
            StringBuilder sb = new StringBuilder();
            while (!string.IsNullOrEmpty(strGet = sr.ReadLine()))
            {
                sb.AppendLine(strGet);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 上传文件到Ftp服务上
        /// </summary>
        /// <param name="strPathFile">上传的文件路劲</param>
        public static void UpLoadFileToFtp(string strIp, string strUser, string strPassword, string strPathFile)
        {
            //构造一个web服务器的请求对象 
            FtpWebRequest ftp;
            //实例化一个文件对象 
            FileInfo f = new FileInfo(strPathFile);
            ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strIp + "/" + f.Name));
            //创建用户名和密码 
            ftp.Credentials = new NetworkCredential(strUser, strPassword);
            ftp.KeepAlive = false;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            ftp.UseBinary = true;
            ftp.ContentLength = f.Length;
            int buffLength = 20480;
            byte[] buff = new byte[buffLength];
            int contentLen;
            try
            {
                //获得请求对象的输入流 
                FileStream fs = f.OpenRead();
                Stream sw = ftp.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    sw.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion


        #region Create Entity Class

        /// <summary>
        /// 从数据库中获取所有表  和 表字段信息 的SQL语句
        /// </summary>
        private static string _sql = @"SELECT 表名      =CASE WHEN a.colorder=1 THEN d.name ELSE '' END
        , 表说明 = CASE WHEN a.colorder=1 THEN ISNULL(f.value,'') ELSE '' END
        ,字段序号   =a.colorder
        ,字段名    =a.name
        ,标识     =CASE WHEN COLUMNPROPERTY(a.id, a.name,'IsIdentity')=1 THEN '√'ELSE '' END
        ,主键     =CASE WHEN EXISTS(SELECT 1 FROM sysobjects WHERE xtype = 'PK' AND name IN (
                    SELECT name FROM sysindexes WHERE indid IN(
                        SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid
                ))) THEN '√' ELSE '' END
        ,类型     =b.name
        ,占用字节   =a.length
        ,长度     =COLUMNPROPERTY(a.id, a.name,'PRECISION')
        ,小数位数   =ISNULL(COLUMNPROPERTY(a.id, a.name,'Scale'),0)
        ,允许空    =CASE WHEN a.isnullable=1 THEN '√'ELSE '' END
        ,默认值    =ISNULL(e.text,'')
        ,字段说明   =ISNULL(g.[value],'')
        FROM syscolumns a
        LEFT JOIN systypes b ON a.xusertype=b.xusertype
        INNER JOIN sysobjects d ON a.id=d.id AND d.xtype='U' AND d.name<>'dtproperties'
        LEFT JOIN syscomments e ON a.cdefault= e.id
        LEFT JOIN sys.extended_properties g ON a.id= g.major_id AND a.colid= g.minor_id
        LEFT JOIN sys.extended_properties f ON d.id= f.major_id AND f.minor_id= 0
        -- WHERE d.name= 'orders'--如果只查询指定表, 加上此条件
        ORDER BY a.id, a.colorder";

        public static string CreateEntityClass(string info, string iNotifyString)
        {
            StringBuilder sbResult = new StringBuilder();
            var rows = Regex.Split(info, "\r\n|\r");
            foreach (var row in rows)
            {
                if (string.IsNullOrEmpty(row) == true)
                    continue;

                var columns = Regex.Split(row, "\t");

                //属性赋值，需根据具体情况调整
                var field = columns[0];
                var type = columns.Length > 1 ? columns[3] : string.Empty;
                var remark = columns.Length > 2 ? columns[9] : string.Empty;

                sbResult.AppendLine(@"// <summary>");
                sbResult.AppendLine($"// {remark}");
                sbResult.AppendLine(@"// <summary>");
                sbResult.AppendLine($"public {PaserType(type)} {field}");
                sbResult.AppendLine("{");
                sbResult.AppendLine($"get {{ return {field};}}");
                sbResult.AppendLine($"set {{{GetFiled(field)} = value; {GetCondition(field, iNotifyString)}}}");
                sbResult.AppendLine("}");
                sbResult.AppendLine($"private {PaserType(type)} {GetFiled(field)};");
                sbResult.AppendLine();
            }

            return sbResult.ToString();
        }

        private static string PaserType(string type)
        {

            switch (type.ToLower())
            {
                case "int":
                    {
                        return "int";
                    }
                case "long":
                    {
                        return "long";
                    }
                case "bit":
                    {
                        return "int";
                    }
                case "datetime":
                    {
                        return "DateTime";
                    }
                case "decimal":
                    {
                        return "decimal";
                    }
                case "bool":
                    {
                        return "bool";
                    }
                case "boolean":
                    {
                        return "bool";
                    }
                default:
                    {
                        return "string";
                    }
            }
        }

        private static string GetFiled(string field)
        {
            if (string.IsNullOrEmpty(field) == true)
                return string.Empty;

            var header = field.Substring(0, 1).ToLower();
            var contant = field.Length > 1 ? field.Substring(1) : string.Empty;
            return $"_{header}{contant}";
        }

        private static string GetCondition(string field, string iNotifyString)
        {
            return $"{iNotifyString}(() => {field});";
        }

        #endregion


        #region 读配置文件

        /// <summary>
        /// 获取webapi中的配置项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfig(string name)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
                var config = builder.Build();
                return config[name];
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        #endregion



        #region other

        public static string GetGuid()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        public static string GetRandom(int lenght)
        {
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            StringBuilder sb = new StringBuilder();
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < lenght; i++)
            {
                int rnd = random.Next(0, n);
                sb.Append(Pattern[rnd]);
            }

            return sb.ToString().ToUpper();
        }

        #endregion


        #region 经纬度 距离 换算

        private const double EARTH_RADIUS = 6378137;

        /// <summary>
        /// 计算两点位置的距离，返回两点的距离，单位：米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="lng1">第一点经度</param>
        /// <param name="lat1">第一点纬度</param>        
        /// <param name="lng2">第二点经度</param>
        /// <param name="lat2">第二点纬度</param>
        /// <returns></returns>
        public static double GetDistance(double lng1, double lat1, double lng2, double lat2, bool isKm = true)
        {
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return isKm ? (result / 1000) : result;
        }

        /// <summary>
        /// 根据一个给定经纬度的点和距离，进行附近地点查询
        /// </summary>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <param name="distance">距离（单位：公里或千米）</param>
        /// <returns>返回一个范围的4个点，最小纬度和纬度，最大经度和纬度</returns>
        public static PositionModel FindNeighPosition(double longitude, double latitude, double distance)
        {
            //先计算查询点的经纬度范围  
            double r = 6378.137;//地球半径千米  
            double dis = distance;//千米距离    
            double dlng = 2 * Math.Asin(Math.Sin(dis / (2 * r)) / Math.Cos(latitude * Math.PI / 180));
            dlng = dlng * 180 / Math.PI;//角度转为弧度  
            double dlat = dis / r;
            dlat = dlat * 180 / Math.PI;
            double minlat = latitude - dlat;
            double maxlat = latitude + dlat;
            double minlng = longitude - dlng;
            double maxlng = longitude + dlng;
            return new PositionModel
            {
                MinLatitude = minlat,
                MaxLatitude = maxlat,
                MinLongitude = minlng,
                MaxLongitude = maxlng
            };
        }

        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }

        #endregion


        #region MD5

        /// <summary>
        /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="password">待加密的字符串</param>
        /// <param name="bit">位数，一般取值16 或 32</param>
        /// <returns>返回的加密后的字符串</returns>
        public static string MD5Encrypt(string password, int bit)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            if (bit == 16)
                return tmp.ToString().Substring(8, 16);
            else
            if (bit == 32) return tmp.ToString();//默认情况
            else return string.Empty;
        }

        /// <summary> /// 加密字符串   
        /// </summary>  
        /// <param name="str">要加密的字符串</param>  
        /// <param name="key">秘钥</param>  
        /// <returns>加密后的字符串</returns>  
        public static string Md5Encrypt(string str, string myKey = SKey)
        {
            string encryptKeyall = Convert.ToString(myKey);    //定义密钥  
            if (encryptKeyall.Length < 9)
            {
                for (; ; )
                {
                    if (encryptKeyall.Length < 9)
                        encryptKeyall += encryptKeyall;
                    else
                        break;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 8);
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            byte[] key = Encoding.UTF8.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            byte[] data = Encoding.UTF8.GetBytes(str);//定义字节数组，用来存储要加密的字符串  
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      
            //使用内存流实例化加密流对象   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据      
            CStream.FlushFinalBlock();              //释放加密流      

            return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
        }


        /// <summary>  
        /// 解密字符串   
        /// </summary>  
        /// <param name="str">要解密的字符串</param>  
        ///  <param name="myKey">秘钥</param>  
        /// <returns>解密后的字符串</returns>  
        public static string Md5Decrypt(string str, string myKey = SKey)
        {
            string encryptKeyall = Convert.ToString(myKey);    //定义密钥  
            if (encryptKeyall.Length < 9)
            {
                for (; ; )
                {
                    if (encryptKeyall.Length < 9)
                        encryptKeyall += encryptKeyall;
                    else
                        break;
                }
            }
            string encryptKey = encryptKeyall.Substring(0, 8);
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    
            byte[] key = Encoding.UTF8.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            byte[] data = Convert.FromBase64String(str);//定义字节数组，用来存储要解密的字符串  
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      
            //使用内存流实例化解密流对象       
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);      //向解密流中写入数据     
            CStream.FlushFinalBlock();               //释放解密流      

            return Encoding.UTF8.GetString(MStream.ToArray());       //返回解密后的字符串  
        }


        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// MD5流加密
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static string GenerateMD5(Stream inputStream)
        {
            using (MD5 mi = MD5.Create())
            {
                //开始加密
                byte[] newBuffer = mi.ComputeHash(inputStream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        #endregion


        #region
        public static string ImgToBase64String(string Imagefilename)
        {
            Bitmap bmp = new Bitmap(Imagefilename);

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] arr = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(arr, 0, (int)ms.Length);
            ms.Close();
            return Convert.ToBase64String(arr);
        }

        //base64编码的字符串转为图片
        public static Bitmap Base64StringToImage(string strbase64, string path = null)
        {
            MemoryStream ms = null;
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64.Replace("data:image/png;base64,", ""));
                ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                //bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                //ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                ms.Close();
            }
        }

        public static string GetSpellCode(string CnStr)
        {

            string strTemp = "";
            int iLen = CnStr.Length;
            int i = 0;
            for (i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));
            }

            return strTemp;
        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <returns>单个大写字母</returns>

        private static string GetCharSpellCode(string CnChar)
        {
            long iCnChar;
            byte[] ZW = System.Text.Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回
            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }
            else
            {
                // get the array of byte from the single char
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }

            // iCnChar match the constant
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
                return "A";
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
                return "B";
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
                return "C";
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
                return "D";
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
                return "E";
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
                return "F";
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
                return "G";
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
                return "H";
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
                return "J";
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
                return "K";
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
                return "L";
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
                return "M";
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
                return "N";
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
                return "O";
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
                return "P";
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
                return "Q";
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
                return "R";
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
                return "S";
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
                return "T";
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
                return "W";
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
                return "X";
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
                return "Y";
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
                return "Z";
            else
                return ("?");
        }

        public static bool SendCode(string mobile, string temlateCode, string code)
        {
            try
            {
                IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAI4Fm7QmFAgMk9hYAu3Qpt", "vCc7baaC4aq2SDyvQ6NArbWkBzBneO");
                DefaultAcsClient client = new DefaultAcsClient(profile);
                CommonRequest request = new CommonRequest();
                request.Method = MethodType.POST;
                request.Domain = "dysmsapi.aliyuncs.com";
                request.Version = "2017-05-25";
                request.Action = "SendSms";
                request.AddQueryParameters("PhoneNumbers", mobile);
                request.AddQueryParameters("SignName", "DandingMessa");
                request.AddQueryParameters("TemplateCode", temlateCode);
                request.AddQueryParameters("TemplateParam", "{code:" + code + "}");

                CommonResponse response = client.GetCommonResponse(request);
                var view = System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static string UrlSeCode(string url, Encoding encoding = null)
        {
            Encoding encod = Encoding.UTF8;
            if (encoding != null)
            {
                encod = encoding;
            }
            return HttpUtility.UrlEncode(url, encod);
        }

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth">宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();

            }
        }

        /// <summary>
        /// 头像图片压缩
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool HearderThumbnail(string file)
        {
            Image iSource = null;
            ImageFormat tFormat = null;
            Bitmap ob = null;
            try
            {
                iSource = Image.FromFile(file);
                tFormat = iSource.RawFormat;
                int sW = 0, sH = 0;

                var rate = iSource.Height / iSource.Width;
                if (rate < 1)
                    return true;
                else
                    sW = sH = (iSource.Width >= 200 ? 200 : 100);

                ob = new Bitmap(sW, sH);
                Graphics g = Graphics.FromImage(ob);
                g.Clear(Color.WhiteSmoke);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(iSource, new Rectangle(0, 0, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
                g.Dispose();
                //以下代码为保存图片时，设置压缩质量
                EncoderParameters ep = new EncoderParameters();
                long[] qy = new long[1];
                qy[0] = 100;//设置压缩的比例1-100
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
                ep.Param[0] = eParam;

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Dispose();
                    ob.Save(file, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(file, tFormat);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                iSource?.Dispose();
                ob?.Dispose();
            }
        }
        #endregion


        #region MakeSQL

        public static string InsertOrclSQL<T>(T t, string tabName)
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

        private static string GetValue(object val, Type ty, bool isOrcl = false)
        {
            var value = val?.ToString();
            var type = ty.ToString().ToLower();

            if (string.IsNullOrEmpty(value) == true)
                return "''";

            if (type.EndsWith("int32") || type.EndsWith("long") ||
                type.EndsWith("float") || type.EndsWith("double"))
                return value;
            else if (type.EndsWith("datetime"))
                return isOrcl == true ? $"to_date('{value}','yyyy-MM-dd hh24:mi:ss')" : $"'{value}'";
            else if (type.EndsWith("boolean"))
                return value.ToLower() == "true" ? "1" : "0";
            else if (type.EndsWith("string"))
                return $"'{value}'";
            else  //meun 处理
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

    public class PositionModel
    {
        /// <summary>
        /// 最小的纬度
        /// </summary>
        public double MinLatitude { get; set; }

        /// <summary>
        /// 最大的纬度
        /// </summary>
        public double MaxLatitude { get; set; }

        /// <summary>
        /// 最小的经度
        /// </summary>
        public double MinLongitude { get; set; }

        /// <summary>
        /// 最大的经度
        /// </summary>
        public double MaxLongitude { get; set; }
    }
}