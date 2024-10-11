using System;
using AutoMapper;
using BudgetMate.Application.DTO.User;
using BudgetMate.Application.Interfaces.User;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BudgetMate.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<UserProfileDto> GetUserProfileAsync(string UserId)
    {
        var User = await _userManager.FindByIdAsync(UserId);
        return _mapper.Map<UserProfileDto>(User);
    }
}
