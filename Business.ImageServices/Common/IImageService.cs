using Business.ImageModels.Common;
using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.ImageServices.Common
{
    public interface IImageService
    {
        string HttpImageDomain { get; }

        string ImageSavePath { get; }

        /// <summary>
        /// 保存图片 64string
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="imageDataString"></param>
        /// <returns></returns>
        Task<ResponseModel> Create(RequestModel request);

        ///// <summary>
        ///// 保存截图的图片 图片通过64string转化
        ///// </summary>
        ///// <param name="merchantId"></param>
        ///// <param name="imageDataString"></param>
        ///// <param name="left"></param>
        ///// <param name="top"></param>
        ///// <param name="width"></param>
        ///// <param name="height"></param>
        ///// <returns></returns>
        //Task<ResponseModel> Save(string merchantId, string imageDataString, int left, int top, int width, int height);

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ResponseModel> Delete(RequestModel request);

    }
}
