using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace BudgetMate.Core.Entities;

public enum Role
{
    Administrator = 0,
    App_User = 1
}

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public bool IsEnabled { get; set; } = true;
    public virtual DateTime? LastLogin { get; set; } = null;
    public virtual DateTime? CreationDate { get; set; } = DateTime.Now;
}
