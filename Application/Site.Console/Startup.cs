using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.IoC;
using EZNEW.Mvc.CustomModelDisplayName;
using EZNEW.Mvc.DataAnnotationsModelValidatorConfig;
using EZNEW.Web.Security.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Site.Console.Config;
using Site.Console.Filters;
using Site.Console.Util;

namespace Site.Console
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ContainerFactory.RegisterServices(services);
            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {
                options.ModelValidatorProviders.Add(new CustomDataAnnotationsModelValidatorProvider());
                options.ModelMetadataDetailsProviders.Add(new MvcCustomModelDisplayProvider());
                options.Filters.Add<OperationAuthorizeFilter>();
            })
            .AddViewOptions(vo =>
            {
                vo.ClientModelValidatorProviders.Add(new CustomDataAnnotationsClientModelValidatorProvider());
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookieAuthentication(option =>
            {
                option.ForceValidatePrincipal = true;
                option.ValidatePrincipalAsync = IdentityManager.ValidatePrincipalAsync;
                option.CookieConfiguration = options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/accessdenied";
                };
            });
            var serviceProvider = ContainerFactory.GetServiceProvider();
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "accessdenied",
                    template: "accessdenied",
                    defaults: new { controller = "account", action = "AccessDenied" });
                routes.MapRoute(
                    name: "login",
                    template: "login",
                    defaults: new { controller = "account", action = "login" });
                routes.MapRoute(
                    name: "loginout",
                    template: "loginout",
                    defaults: new { controller = "account", action = "LoginOut" });
                routes.MapRoute(
                    name: "noauth",
                    template: "noauth",
                    defaults: new { controller = "home", action = "authentication" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            AppConfig.Init();
            CacheDataManager.InitData();
        }
    }
}
