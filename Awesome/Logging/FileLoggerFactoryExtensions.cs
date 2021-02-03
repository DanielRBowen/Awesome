using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Awesome.Logging
{
	/// <summary>
	/// Extensions for adding the <see cref="FileLoggerProvider" /> to the <see cref="ILoggingBuilder" />
	/// </summary>
	public static class FileLoggerFactoryExtensions
	{
		/// <summary>
		/// Adds a file logger named 'File' to the factory.
		/// </summary>
		/// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
		public static ILoggingBuilder AddBatchFile(this ILoggingBuilder builder)
		{
			builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
			return builder;
		}

		/// <summary>
		/// Adds a file logger named 'File' to the factory.
		/// </summary>
		/// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
		/// <param name="filename">Sets the filename prefix to use for log files</param>
		public static ILoggingBuilder AddBatchFile(this ILoggingBuilder builder, string filename)
		{
			builder.AddBatchFile(options => options.FileName = "log-");
			return builder;
		}

		/// <summary>
		/// Adds a file logger named 'File' to the factory.
		/// </summary>
		/// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
		/// <param name="configure">Configure an instance of the <see cref="FileLoggerOptions" /> to set logging options</param>
		public static ILoggingBuilder AddBatchFile(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
		{
			if (configure == null)
			{
				throw new ArgumentNullException(nameof(configure));
			}
			builder.AddBatchFile();
			builder.Services.Configure(configure);

			return builder;
		}
	}
}
