using CBP.BaseServices;
using CBP.Models;
using Public.Tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CBP.Permission.Services.Permission
{
    public class PermissionService : BaseService, IPermissionService
    {
        public override string ServiceName => "Permission";

        public PermissionService() { }

        public Task<ResponseModel> GetPermission(RequestModel request)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    List<Menu> lst = new List<Menu>();

                    Menu menu = new Menu();
                    menu.path = "/goods";
                    menu.name = "goods";
                    menu.title = "商品管理";
                    menu.children = new List<Menu>();
                    menu.children.Add(new Menu() { path = "/goods", name = "goods", title = "商品信息" });
                    menu.children.Add(new Menu() { path = "/category", name = "category", title = "商品种类" });
                    lst.Add(menu);

                    Menu menuP = new Menu();
                    menuP.path = "/permission";
                    menuP.name = "permission";
                    menuP.title = "权限管理";
                    menuP.children = new List<Menu>();
                    menuP.children.Add(new Menu() { path = "/function", name = "goods", title = "功能设置" });
                    menuP.children.Add(new Menu() { path = "/role", name = "role", title = "角色设置" });
                    menuP.children.Add(new Menu() { path = "/user", name = "user", title = "用户角色" });
                    lst.Add(menuP);

                    return ResponseModel.Success(lst);
                }
                catch(Exception ex)
                {
                    return ResponseModel.Excetption("获取权限异常", ex);
                }
            });
        }

    }

    public class Menu
    {
        public string path { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public string icon { get; set; }

        public List<Menu> children { get; set; }
    }
}
