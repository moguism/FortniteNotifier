namespace backend.Services;

public class HeartBeatBackgroundService : BackgroundService
{
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(1);

#if DEBUG
    private const string API_URL = "https://localhost:7106/api/refresh";
#else
    private const string API_URL = "https://fortnite-notifier.runasp.net/api/refresh";
#endif

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(API_URL);
                    response.EnsureSuccessStatusCode();

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(jsonResponse);
                }
                catch { }
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }
    }
}
