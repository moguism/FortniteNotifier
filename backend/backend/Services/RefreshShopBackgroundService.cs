using backend.Models;

namespace backend.Services;

public class RefreshShopBackgroundService : BackgroundService
{
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(1);
    private readonly IServiceProvider _serviceProvider;

    public RefreshShopBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private async Task WaitUntilMidnightAsync(CancellationToken stoppingToken)
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime nextMidnight = utcNow.Date.AddDays(1);

        // El tiempo que falta hasta medianoche
        TimeSpan timeUntilMidnight = nextMidnight - utcNow;

        // Se espera ese tiempo
        await Task.Delay(timeUntilMidnight, stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //await WaitUntilMidnightAsync(stoppingToken);

            /*using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                UnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                EmailService emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
                PlaywrightService playwrightService = scope.ServiceProvider.GetRequiredService<PlaywrightService>();

                List<User> users = await unitOfWork.UserRepository.GetAllFullUsersAsync();
                foreach(User user in users)
                {
                    List<string> itemsFound = await playwrightService.DoWebScrapping([.. user.Wishlists.Select(w => w.Item)]);
                    if(itemsFound.Count >= 1)
                    {
                        await emailService.CreateEmailUser(user.Email, itemsFound);
                    }
                }
            }*/

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}
