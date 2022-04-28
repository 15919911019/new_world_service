using Business.ImageModels.Common;
using CBP.BaseServices;
using CBP.Models;
using Public.Log;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Business.ImageServices.Common
{
    public class ImageService : BaseService, IImageService
    {
        public string HttpImageDomain { get; } = Tool.GetConfig("HttpImageDomain");

        public string ImageSavePath { get; } = Tool.GetConfig("ImageSavePath");

        public override string ServiceName => "Image";


        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="categoryId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Task<ResponseModel> Create(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var data = request?.Data.ToString();
                    if (string.IsNullOrEmpty(data) == true)
                        return ResponseModel.Fail("数据为空");

                    string name = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}" +
                        $"_{new Random().Next(1000, 9999).ToString()}.png";

                    var dir = Path.Combine(this.ImageSavePath, request.UserID);
                    if (Directory.Exists(dir) == false)
                        Directory.CreateDirectory(dir);

                    var fileP = Path.Combine(dir, name);

                    int idx = data.IndexOf(',');
                    string str = data.Substring(idx + 1);
                    byte[] arr = Convert.FromBase64String(str);
                    MemoryStream ms = new MemoryStream(arr);
                    Bitmap bmp = new Bitmap(ms);
                    if (bmp == null)
                        return ResponseModel.Fail("图片转化失败");

                    //var img = Tool.Base64StringToImage(imageDataString);
                    bmp.Save(fileP);

                    return ResponseModel.Success($"{HttpImageDomain}/{request.UserID}/{name}");
                }
                catch (Exception ex)
                {
                    Logger.Error("保存图片异常 1", ex);
                    return ResponseModel.Excetption("保存图片异常", ex);
                }
            });
        }


        /// <summary>
        /// 保存项目图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="categoryId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Task<ResponseModel> Save(string merchantId, string imageDataString, int left, int top, int width, int height)
        {
            return Task.Factory.StartNew(() =>
            {
                Graphics graphic = null;
                System.Drawing.Image saveImage = null;
                Bitmap img = null;

                try
                {
                    string name = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}" +
                    $"_{new Random().Next(1000, 9999).ToString()}.png";

                    var dir = Path.Combine(this.ImageSavePath, merchantId);
                    if (Directory.Exists(dir) == false)
                        Directory.CreateDirectory(dir);

                    var fileP = Path.Combine(dir, name);

                    img = Tool.Base64StringToImage(imageDataString);

                    Bitmap bit = new Bitmap(width, height);//实例化一个和窗体一样大的bitmap
                    Graphics g = Graphics.FromImage(bit);
                    g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                    g.DrawImage(img, 0, 0, new Rectangle(top, left, width, height), GraphicsUnit.Pixel);//保存整个窗体为图片
                    bit.Save(fileP);

                    //graphic = Graphics.FromImage(img);  //截取原图相应区域写入作图区
                    //graphic.DrawImage(img, 0, 0, new Rectangle(top, left, width, height), GraphicsUnit.Pixel); //从作图区生成新图
                    //saveImage = System.Drawing.Image.FromHbitmap(img.GetHbitmap()); //保存图片
                    //saveImage.Save(fileP, ImageFormat.Png); //释放资源   

                    return ResponseModel.Success($"{HttpImageDomain}/{merchantId}/{name}");
                }
                catch (Exception ex)
                {
                    Logger.Error("保存图片异常 2", ex);
                    return ResponseModel.Excetption("保存图片异常", ex);
                }
                finally
                {
                    saveImage?.Dispose();
                    graphic?.Dispose();
                    img?.Dispose();
                }
            });
        }


        /// <summary>
        /// 保存项目图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="categoryId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public Task<ResponseModel> SaveImageNew(Image64StringModel data)
        {
            return Task.Factory.StartNew(() =>
            {
                Bitmap bmp = null;
                Bitmap newImg = null;
                try
                {
                    string name = $"{DateTime.Now.ToString("yyyyMMdd_HHmmss")}" +
                    $"_{new Random().Next(1000, 9999).ToString()}.png";

                    var dir = Path.Combine(this.ImageSavePath, data.MerchantId);
                    if (Directory.Exists(dir) == false)
                        Directory.CreateDirectory(dir);
                    var fileP = Path.Combine(dir, name);

                    var idx = data.Data.IndexOf(',');
                    var str = data.Data.Substring(idx + 1);

                    byte[] temp = Convert.FromBase64String(str);
                    MemoryStream ms = new MemoryStream(temp);
                    bmp = new Bitmap(ms);
                    if (bmp == null)
                        return ResponseModel.Fail("图片转化失败");

                    if (data.Width == 0 || data.Height == 0)
                        bmp.Save(fileP);
                    else
                    {
                        newImg = bmp.Clone(new Rectangle((int)data.Top, (int)data.Left, (int)data.Width, (int)data.Height), bmp.PixelFormat);
                        newImg.Save(fileP);
                    }

                    //压缩图片  H/W = 1
                    Tool.HearderThumbnail(fileP);

                    return ResponseModel.Success(new
                    {
                        HttpUrl = HttpImageDomain,
                        ImagePath = $"{data.MerchantId}/{name}"
                    });
                }
                catch (Exception ex)
                {
                    Logger.Error("保存图片异常 2", ex);
                    return ResponseModel.Excetption("保存图片异常", ex);
                }
                finally
                {
                    bmp?.Dispose();
                    newImg?.Dispose();
                }
            });
        }


        /// <summary>
        /// 删除技师图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="categoryId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<ResponseModel> Delete(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                var fileName = request?.Param?[0].ToString();
                if (string.IsNullOrEmpty(fileName) == true)
                    return ResponseModel.Fail("文件名称为空");

                var file = Path.Combine(ImageSavePath, request.UserID, fileName);
                if (File.Exists(file) == true)
                {
                    File.Delete(file);
                    return ResponseModel.Success("删除成功");
                }

                return ResponseModel.Fail("文件不存在");
            });
        }
    }
}
