using RuleEngineSample.Utils;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace RuleEngineSample.Utils
{
    public static class GeneralFunctions
    {
        public static string GetCompetitor(string name) => name == "1" ? "Home" : "Away";
    }


    public class MyCustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes()
        {
            var types = base.GetCustomTypes();
            types.Add(typeof(GeneralFunctions)); // Your static helper class
            return types;
        }
    }
}

