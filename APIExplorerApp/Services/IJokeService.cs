using APIExplorerApp.Models;

namespace APIExplorerApp.Services
{
    public interface IJokeService
    {
        Task<Joke> GetRandomJokeAsync();
        Task<Joke> GetProgrammingJokeAsync();
        Task<List<string>> GetJokeCategoriesAsync();
    }
}