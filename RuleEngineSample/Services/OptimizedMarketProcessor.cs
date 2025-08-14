using RuleEngineSample.Models;
using System.Linq.Dynamic.Core;

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
            var allMarkets = new List<DbMarket>();
            var allOdds = new List<DbOdd>();

            foreach (var configGroup in sportConfig.MarketConfigs)
            {
                // Use compiled regex - much faster
                if (!configGroup.CompiledRegex.IsMatch(marketDto.Name))
                    continue;

                // Process markets within this group
                foreach (var marketConfig in configGroup.Markets)
                {
                    List<string> marketNames = new();

                    if (marketConfig.NameMustSetFromMarketName)
                    {
                        marketNames.Add(marketConfig.MarketName);
                    }
                    else
                    {
                        // Use compiled expressions if available, fallback to dynamic if not
                        if (marketConfig.CompiledMarketWhere != null && marketConfig.CompiledMarketSelect != null)
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
                        else
                        {
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
            }

            return (allMarkets, allOdds);
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
                if (marketConfig.CompiledOddWhere != null)
                {
                    // Use compiled expression for filtering
                    var filteredOddsList = marketDto.Odds.Where(marketConfig.CompiledOddWhere);

                    foreach (var odd in filteredOddsList)
                    {
                        try
                        {
                            if (marketConfig.CompiledOddSelect != null)
                            {
                                // Use compiled expression for selection
                                var result = marketConfig.CompiledOddSelect(odd);
                                var dbOdd = CreateDbOddFromResult(result, marketName);
                                if (dbOdd != null)
                                {
                                    filteredOdds.Add(dbOdd);
                                }
                            }
                            else
                            {
                                // Fallback to dynamic processing
                                var dbOdd = ProcessOddWithFallback(odd, marketConfig, marketName);
                                if (dbOdd != null)
                                {
                                    filteredOdds.Add(dbOdd);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing odd with compiled expression: {ex.Message}");
                            // Fallback to dynamic processing
                            var dbOdd = ProcessOddWithFallback(odd, marketConfig, marketName);
                            if (dbOdd != null)
                            {
                                filteredOdds.Add(dbOdd);
                            }
                        }
                    }
                }
                else
                {
                    // Fallback to dynamic processing
                    var dynamicResults = marketDto.Odds.AsQueryable()
                        .Where(marketConfig.OddWhere)
                        .Select(_fallbackConfig, marketConfig.OddSelect);

                    foreach (var dynamicObj in dynamicResults)
                    {
                        var dbOdd = CreateDbOddFromDynamicResult(dynamicObj, marketName);
                        if (dbOdd != null)
                        {
                            filteredOdds.Add(dbOdd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing odds for market {marketName}: {ex.Message}");
            }

            return filteredOdds;
        }

        private DbOdd? ProcessOddWithFallback(OddsDto odd, CompiledMarketConfig marketConfig, string marketName)
        {
            try
            {
                var dynamicResults = new List<OddsDto> { odd }.AsQueryable()
                    .Where(marketConfig.OddWhere)
                    .Select(_fallbackConfig, marketConfig.OddSelect);

                foreach (var dynamicObj in dynamicResults)
                {
                    return CreateDbOddFromDynamicResult(dynamicObj, marketName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in fallback odd processing: {ex.Message}");
            }

            return null;
        }

        private DbOdd? CreateDbOddFromResult(object result, string marketName)
        {
            try
            {
                var resultType = result.GetType();
                var nameProperty = resultType.GetProperty("Name");
                var oddProperty = resultType.GetProperty("Odd");
                var handicapProperty = resultType.GetProperty("Handicap");

                if (nameProperty != null && oddProperty != null)
                {
                    var name = nameProperty.GetValue(result)?.ToString() ?? "";
                    var odd = Convert.ToDecimal(oddProperty.GetValue(result));

                    if (handicapProperty != null)
                    {
                        var handicap = handicapProperty.GetValue(result);
                        if (handicap != null && decimal.TryParse(handicap.ToString(), out var handicapValue))
                        {
                            return new DbOdd(marketName, name, odd, handicapValue);
                        }
                    }

                    return new DbOdd(marketName, name, odd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating DbOdd from compiled result: {ex.Message}");
            }

            return null;
        }

        private DbOdd? CreateDbOddFromDynamicResult(dynamic dynamicObj, string marketName)
        {
            try
            {
                dynamic obj = dynamicObj;

                if (((object)obj).GetType().GetProperty("Handicap") != null)
                {
                    return new DbOdd(marketName, obj.Name, obj.Odd, obj.Handicap);
                }
                else
                {
                    return new DbOdd(marketName, obj.Name, obj.Odd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating DbOdd from dynamic result: {ex.Message}");
                return null;
            }
        }
    }
}
