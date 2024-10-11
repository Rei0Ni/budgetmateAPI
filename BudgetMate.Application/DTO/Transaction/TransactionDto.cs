using System;
using BudgetMate.Core.Entities;

namespace BudgetMate.Application.DTO.Transaction;

public class TransactionDto
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public TransactionCategory Category { get; set; }
    public string? Description { get; set; } = "";
    public DateTime Date { get; set; }
    public string Invoice { get; set; } = "";
}
