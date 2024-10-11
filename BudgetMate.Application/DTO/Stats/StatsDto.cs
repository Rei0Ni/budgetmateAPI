using System;

namespace BudgetMate.Application.DTO.Stats;

public class StatsDto
{
    public PeriodStatsDto Daily { get; set; }
    public PeriodStatsDto Weekly { get; set; }
    public PeriodStatsDto Monthly { get; set; }
    public PeriodStatsDto Yearly { get; set; }
}
