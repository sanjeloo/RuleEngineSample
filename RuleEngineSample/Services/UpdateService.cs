using RuleEngineSample.Models;
using RuleEngineSample.Services;

namespace RuleEngineSample.Services
{
    public class UpdateService
    {
        private readonly MongoService _mongoService;
        private readonly CompilationService _compilationService;
        private readonly CacheService _cacheService;

        public UpdateService(
            MongoService mongoService,
            CompilationService compilationService,
            CacheService cacheService)
        {
            _mongoService = mongoService;
            _compilationService = compilationService;
            _cacheService = cacheService;
        }

        public async Task<bool> UpdateSportConfigurationAsync(string sportId, SportConfiguration updatedConfig)
        {
            try
            {
                // Update in database
                var success = await _mongoService.UpdateSportConfigurationAsync(sportId, updatedConfig);
                
                if (success)
                {
                    // Invalidate cache for this sport
                    _cacheService.InvalidateCache(updatedConfig.Sport);
                    
                    // Recompile and cache the updated configuration
                    var compiledConfig = _compilationService.CompileSportConfiguration(updatedConfig);
                    _cacheService.CacheConfiguration(updatedConfig.Sport, compiledConfig);
                    
                    Console.WriteLine($"Configuration updated for {updatedConfig.Sport} and cache refreshed.");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddNewSportConfigurationAsync(SportConfiguration newConfig)
        {
            try
            {
                // Insert into database
                var success = await _mongoService.InsertSportConfigurationAsync(newConfig);
                
                if (success)
                {
                    // Compile and cache the new configuration
                    var compiledConfig = _compilationService.CompileSportConfiguration(newConfig);
                    _cacheService.CacheConfiguration(newConfig.Sport, compiledConfig);
                    
                    Console.WriteLine($"New configuration added for {newConfig.Sport} and cached.");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteSportConfigurationAsync(string sportId, string sportName)
        {
            try
            {
                // Delete from database
                var success = await _mongoService.DeleteSportConfigurationAsync(sportId);
                
                if (success)
                {
                    // Remove from cache
                    _cacheService.RemoveConfiguration(sportName);
                    
                    Console.WriteLine($"Configuration deleted for {sportName} and removed from cache.");
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sport configuration: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RefreshConfigurationAsync(string sport)
        {
            try
            {
                // Invalidate cache
                _cacheService.InvalidateCache(sport);
                
                // Reload from database
                var dbConfig = await _mongoService.GetSportConfigurationBySportAsync(sport);
                if (dbConfig != null)
                {
                    // Recompile and cache
                    var compiledConfig = _compilationService.CompileSportConfiguration(dbConfig);
                    _cacheService.CacheConfiguration(sport, compiledConfig);
                    
                    Console.WriteLine($"Configuration refreshed for {sport}.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Configuration not found for {sport}.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing configuration for {sport}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RefreshAllConfigurationsAsync()
        {
            try
            {
                // Invalidate all cache
                _cacheService.InvalidateAllCache();
                
                // Reload all from database
                var allConfigs = await _mongoService.GetAllSportConfigurationsAsync();
                
                // Recompile and cache all
                var compiledConfigs = _compilationService.CompileAllSportConfigurations(allConfigs);
                
                foreach (var compiledConfig in compiledConfigs)
                {
                    _cacheService.CacheConfiguration(compiledConfig.Sport, compiledConfig);
                }
                
                Console.WriteLine($"All configurations refreshed. {compiledConfigs.Count} configurations cached.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing all configurations: {ex.Message}");
                return false;
            }
        }

    }
}
