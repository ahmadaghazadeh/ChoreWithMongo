using MongoDB.Driver;
using MongoNetCore.Models;

namespace MongoNetCore.DataAccess;

public class ChoreDataAccess
{
    private const string ConnectionString = "mongodb://root:example@127.0.0.1:27017";
    private const string DatabaseName = "choredb";
    private const string ChoreCollection = "chore_chart";
    private const string UserCollection = "users";
    private const string ChoreHistoryCollection = "chore_history";

    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection);
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        var userCollection = ConnectToMongo<UserModel>(UserCollection);
        var results = await userCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChores()
    {
        var choreCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choreCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChoresForAUser(UserModel userModel)
    {
        var choreCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choreCollection.FindAsync(c => c.AssignedTo.Id == userModel.Id);
        return results.ToList();
    }

    public Task CreateUser(UserModel userModel)
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        return usersCollection.InsertOneAsync(userModel);
    }

    public Task CreateChore(ChoreModel choreModel)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        return choresCollection.InsertOneAsync(choreModel);
    }

    public Task UpdateChore(ChoreModel choreModel)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var filer = Builders<ChoreModel>.Filter.Eq("Id", choreModel.Id);
        return choresCollection.ReplaceOneAsync(filer, choreModel, new ReplaceOptions() {IsUpsert = true});
    }

    public Task DeleteChore(ChoreModel choreModel)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        return choresCollection.DeleteOneAsync(c => c.Id == choreModel.Id);
    }

    public async Task CompleteChore(ChoreModel choreModel)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var filter = Builders<ChoreModel>.Filter.Eq("Id", choreModel.Id);
        await choresCollection.ReplaceOneAsync(filter, choreModel);

        var choreHistoryCollection = ConnectToMongo<ChoreHistoryModel>(ChoreHistoryCollection);
        await choreHistoryCollection.InsertOneAsync(new ChoreHistoryModel(choreModel));
    }
}