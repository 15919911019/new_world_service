using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CBP.BaseServices
{
    public static class ModuleManager
    {
        public static Dictionary<string, BaseService> DictService { get; set; }
        public static Dictionary<string, MethodInfo> DictMethodInfo { get; set; }

        static ModuleManager()
        {
            DictService = new Dictionary<string, BaseService>();
            DictMethodInfo = new Dictionary<string, MethodInfo>();
        }


        public static void AddInstance(string moduleName, string intefaceName, BaseService instance)
        {
            var name = $"{moduleName?.Trim().ToLower()}_{intefaceName?.Trim().ToLower()}";
            if (DictService.ContainsKey(name) == false)
                DictService.Add(name, instance);
        }

        public static BaseService GetInstance(string moduleName, string intefaceName)
        {
            var name = $"{moduleName?.Trim().ToLower()}_{intefaceName?.Trim().ToLower()}";
            if (DictService.ContainsKey(name) == false)
                return null;
            else
                return DictService[name];
        }

        public static void AddMethod(string moduleName, string intefaceName, string commandName, MethodInfo method)
        {
            var name = $"{moduleName?.Trim().ToLower()}_{intefaceName?.Trim().ToLower()}_{commandName?.Trim().ToLower()}";
            if (DictMethodInfo.ContainsKey(name) == false)
                DictMethodInfo.Add(name, method);
        }

        public static MethodInfo GetMethodInfo(string moduleName, string intefaceName, string commandName)
        {
            var name = $"{moduleName?.Trim().ToLower()}_{intefaceName?.Trim().ToLower()}_{commandName?.Trim().ToLower()}";

            if (DictMethodInfo.ContainsKey(name) == false)
                return null;

            return DictMethodInfo[name];
        }

    }
}
