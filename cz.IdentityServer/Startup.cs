using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using cz.IdentityServer.Seeds;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace cz.IdentityServer
{
    public class Startup
    {

        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.AddIdentityServer()
              .AddDeveloperSigningCredential()
              //api资源
              .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
              //4.0版本需要添加，不然调用时提示invalid_scope错误
              .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
              .AddTestUsers(InMemoryConfig.Users().ToList())
              .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
              .AddInMemoryClients(InMemoryConfig.GetClients());

            //获取连接串
            string connString = _configuration.GetConnectionString("Default");
            string migrationsAssembly = Assembly.GetEntryAssembly().GetName().Name;
            ////添加IdentityServer服务
            //services.AddIdentityServer()
            //    //添加这配置数据(客户端、资源)
            //    .AddConfigurationStore(opt =>
            //    {
            //        opt.ConfigureDbContext = c =>
            //        {
            //            c.UseMySql(connString, sql => sql.MigrationsAssembly(migrationsAssembly));
            //        };
            //    })
            //    //添加操作数据(codes、tokens、consents)
            //    .AddOperationalStore(opt =>
            //    {
            //        opt.ConfigureDbContext = c =>
            //        {
            //            c.UseMySql(connString, sql => sql.MigrationsAssembly(migrationsAssembly));
            //        };
            //        //token自动清理
            //        opt.EnableTokenCleanup = true;
            //        //token自动清理间隔：默认1H
            //        opt.TokenCleanupInterval = 3600;
            //        ////token自动清理每次数量
            //        //opt.TokenCleanupBatchSize = 100;
            //    })
            //    .AddTestUsers(InMemoryConfig.Users().ToList());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //初始化数据
            //SeedData.InitData(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseIdentityServer();

            app.UseAuthentication();
            //使用默认UI，必须添加
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
