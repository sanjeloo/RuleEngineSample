using MongoDB.Driver;
using RuleEngineSample.Models;
using RuleEngineSample.Utils;

namespace RuleEngineSample.Services
{
    public class MongoService
    {
        private readonly IMongoCollection<SportConfiguration> _collection;
        private readonly IMongoDatabase _database;

        public MongoService()
        {
            var connectionString = GeneralFunctions.GetMongoConnectionString();
            var databaseName = GeneralFunctions.GetDatabaseName();
            var collectionName = GeneralFunctions.GetCollectionName();

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            _collection = _database.GetCollection<SportConfiguration>(collectionName);
        }

        public async Task<List<SportConfiguration>> GetAllSportConfigurationsAsync()
        {
            try
            {
                var filter = Builders<SportConfiguration>.Filter.Empty;
                var result = await _collection.Find(filter).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving sport configurations: {ex.Message}");
                return new List<SportConfiguration>();
            }
        }

        public async Task<SportConfiguration?> GetSportConfigurationBySportAsync(string sport)
        {
            try
            {
                var filter = Builders<SportConfiguration>.Filter.Eq(x => x.Sport, sport);
                var result = await _collection.Find(filter).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving sport configuration for {sport}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> InsertSportConfigurationAsync(SportConfiguration configuration)
        {
            try
            {
                await _collection.InsertOneAsync(configuration);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSportConfigurationAsync(string id, SportConfiguration configuration)
        {
            try
            {
                configuration.UpdatedAt = DateTime.UtcNow;
                var filter = Builders<SportConfiguration>.Filter.Eq(x => x.Id, id);
                var update = Builders<SportConfiguration>.Update
                    .Set(x => x.Sport, configuration.Sport)
                    .Set(x => x.MarketConfigs, configuration.MarketConfigs)
                    .Set(x => x.UpdatedAt, configuration.UpdatedAt);

                var result = await _collection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSportConfigurationAsync(string id)
        {
            try
            {
                var filter = Builders<SportConfiguration>.Filter.Eq(x => x.Id, id);
                var result = await _collection.DeleteOneAsync(filter);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> InitializeDefaultCricketConfigurationAsync()
        {
            try
            {
                // Check if Cricket configuration already exists
                var existing = await GetSportConfigurationBySportAsync("Cricket");
                if (existing != null)
                {
                    Console.WriteLine("Cricket configuration already exists in database.");
                    return true;
                }

                // Create default Cricket configuration
                var cricketConfig = new SportConfiguration
                {
                    Sport = "Cricket",
                    MarketConfigs = new List<MarketConfigGroup>
                    {
                        new MarketConfigGroup
                        {
                            Name = "Runs at Fall of 1st Wicket",
                            MarketRegex = "^Runs at Fall of 1st Wicket$",
                            Markets = new List<MarketConfig>
                            {
                                new MarketConfig
                                {
                                    MarketName = "Runs at Fall of 1st Wicket",
                                    MarketRegex = "^Runs at Fall of 1st Wicket$",
                                    NameMustSetFromMarketName = true,
                                    MarketWhere = null,
                                    MarketGroupBy = null,
                                    MarketSelect = null,
                                    Description = "bet is valid after game is done",
                                    Order = 10,
                                    Tags = new[] { "Main Markets" },
                                    OutcomeWhere = "Header != null",
                                    OutcomeName = "(Header) + \" (\" + Name + \")\"",
                                    OutcomeOdd = "Odd",
                                    OutcomeHandicap = "decimal.Parse(Name)"
                                    //OddSelect = "new DbOdd ( (Header) + \" (\" + Name + \")\" as Name, Odd,decimal.Parse(Name) as Handicap)"
                                }
                            }
                        },
                        new MarketConfigGroup
                        {
                            Name = "Batter Matches (Most Runs)",
                            MarketRegex = "^Batter Matches \\(Most Runs\\)$",
                            Markets = new List<MarketConfig>
                            {
                                new MarketConfig
                                {
                                    MarketName = "Batter Matches (Most Runs) - Player v Player",
                                    MarketRegex = "^Batter Matches \\(Most Runs\\) - (.+) v (.+)$",
                                    NameMustSetFromMarketName = false,
                                    MarketWhere = "Name != null",
                                    MarketGroupBy = "Name",
                                    MarketSelect = "\"Batter Matches (Most Runs) - \" + Key",
                                    Description = "bet is valid after game is done",
                                    Order = 170,
                                    Tags = new[] { "Batter Markets" },
                                    OutcomeWhere = "Header != null",
                                    OutcomeName = "(Name.Split(\" v \")[Header == \"1\" ? 0 : 1])",
                                    OutcomeOdd = "Odd",
                                    OutcomeHandicap = null,
                                    //OddSelect = "new DbOdd ( (Name.Split(\" v \")[Header == \"1\" ? 0 : 1]) as Name, Odd)"
                                }
                            }
                        },
                        new MarketConfigGroup
                        {
                            Name = "Match Lines",
                            MarketRegex = "^match lines$",
                            Markets = new List<MarketConfig>
                            {
                                new MarketConfig
                                {
                                    MarketName = "Match Result",
                                    MarketRegex = "^Match Result$",
                                    NameMustSetFromMarketName = true,
                                    MarketWhere = null,
                                    MarketGroupBy = null,
                                    MarketSelect = null,
                                    Description = "bet is valid after game is done",
                                    Order = 110,
                                    Tags = new[] { "Main Markets" },
                                    OutcomeWhere = "Header != null && Name == \"To Win\" ",
                                    OutcomeName = "GeneralFunctions.GetCompetitor(Header)",
                                    OutcomeOdd = "Odd",
                                    OutcomeHandicap = null,
                                    //OddSelect = "new DbOdd ( GeneralFunctions.GetCompetitor(Header) as Name, Odd)"
                                },
                                new MarketConfig
                                {
                                    MarketName = "Total Score",
                                    MarketRegex = "^Total Score$",
                                    NameMustSetFromMarketName = true,
                                    MarketWhere = null,
                                    MarketGroupBy = null,
                                    MarketSelect = null,
                                    Description = "bet is valid after game is done",
                                    Order = 130,
                                    Tags = new[] { "Main Markets" },
                                    OutcomeWhere = "Header != null && Handicap != null && Name == \"Total\" ",
                                    OutcomeName = "Handicap.StartsWith(\"O\") ? \"Over (\" + Handicap.Substring(2) + \")\" : Handicap.StartsWith(\"U\") ? \"Under(\" + Handicap.Substring(2) + \")\" : Handicap",
                                    OutcomeOdd = "Odd",
                                    OutcomeHandicap = "decimal.Parse(Handicap.Substring(2))"
                                   // OddSelect = "new DbOdd ( ( GeneralFunctions.GetCompetitor(Header) + \" \" + (Handicap.StartsWith(\"O\") ? \"Over (\" + Handicap.Substring(2) + \")\" : Handicap.StartsWith(\"U\") ? \"Under(\" + Handicap.Substring(2) + \")\" : Handicap)) as Name, Odd, (decimal.Parse(Handicap.Substring(2))) as Handicap)"
                                },
                                new MarketConfig
                                {
                                    MarketName = "Set Handicap",
                                    MarketRegex = "^Set Handicap$",
                                    NameMustSetFromMarketName = true,
                                    MarketWhere = null,
                                    MarketGroupBy = null,
                                    MarketSelect = null,
                                    Description = "bet is valid after game is done",
                                    Order = 120,
                                    Tags = new[] { "Main Markets" },
                                    OutcomeWhere = "Header != null && Handicap != null && Name == \"Handicap\" ",
                                    OutcomeName = "GeneralFunctions.GetCompetitor(Header) + \" (\" + Handicap + \")\"",
                                    OutcomeOdd = "Odd",
                                    OutcomeHandicap = "decimal.Parse(Handicap)"
                                   // OddSelect = "new DbOdd ( ( GeneralFunctions.GetCompetitor(Header) + \" (\" + Handicap + \")\") as Name, Odd, decimal.Parse(Handicap) as Handicap)"
                                }
                            }
                        }
                    }
                };

                var result = await InsertSportConfigurationAsync(cricketConfig);
                if (result)
                {
                    Console.WriteLine("Default Cricket configuration created successfully.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing default Cricket configuration: {ex.Message}");
                return false;
            }
        }
    }
}
