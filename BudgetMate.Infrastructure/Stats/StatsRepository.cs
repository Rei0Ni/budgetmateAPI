using System;
using BudgetMate.Application.DTO.Stats;
using BudgetMate.Application.Interfaces.Stats;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure.Stats;

public class StatsRepository : IStatsRepository
{
    private readonly ApplicationDBContext _context;

    public StatsRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<PeriodStatsDto> GetDailyStatsAsync(string UserId)
    {
        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;

        var dailyStats = await CalculateStats(UserId, startDate, endDate);
        return dailyStats;
    }

    public async Task<PeriodStatsDto> GetMonthlyStatsAsync(string UserId)
    {
        var today = DateTime.UtcNow;
        var startDate = new DateTime(today.Year, today.Month, 1);
        var endDate = today.Date.AddDays(1);

        var monthlyStats = await CalculateStats(UserId, startDate, endDate);
        return monthlyStats;
    }

    public async Task<PeriodStatsDto> GetWeeklyStatsAsync(string UserId)
    {
        var today = DateTime.UtcNow;
        var startDate = today.AddDays(-6);
        var endDate = today.Date.AddDays(1);

        var weeklyStats = await CalculateStats(UserId, startDate, endDate);
        return weeklyStats;
    }

    public async Task<PeriodStatsDto> GetYearlyStatsAsync(string UserId)
    {
        var today = DateTime.UtcNow;
        var startDate = new DateTime(today.Year, 1, 1);
        var endDate = today.Date.AddDays(1);

        var yearlyStats = await CalculateStats(UserId, startDate, endDate);
        return yearlyStats;
    }

    private async Task<PeriodStatsDto> CalculateStats(string UserId, DateTime startDate, DateTime endDate)
    {
        var totalIncome = _context.Transactions
            .Find(t => t.UserId == new Guid(UserId) && t.Type == TransactionType.INCOME && t.Date >= startDate && t.Date < endDate)
            .ToList().Sum(t => t.Amount);

        var totalExpense = _context.Transactions
            .Find(t => t.UserId == new Guid(UserId) && t.Type == TransactionType.EXPENSE && t.Date >= startDate && t.Date < endDate)
            .ToList().Sum(t => t.Amount);

        var categoryWiseSpending = _context.Transactions
            .Find(t => t.UserId == new Guid(UserId) && t.Type == TransactionType.EXPENSE && t.Date >= startDate && t.Date < endDate)
            .ToList()
            .GroupBy(t => t.Category)
            .Select(c => new CategorySpendingDto
            {
                Category = c.Key.ToString(),
                TotalSpent = c.Sum(t => t.Amount)
            })
            .ToList();

        return new PeriodStatsDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpense,
            CategoryWiseSpending = categoryWiseSpending
        };
    }
}
