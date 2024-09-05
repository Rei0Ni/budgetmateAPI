using System;

namespace BudgetMate.API.Models;

public class MongoDBConfig
{
    public string ConnectionURI { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
