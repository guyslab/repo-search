using Microsoft.Extensions.Hosting;

namespace TibaRepoSearch;

public class AnalysisFetcherService : BackgroundService
{
    private readonly MessageReceiver _messageReceiver;

    public AnalysisFetcherService(MessageReceiver messageReceiver)
    {
        _messageReceiver = messageReceiver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _messageReceiver.StartListeningAsync();
    }

    public override void Dispose()
    {
        if (_messageReceiver != null)
            _messageReceiver.Dispose();
        base.Dispose();
    }
}