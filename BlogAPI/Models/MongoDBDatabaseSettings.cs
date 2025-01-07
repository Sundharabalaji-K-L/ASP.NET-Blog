namespace BlogAPI.Models;

public class MongoDBDatabaseSettings: IMongoDBDatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string PostCollectionName { get; set; } = string.Empty;
    public string UserCollectionName { get; set; } = string.Empty;
}

