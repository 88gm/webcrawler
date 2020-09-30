using Serilog;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCrawler.Infrastructure.ExtensionMethods;
using WebCrawler.Domain.Services;
using System.Threading.Tasks;

namespace WebCrawler
{
    class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger.Information("Iniciando a aplicacao");

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDomain();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<ProxyPageService>(host.Services);

            await svc.ExtractProxyList();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }
    }
}
