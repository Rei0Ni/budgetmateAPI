using System;
using BudgetMate.Application.DTO.Stats;

namespace BudgetMate.Application.Interfaces.Stats;

public interface IStatsRepository
{
    Task<PeriodStatsDto> GetDailyStatsAsync(string UserId);
    Task<PeriodStatsDto> GetWeeklyStatsAsync(string UserId);
    Task<PeriodStatsDto> GetMonthlyStatsAsync(string UserId);
    Task<PeriodStatsDto> GetYearlyStatsAsync(string UserId);
}
