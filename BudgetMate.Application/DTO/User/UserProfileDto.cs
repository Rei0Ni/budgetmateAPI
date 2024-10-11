using System;

namespace BudgetMate.Application.DTO.User;

public class UserProfileDto
{
    public string UserName { get; set;}
    public string Email { get; set;}
    public DateTime CreationDate { get; set; }
}
