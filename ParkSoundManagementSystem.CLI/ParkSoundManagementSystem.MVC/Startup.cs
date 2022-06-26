using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.DataAccess;
using ParkSoundManagementSystem.DataAccess.ArgsClasses;
using ParkSoundManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.MVC
{
    public class Startup
    {
        private static ISystemProcessService _systemProcessService;
        public Startup(IConfiguration configuration)
        {

            var builder = new ConfigurationBuilder().AddJsonFile("conf.json");
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddSingleton<ProcessRepositoryArgs>(_ => new ProcessRepositoryArgs { FilePath = Configuration["ProcessFile"] });
            services.AddScoped<ISystemProcessRepository, SystemProccessRepository>();
            services.AddScoped<ISystemProcessService, SystemProcessService>();



            services.AddSingleton<TimeRepositoryArgs>(_ => new TimeRepositoryArgs { FilePath = Configuration["TimeFile"] });
            services.AddScoped<ITimeRepository, TimeRepository>();
            services.AddScoped<ITimeService, TimeService>();

            services.AddSingleton<RepeatCountArgs>(_ => new RepeatCountArgs { FilePath = Configuration["CountFile"] });
            services.AddSingleton<IRepeatCountRepository, RepeatCountRepository>();
            services.AddSingleton<IRepeatCountService, RepeatCountService>();

            services.AddScoped<IAudioControlService, AudioControlService>();

       
            services.AddSignalR();
            services.AddControllersWithViews(x => x.SslPort = 5002);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                _systemProcessService=context.RequestServices.GetService<ISystemProcessService>();
                var name = await _systemProcessService.SetProcessAutomatically();
                await next.Invoke();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<NotifyHub>("/send");
            });
        }
    }
}
