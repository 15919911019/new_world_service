
using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Public.Log;
using CBP.BaseServices;

namespace CBP.Main.Core.Service
{
    public static class ServiceRegister
    {

        public static void Regiser(this IServiceCollection services, IConfiguration configuration)
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            foreach(var file in files)
            {
                if (file.EndsWith("dll") == false)
                    continue;

                var name = file.Split('\\').Last();
                name = name.Substring(0, name.Length - 4);
                if (name.ToLower().EndsWith("services") == false)
                    continue;

                try
                {
                    var asm = Assembly.Load(new AssemblyName(name));
                    var serviceTypes = asm.GetTypes()
                    .Where(x => typeof(BaseService).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract).ToList();

                    foreach (var serviceType in serviceTypes)
                    {
                        var instance = (BaseService)Activator.CreateInstance(serviceType);
                        if (instance == null)
                            continue;

                        var interfaces = serviceType.GetInterfaces().ToList();
                        if(string.IsNullOrEmpty(instance.ServiceName) == true)
                        {
                            Logger.Error($"组件没有名称：{serviceType.Name}");
                            continue;
                        }

                        ModuleManager.AddInstance(instance.ServiceName, serviceType.Name, instance);

                        foreach(var inter in interfaces)
                        {
                            var methods = inter.GetMethods().ToList();
                            foreach(var met in methods)
                            {
                                if (met.IsConstructor == true)
                                    continue;

                                if (string.IsNullOrEmpty(instance.ServiceName) == true)
                                    continue;

                                ModuleManager.AddMethod(instance.ServiceName, inter.Name.Substring(1), met.Name, met);
                            }
                        }

                        foreach (var serviceInterface in serviceType.GetInterfaces())
                            services.AddSingleton(serviceInterface, serviceType);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"组件实例化异常 Name:{name}", ex);
                }
            }
        }
    }
}
