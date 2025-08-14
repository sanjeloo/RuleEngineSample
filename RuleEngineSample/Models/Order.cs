namespace RuleEngineSample.Models
{
    public class MarketDto
    {
        public string Name { get; set; }
        public List<OddsDto> Odds { get; set; }
    }
    public class OddsDto
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public string Handicap { get; set; }
        public decimal Odd { get; set; }
        
    }

    public record DbMarket(string Name, string Description, int Order, string[] Tags);

    public record DbOdd(string MarketName, string Name, decimal Odd, decimal? Handicap = null);
    //{
    //    public string Name { get; set; } = name;
    //    public decimal Odd { get; set; } = odd;
    //    public decimal? Handicap { get; set; } = handicap;
    //}


    public class MarketConfig
    {
        public string MarketName { get; set; }
        public string MarketRegex { get; set; }
        public bool NameMustSetFromMarketName { get; set; }
        public string MarketWhere { get; set; }
        public string MarketGroupBy { get; set; }
        public string MarketSelect { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string[] Tags { get; set; }
        public string OddWhere { get; set; }
        public string OddSelect { get; set; }

    }
}
