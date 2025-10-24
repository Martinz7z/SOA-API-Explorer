using APIExplorerApp.Models;

namespace APIExplorerApp.Services
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByNameAsync(string name);
        Task<List<Country>> GetCountriesByRegionAsync(string region);
        Task<List<Country>> SearchCountriesAsync(string searchTerm);
    }
}