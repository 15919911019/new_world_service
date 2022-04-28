using CBP.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBP.Permission.Services.UserRole
{
    public interface IUserRoleService
    {
        /// <summary>
        /// 用户-角色 表名
        /// </summary>
        public string UserRoleTName { get; }

        /// <summary>
        /// 创建用户-角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public Task<ResponseModel> Create(string userID, string roleID);

        /// <summary>
        /// 删除用户-角色
        /// </summary>
        /// <param name="recordID"></param>
        /// <returns></returns>
        public Task<ResponseModel> Delete(string userID, string roleID);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ResponseModel> Search(RequestModel request);
    }
}
