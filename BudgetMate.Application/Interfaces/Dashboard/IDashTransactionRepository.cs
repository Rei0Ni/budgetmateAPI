using System;

namespace BudgetMate.Application.Interfaces.Dashboard;

public interface IDashTransactionRepository
{
    List<Core.Entities.Transaction> GetLatestTransactions(string UserId);
}
