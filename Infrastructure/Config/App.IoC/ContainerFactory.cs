using System;
using Microsoft.Extensions.DependencyInjection;
using EZNEW.DependencyInjection;
using EZNEW.Drawing.VerificationCode;
using EZNEW.VerificationCode.SkiaSharp;
using EZNEW.Web.DependencyInjection;

namespace App.IoC
{
    public static class ContainerFactory
    {
        public static IServiceProvider GetServiceProvider()
        {
            return ContainerManager.BuildServiceProvider();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            ContainerManager.Init(services, configureServiceAction: RegisterServices);
        }

        /// <summary>
        /// 自定义服务注入
        /// </summary>
        /// <param name="container"></param>
        static void RegisterServices(IDIContainer container)
        {
            WebDependencyInjectionManager.ConfigureDefaultWebService();
            container.Register(typeof(VerificationCodeProvider), typeof(SkiaSharpVerificationCode));
        }
    }
}
