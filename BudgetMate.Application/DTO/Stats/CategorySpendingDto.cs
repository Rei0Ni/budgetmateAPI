using System;
using BudgetMate.Core.Entities;

namespace BudgetMate.Application.DTO.Stats;

public class CategorySpendingDto
{
    public TransactionCategory Category { get; set; }
    public decimal TotalSpent { get; set; }
}
