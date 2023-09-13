using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace client.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ITokenAcquisition _tokenAcquisition;

    public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition)
    {
        _logger = logger;
        _tokenAcquisition = tokenAcquisition;
    }

    public string Weather { get; set; } = "";

    public async Task OnGet()
    {
        string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { "api://41e97338-28de-4668-99f0-7ae267d1a698/Weather.Read" });

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.GetAsync("https://localhost:7174/WeatherForecast");

        if (response.IsSuccessStatusCode)
        {
            Weather = await response.Content.ReadAsStringAsync();
        }
        else
        {
            _logger.LogError("Error: {ResponseStatusCode}", response.StatusCode);
        }
    }
}
