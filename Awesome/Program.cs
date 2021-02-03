using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Awesome.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Awesome
{
	public class Program
	{
		private static string s_serviceControlsUrl;

		////See https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service
		public static async Task Main(string[] args)
		{
			var isService = !(Debugger.IsAttached || args.Contains("--console"));

			if (isService)
			{
				var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
				var pathToContentRoot = Path.GetDirectoryName(pathToExe);
				Directory.SetCurrentDirectory(pathToContentRoot);
			}

			var builder = CreateWebHostBuilder(
				args.Where(arg => arg != "--console").ToArray());

			var host = builder.Build();
			var configuration = host.Services.GetRequiredService<IConfiguration>();
			var logger = host.Services.GetRequiredService<ILogger<Program>>();
			var serviceControlsUrlMessage = $"The url which the service controls can be found are at: {configuration["Kestrel:Endpoints:Http:Url"]}";
			logger.LogInformation(serviceControlsUrlMessage);
			//Console.WriteLine(serviceControlsUrlMessage);

			if (isService)
			{
				var moneyWebHostService = new MoneyWebHostService(host, configuration, logger);
				ServiceBase.Run(new ServiceBase[] { moneyWebHostService });
			}
			else
			{
				var moneyService = new MoneyService(configuration, logger);
				await moneyService.StartAsync();
				host.Run();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					s_serviceControlsUrl = config.Build()["Kestrel:Endpoints:Http:Url"];
				})
				.ConfigureLogging((hostingContext, logging) => 
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
					logging.AddDebug();
					logging.AddEventLog();
					logging.AddEventSourceLogger();
					logging.AddBatchFile();
				})
				.UseUrls(s_serviceControlsUrl)
				.PreferHostingUrls(true)
				.UseStartup<Startup>();
	}
}
