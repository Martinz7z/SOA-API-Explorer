using System.Text.Json;
using APIExplorerApp.Models;

namespace APIExplorerApp.Services
{
    public class DogService : IDogService
    {
        private readonly HttpClient _httpClient;

        public DogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://dog.ceo/api/");
        }

        public async Task<DogImage> GetRandomDogImageAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("breeds/image/random");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<DogImage>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching random dog image: {ex.Message}");
            }
            return new DogImage();
        }

        public async Task<List<string>> GetDogBreedsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("breeds/list/all");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var breedsResponse = JsonSerializer.Deserialize<DogBreedsResponse>(json,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return breedsResponse?.Message?.Keys.ToList() ?? new List<string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching dog breeds: {ex.Message}");
            }
            return new List<string>();
        }

        public async Task<List<DogImage>> GetDogImagesByBreedAsync(string breed, int count = 5)
        {
            return new List<DogImage>(); // We can implement this later
        }
    }

    public class DogBreedsResponse
    {
        public Dictionary<string, string[]> Message { get; set; }
        public string Status { get; set; }
    }
}