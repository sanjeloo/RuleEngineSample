using RuleEngineSample.Models;
using RuleEngineSample.Services;

namespace RuleEngineSample.Services
{
    public class CacheService
    {
        private readonly Dictionary<string, CompiledSportConfiguration> _cache = new();
        private readonly Dictionary<string, DateTime> _lastUpdated = new();
        private readonly object _lockObject = new object();
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30); // Configurable cache expiration

        public CompiledSportConfiguration? GetCachedConfiguration(string sport)
        {
            lock (_lockObject)
            {
                if (_cache.TryGetValue(sport, out var config))
                {
                    if (_lastUpdated.TryGetValue(sport, out var lastUpdate))
                    {
                        // Check if cache is still valid
                        if (DateTime.UtcNow - lastUpdate < _cacheExpiration)
                        {
                            return config;
                        }
                        else
                        {
                            // Cache expired, remove it
                            _cache.Remove(sport);
                            _lastUpdated.Remove(sport);
                        }
                    }
                }
                return null;
            }
        }

        public void CacheConfiguration(string sport, CompiledSportConfiguration configuration)
        {
            lock (_lockObject)
            {
                _cache[sport] = configuration;
                _lastUpdated[sport] = DateTime.UtcNow;
            }
        }

        public void InvalidateCache(string sport)
        {
            lock (_lockObject)
            {
                _cache.Remove(sport);
                _lastUpdated.Remove(sport);
            }
        }

        public void InvalidateAllCache()
        {
            lock (_lockObject)
            {
                _cache.Clear();
                _lastUpdated.Clear();
            }
        }

        public bool IsConfigurationCached(string sport)
        {
            lock (_lockObject)
            {
                return _cache.ContainsKey(sport) && 
                       _lastUpdated.ContainsKey(sport) && 
                       DateTime.UtcNow - _lastUpdated[sport] < _cacheExpiration;
            }
        }

        public List<string> GetCachedSports()
        {
            lock (_lockObject)
            {
                return _cache.Keys.ToList();
            }
        }

        public void UpdateConfiguration(string sport, CompiledSportConfiguration configuration)
        {
            lock (_lockObject)
            {
                _cache[sport] = configuration;
                _lastUpdated[sport] = DateTime.UtcNow;
            }
        }

        public void RemoveConfiguration(string sport)
        {
            lock (_lockObject)
            {
                _cache.Remove(sport);
                _lastUpdated.Remove(sport);
            }
        }

        public int GetCacheSize()
        {
            lock (_lockObject)
            {
                return _cache.Count;
            }
        }

        public void ClearExpiredCache()
        {
            lock (_lockObject)
            {
                var expiredSports = _lastUpdated
                    .Where(kvp => DateTime.UtcNow - kvp.Value >= _cacheExpiration)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var sport in expiredSports)
                {
                    _cache.Remove(sport);
                    _lastUpdated.Remove(sport);
                }
            }
        }
    }
}
