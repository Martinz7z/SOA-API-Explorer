using System.Text.Json;
using APIExplorerApp.Models;

namespace APIExplorerApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://restcountries.com/v3.1/");


            // Add a user agent header to avoid 400 errors
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "APIExplorerApp/1.0");
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            try
            {
                // Try a simpler endpoint first
                var response = await _httpClient.GetAsync("all?fields=name,capital,region,population,languages,flags");

                Console.WriteLine($"=== API CALL: {_httpClient.BaseAddress}all ===");
                Console.WriteLine($"=== RESPONSE STATUS: {response.StatusCode} ===");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    // Debug: Print a small sample
                    Console.WriteLine("=== API RESPONSE SAMPLE ===");
                    if (!string.IsNullOrEmpty(json) && json.Length > 500)
                    {
                        Console.WriteLine(json.Substring(0, 500) + "...");
                    }

                    var countries = JsonSerializer.Deserialize<List<Country>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"=== PARSED COUNTRIES: {countries?.Count ?? 0} ===");

                    return countries?.OrderBy(c => c.Name.Common).ToList() ?? new List<Country>();
                }
                else
                {
                    Console.WriteLine($"=== API ERROR: {response.StatusCode} - {response.ReasonPhrase} ===");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"=== ERROR DETAILS: {errorContent} ===");
                }
                return new List<Country>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== EXCEPTION: {ex.Message} ===");
                Console.WriteLine($"=== STACK TRACE: {ex.StackTrace} ===");
                return new List<Country>();
            }
        }

        public async Task<Country> GetCountryByNameAsync(string name)
        {
            try
            {
                var response = await _httpClient.GetAsync($"name/{name}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonSerializer.Deserialize<List<Country>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return countries?.FirstOrDefault();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching country {name}: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Country>> GetCountriesByRegionAsync(string region)
        {
            try
            {
                var response = await _httpClient.GetAsync($"region/{region}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonSerializer.Deserialize<List<Country>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return countries ?? new List<Country>();
                }
                return new List<Country>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching countries in region {region}: {ex.Message}");
                return new List<Country>();
            }
        }

        public async Task<List<Country>> SearchCountriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCountriesAsync();

            var allCountries = await GetAllCountriesAsync();
            return allCountries
                .Where(c => c.Name.Common.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           c.Region.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                           (c.Subregion?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
                .ToList();
        }
    }
}