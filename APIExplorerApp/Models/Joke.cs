

namespace APIExplorerApp.Models
{
    public class Joke
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Setup { get; set; }
        public string Delivery { get; set; }
        public string JokeText { get; set; }
        public bool Safe { get; set; }
    }
}