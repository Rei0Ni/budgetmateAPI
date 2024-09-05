using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetMate.Application.DTO.User;

public class NewUserDto
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]

    public string? Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public required string ConfirmPassword { get; set; }
}
