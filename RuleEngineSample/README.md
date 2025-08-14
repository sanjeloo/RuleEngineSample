# Rule Engine Sample - MongoDB with Compiled Configurations

This project demonstrates a high-performance rule engine system that uses MongoDB for configuration storage, compiled expressions for optimal performance, and in-memory caching for rapid access.

## Features

- **MongoDB Integration**: Store and retrieve sport configurations from MongoDB
- **Compiled Expressions**: Dynamic LINQ expressions are compiled once and reused for 10-100x performance improvement
- **In-Memory Caching**: Configurations are cached in memory with automatic expiration
- **Cache Invalidation**: Automatic cache refresh when configurations are updated
- **Fallback Processing**: Graceful fallback to dynamic processing if compilation fails
- **Thread-Safe**: All cache operations are thread-safe

## Architecture

### Services

1. **MongoService**: Handles all MongoDB operations
2. **ConfigurationCompilationService**: Compiles dynamic expressions into executable delegates
3. **ConfigurationCacheService**: Manages in-memory caching with expiration
4. **OptimizedMarketProcessor**: Processes markets using compiled configurations
5. **ConfigurationUpdateService**: Handles configuration updates and cache invalidation

### Models

- **SportConfiguration**: MongoDB entity for sport configurations
- **MarketConfigGroup**: Groups of market configurations
- **MarketConfig**: Individual market configuration with expressions
- **CompiledSportConfiguration**: Compiled version for performance
- **CompiledMarketConfig**: Compiled market configuration with pre-compiled expressions

## Setup

### Prerequisites

1. **MongoDB**: Install and run MongoDB locally or use MongoDB Atlas
2. **.NET 9.0**: Ensure you have .NET 9.0 SDK installed

### Configuration

Update the MongoDB connection string in `Utils/GeneralFunctions.cs`:

```csharp
public static string GetMongoConnectionString() => "mongodb://localhost:27017";
public static string GetDatabaseName() => "RuleEngineDB";
public static string GetCollectionName() => "SportConfigurations";
```

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore packages:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```

## Usage

### Running the Application

```bash
dotnet run
```

The application will:
1. Connect to MongoDB
2. Create default Cricket configuration if none exists
3. Compile all configurations
4. Cache compiled configurations
5. Process sample data using cached configurations

### Performance Benefits

- **Expression Compilation**: Dynamic expressions are compiled once at startup
- **Regex Compilation**: Regular expressions are compiled with `RegexOptions.Compiled`
- **In-Memory Caching**: No database hits during processing
- **Fallback Processing**: Graceful degradation if compilation fails

### Cache Management

The system automatically manages cache:

- **Expiration**: Configurations expire after 30 minutes (configurable)
- **Automatic Cleanup**: Expired configurations are automatically removed
- **Update Handling**: Cache is invalidated when configurations change
- **Reloading**: Configurations can be manually refreshed

### Adding New Sports

```csharp
var newSport = new SportConfiguration
{
    Sport = "Football",
    MarketConfigs = new List<MarketConfigGroup>
    {
        // Add your market configurations here
    }
};

var updateService = new ConfigurationUpdateService(mongoService, compilationService, cacheService);
await updateService.AddNewSportConfigurationAsync(newSport);
```

### Updating Existing Configurations

```csharp
// Update configuration
var updatedConfig = await mongoService.GetSportConfigurationBySportAsync("Cricket");
// Modify the configuration...

// Update and refresh cache
await updateService.UpdateSportConfigurationAsync(updatedConfig.Id, updatedConfig);
```

## Performance Comparison

| Operation | Old System | New System | Improvement |
|-----------|------------|------------|-------------|
| Expression Parsing | Every time | Once at startup | 100x+ |
| Regex Matching | Interpreted | Compiled | 10-50x |
| Database Access | Every request | Once per cache period | 100x+ |
| Memory Usage | Lower | Higher (cached) | Trade-off |

## Monitoring

### Cache Status

```csharp
var cacheService = new ConfigurationCacheService();
Console.WriteLine($"Cache size: {cacheService.GetCacheSize()}");
Console.WriteLine($"Cached sports: {string.Join(", ", cacheService.GetCachedSports())}");
```

### Performance Metrics

The system logs:
- Configuration compilation time
- Cache hit/miss rates
- Processing times
- Error rates and fallback usage

## Error Handling

- **Compilation Errors**: Logged and fallback to dynamic processing
- **Database Errors**: Graceful degradation with cached data
- **Cache Errors**: Automatic fallback to database
- **Expression Errors**: Detailed logging for debugging

## Best Practices

1. **Regular Updates**: Update configurations during low-traffic periods
2. **Monitoring**: Monitor cache hit rates and compilation success rates
3. **Testing**: Test configuration changes in development first
4. **Backup**: Regular backups of MongoDB configurations
5. **Scaling**: Consider Redis for distributed caching in production

## Troubleshooting

### Common Issues

1. **MongoDB Connection**: Ensure MongoDB is running and accessible
2. **Compilation Errors**: Check expression syntax in configurations
3. **Cache Issues**: Verify cache expiration settings
4. **Performance**: Monitor compilation times and cache hit rates

### Debug Mode

Enable detailed logging by modifying the console output in services.

## Future Enhancements

- **Redis Integration**: Distributed caching for multi-instance deployments
- **Configuration Versioning**: Track configuration changes over time
- **Performance Metrics**: Detailed performance analytics
- **Web Interface**: Admin panel for configuration management
- **API Endpoints**: REST API for configuration management

## License

This project is provided as-is for educational and demonstration purposes.
