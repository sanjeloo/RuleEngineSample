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
                                }
                            }
                        },

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
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 2nd Wicket",
    MarketRegex = "^Runs at Fall of 2nd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 2nd Wicket",
            MarketRegex = "^Runs at Fall of 2nd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 11,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 3rd Wicket",
    MarketRegex = "^Runs at Fall of 3rd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 3rd Wicket",
            MarketRegex = "^Runs at Fall of 3rd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 12,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 4th Wicket",
    MarketRegex = "^Runs at Fall of 4th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 4th Wicket",
            MarketRegex = "^Runs at Fall of 4th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 13,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 5th Wicket",
    MarketRegex = "^Runs at Fall of 5th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 5th Wicket",
            MarketRegex = "^Runs at Fall of 5th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 14,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 6th Wicket",
    MarketRegex = "^Runs at Fall of 6th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 6th Wicket",
            MarketRegex = "^Runs at Fall of 6th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 15,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 7th Wicket",
    MarketRegex = "^Runs at Fall of 7th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 7th Wicket",
            MarketRegex = "^Runs at Fall of 7th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 16,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 8th Wicket",
    MarketRegex = "^Runs at Fall of 8th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 8th Wicket",
            MarketRegex = "^Runs at Fall of 8th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 17,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 9th Wicket",
    MarketRegex = "^Runs at Fall of 9th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 9th Wicket",
            MarketRegex = "^Runs at Fall of 9th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 18,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 10th Wicket",
    MarketRegex = "^Runs at Fall of 10th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 10th Wicket",
            MarketRegex = "^Runs at Fall of 10th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 19,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 1st Over",
    MarketRegex = "^Total Runs in 1st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 1st Over",
            MarketRegex = "^Total Runs in 1st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 20,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 2nd Over",
    MarketRegex = "^Total Runs in 2nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 2nd Over",
            MarketRegex = "^Total Runs in 2nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 21,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 3rd Over",
    MarketRegex = "^Total Runs in 3rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 3rd Over",
            MarketRegex = "^Total Runs in 3rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 22,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 4th Over",
    MarketRegex = "^Total Runs in 4th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 4th Over",
            MarketRegex = "^Total Runs in 4th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 23,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 5th Over",
    MarketRegex = "^Total Runs in 5th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 5th Over",
            MarketRegex = "^Total Runs in 5th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 24,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 6th Over",
    MarketRegex = "^Total Runs in 6th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 6th Over",
            MarketRegex = "^Total Runs in 6th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 25,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 7th Over",
    MarketRegex = "^Total Runs in 7th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 7th Over",
            MarketRegex = "^Total Runs in 7th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 26,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 8th Over",
    MarketRegex = "^Total Runs in 8th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 8th Over",
            MarketRegex = "^Total Runs in 8th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 27,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 9th Over",
    MarketRegex = "^Total Runs in 9th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 9th Over",
            MarketRegex = "^Total Runs in 9th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 28,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 10th Over",
    MarketRegex = "^Total Runs in 10th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 10th Over",
            MarketRegex = "^Total Runs in 10th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 29,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 11th Over",
    MarketRegex = "^Total Runs in 11th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 11th Over",
            MarketRegex = "^Total Runs in 11th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 30,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 12th Over",
    MarketRegex = "^Total Runs in 12th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 12th Over",
            MarketRegex = "^Total Runs in 12th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 31,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 13th Over",
    MarketRegex = "^Total Runs in 13th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 13th Over",
            MarketRegex = "^Total Runs in 13th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 32,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 14th Over",
    MarketRegex = "^Total Runs in 14th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 14th Over",
            MarketRegex = "^Total Runs in 14th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 33,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 15th Over",
    MarketRegex = "^Total Runs in 15th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 15th Over",
            MarketRegex = "^Total Runs in 15th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 34,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 16th Over",
    MarketRegex = "^Total Runs in 16th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 16th Over",
            MarketRegex = "^Total Runs in 16th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 35,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 17th Over",
    MarketRegex = "^Total Runs in 17th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 17th Over",
            MarketRegex = "^Total Runs in 17th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 36,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 18th Over",
    MarketRegex = "^Total Runs in 18th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 18th Over",
            MarketRegex = "^Total Runs in 18th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 37,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 19th Over",
    MarketRegex = "^Total Runs in 19th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 19th Over",
            MarketRegex = "^Total Runs in 19th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 38,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 20th Over",
    MarketRegex = "^Total Runs in 20th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 20th Over",
            MarketRegex = "^Total Runs in 20th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 39,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 21st Over",
    MarketRegex = "^Total Runs in 21st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 21st Over",
            MarketRegex = "^Total Runs in 21st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 40,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 22nd Over",
    MarketRegex = "^Total Runs in 22nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 22nd Over",
            MarketRegex = "^Total Runs in 22nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 41,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 23rd Over",
    MarketRegex = "^Total Runs in 23rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 23rd Over",
            MarketRegex = "^Total Runs in 23rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 42,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 24th Over",
    MarketRegex = "^Total Runs in 24th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 24th Over",
            MarketRegex = "^Total Runs in 24th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 43,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 25th Over",
    MarketRegex = "^Total Runs in 25th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 25th Over",
            MarketRegex = "^Total Runs in 25th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 44,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 26th Over",
    MarketRegex = "^Total Runs in 26th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 26th Over",
            MarketRegex = "^Total Runs in 26th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 45,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 27th Over",
    MarketRegex = "^Total Runs in 27th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 27th Over",
            MarketRegex = "^Total Runs in 27th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 46,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 28th Over",
    MarketRegex = "^Total Runs in 28th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 28th Over",
            MarketRegex = "^Total Runs in 28th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 47,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 29th Over",
    MarketRegex = "^Total Runs in 29th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 29th Over",
            MarketRegex = "^Total Runs in 29th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 48,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 30th Over",
    MarketRegex = "^Total Runs in 30th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 30th Over",
            MarketRegex = "^Total Runs in 30th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 49,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 31st Over",
    MarketRegex = "^Total Runs in 31st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 31st Over",
            MarketRegex = "^Total Runs in 31st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 50,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 32nd Over",
    MarketRegex = "^Total Runs in 32nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 32nd Over",
            MarketRegex = "^Total Runs in 32nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 51,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 33rd Over",
    MarketRegex = "^Total Runs in 33rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 33rd Over",
            MarketRegex = "^Total Runs in 33rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 52,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 34th Over",
    MarketRegex = "^Total Runs in 34th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 34th Over",
            MarketRegex = "^Total Runs in 34th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 53,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 35th Over",
    MarketRegex = "^Total Runs in 35th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 35th Over",
            MarketRegex = "^Total Runs in 35th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 54,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 36th Over",
    MarketRegex = "^Total Runs in 36th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 36th Over",
            MarketRegex = "^Total Runs in 36th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 55,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 37th Over",
    MarketRegex = "^Total Runs in 37th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 37th Over",
            MarketRegex = "^Total Runs in 37th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 56,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 38th Over",
    MarketRegex = "^Total Runs in 38th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 38th Over",
            MarketRegex = "^Total Runs in 38th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 57,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 39th Over",
    MarketRegex = "^Total Runs in 39th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 39th Over",
            MarketRegex = "^Total Runs in 39th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 58,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 40th Over",
    MarketRegex = "^Total Runs in 40th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 40th Over",
            MarketRegex = "^Total Runs in 40th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 59,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 41st Over",
    MarketRegex = "^Total Runs in 41st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 41st Over",
            MarketRegex = "^Total Runs in 41st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 60,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 42nd Over",
    MarketRegex = "^Total Runs in 42nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 42nd Over",
            MarketRegex = "^Total Runs in 42nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 61,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 43rd Over",
    MarketRegex = "^Total Runs in 43rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 43rd Over",
            MarketRegex = "^Total Runs in 43rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 62,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 44th Over",
    MarketRegex = "^Total Runs in 44th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 44th Over",
            MarketRegex = "^Total Runs in 44th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 63,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 45th Over",
    MarketRegex = "^Total Runs in 45th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 45th Over",
            MarketRegex = "^Total Runs in 45th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 64,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 46th Over",
    MarketRegex = "^Total Runs in 46th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 46th Over",
            MarketRegex = "^Total Runs in 46th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 65,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 47th Over",
    MarketRegex = "^Total Runs in 47th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 47th Over",
            MarketRegex = "^Total Runs in 47th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 66,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 48th Over",
    MarketRegex = "^Total Runs in 48th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 48th Over",
            MarketRegex = "^Total Runs in 48th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 67,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 49th Over",
    MarketRegex = "^Total Runs in 49th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 49th Over",
            MarketRegex = "^Total Runs in 49th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 68,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 50th Over",
    MarketRegex = "^Total Runs in 50th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 50th Over",
            MarketRegex = "^Total Runs in 50th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 69,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 51st Over",
    MarketRegex = "^Total Runs in 51st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 51st Over",
            MarketRegex = "^Total Runs in 51st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 70,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 52nd Over",
    MarketRegex = "^Total Runs in 52nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 52nd Over",
            MarketRegex = "^Total Runs in 52nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 71,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 53rd Over",
    MarketRegex = "^Total Runs in 53rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 53rd Over",
            MarketRegex = "^Total Runs in 53rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 72,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 54th Over",
    MarketRegex = "^Total Runs in 54th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 54th Over",
            MarketRegex = "^Total Runs in 54th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 73,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 55th Over",
    MarketRegex = "^Total Runs in 55th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 55th Over",
            MarketRegex = "^Total Runs in 55th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 74,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 56th Over",
    MarketRegex = "^Total Runs in 56th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 56th Over",
            MarketRegex = "^Total Runs in 56th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 75,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 57th Over",
    MarketRegex = "^Total Runs in 57th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 57th Over",
            MarketRegex = "^Total Runs in 57th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 76,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 58th Over",
    MarketRegex = "^Total Runs in 58th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 58th Over",
            MarketRegex = "^Total Runs in 58th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 77,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 59th Over",
    MarketRegex = "^Total Runs in 59th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 59th Over",
            MarketRegex = "^Total Runs in 59th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 78,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 60th Over",
    MarketRegex = "^Total Runs in 60th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 60th Over",
            MarketRegex = "^Total Runs in 60th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 79,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 61st Over",
    MarketRegex = "^Total Runs in 61st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 61st Over",
            MarketRegex = "^Total Runs in 61st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 80,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 62nd Over",
    MarketRegex = "^Total Runs in 62nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 62nd Over",
            MarketRegex = "^Total Runs in 62nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 81,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 63rd Over",
    MarketRegex = "^Total Runs in 63rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 63rd Over",
            MarketRegex = "^Total Runs in 63rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 82,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 64th Over",
    MarketRegex = "^Total Runs in 64th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 64th Over",
            MarketRegex = "^Total Runs in 64th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 83,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 65th Over",
    MarketRegex = "^Total Runs in 65th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 65th Over",
            MarketRegex = "^Total Runs in 65th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 84,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 66th Over",
    MarketRegex = "^Total Runs in 66th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 66th Over",
            MarketRegex = "^Total Runs in 66th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 85,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 67th Over",
    MarketRegex = "^Total Runs in 67th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 67th Over",
            MarketRegex = "^Total Runs in 67th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 86,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 68th Over",
    MarketRegex = "^Total Runs in 68th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 68th Over",
            MarketRegex = "^Total Runs in 68th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 87,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 69th Over",
    MarketRegex = "^Total Runs in 69th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 69th Over",
            MarketRegex = "^Total Runs in 69th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 88,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 70th Over",
    MarketRegex = "^Total Runs in 70th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 70th Over",
            MarketRegex = "^Total Runs in 70th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 89,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 71st Over",
    MarketRegex = "^Total Runs in 71st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 71st Over",
            MarketRegex = "^Total Runs in 71st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 90,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 72nd Over",
    MarketRegex = "^Total Runs in 72nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 72nd Over",
            MarketRegex = "^Total Runs in 72nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 91,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 73rd Over",
    MarketRegex = "^Total Runs in 73rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 73rd Over",
            MarketRegex = "^Total Runs in 73rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 92,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 74th Over",
    MarketRegex = "^Total Runs in 74th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 74th Over",
            MarketRegex = "^Total Runs in 74th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 93,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 75th Over",
    MarketRegex = "^Total Runs in 75th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 75th Over",
            MarketRegex = "^Total Runs in 75th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 94,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 76th Over",
    MarketRegex = "^Total Runs in 76th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 76th Over",
            MarketRegex = "^Total Runs in 76th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 95,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 77th Over",
    MarketRegex = "^Total Runs in 77th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 77th Over",
            MarketRegex = "^Total Runs in 77th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 96,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 78th Over",
    MarketRegex = "^Total Runs in 78th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 78th Over",
            MarketRegex = "^Total Runs in 78th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 97,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 79th Over",
    MarketRegex = "^Total Runs in 79th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 79th Over",
            MarketRegex = "^Total Runs in 79th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 98,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 80th Over",
    MarketRegex = "^Total Runs in 80th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 80th Over",
            MarketRegex = "^Total Runs in 80th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 99,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 81st Over",
    MarketRegex = "^Total Runs in 81st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 81st Over",
            MarketRegex = "^Total Runs in 81st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 100,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 82nd Over",
    MarketRegex = "^Total Runs in 82nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 82nd Over",
            MarketRegex = "^Total Runs in 82nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 101,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 83rd Over",
    MarketRegex = "^Total Runs in 83rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 83rd Over",
            MarketRegex = "^Total Runs in 83rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 102,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 84th Over",
    MarketRegex = "^Total Runs in 84th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 84th Over",
            MarketRegex = "^Total Runs in 84th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 103,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 85th Over",
    MarketRegex = "^Total Runs in 85th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 85th Over",
            MarketRegex = "^Total Runs in 85th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 104,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 86th Over",
    MarketRegex = "^Total Runs in 86th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 86th Over",
            MarketRegex = "^Total Runs in 86th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 105,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 87th Over",
    MarketRegex = "^Total Runs in 87th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 87th Over",
            MarketRegex = "^Total Runs in 87th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 106,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 88th Over",
    MarketRegex = "^Total Runs in 88th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 88th Over",
            MarketRegex = "^Total Runs in 88th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 107,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 89th Over",
    MarketRegex = "^Total Runs in 89th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 89th Over",
            MarketRegex = "^Total Runs in 89th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 108,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 90th Over",
    MarketRegex = "^Total Runs in 90th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 90th Over",
            MarketRegex = "^Total Runs in 90th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 109,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Total Score",
                                    MarketRegex = "^Total Score$",
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Set Handicap",
                                    MarketRegex = "^Set Handicap$",
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
                                }
                            }
                        }
                    }
                };
                 var baseballConfig = new SportConfiguration
                {
                    Sport = "Baseball",
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
                                }
                            }
                        },

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
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 2nd Wicket",
    MarketRegex = "^Runs at Fall of 2nd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 2nd Wicket",
            MarketRegex = "^Runs at Fall of 2nd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 11,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 3rd Wicket",
    MarketRegex = "^Runs at Fall of 3rd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 3rd Wicket",
            MarketRegex = "^Runs at Fall of 3rd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 12,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 4th Wicket",
    MarketRegex = "^Runs at Fall of 4th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 4th Wicket",
            MarketRegex = "^Runs at Fall of 4th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 13,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 5th Wicket",
    MarketRegex = "^Runs at Fall of 5th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 5th Wicket",
            MarketRegex = "^Runs at Fall of 5th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 14,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 6th Wicket",
    MarketRegex = "^Runs at Fall of 6th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 6th Wicket",
            MarketRegex = "^Runs at Fall of 6th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 15,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 7th Wicket",
    MarketRegex = "^Runs at Fall of 7th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 7th Wicket",
            MarketRegex = "^Runs at Fall of 7th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 16,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 8th Wicket",
    MarketRegex = "^Runs at Fall of 8th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 8th Wicket",
            MarketRegex = "^Runs at Fall of 8th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 17,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 9th Wicket",
    MarketRegex = "^Runs at Fall of 9th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 9th Wicket",
            MarketRegex = "^Runs at Fall of 9th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 18,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 10th Wicket",
    MarketRegex = "^Runs at Fall of 10th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 10th Wicket",
            MarketRegex = "^Runs at Fall of 10th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 19,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 1st Over",
    MarketRegex = "^Total Runs in 1st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 1st Over",
            MarketRegex = "^Total Runs in 1st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 20,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 2nd Over",
    MarketRegex = "^Total Runs in 2nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 2nd Over",
            MarketRegex = "^Total Runs in 2nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 21,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 3rd Over",
    MarketRegex = "^Total Runs in 3rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 3rd Over",
            MarketRegex = "^Total Runs in 3rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 22,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 4th Over",
    MarketRegex = "^Total Runs in 4th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 4th Over",
            MarketRegex = "^Total Runs in 4th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 23,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 5th Over",
    MarketRegex = "^Total Runs in 5th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 5th Over",
            MarketRegex = "^Total Runs in 5th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 24,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 6th Over",
    MarketRegex = "^Total Runs in 6th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 6th Over",
            MarketRegex = "^Total Runs in 6th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 25,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 7th Over",
    MarketRegex = "^Total Runs in 7th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 7th Over",
            MarketRegex = "^Total Runs in 7th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 26,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 8th Over",
    MarketRegex = "^Total Runs in 8th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 8th Over",
            MarketRegex = "^Total Runs in 8th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 27,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 9th Over",
    MarketRegex = "^Total Runs in 9th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 9th Over",
            MarketRegex = "^Total Runs in 9th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 28,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 10th Over",
    MarketRegex = "^Total Runs in 10th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 10th Over",
            MarketRegex = "^Total Runs in 10th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 29,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 11th Over",
    MarketRegex = "^Total Runs in 11th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 11th Over",
            MarketRegex = "^Total Runs in 11th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 30,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 12th Over",
    MarketRegex = "^Total Runs in 12th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 12th Over",
            MarketRegex = "^Total Runs in 12th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 31,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 13th Over",
    MarketRegex = "^Total Runs in 13th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 13th Over",
            MarketRegex = "^Total Runs in 13th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 32,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 14th Over",
    MarketRegex = "^Total Runs in 14th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 14th Over",
            MarketRegex = "^Total Runs in 14th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 33,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 15th Over",
    MarketRegex = "^Total Runs in 15th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 15th Over",
            MarketRegex = "^Total Runs in 15th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 34,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 16th Over",
    MarketRegex = "^Total Runs in 16th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 16th Over",
            MarketRegex = "^Total Runs in 16th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 35,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 17th Over",
    MarketRegex = "^Total Runs in 17th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 17th Over",
            MarketRegex = "^Total Runs in 17th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 36,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 18th Over",
    MarketRegex = "^Total Runs in 18th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 18th Over",
            MarketRegex = "^Total Runs in 18th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 37,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 19th Over",
    MarketRegex = "^Total Runs in 19th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 19th Over",
            MarketRegex = "^Total Runs in 19th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 38,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 20th Over",
    MarketRegex = "^Total Runs in 20th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 20th Over",
            MarketRegex = "^Total Runs in 20th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 39,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 21st Over",
    MarketRegex = "^Total Runs in 21st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 21st Over",
            MarketRegex = "^Total Runs in 21st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 40,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 22nd Over",
    MarketRegex = "^Total Runs in 22nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 22nd Over",
            MarketRegex = "^Total Runs in 22nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 41,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 23rd Over",
    MarketRegex = "^Total Runs in 23rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 23rd Over",
            MarketRegex = "^Total Runs in 23rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 42,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 24th Over",
    MarketRegex = "^Total Runs in 24th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 24th Over",
            MarketRegex = "^Total Runs in 24th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 43,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 25th Over",
    MarketRegex = "^Total Runs in 25th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 25th Over",
            MarketRegex = "^Total Runs in 25th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 44,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 26th Over",
    MarketRegex = "^Total Runs in 26th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 26th Over",
            MarketRegex = "^Total Runs in 26th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 45,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 27th Over",
    MarketRegex = "^Total Runs in 27th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 27th Over",
            MarketRegex = "^Total Runs in 27th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 46,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 28th Over",
    MarketRegex = "^Total Runs in 28th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 28th Over",
            MarketRegex = "^Total Runs in 28th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 47,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 29th Over",
    MarketRegex = "^Total Runs in 29th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 29th Over",
            MarketRegex = "^Total Runs in 29th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 48,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 30th Over",
    MarketRegex = "^Total Runs in 30th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 30th Over",
            MarketRegex = "^Total Runs in 30th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 49,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 31st Over",
    MarketRegex = "^Total Runs in 31st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 31st Over",
            MarketRegex = "^Total Runs in 31st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 50,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 32nd Over",
    MarketRegex = "^Total Runs in 32nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 32nd Over",
            MarketRegex = "^Total Runs in 32nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 51,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 33rd Over",
    MarketRegex = "^Total Runs in 33rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 33rd Over",
            MarketRegex = "^Total Runs in 33rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 52,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 34th Over",
    MarketRegex = "^Total Runs in 34th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 34th Over",
            MarketRegex = "^Total Runs in 34th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 53,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 35th Over",
    MarketRegex = "^Total Runs in 35th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 35th Over",
            MarketRegex = "^Total Runs in 35th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 54,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 36th Over",
    MarketRegex = "^Total Runs in 36th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 36th Over",
            MarketRegex = "^Total Runs in 36th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 55,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 37th Over",
    MarketRegex = "^Total Runs in 37th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 37th Over",
            MarketRegex = "^Total Runs in 37th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 56,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 38th Over",
    MarketRegex = "^Total Runs in 38th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 38th Over",
            MarketRegex = "^Total Runs in 38th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 57,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 39th Over",
    MarketRegex = "^Total Runs in 39th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 39th Over",
            MarketRegex = "^Total Runs in 39th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 58,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 40th Over",
    MarketRegex = "^Total Runs in 40th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 40th Over",
            MarketRegex = "^Total Runs in 40th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 59,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 41st Over",
    MarketRegex = "^Total Runs in 41st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 41st Over",
            MarketRegex = "^Total Runs in 41st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 60,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 42nd Over",
    MarketRegex = "^Total Runs in 42nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 42nd Over",
            MarketRegex = "^Total Runs in 42nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 61,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 43rd Over",
    MarketRegex = "^Total Runs in 43rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 43rd Over",
            MarketRegex = "^Total Runs in 43rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 62,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 44th Over",
    MarketRegex = "^Total Runs in 44th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 44th Over",
            MarketRegex = "^Total Runs in 44th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 63,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 45th Over",
    MarketRegex = "^Total Runs in 45th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 45th Over",
            MarketRegex = "^Total Runs in 45th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 64,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 46th Over",
    MarketRegex = "^Total Runs in 46th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 46th Over",
            MarketRegex = "^Total Runs in 46th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 65,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 47th Over",
    MarketRegex = "^Total Runs in 47th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 47th Over",
            MarketRegex = "^Total Runs in 47th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 66,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 48th Over",
    MarketRegex = "^Total Runs in 48th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 48th Over",
            MarketRegex = "^Total Runs in 48th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 67,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 49th Over",
    MarketRegex = "^Total Runs in 49th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 49th Over",
            MarketRegex = "^Total Runs in 49th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 68,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 50th Over",
    MarketRegex = "^Total Runs in 50th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 50th Over",
            MarketRegex = "^Total Runs in 50th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 69,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 51st Over",
    MarketRegex = "^Total Runs in 51st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 51st Over",
            MarketRegex = "^Total Runs in 51st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 70,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 52nd Over",
    MarketRegex = "^Total Runs in 52nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 52nd Over",
            MarketRegex = "^Total Runs in 52nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 71,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 53rd Over",
    MarketRegex = "^Total Runs in 53rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 53rd Over",
            MarketRegex = "^Total Runs in 53rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 72,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 54th Over",
    MarketRegex = "^Total Runs in 54th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 54th Over",
            MarketRegex = "^Total Runs in 54th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 73,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 55th Over",
    MarketRegex = "^Total Runs in 55th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 55th Over",
            MarketRegex = "^Total Runs in 55th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 74,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 56th Over",
    MarketRegex = "^Total Runs in 56th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 56th Over",
            MarketRegex = "^Total Runs in 56th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 75,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 57th Over",
    MarketRegex = "^Total Runs in 57th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 57th Over",
            MarketRegex = "^Total Runs in 57th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 76,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 58th Over",
    MarketRegex = "^Total Runs in 58th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 58th Over",
            MarketRegex = "^Total Runs in 58th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 77,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 59th Over",
    MarketRegex = "^Total Runs in 59th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 59th Over",
            MarketRegex = "^Total Runs in 59th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 78,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 60th Over",
    MarketRegex = "^Total Runs in 60th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 60th Over",
            MarketRegex = "^Total Runs in 60th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 79,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 61st Over",
    MarketRegex = "^Total Runs in 61st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 61st Over",
            MarketRegex = "^Total Runs in 61st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 80,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 62nd Over",
    MarketRegex = "^Total Runs in 62nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 62nd Over",
            MarketRegex = "^Total Runs in 62nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 81,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 63rd Over",
    MarketRegex = "^Total Runs in 63rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 63rd Over",
            MarketRegex = "^Total Runs in 63rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 82,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 64th Over",
    MarketRegex = "^Total Runs in 64th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 64th Over",
            MarketRegex = "^Total Runs in 64th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 83,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 65th Over",
    MarketRegex = "^Total Runs in 65th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 65th Over",
            MarketRegex = "^Total Runs in 65th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 84,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 66th Over",
    MarketRegex = "^Total Runs in 66th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 66th Over",
            MarketRegex = "^Total Runs in 66th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 85,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 67th Over",
    MarketRegex = "^Total Runs in 67th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 67th Over",
            MarketRegex = "^Total Runs in 67th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 86,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 68th Over",
    MarketRegex = "^Total Runs in 68th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 68th Over",
            MarketRegex = "^Total Runs in 68th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 87,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 69th Over",
    MarketRegex = "^Total Runs in 69th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 69th Over",
            MarketRegex = "^Total Runs in 69th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 88,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 70th Over",
    MarketRegex = "^Total Runs in 70th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 70th Over",
            MarketRegex = "^Total Runs in 70th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 89,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 71st Over",
    MarketRegex = "^Total Runs in 71st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 71st Over",
            MarketRegex = "^Total Runs in 71st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 90,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 72nd Over",
    MarketRegex = "^Total Runs in 72nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 72nd Over",
            MarketRegex = "^Total Runs in 72nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 91,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 73rd Over",
    MarketRegex = "^Total Runs in 73rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 73rd Over",
            MarketRegex = "^Total Runs in 73rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 92,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 74th Over",
    MarketRegex = "^Total Runs in 74th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 74th Over",
            MarketRegex = "^Total Runs in 74th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 93,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 75th Over",
    MarketRegex = "^Total Runs in 75th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 75th Over",
            MarketRegex = "^Total Runs in 75th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 94,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 76th Over",
    MarketRegex = "^Total Runs in 76th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 76th Over",
            MarketRegex = "^Total Runs in 76th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 95,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 77th Over",
    MarketRegex = "^Total Runs in 77th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 77th Over",
            MarketRegex = "^Total Runs in 77th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 96,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 78th Over",
    MarketRegex = "^Total Runs in 78th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 78th Over",
            MarketRegex = "^Total Runs in 78th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 97,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 79th Over",
    MarketRegex = "^Total Runs in 79th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 79th Over",
            MarketRegex = "^Total Runs in 79th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 98,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 80th Over",
    MarketRegex = "^Total Runs in 80th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 80th Over",
            MarketRegex = "^Total Runs in 80th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 99,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 81st Over",
    MarketRegex = "^Total Runs in 81st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 81st Over",
            MarketRegex = "^Total Runs in 81st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 100,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 82nd Over",
    MarketRegex = "^Total Runs in 82nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 82nd Over",
            MarketRegex = "^Total Runs in 82nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 101,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 83rd Over",
    MarketRegex = "^Total Runs in 83rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 83rd Over",
            MarketRegex = "^Total Runs in 83rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 102,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 84th Over",
    MarketRegex = "^Total Runs in 84th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 84th Over",
            MarketRegex = "^Total Runs in 84th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 103,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 85th Over",
    MarketRegex = "^Total Runs in 85th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 85th Over",
            MarketRegex = "^Total Runs in 85th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 104,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 86th Over",
    MarketRegex = "^Total Runs in 86th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 86th Over",
            MarketRegex = "^Total Runs in 86th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 105,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 87th Over",
    MarketRegex = "^Total Runs in 87th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 87th Over",
            MarketRegex = "^Total Runs in 87th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 106,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 88th Over",
    MarketRegex = "^Total Runs in 88th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 88th Over",
            MarketRegex = "^Total Runs in 88th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 107,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 89th Over",
    MarketRegex = "^Total Runs in 89th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 89th Over",
            MarketRegex = "^Total Runs in 89th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 108,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 90th Over",
    MarketRegex = "^Total Runs in 90th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 90th Over",
            MarketRegex = "^Total Runs in 90th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 109,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Total Score",
                                    MarketRegex = "^Total Score$",
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Set Handicap",
                                    MarketRegex = "^Set Handicap$",
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
                                }
                            }
                        }
                    }
                };
                 var tennisConfig = new SportConfiguration
                {
                    Sport = "Tennis",
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
                                }
                            }
                        },

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
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 2nd Wicket",
    MarketRegex = "^Runs at Fall of 2nd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 2nd Wicket",
            MarketRegex = "^Runs at Fall of 2nd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 11,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 3rd Wicket",
    MarketRegex = "^Runs at Fall of 3rd Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 3rd Wicket",
            MarketRegex = "^Runs at Fall of 3rd Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 12,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 4th Wicket",
    MarketRegex = "^Runs at Fall of 4th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 4th Wicket",
            MarketRegex = "^Runs at Fall of 4th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 13,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 5th Wicket",
    MarketRegex = "^Runs at Fall of 5th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 5th Wicket",
            MarketRegex = "^Runs at Fall of 5th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 14,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 6th Wicket",
    MarketRegex = "^Runs at Fall of 6th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 6th Wicket",
            MarketRegex = "^Runs at Fall of 6th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 15,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 7th Wicket",
    MarketRegex = "^Runs at Fall of 7th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 7th Wicket",
            MarketRegex = "^Runs at Fall of 7th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 16,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 8th Wicket",
    MarketRegex = "^Runs at Fall of 8th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 8th Wicket",
            MarketRegex = "^Runs at Fall of 8th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 17,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 9th Wicket",
    MarketRegex = "^Runs at Fall of 9th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 9th Wicket",
            MarketRegex = "^Runs at Fall of 9th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 18,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Runs at Fall of 10th Wicket",
    MarketRegex = "^Runs at Fall of 10th Wicket$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Runs at Fall of 10th Wicket",
            MarketRegex = "^Runs at Fall of 10th Wicket$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 19,
            Tags = new[] { "Main Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 1st Over",
    MarketRegex = "^Total Runs in 1st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 1st Over",
            MarketRegex = "^Total Runs in 1st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 20,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 2nd Over",
    MarketRegex = "^Total Runs in 2nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 2nd Over",
            MarketRegex = "^Total Runs in 2nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 21,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 3rd Over",
    MarketRegex = "^Total Runs in 3rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 3rd Over",
            MarketRegex = "^Total Runs in 3rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 22,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 4th Over",
    MarketRegex = "^Total Runs in 4th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 4th Over",
            MarketRegex = "^Total Runs in 4th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 23,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 5th Over",
    MarketRegex = "^Total Runs in 5th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 5th Over",
            MarketRegex = "^Total Runs in 5th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 24,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 6th Over",
    MarketRegex = "^Total Runs in 6th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 6th Over",
            MarketRegex = "^Total Runs in 6th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 25,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 7th Over",
    MarketRegex = "^Total Runs in 7th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 7th Over",
            MarketRegex = "^Total Runs in 7th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 26,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 8th Over",
    MarketRegex = "^Total Runs in 8th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 8th Over",
            MarketRegex = "^Total Runs in 8th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 27,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 9th Over",
    MarketRegex = "^Total Runs in 9th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 9th Over",
            MarketRegex = "^Total Runs in 9th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 28,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 10th Over",
    MarketRegex = "^Total Runs in 10th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 10th Over",
            MarketRegex = "^Total Runs in 10th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 29,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 11th Over",
    MarketRegex = "^Total Runs in 11th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 11th Over",
            MarketRegex = "^Total Runs in 11th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 30,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 12th Over",
    MarketRegex = "^Total Runs in 12th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 12th Over",
            MarketRegex = "^Total Runs in 12th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 31,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 13th Over",
    MarketRegex = "^Total Runs in 13th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 13th Over",
            MarketRegex = "^Total Runs in 13th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 32,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 14th Over",
    MarketRegex = "^Total Runs in 14th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 14th Over",
            MarketRegex = "^Total Runs in 14th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 33,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 15th Over",
    MarketRegex = "^Total Runs in 15th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 15th Over",
            MarketRegex = "^Total Runs in 15th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 34,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 16th Over",
    MarketRegex = "^Total Runs in 16th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 16th Over",
            MarketRegex = "^Total Runs in 16th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 35,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 17th Over",
    MarketRegex = "^Total Runs in 17th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 17th Over",
            MarketRegex = "^Total Runs in 17th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 36,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 18th Over",
    MarketRegex = "^Total Runs in 18th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 18th Over",
            MarketRegex = "^Total Runs in 18th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 37,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 19th Over",
    MarketRegex = "^Total Runs in 19th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 19th Over",
            MarketRegex = "^Total Runs in 19th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 38,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 20th Over",
    MarketRegex = "^Total Runs in 20th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 20th Over",
            MarketRegex = "^Total Runs in 20th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 39,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 21st Over",
    MarketRegex = "^Total Runs in 21st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 21st Over",
            MarketRegex = "^Total Runs in 21st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 40,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 22nd Over",
    MarketRegex = "^Total Runs in 22nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 22nd Over",
            MarketRegex = "^Total Runs in 22nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 41,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 23rd Over",
    MarketRegex = "^Total Runs in 23rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 23rd Over",
            MarketRegex = "^Total Runs in 23rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 42,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 24th Over",
    MarketRegex = "^Total Runs in 24th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 24th Over",
            MarketRegex = "^Total Runs in 24th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 43,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 25th Over",
    MarketRegex = "^Total Runs in 25th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 25th Over",
            MarketRegex = "^Total Runs in 25th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 44,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 26th Over",
    MarketRegex = "^Total Runs in 26th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 26th Over",
            MarketRegex = "^Total Runs in 26th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 45,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 27th Over",
    MarketRegex = "^Total Runs in 27th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 27th Over",
            MarketRegex = "^Total Runs in 27th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 46,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 28th Over",
    MarketRegex = "^Total Runs in 28th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 28th Over",
            MarketRegex = "^Total Runs in 28th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 47,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 29th Over",
    MarketRegex = "^Total Runs in 29th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 29th Over",
            MarketRegex = "^Total Runs in 29th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 48,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 30th Over",
    MarketRegex = "^Total Runs in 30th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 30th Over",
            MarketRegex = "^Total Runs in 30th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 49,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 31st Over",
    MarketRegex = "^Total Runs in 31st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 31st Over",
            MarketRegex = "^Total Runs in 31st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 50,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 32nd Over",
    MarketRegex = "^Total Runs in 32nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 32nd Over",
            MarketRegex = "^Total Runs in 32nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 51,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 33rd Over",
    MarketRegex = "^Total Runs in 33rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 33rd Over",
            MarketRegex = "^Total Runs in 33rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 52,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 34th Over",
    MarketRegex = "^Total Runs in 34th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 34th Over",
            MarketRegex = "^Total Runs in 34th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 53,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 35th Over",
    MarketRegex = "^Total Runs in 35th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 35th Over",
            MarketRegex = "^Total Runs in 35th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 54,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 36th Over",
    MarketRegex = "^Total Runs in 36th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 36th Over",
            MarketRegex = "^Total Runs in 36th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 55,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 37th Over",
    MarketRegex = "^Total Runs in 37th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 37th Over",
            MarketRegex = "^Total Runs in 37th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 56,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 38th Over",
    MarketRegex = "^Total Runs in 38th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 38th Over",
            MarketRegex = "^Total Runs in 38th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 57,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 39th Over",
    MarketRegex = "^Total Runs in 39th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 39th Over",
            MarketRegex = "^Total Runs in 39th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 58,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 40th Over",
    MarketRegex = "^Total Runs in 40th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 40th Over",
            MarketRegex = "^Total Runs in 40th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 59,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 41st Over",
    MarketRegex = "^Total Runs in 41st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 41st Over",
            MarketRegex = "^Total Runs in 41st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 60,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 42nd Over",
    MarketRegex = "^Total Runs in 42nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 42nd Over",
            MarketRegex = "^Total Runs in 42nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 61,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 43rd Over",
    MarketRegex = "^Total Runs in 43rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 43rd Over",
            MarketRegex = "^Total Runs in 43rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 62,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 44th Over",
    MarketRegex = "^Total Runs in 44th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 44th Over",
            MarketRegex = "^Total Runs in 44th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 63,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 45th Over",
    MarketRegex = "^Total Runs in 45th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 45th Over",
            MarketRegex = "^Total Runs in 45th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 64,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 46th Over",
    MarketRegex = "^Total Runs in 46th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 46th Over",
            MarketRegex = "^Total Runs in 46th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 65,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 47th Over",
    MarketRegex = "^Total Runs in 47th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 47th Over",
            MarketRegex = "^Total Runs in 47th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 66,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 48th Over",
    MarketRegex = "^Total Runs in 48th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 48th Over",
            MarketRegex = "^Total Runs in 48th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 67,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 49th Over",
    MarketRegex = "^Total Runs in 49th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 49th Over",
            MarketRegex = "^Total Runs in 49th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 68,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 50th Over",
    MarketRegex = "^Total Runs in 50th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 50th Over",
            MarketRegex = "^Total Runs in 50th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 69,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 51st Over",
    MarketRegex = "^Total Runs in 51st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 51st Over",
            MarketRegex = "^Total Runs in 51st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 70,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 52nd Over",
    MarketRegex = "^Total Runs in 52nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 52nd Over",
            MarketRegex = "^Total Runs in 52nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 71,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 53rd Over",
    MarketRegex = "^Total Runs in 53rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 53rd Over",
            MarketRegex = "^Total Runs in 53rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 72,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 54th Over",
    MarketRegex = "^Total Runs in 54th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 54th Over",
            MarketRegex = "^Total Runs in 54th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 73,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 55th Over",
    MarketRegex = "^Total Runs in 55th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 55th Over",
            MarketRegex = "^Total Runs in 55th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 74,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 56th Over",
    MarketRegex = "^Total Runs in 56th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 56th Over",
            MarketRegex = "^Total Runs in 56th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 75,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 57th Over",
    MarketRegex = "^Total Runs in 57th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 57th Over",
            MarketRegex = "^Total Runs in 57th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 76,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 58th Over",
    MarketRegex = "^Total Runs in 58th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 58th Over",
            MarketRegex = "^Total Runs in 58th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 77,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 59th Over",
    MarketRegex = "^Total Runs in 59th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 59th Over",
            MarketRegex = "^Total Runs in 59th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 78,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 60th Over",
    MarketRegex = "^Total Runs in 60th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 60th Over",
            MarketRegex = "^Total Runs in 60th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 79,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 61st Over",
    MarketRegex = "^Total Runs in 61st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 61st Over",
            MarketRegex = "^Total Runs in 61st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 80,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 62nd Over",
    MarketRegex = "^Total Runs in 62nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 62nd Over",
            MarketRegex = "^Total Runs in 62nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 81,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 63rd Over",
    MarketRegex = "^Total Runs in 63rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 63rd Over",
            MarketRegex = "^Total Runs in 63rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 82,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 64th Over",
    MarketRegex = "^Total Runs in 64th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 64th Over",
            MarketRegex = "^Total Runs in 64th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 83,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 65th Over",
    MarketRegex = "^Total Runs in 65th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 65th Over",
            MarketRegex = "^Total Runs in 65th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 84,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},

new MarketConfigGroup
{
    Name = "Total Runs in 66th Over",
    MarketRegex = "^Total Runs in 66th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 66th Over",
            MarketRegex = "^Total Runs in 66th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 85,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 67th Over",
    MarketRegex = "^Total Runs in 67th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 67th Over",
            MarketRegex = "^Total Runs in 67th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 86,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 68th Over",
    MarketRegex = "^Total Runs in 68th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 68th Over",
            MarketRegex = "^Total Runs in 68th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 87,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 69th Over",
    MarketRegex = "^Total Runs in 69th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 69th Over",
            MarketRegex = "^Total Runs in 69th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 88,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 70th Over",
    MarketRegex = "^Total Runs in 70th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 70th Over",
            MarketRegex = "^Total Runs in 70th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 89,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 71st Over",
    MarketRegex = "^Total Runs in 71st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 71st Over",
            MarketRegex = "^Total Runs in 71st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 90,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 72nd Over",
    MarketRegex = "^Total Runs in 72nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 72nd Over",
            MarketRegex = "^Total Runs in 72nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 91,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 73rd Over",
    MarketRegex = "^Total Runs in 73rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 73rd Over",
            MarketRegex = "^Total Runs in 73rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 92,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 74th Over",
    MarketRegex = "^Total Runs in 74th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 74th Over",
            MarketRegex = "^Total Runs in 74th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 93,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 75th Over",
    MarketRegex = "^Total Runs in 75th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 75th Over",
            MarketRegex = "^Total Runs in 75th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 94,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 76th Over",
    MarketRegex = "^Total Runs in 76th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 76th Over",
            MarketRegex = "^Total Runs in 76th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 95,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 77th Over",
    MarketRegex = "^Total Runs in 77th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 77th Over",
            MarketRegex = "^Total Runs in 77th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 96,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 78th Over",
    MarketRegex = "^Total Runs in 78th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 78th Over",
            MarketRegex = "^Total Runs in 78th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 97,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 79th Over",
    MarketRegex = "^Total Runs in 79th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 79th Over",
            MarketRegex = "^Total Runs in 79th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 98,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 80th Over",
    MarketRegex = "^Total Runs in 80th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 80th Over",
            MarketRegex = "^Total Runs in 80th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 99,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 81st Over",
    MarketRegex = "^Total Runs in 81st Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 81st Over",
            MarketRegex = "^Total Runs in 81st Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 100,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 82nd Over",
    MarketRegex = "^Total Runs in 82nd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 82nd Over",
            MarketRegex = "^Total Runs in 82nd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 101,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 83rd Over",
    MarketRegex = "^Total Runs in 83rd Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 83rd Over",
            MarketRegex = "^Total Runs in 83rd Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 102,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 84th Over",
    MarketRegex = "^Total Runs in 84th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 84th Over",
            MarketRegex = "^Total Runs in 84th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 103,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 85th Over",
    MarketRegex = "^Total Runs in 85th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 85th Over",
            MarketRegex = "^Total Runs in 85th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 104,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 86th Over",
    MarketRegex = "^Total Runs in 86th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 86th Over",
            MarketRegex = "^Total Runs in 86th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 105,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 87th Over",
    MarketRegex = "^Total Runs in 87th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 87th Over",
            MarketRegex = "^Total Runs in 87th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 106,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 88th Over",
    MarketRegex = "^Total Runs in 88th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 88th Over",
            MarketRegex = "^Total Runs in 88th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 107,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 89th Over",
    MarketRegex = "^Total Runs in 89th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 89th Over",
            MarketRegex = "^Total Runs in 89th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 108,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
        }
    }
},
new MarketConfigGroup
{
    Name = "Total Runs in 90th Over",
    MarketRegex = "^Total Runs in 90th Over$",
    Markets = new List<MarketConfig>
    {
        new MarketConfig
        {
            MarketName = "Total Runs in 90th Over",
            MarketRegex = "^Total Runs in 90th Over$",
            MarketWhere = null,
            MarketGroupBy = null,
            MarketSelect = null,
            Description = "bet is valid after game is done",
            Order = 109,
            Tags = new[] { "Over Markets" },
            OutcomeWhere = "Header != null",
            OutcomeName = "(Header) + \" (\" + Name + \")\"",
            OutcomeOdd = "Odd",
            OutcomeHandicap = "decimal.Parse(Name)"
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Total Score",
                                    MarketRegex = "^Total Score$",
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
                                },
                                new MarketConfig
                                {
                                    MarketName = "Set Handicap",
                                    MarketRegex = "^Set Handicap$",
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
                                }
                            }
                        }
                    }
                };

                var result = await InsertSportConfigurationAsync(cricketConfig);
                var result1 = await InsertSportConfigurationAsync(baseballConfig);
                var result2 = await InsertSportConfigurationAsync(tennisConfig);
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
