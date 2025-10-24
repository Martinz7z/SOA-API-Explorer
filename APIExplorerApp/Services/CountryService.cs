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
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("all");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var countries = JsonSerializer.Deserialize<List<Country>>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return countries?.OrderBy(c => c.Name.Common).ToList() ?? new List<Country>();
                }
                return new List<Country>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching countries: {ex.Message}");
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