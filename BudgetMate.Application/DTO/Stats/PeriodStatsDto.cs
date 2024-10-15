using System;

namespace BudgetMate.Application.DTO.Stats;

public class PeriodStatsDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public List<CategorySpendingDto> CategoryWiseSpending { get; set; }
    public List<CategorySpendingDto> CategoryWiseIncome { get; set; }
}
