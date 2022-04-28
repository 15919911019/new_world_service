
using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBP.Permission.Services.Function
{
    public interface IFunctionService 
    {
        public string FunctionTabName { get; }

        /// <summary>
        /// 新增功能
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<ResponseModel> Create(string name,string value);

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel> Delete(string id);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResponseModel> Search(RequestModel request);

        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseModel> Update(string id, string name, string value);

        /// <summary>
        /// 查询所有功能
        /// </summary>
        /// <returns></returns>
        Task<ResponseModel> GetAllFunction();
    }
}
