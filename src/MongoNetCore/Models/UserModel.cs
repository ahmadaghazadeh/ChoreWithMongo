﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoNetCore.Models;

public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}