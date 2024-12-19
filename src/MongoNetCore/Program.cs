using MongoDB.Driver;
using MongoNetCore.DataAccess;
using MongoNetCore.Models;


ChoreDataAccess db = new ChoreDataAccess();

await db.CreateUser(new UserModel() {FirstName = "Ahmad", LastName = "Aghazadeh",});

var users = await db.GetAllUsers();

var chore = new ChoreModel() {AssignedTo = users.First(), ChoreText = "Now the lawn", FrequencyInDays = 7};

await db.CreateChore(chore);