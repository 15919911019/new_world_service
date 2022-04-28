using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBP.Permission.Services.Role
{
    public interface IRoleService
    {
        public string RoleTabName { get; }

        public string RoleFunctionTabName { get; }

        public string RoleMenuTabName { get; }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<ResponseModel> Create(string name);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public Task<ResponseModel> Delete(string recordID);

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ResponseModel> Search(RequestModel request);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResponseModel> Update(string id, string name);


        public Task<ResponseModel> CreateRoleFunc(string roleID, string functionID);

        /// <summary>
        /// 删除角色-功能
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteRoleFunc(string roleID, string funcID);

        public Task<ResponseModel> SearchFunctionByRoleID(string roleID);


        public Task<ResponseModel> CreateRoleMenu(string roleID, string menuID);

        /// <summary>
        /// 删除角色-功能
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public Task<ResponseModel> DeleteRoleMenu(string roleID, string menuID);

        /// <summary>
        /// 查询角色-功能
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ResponseModel> SearchRoleMenu(RequestModel request);

        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        public Task<ResponseModel> GetAllRole();
    }
}
