using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Public.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CBP.Main.Core.CoreAutoMapper
{
    public static class AutoMapperRegister
    {
        public static IMapper Mapper { get; private set; }

        public static IServiceCollection AutoMapperProfilesRegister(this IServiceCollection services)
        {
            var parentType = typeof(Profile);
            var profiles = new List<Type>();
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            foreach (var file in files)
            {
                try
                {
                    if (file.EndsWith("dll") == false)
                        continue;

                    var name = file.Split('\\').Last();
                    name = name.Substring(0, name.Length - 4);
                    if (name.ToLower().EndsWith("models") == false)
                        continue;

                    var type = Assembly.Load(name).GetTypes()
                        .Where(q => q.BaseType != null && q.BaseType.Name == parentType.Name);

                    if (type.Count() != 0 || type.Any())
                        profiles.AddRange(type);
                }
                catch (Exception ex)
                {
                    Logger.Error($"AutoMapperProfilesRegister Name:{file}", ex);
                }
            }
            if (profiles.Count() != 0 || profiles.Any())
                services.AddAutoMapper(profiles.ToArray());

            return services;
        }
    }
}
