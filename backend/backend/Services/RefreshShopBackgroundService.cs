using backend.Models;

namespace backend.Services;

public class RefreshShopBackgroundService : BackgroundService
{
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(1);
    private readonly IServiceProvider _serviceProvider;

    public RefreshShopBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
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
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}
