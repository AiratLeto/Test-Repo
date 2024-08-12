using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Cars.Api.Worker.Workers
{
	/// <summary>
	/// Тестовая служба
	/// </summary>
	public class CarTestWorker : IWorker
	{
		private readonly ILogger _logger;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="logger">Логгер</param>
		public CarTestWorker(ILogger<CarTestWorker> logger)
			=> _logger = logger;

		/// <inheritdoc/>
		public async Task RunAsync()
		{
			await Task.CompletedTask;
			_logger.LogInformation("Worker is working...");
		}
	}
}
