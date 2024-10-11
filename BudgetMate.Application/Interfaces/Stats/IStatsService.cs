using System;
using BudgetMate.Application.DTO.Stats;

namespace BudgetMate.Application.Interfaces.Stats;

public interface IStatsService
{
    Task<StatsDto> GetStatsAsync(string UserId);
}
