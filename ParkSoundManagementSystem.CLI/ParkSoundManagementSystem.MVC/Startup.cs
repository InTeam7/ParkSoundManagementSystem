using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkSoundManagementSystem.Core.Repositories;
using ParkSoundManagementSystem.Core.Services;
using ParkSoundManagementSystem.DataAccess;
using ParkSoundManagementSystem.DataAccess.ArgsClasses;
using ParkSoundManagementSystem.MVC.Quartz;
using ParkSoundManagementSystem.Services;
using Quartz;

namespace ParkSoundManagementSystem.MVC
{
    public class Startup
    {
        private static ISystemProcessService _systemProcessService;
        private static IGarbageCleaningService _garbageCleaningService;
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

            services.AddSingleton<IAudioControlService, AudioControlService>();
            services.AddSingleton<ITextToSpeechService, TextToSpeechService>();
            services.AddScoped<IPlayAudioFileService, PlayAudioFileService>();
            services.AddSingleton<IGarbageCleaningService, GarbageCleaningService>();
            services.AddSingleton<IParkVolumeService, ParkVolumeService>();
            services.AddControllersWithViews(x => x.SslPort = 5002);
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var TimeJobKey = new JobKey("TimeJob");
                q.AddJob<TimeJob>(opts => opts.WithIdentity(TimeJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(TimeJobKey)
                    .WithIdentity("TimeJob-trigger")
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(1)
                        .RepeatForever()));
            });

            services.AddCors();
            services.AddQuartzHostedService(
                    q => q.WaitForJobsToComplete = true);
            services.AddSignalR();
            var sp = services.BuildServiceProvider();
            var _systemProcessService = sp.GetService<ISystemProcessService>();
            var _audioControlService = sp.GetService<IAudioControlService>();
            var name =  _systemProcessService.SetProcessAutomatically().Result;
            var pId =  _systemProcessService.GetProcessId().Result;
            _audioControlService.SetApplicationMute(pId, false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();
            app.Use(async (context, next) =>
            {
                _garbageCleaningService = context.RequestServices.GetService<IGarbageCleaningService>();
                int count = _garbageCleaningService.GetCountFiles();
                if (count > 50)
                {
                    _garbageCleaningService.DeleteFiles();
                }
                _systemProcessService = context.RequestServices.GetService<ISystemProcessService>();
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
            app.UseCors(options => options.AllowAnyOrigin());
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
