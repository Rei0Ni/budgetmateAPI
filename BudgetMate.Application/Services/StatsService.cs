using System;
using BudgetMate.Application.DTO.Stats;
using BudgetMate.Application.Interfaces.Stats;

namespace BudgetMate.Application.Services;

public class StatsService : IStatsService
{
    private readonly IStatsRepository _repository;

    public StatsService(IStatsRepository repository)
    {
        _repository = repository;
    }
    public async Task<StatsDto> GetStatsAsync(string UserId)
    {
        var dailyStats = await _repository.GetDailyStatsAsync(UserId);
        var weeklyStats = await _repository.GetWeeklyStatsAsync(UserId);
        var monthlyStats = await _repository.GetMonthlyStatsAsync(UserId);
        var yearlyStats = await _repository.GetYearlyStatsAsync(UserId);

        return new StatsDto
        {
            Daily = dailyStats,
            Weekly = weeklyStats,
            Monthly = monthlyStats,
            Yearly = yearlyStats
        };
    }
}
