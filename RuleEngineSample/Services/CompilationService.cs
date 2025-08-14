using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using RuleEngineSample.Models;
using RuleEngineSample.Utils;

namespace RuleEngineSample.Services
{
    public class CompilationService
    {
        private readonly ParsingConfig _config;

        public CompilationService()
        {
            _config = new ParsingConfig
            {
                CustomTypeProvider = new MyCustomTypeProvider()
            };
        }

        public CompiledSportConfiguration CompileSportConfiguration(SportConfiguration sportConfig)
        {
            var compiledConfig = new CompiledSportConfiguration
            {
                Id = sportConfig.Id,
                Sport = sportConfig.Sport,
                MarketConfigs = new List<CompiledMarketConfigGroup>()
            };

            foreach (var configGroup in sportConfig.MarketConfigs)
            {
                var compiledGroup = new CompiledMarketConfigGroup
                {
                    Id = configGroup.Id,
                    Name = configGroup.Name,
                    CompiledRegex = new Regex(configGroup.MarketRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    Markets = new List<CompiledMarketConfig>()
                };

                foreach (var marketConfig in configGroup.Markets)
                {
                    var compiledMarket = new CompiledMarketConfig
                    {
                        Id = marketConfig.Id,
                        MarketName = marketConfig.MarketName,
                        MarketRegex = marketConfig.MarketRegex,
                        NameMustSetFromMarketName = marketConfig.NameMustSetFromMarketName,
                        MarketWhere = marketConfig.MarketWhere,
                        MarketGroupBy = marketConfig.MarketGroupBy,
                        MarketSelect = marketConfig.MarketSelect,
                        Description = marketConfig.Description,
                        Order = marketConfig.Order,
                        Tags = marketConfig.Tags,
                        OddWhere = marketConfig.OddWhere,
                        OddSelect = marketConfig.OddSelect
                    };

                    // Compile expressions for performance
                    try
                    {
                        // Compile MarketWhere expression
                        if (!string.IsNullOrEmpty(marketConfig.MarketWhere))
                        {
                            compiledMarket.CompiledMarketWhere = DynamicExpressionParser
                                .ParseLambda<OddsDto, bool>(_config,false, marketConfig.MarketWhere)
                                .Compile();
                        }

                        // Compile MarketSelect expression
                        if (!string.IsNullOrEmpty(marketConfig.MarketSelect))
                        {
                            compiledMarket.CompiledMarketSelect = DynamicExpressionParser
                                .ParseLambda<IGrouping<string, OddsDto>, string>(_config, false, marketConfig.MarketSelect)
                                .Compile();
                        }

                        // Compile OddWhere expression
                        compiledMarket.CompiledOddWhere = DynamicExpressionParser
                            .ParseLambda<OddsDto, bool>(_config, false, marketConfig.OddWhere)
                            .Compile();

                        // Compile OddSelect expression
                        compiledMarket.CompiledOddSelect = DynamicExpressionParser
                            .ParseLambda<OddsDto, object>(_config, false, marketConfig.OddSelect)
                            .Compile();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error compiling expressions for market {marketConfig.MarketName}: {ex.Message}");
                        // Continue with uncompiled version if compilation fails
                    }

                    compiledGroup.Markets.Add(compiledMarket);
                }

                compiledConfig.MarketConfigs.Add(compiledGroup);
            }

            return compiledConfig;
        }

        public List<CompiledSportConfiguration> CompileAllSportConfigurations(List<SportConfiguration> sportConfigs)
        {
            var compiledConfigs = new List<CompiledSportConfiguration>();
            
            foreach (var sportConfig in sportConfigs)
            {
                try
                {
                    var compiled = CompileSportConfiguration(sportConfig);
                    compiledConfigs.Add(compiled);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error compiling sport configuration for {sportConfig.Sport}: {ex.Message}");
                }
            }

            return compiledConfigs;
        }
    }
}
