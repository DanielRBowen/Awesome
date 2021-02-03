using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awesome
{
	public class MoneyService : IDisposable
	{
		private readonly IConfiguration _configuration;
		private readonly ILogger _logger;
		private System.Timers.Timer _moneyTimer = new System.Timers.Timer(4000);


		public MoneyService(IConfiguration configuration, ILogger logger)
		{
			_configuration = configuration;
			_logger = logger;
			_moneyTimer.Elapsed += new System.Timers.ElapsedEventHandler(MoneyTimer_Elapsed);
		}

		public async Task StartAsync()
		{
			await Task.Delay(0);
			_moneyTimer.Start();
		}

		private void MoneyTimer_Elapsed(object sender, EventArgs e)
		{
			_logger.LogInformation("Money timer Elapsed");
		}

		public void Stop()
		{
			try
			{
				Dispose();
				_logger.LogInformation("Logger stopped");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString(), ex);
			}
		}

		public void Dispose()
		{

		}
	}
}
