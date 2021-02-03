using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Awesome
{
	public class MoneyWebHostService : WebHostService, IDisposable
	{
		private readonly MoneyService _moneyService;

		public MoneyWebHostService(IWebHost host, IConfiguration configuration, ILogger logger) : base(host)
		{
			_moneyService = new MoneyService(configuration, logger);
		}

		protected override void OnStarting(string[] args)
		{
			base.OnStarting(args);			
		}

		protected override void OnStarted()
		{
			base.OnStarted();
			Task.Run(() => _moneyService.StartAsync());
		}

		protected override void OnStopping()
		{
			base.OnStopping();
			_moneyService.Stop();
		}
	}
}
