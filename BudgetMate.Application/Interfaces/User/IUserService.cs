using System;
using BudgetMate.Application.DTO.User;

namespace BudgetMate.Application.Interfaces.User;

public interface IUserService
{
    Task<UserProfileDto> GetUserProfileAsync(string UserId);
}
