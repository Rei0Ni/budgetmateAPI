using System;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Core.Contexts;

public class ApplicationDBContext
{
        private readonly IMongoDatabase _database;

        public ApplicationDBContext(string connectionString, string databaseName)
        {
                var client = new MongoClient(connectionString);
                _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<ApplicationUser> Users =>
                _database.GetCollection<ApplicationUser>("Users");
        public IMongoCollection<ApplicationRole> Roles =>
                _database.GetCollection<ApplicationRole>("Roles");

        public IMongoCollection<Transaction> Transactions =>
                _database.GetCollection<Transaction>("Transactions");

        public IMongoCollection<Wallet> Wallets =>
                _database.GetCollection<Wallet>("Wallets");
}
