using App.DBConfig;
using App.Mapper;
using EZNEW.Framework.ObjectMap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using AutoMapper;
using EZNEW.ModuleConfig.Sys;

namespace Site.Console.Config
{
    public static class AppConfig
    {
        public static void Init()
        {
            //对象转换映射
            MapperFactory.CreateMapper();

            //数据库配置
            DbConfig.Init();

            //mvc配置
            MvcConfig.Config();

            //缓存配置
            CacheConfig.Init();
        }
    }
}
