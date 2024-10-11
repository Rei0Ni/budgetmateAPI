using System;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace BudgetMate.Application.DTO.Transaction;

public class NewTransactionDto
{
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public TransactionCategory Category { get; set; }
    public string? Description { get; set; } = "";
    public DateTime? Date { get; set; }
    public IFormFile? Invoice { get; set; }
}
