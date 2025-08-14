using RuleEngineSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineSample.Sample
{
    public static class SampleData
    {
       public static List<MarketDto> GetSamples() => [
            new ()
            {
                Name = "match lines",
                Odds =
                [
                    new (){Name= "To Win", Header="1", Odd= 1.90M, Handicap= ""},
                    new (){Name= "To Win", Header="2", Odd= 1.90M, Handicap= ""},
                    new (){Name= "Total", Header="1", Odd= 1.90M, Handicap="O 74.5"},
                    new (){Name= "Total", Header="2", Odd= 1.90M, Handicap="U 74.5"},
                    new (){Name= "Handicap", Header="2", Odd= 1.90M, Handicap="+1.5"},
                    new (){Name= "Handicap", Header="1", Odd= 1.90M, Handicap="+1.5"},
                ]
            },
            new ()
            {
                Name = "Batter Matches (Most Runs)",
                Odds =
                [
                    new (){Name= "R Rickelton v WG Jacks", Header="1", Odd= 1.90M, Handicap= ""},
                    new (){Name= "TM Head v Abhishek Sharma", Header="2", Odd= 1.90M, Handicap= ""},
                ]
            },
            new ()
            {
                Name = "Runs at Fall of 1st Wicket",
                Odds =
                [
                    new (){Name= "26.5", Header="Over", Odd= 1.90M, Handicap= ""},
                    new (){Name= "26.5", Header="Under", Odd= 1.90M, Handicap= ""},
                ]
            }
        ];
    }
}
