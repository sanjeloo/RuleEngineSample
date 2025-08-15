using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RuleEngineSample.Models
{
    public class MarketDto
    {
        public string Name { get; set; } = string.Empty;
        public List<OddsDto> Odds { get; set; } = new();
    }

    public class OddsDto
    {
        public string Name { get; set; } = string.Empty;
        public string Header { get; set; } = string.Empty;
        public string Handicap { get; set; } = string.Empty;
        public decimal Odd { get; set; }
    }

    public record DbMarket(string Name, string Description, int Order, string[] Tags);

    public class DbOdd
    {
        public DbOdd()
        {

        }
        public DbOdd(string name, decimal odd, decimal? handicap = null)
        {
            Name = name;
            Odd = odd;
            Handicap = handicap;
        }
        public string MarketName { get; set; } = string.Empty;
        public string Name { get; set; }
        public decimal Odd { get; set; }
        public decimal? Handicap { get; set; }
    }

    // MongoDB Models
    [BsonIgnoreExtraElements]
    public class SportConfiguration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Sport")]
        public string Sport { get; set; } = string.Empty;

        [BsonElement("MarketConfigs")]
        public List<MarketConfigGroup> MarketConfigs { get; set; } = new();

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    [BsonIgnoreExtraElements]
    public class MarketConfigGroup
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("MarketRegex")]
        public string MarketRegex { get; set; } = string.Empty;

        [BsonElement("Markets")]
        public List<MarketConfig> Markets { get; set; } = new();

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    [BsonIgnoreExtraElements]
    public class MarketConfig
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("MarketName")]
        public string MarketName { get; set; } = string.Empty;

        [BsonElement("MarketRegex")]
        public string MarketRegex { get; set; } = string.Empty;

        [BsonElement("NameMustSetFromMarketName")]
        public bool NameMustSetFromMarketName { get; set; }

        [BsonElement("MarketWhere")]
        public string? MarketWhere { get; set; }

        [BsonElement("MarketGroupBy")]
        public string? MarketGroupBy { get; set; }

        [BsonElement("MarketSelect")]
        public string? MarketSelect { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("Order")]
        public int Order { get; set; }

        [BsonElement("Tags")]
        public string[] Tags { get; set; } = Array.Empty<string>();

        [BsonElement("OddWhere")]
        public string OddWhere { get; set; } = string.Empty;

        [BsonElement("OddSelect")]
        public string OddSelect { get; set; } = string.Empty;

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    // Compiled Configuration Models for Performance
    public class CompiledSportConfiguration
    {
        public string Id { get; set; } = string.Empty;
        public string Sport { get; set; } = string.Empty;
        public List<CompiledMarketConfigGroup> MarketConfigs { get; set; } = new();
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }

    public class CompiledMarketConfigGroup
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Regex CompiledRegex { get; set; } = null!;
        public List<CompiledMarketConfig> Markets { get; set; } = new();
    }

    public class CompiledMarketConfig
    {
        public string Id { get; set; } = string.Empty;
        public string MarketName { get; set; } = string.Empty;
        public string MarketRegex { get; set; } = string.Empty;
        public bool NameMustSetFromMarketName { get; set; }
        public string? MarketWhere { get; set; }
        public string? MarketGroupBy { get; set; }
        public string? MarketSelect { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string OddWhere { get; set; } = string.Empty;
        public string OddSelect { get; set; } = string.Empty;

        // Compiled expressions for performance
        public Func<OddsDto, bool>? CompiledMarketWhere { get; set; }
        public Func<IGrouping<string, OddsDto>, string>? CompiledMarketSelect { get; set; }
        public Func<OddsDto, bool> CompiledOddWhere { get; set; } = null!;
        public Func<OddsDto, object> CompiledOddSelect { get; set; } = null!;
    }

    // Legacy models for backward compatibility
    public class MarketConfigLegacy
    {
        public string MarketName { get; set; } = string.Empty;
        public string MarketRegex { get; set; } = string.Empty;
        public bool NameMustSetFromMarketName { get; set; }
        public string? MarketWhere { get; set; }
        public string? MarketGroupBy { get; set; }
        public string? MarketSelect { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public string OddWhere { get; set; } = string.Empty;
        public string OddSelect { get; set; } = string.Empty;
    }
}
