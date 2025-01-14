using CarAndAll.Server.Data;
using Microsoft.EntityFrameworkCore;

public class BehandelVerwijderingsverzoekService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _serviceProvider;

    public BehandelVerwijderingsverzoekService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(ProcessVerwijderingsverzoeken, null, TimeSpan.Zero, TimeSpan.FromDays(1));
        return Task.CompletedTask;
    }

    private async void ProcessVerwijderingsverzoeken(object state)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CarAndAllContext>();

        var verwijderDatum = DateTime.Now.AddMonths(-6);

        var verwijderingsverzoeken = await context.Verwijderingsverzoeken
            .Where(v => v.Datum <= verwijderDatum)
            .Include(v => v.Huurder)
            .ToListAsync();

        foreach (var verzoek in verwijderingsverzoeken)
        {
            context.Huurders.Remove(verzoek.Huurder);
            context.Verwijderingsverzoeken.Remove(verzoek);
        }

        await context.SaveChangesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
