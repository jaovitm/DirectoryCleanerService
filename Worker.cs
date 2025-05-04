using System.Text.Json;

namespace DirectoryCleanerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private ServiceConfig _config = new();
    private Timer? _timer;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        LoadConfig();
		_logger.LogInformation("Serviço iniciado. Limpando a cada {0} minutos.", _config.CheckIntervalMinutes);

        _timer = new Timer(async _ =>
        {
            try
            {
                await CleanFolders();

            } catch (Exception ex)
            {
				_logger.LogError(ex, "Erro durante limpeza.");
			}
		},null, TimeSpan.Zero, TimeSpan.FromMinutes(_config.CheckIntervalMinutes));

        await Task.Delay(Timeout.Infinite, stoppingToken);

	}

	private void LoadConfig()
    {
        try
        {
            string json = File.ReadAllText("config.json");
            _config = JsonSerializer.Deserialize<ServiceConfig>(json)!;

		}
		catch (Exception ex)
        {
			_logger.LogError(ex, "Erro ao carregar config.json. Usando configurações padrão.");

		}
	}
    
    private Task CleanFolders()
    {
        foreach (var folder in _config.FoldersToClean)
        {
			if (!Directory.Exists(folder))
			{
				_logger.LogWarning("Pasta não encontrada: {0}", folder);
				continue;
			}

            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                try
                {
                    var fileInfor = new FileInfo(file);
                    var age = DateTime.Now - fileInfor.LastWriteTime;

                    if (age.TotalDays > _config.DeleteFilesOlderThanDays)
                    {
                        File.Delete(file);
						_logger.LogInformation("Arquivo apagado: {0}", file);
					}
				} catch (Exception ex)
                {
					_logger.LogError(ex, "Erro ao apagar arquivo: {0}", file);

				}
			}
		}
		return Task.CompletedTask;
	}


}
