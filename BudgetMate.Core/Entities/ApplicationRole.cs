using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace BudgetMate.Core.Entities;

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{
}
