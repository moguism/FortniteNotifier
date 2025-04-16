using System.Text.Json;

namespace backend.Services;

public class PlaywrightService
{
    private const string API_URL = "https://fortnite-api.com/v2/shop";

    public async Task<List<string>> DoWebScrapping(string[] items)
    {
        List<string> itemsFound = new List<string>();

        using (HttpClient client = new HttpClient())
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(API_URL);
                response.EnsureSuccessStatusCode();
                
                string jsonResponse = await response.Content.ReadAsStringAsync();

                JsonDocument dxoc = JsonDocument.Parse(jsonResponse);
                JsonElement elem = dxoc.RootElement;

                var entries = elem.GetProperty("data").GetProperty("entries");

                foreach (var entry in entries.EnumerateArray())
                {
                    if (entry.TryGetProperty("brItems", out JsonElement brItems))
                    {
                        foreach (var brItem in brItems.EnumerateArray())
                        {
                            if (brItem.TryGetProperty("name", out JsonElement name))
                            {
                                string itemName = name.GetString();
                                if(items.Select(s => s.ToLower()).Contains(itemName.ToLower()))
                                {
                                    itemsFound.Add(itemName);
                                }
                                //Console.WriteLine(itemName);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return itemsFound;
    }
}
