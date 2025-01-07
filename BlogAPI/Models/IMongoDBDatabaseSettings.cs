namespace BlogAPI.Models;

public interface IMongoDBDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string PostCollectionName { get; set; }
    string UserCollectionName { get; set; }
}