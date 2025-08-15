using RuleEngineSample.Models;
using RuleEngineSample.Utils;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.RegularExpressions;

namespace RuleEngineSample.Utils
{
    public static class GeneralFunctions
    {
        public static string GetCompetitor(string name) => name == "1" ? "Home" : "Away";
        
        // MongoDB connection string - update this with your actual connection string
        public static string GetMongoConnectionString() => "mongodb://localhost:27017";
        public static string GetDatabaseName() => "RuleEngineDB";
        public static string GetCollectionName() => "SportConfigurations";
        
        // String extension methods for dynamic LINQ
        public static string[] Split(this string str, string separator) => str.Split(separator);
        public static string[] Split(this string str, string separator, StringSplitOptions options) => str.Split(separator, options);
        public static string[] Split(this string str, char separator) => str.Split(separator);
        public static string[] Split(this string str, char separator, StringSplitOptions options) => str.Split(separator, options);
        
        public static bool StartsWith(this string str, string value) => str.StartsWith(value);
        public static bool StartsWith(this string str, string value, StringComparison comparisonType) => str.StartsWith(value, comparisonType);
        
        public static string Substring(this string str, int startIndex) => str.Substring(startIndex);
        public static string Substring(this string str, int startIndex, int length) => str.Substring(startIndex, length);
        
        public static string Trim(this string str) => str.Trim();
        public static string Trim(this string str, char trimChar) => str.Trim(trimChar);
        public static string Trim(this string str, char[] trimChars) => str.Trim(trimChars);
        
        public static string ToUpper(this string str) => str.ToUpper();
        public static string ToLower(this string str) => str.ToLower();
        
        public static int IndexOf(this string str, string value) => str.IndexOf(value);
        public static int IndexOf(this string str, char value) => str.IndexOf(value);
        
        public static int LastIndexOf(this string str, string value) => str.LastIndexOf(value);
        public static int LastIndexOf(this string str, char value) => str.LastIndexOf(value);
        
        public static string Replace(this string str, string oldValue, string newValue) => str.Replace(oldValue, newValue);
        public static string Replace(this string str, char oldChar, char newChar) => str.Replace(oldChar, newChar);
        
        public static bool Contains(this string str, string value) => str.Contains(value);
        public static bool Contains(this string str, char value) => str.Contains(value);
        
        public static bool EndsWith(this string str, string value) => str.EndsWith(value);
        public static bool EndsWith(this string str, string value, StringComparison comparisonType) => str.EndsWith(value, comparisonType);
        
        public static string PadLeft(this string str, int totalWidth) => str.PadLeft(totalWidth);
        public static string PadLeft(this string str, int totalWidth, char paddingChar) => str.PadLeft(totalWidth, paddingChar);
        
        public static string PadRight(this string str, int totalWidth) => str.PadRight(totalWidth);
        public static string PadRight(this string str, int totalWidth, char paddingChar) => str.PadRight(totalWidth, paddingChar);
        
        public static string Remove(this string str, int startIndex) => str.Remove(startIndex);
        public static string Remove(this string str, int startIndex, int count) => str.Remove(startIndex, count);
        
        public static string Insert(this string str, int startIndex, string value) => str.Insert(startIndex, value);
        
        // Math helper methods
        public static decimal ParseDecimal(string value) => decimal.Parse(value);
        public static int ParseInt(string value) => int.Parse(value);
        public static double ParseDouble(string value) => double.Parse(value);
        
        // Regex helper methods
        public static bool IsMatch(string input, string pattern) => Regex.IsMatch(input, pattern);
        public static bool IsMatch(string input, string pattern, RegexOptions options) => Regex.IsMatch(input, pattern, options);
        
        // Array/Collection helper methods
        public static T ElementAt<T>(IEnumerable<T> source, int index) => source.ElementAt(index);
        public static T First<T>(IEnumerable<T> source) => source.First();
        public static T FirstOrDefault<T>(IEnumerable<T> source) => source.FirstOrDefault();
        public static T Last<T>(IEnumerable<T> source) => source.Last();
        public static T LastOrDefault<T>(IEnumerable<T> source) => source.LastOrDefault();
        public static int Count<T>(IEnumerable<T> source) => source.Count();
        public static bool Any<T>(IEnumerable<T> source) => source.Any();
        public static bool All<T>(IEnumerable<T> source, Func<T, bool> predicate) => source.All(predicate);
    }

    public class MyCustomTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public override HashSet<Type> GetCustomTypes()
        {
            var types = base.GetCustomTypes();
            types.Add(typeof(GeneralFunctions));
            types.Add(typeof(DbOdd));
            types.Add(typeof(OddsDto));
            types.Add(typeof(Regex)); 
            types.Add(typeof(RegexOptions)); 
            types.Add(typeof(StringSplitOptions)); 
            types.Add(typeof(StringComparison));
            types.Add(typeof(decimal)); 
            types.Add(typeof(int)); 
            types.Add(typeof(double)); 
            types.Add(typeof(bool));
            types.Add(typeof(string));
            types.Add(typeof(Array));
            types.Add(typeof(IEnumerable<>)); 
            types.Add(typeof(List<>));
            return types;
        }
    }
}

