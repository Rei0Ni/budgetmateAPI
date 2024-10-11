using System;

namespace BudgetMate.Application.DTO.Stats;

public class CategorySpendingDto
{
    public string Category { get; set; }
    public decimal TotalSpent { get; set; }
}
