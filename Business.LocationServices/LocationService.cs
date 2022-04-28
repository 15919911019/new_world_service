using Business.LocationModels;
using CBP.BaseServices;
using CBP.Models;
using Newtonsoft.Json.Linq;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.LocationServices
{
    public class LocationService : BaseService, ILocationService
    {
        public override string ServiceName => "Location";


        public Task<ResponseModel> GetCityCode(double longitude, double latitude)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var ak = Tool.GetConfig("AK");
                    var url = $"http://api.map.baidu.com/reverse_geocoding/v3/?ak={ak}&" +
                    $"output=json&coordtype=wgs84ll&location={latitude},{longitude}";

                    var json = new WebTool().GetHtml(url);
                    if (json == null)
                        return ResponseModel.Error("百度接口请求异常");

                    var jobj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json.Replace("@", ""));
                    if (jobj == null)
                    {
                        Logger.Error($"根据经纬度获取城市代码异常：data {json}");
                        return ResponseModel.Error("根据经纬度获取城市代码异常");
                    }
                    var code = jobj["result"]["addressComponent"]["adcode"]?.ToString();
                    if (code == null)
                    {
                        Logger.Error($"解析城市代码异常：data {json}");
                        return ResponseModel.Error("解析城市代码异常");
                    }

                    var region = AreaInfo.Regional.Find(q => q.value == code);
                    if (region == null)
                        return ResponseModel.Success(code);

                    var city = AreaInfo.City.Find(q => q.value == region.parentVal);
                    if (city.text.StartsWith("市辖") == false)
                        return ResponseModel.Success(city.value);

                    var province = AreaInfo.Province.Find(q => q.value == city.parentVal);
                    return ResponseModel.Success(province.value);
                }
                catch (Exception ex)
                {
                    Logger.Error("GetCityCode", ex);
                    return ResponseModel.Excetption(ex.Message);
                }
            });
        }
    }
}
