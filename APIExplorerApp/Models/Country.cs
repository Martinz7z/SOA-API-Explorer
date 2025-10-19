

namespace APIExplorerApp.Models
{
    public class Country
    {
        public Name Name { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public long Population { get; set; }
        public string[] Capital { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public Dictionary<string, string> Languages { get; set; }
        public string Flag { get; set; }
        public string[] Borders { get; set; }
    }

    public class Name
    {
        public string Common { get; set; }
        public string Official { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }
}