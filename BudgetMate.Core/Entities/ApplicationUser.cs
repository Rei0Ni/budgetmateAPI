using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace BudgetMate.Core.Entities;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public bool IsEnabled { get; set; } = true;
    public virtual DateTime? LastLogin { get; set; } = null;
    public virtual DateTime? CreationDate { get; set; } = DateTime.Now;
}
