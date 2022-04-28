//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Business.ImageServices.Common;
//using CBP.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace CBP.Main.Controllers.Business.Image
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImageController : BaseController
//    {
//        private readonly IImageService _service;

//        public ImageController(IImageService service)
//        {
//            _service = service;
//        }

//        [HttpGet, Route("create")]
//        public Task<ResponseModel> Create(string merchantID, string imageDataString)
//        {
//            return _service.Save(merchantID, imageDataString);
//        }

//        [HttpGet, Route("delete")]
//        public Task<ResponseModel> Delete(string merchantID, string name)
//        {
//            return _service.Delete(merchantID, name);
//        }
//    }
//}