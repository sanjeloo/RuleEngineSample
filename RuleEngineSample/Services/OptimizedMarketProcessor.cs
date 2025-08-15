using RuleEngineSample.Models;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace RuleEngineSample.Services
{
    public class OptimizedMarketProcessor
    {
        private readonly ParsingConfig _fallbackConfig;

        public OptimizedMarketProcessor()
        {
            _fallbackConfig = new ParsingConfig
            {
                CustomTypeProvider = new Utils.MyCustomTypeProvider()
            };
        }

        public (List<DbMarket> Markets, List<DbOdd> Odds) ProcessMarketWithCompiledConfig(
            MarketDto marketDto, CompiledSportConfiguration sportConfig)
        {
            try
            {
                var allMarkets = new List<DbMarket>();
                var allOdds = new List<DbOdd>();
                var configGroup = sportConfig.MarketConfigs.SingleOrDefault(c => c.CompiledRegex.IsMatch(marketDto.Name));

                if (configGroup is null)
                {
                    return (allMarkets, allOdds);
                }

                // Process markets within this group
                foreach (var marketConfig in configGroup.Markets)
                {
                    List<string> marketNames = new();

                    if (string.IsNullOrEmpty(marketConfig.MarketWhere))
                    {
                        marketNames.Add(marketConfig.MarketName);
                    }
                    else
                    {
                      
                        try
                        {
                            var filteredOdds = marketDto.Odds.Where(marketConfig.CompiledMarketWhere);
                            var groupedOdds = filteredOdds.GroupBy(o => o.Name);

                            foreach (var group in groupedOdds)
                            {
                                var marketName = marketConfig.CompiledMarketSelect(group);
                                if (!string.IsNullOrEmpty(marketName))
                                {
                                    marketNames.Add(marketName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error using compiled expressions for {marketConfig.MarketName}: {ex.Message}");
                            // Fallback to dynamic processing
                            marketNames = ProcessMarketNamesWithFallback(marketDto, marketConfig);
                        }
                       
                    }

                    foreach (var marketName in marketNames)
                    {
                        if (string.IsNullOrEmpty(marketName))
                            continue;

                        allMarkets.Add(new DbMarket(marketName, marketConfig.Description, marketConfig.Order, marketConfig.Tags));

                        // Process odds using compiled expressions
                        var filteredOdds = ProcessOddsWithCompiledConfig(marketDto, marketConfig, marketName);
                        allOdds.AddRange(filteredOdds);
                    }
                }


                return (allMarkets, allOdds);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private List<string> ProcessMarketNamesWithFallback(MarketDto marketDto, CompiledMarketConfig marketConfig)
        {
            try
            {
                var marketNames = marketDto.Odds.AsQueryable()
                    .Where(marketConfig.MarketWhere ?? "true")
                    .GroupBy(marketConfig.MarketGroupBy ?? "Name")
                    .Select(marketConfig.MarketSelect ?? "Key")
                    .Cast<string>()
                    .ToList();

                return marketNames;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in fallback market name processing for {marketConfig.MarketName}: {ex.Message}");
                return new List<string>();
            }
        }

        private List<DbOdd> ProcessOddsWithCompiledConfig(MarketDto marketDto, CompiledMarketConfig marketConfig, string marketName)
        {
            var filteredOdds = new List<DbOdd>();

            try
            {
                // Use compiled expression for filtering
                var filteredOddsList = marketDto.Odds.Where(marketConfig.CompiledOutcomeWhere).ToList();
                filteredOdds = filteredOddsList.Select(
                   c =>
                   {
                       var odd = new DbOdd
                       {
                           MarketName = marketName,
                           Name = marketConfig.CompiledOutcomeName is null ? c.Name : marketConfig.CompiledOutcomeName(c),
                           Odd = marketConfig.CompiledOutcomeOdd is null ? c.Odd : marketConfig.CompiledOutcomeOdd(c),
                           Handicap = marketConfig.CompiledOutcomeHandicap is null ? null : marketConfig.CompiledOutcomeHandicap(c)
                       };
                       return odd;
                   }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing odds for market {marketName}: {ex.Message}");
            }

            return filteredOdds;
        }
    }
}
