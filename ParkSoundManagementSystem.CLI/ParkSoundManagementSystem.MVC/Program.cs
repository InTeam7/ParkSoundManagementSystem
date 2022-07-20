using FubarDev.FtpServer;
using FubarDev.FtpServer.AccountManagement.Anonymous;
using FubarDev.FtpServer.FileSystem.DotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ParkSoundManagementSystem.MVC
{
    public partial class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(
                    services =>
                    {
                        services
                           .AddFtpServer(
                                builder => builder
                                   .UseDotNetFileSystem()
                                   .EnableAnonymousAuthentication());

                        services.Configure<FtpServerOptions>(opt => opt.ServerAddress = "*");
                        services.Configure<DotNetFileSystemOptions>(opt => opt
                            .RootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles"));
                        services.AddSingleton<IAnonymousPasswordValidator>(new NoValidation());

                        services
                           .AddHostedService<HostedFtpService>();
                    })
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseKestrel();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIIS();
                    webBuilder.UseUrls("https://*:8080");
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
