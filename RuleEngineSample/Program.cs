using RulesEngine.Models;
using RuleEngineSample.Models;
using System.Text.RegularExpressions;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using RuleEngineSample.Utils;
using RuleEngineSample.Sample;

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Load the rules from JSON file
        var currentDir = Directory.GetCurrentDirectory();
        var projectRoot = currentDir;

        // Navigate up until we find the directory containing the .csproj file
        while (!Directory.GetFiles(projectRoot, "*.csproj").Any() && Directory.GetParent(projectRoot) != null)
        {
            projectRoot = Directory.GetParent(projectRoot)!.FullName;
        }
        var alljson = Directory.GetFiles(projectRoot, "discount-rules.json");

        var json = File.ReadAllText(alljson[0]);
        var workflowRules = JsonSerializer.Deserialize<List<Workflow>>(json);

        // 2. Create RulesEngine instance
        var reSettings = new ReSettings
        {
            CustomTypes =
            [
                typeof(Regex),
                typeof(RegexOptions),
            ]
        };
        var config = new ParsingConfig
        {
            CustomTypeProvider = new MyCustomTypeProvider()
        };
        var re = new RulesEngine.RulesEngine(workflowRules!.ToArray(), reSettings);
        List<MarketDto> marketDtos = SampleData.GetSamples();
        
        // 3. Sample order object
        //var market = ;
        foreach (var marketDto in marketDtos)
        {
            var resultList = await re.ExecuteAllRulesAsync("Cricket", marketDto);
            var dbMarkets = new List<DbMarket>();
            var dbOdds = new List<DbOdd>();

            var resultSuccess = resultList.SingleOrDefault(c => c.IsSuccess);
            if (resultSuccess is null)
            {
                Console.WriteLine("nothing found");
                continue;
            }

            if (!resultSuccess.Rule.Properties.TryGetValue("Markets", out var marketsObj) ||
                marketsObj is not JsonElement jsonElement || jsonElement.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            var marketConfigs = jsonElement.EnumerateArray()
                .Select(element => JsonSerializer.Deserialize<MarketConfig>(element.GetRawText()))
                .OfType<MarketConfig>()
                .ToList();

            foreach (var marketConfig in marketConfigs)
            {
                string marketName;
                if (marketConfig.NameMustSetFromMarketName)
                {
                    marketName = marketConfig.MarketName!;
                }
                else
                {
                    marketName = marketDto.Odds.AsQueryable()
                        .Where(marketConfig.MarketWhere)
                        .GroupBy(marketConfig.MarketGroupBy)
                        .Select(marketConfig.MarketSelect)
                        .Cast<string>().FirstOrDefault()!;
                }

                dbMarkets.Add(new DbMarket(marketName, marketConfig.Description, marketConfig.Order, marketConfig.Tags));
                // create market here
              


                // Now use the config in Select
                var dynamicResults = marketDto.Odds.AsQueryable()
                    .Where(marketConfig.OddWhere)
                    .Select(config, marketConfig.OddSelect);

                var filteredOdds = new List<DbOdd>();
                foreach (var dynamicObj in dynamicResults)
                {
                    // Cast the dynamic object to DbOdd
                    dynamic obj = dynamicObj;

                    if (((object)obj).GetType().GetProperty("Handicap") != null)
                    {
                        filteredOdds.Add(new DbOdd(marketName, obj.Name, obj.Odd, obj.Handicap));
                    }
                    else
                    {
                        filteredOdds.Add(new DbOdd(marketName, obj.Name, obj.Odd));
                    }
                }
                dbOdds.AddRange(filteredOdds);

            }

            Console.WriteLine("done");

        }

    }

}
