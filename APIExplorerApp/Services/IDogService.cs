using APIExplorerApp.Models;

namespace APIExplorerApp.Services
{
    public interface IDogService
    {
        Task<DogImage> GetRandomDogImageAsync();
        Task<List<string>> GetDogBreedsAsync();
        Task<List<DogImage>> GetDogImagesByBreedAsync(string breed, int count = 5);
    }
}