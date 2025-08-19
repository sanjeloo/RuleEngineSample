using RuleEngineSample.Models;
using RuleEngineSample.Services;
using RuleEngineSample.Sample;

class Program
{
    private static MongoService _mongoService = null!;
    private static CompilationService _compilationService = null!;
    private static CacheService _cacheService = null!;
    private static OptimizedMarketProcessor _marketProcessor = null!;

    static async Task Main(string[] args)
    {
        try
        {
            // Initialize services
            InitializeServices();

            // Initialize database with default Cricket configuration if empty
            await InitializeDatabaseAsync();

            var initialMemory = GC.GetTotalMemory(false) / (1024.0 * 1024.0);

            // Load and compile configurations
            await LoadAndCompileConfigurationsAsync();

            var beforeGCApply = GC.GetTotalMemory(false) / (1024.0 * 1024.0);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            var afterCompiledSize = GC.GetTotalMemory(false) / (1024.0 * 1024.0);

            // Process sample data using cached compiled configurations
            await ProcessSampleDataAsync();

            Console.WriteLine("Processing completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in main program: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    private static void InitializeServices()
    {
        Console.WriteLine("Initializing services...");
        _mongoService = new MongoService();
        _compilationService = new CompilationService();
        _cacheService = new CacheService();
        _marketProcessor = new OptimizedMarketProcessor();
        Console.WriteLine("Services initialized successfully.");
    }

    private static async Task InitializeDatabaseAsync()
    {
        Console.WriteLine("Initializing database...");
        
        try
        {
            // Check if we have any configurations
            var existingConfigs = await _mongoService.GetAllSportConfigurationsAsync();
            
            if (!existingConfigs.Any())
            {
                Console.WriteLine("No configurations found. Creating default Cricket configuration...");
                var success = await _mongoService.InitializeDefaultCricketConfigurationAsync();
                if (success)
                {
                    Console.WriteLine("Default Cricket configuration created successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to create default Cricket configuration.");
                }
            }
            else
            {
                Console.WriteLine($"Found {existingConfigs.Count} existing sport configurations.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            Console.WriteLine("Please ensure MongoDB is running and accessible.");
        }
    }

    private static async Task LoadAndCompileConfigurationsAsync()
    {
        Console.WriteLine("Loading and compiling configurations...");
        
        try
        {
            // Get all sport configurations from database
            var sportConfigs = await _mongoService.GetAllSportConfigurationsAsync();
            
            if (!sportConfigs.Any())
            {
                Console.WriteLine("No sport configurations found in database.");
                return;
            }
            // Compile all configurations
            var compiledConfigs = _compilationService.CompileAllSportConfigurations(sportConfigs);
            
            // Measure the size of compiledConfigs object in MB
           // var compiledConfigsSizeMB = GetObjectSizeInMB(compiledConfigs);
           // Console.WriteLine($"Compiled configurations object size: {compiledConfigsSizeMB:F2} MB");
            
            // Cache compiled configurations
            foreach (var compiledConfig in compiledConfigs)
            {
                _cacheService.CacheConfiguration(compiledConfig.Sport, compiledConfig);
                Console.WriteLine($"Cached compiled configuration for {compiledConfig.Sport} with {compiledConfig.MarketConfigs.Count} market groups.");
            }

            Console.WriteLine($"Successfully compiled and cached {compiledConfigs.Count} sport configurations.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading and compiling configurations: {ex.Message}");
        }
    }

    private static async Task ProcessSampleDataAsync()
    {
        Console.WriteLine("Processing sample data...");
        
        try
        {
            // Get sample data
            List<MarketDto> marketDtos = SampleData.GetSamples();
            Console.WriteLine($"Processing {marketDtos.Count} sample markets...");

            var totalMarkets = 0;
            var totalOdds = 0;

            foreach (var marketDto in marketDtos)
            {
                Console.WriteLine($"Processing market: {marketDto.Name}");
                
                // Try to get cached configuration for Cricket (assuming all samples are Cricket)
                var cachedConfig = _cacheService.GetCachedConfiguration("Cricket");
                
                if (cachedConfig == null)
                {
                    Console.WriteLine("Cricket configuration not found in cache. Reloading from database...");
                    
                    // Reload from database and recompile
                    var dbConfig = await _mongoService.GetSportConfigurationBySportAsync("Cricket");
                    if (dbConfig != null)
                    {
                        cachedConfig = _compilationService.CompileSportConfiguration(dbConfig);
                        _cacheService.CacheConfiguration("Cricket", cachedConfig);
                        Console.WriteLine("Configuration reloaded and cached.");
                    }
                    else
                    {
                        Console.WriteLine("Cricket configuration not found in database. Skipping...");                                                  
                        continue;                                            
                    }
                }

                // Process market using compiled configuration
                var (markets, odds) = _marketProcessor.ProcessMarketWithCompiledConfig(marketDto, cachedConfig);
                
                totalMarkets += markets.Count;
                totalOdds += odds.Count;

                Console.WriteLine($"  -> Generated {markets.Count} markets and {odds.Count} odds");
                
                // Display some details about generated markets
                foreach (var market in markets.Take(3)) // Show first 3 markets
                {
                    Console.WriteLine($"    Market: {market.Name} (Order: {market.Order}, Tags: {string.Join(", ", market.Tags)})");
                }
                
                if (markets.Count > 3)
                {
                    Console.WriteLine($"    ... and {markets.Count - 3} more markets");
                }
            }

            Console.WriteLine($"\nProcessing Summary:");
            Console.WriteLine($"  Total Markets Processed: {marketDtos.Count}");
            Console.WriteLine($"  Total Markets Generated: {totalMarkets}");
            Console.WriteLine($"  Total Odds Generated: {totalOdds}");
            Console.WriteLine($"  Cache Status: {_cacheService.GetCacheSize()} configurations cached");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing sample data: {ex.Message}");
        }
    }

    // Method to demonstrate cache invalidation and reloading
    public static async Task ReloadConfigurationAsync(string sport)
    {
        Console.WriteLine($"Reloading configuration for {sport}...");
        
        try
        {
            // Invalidate cache for the sport
            _cacheService.InvalidateCache(sport);
            
            // Reload from database
            var dbConfig = await _mongoService.GetSportConfigurationBySportAsync(sport);
            if (dbConfig != null)
            {
                // Recompile and cache
                var compiledConfig = _compilationService.CompileSportConfiguration(dbConfig);
                _cacheService.CacheConfiguration(sport, compiledConfig);
                Console.WriteLine($"Configuration for {sport} reloaded and cached successfully.");
            }
            else
            {
                Console.WriteLine($"Configuration for {sport} not found in database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reloading configuration for {sport}: {ex.Message}");
        }
    }

    // Method to demonstrate cache management
    public static void ShowCacheStatus()
    {
        Console.WriteLine("\nCache Status:");
        Console.WriteLine($"  Total Cached Configurations: {_cacheService.GetCacheSize()}");
        Console.WriteLine($"  Cached Sports: {string.Join(", ", _cacheService.GetCachedSports())}");
        
        // Clear expired cache
        _cacheService.ClearExpiredCache();
        Console.WriteLine($"  After cleanup: {_cacheService.GetCacheSize()} configurations");
    }

    // Method to calculate object size in MB using memory profiling
    private static double GetObjectSizeInMB(object obj)
    {
        try
        {
            // Get the initial memory usage
            var initialMemory = GC.GetTotalMemory(false);
            
            // Create a reference to the object to prevent garbage collection
            var reference = obj;
            
            // Force garbage collection to get accurate measurement
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            // Get memory after GC
            var afterGCMemory = GC.GetTotalMemory(false);
            
            // Calculate the difference (this is approximate)
            var memoryDifference = Math.Abs(initialMemory - afterGCMemory);
            
            // Convert to MB
            var sizeInMB = memoryDifference / (1024.0 * 1024.0);
            
            return sizeInMB;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not measure object size: {ex.Message}");
            return 0.0;
        }
    }
}
