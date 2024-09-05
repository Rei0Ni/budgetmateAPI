using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetMate.Application.DTO.User;

public class LoginDto
{
    [Required]
    // [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
