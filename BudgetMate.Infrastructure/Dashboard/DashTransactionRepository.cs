using System;
using BudgetMate.Application.Interfaces.Dashboard;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure.Dashboard;

public class DashTransactionRepository : IDashTransactionRepository
{
    private readonly ApplicationDBContext _context;

    public DashTransactionRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public List<Transaction> GetLatestTransactions(string UserId)
    {
        var latestTransactions = _context.Transactions.Find(x => x.UserId == new Guid(UserId))
                .SortByDescending(t => t.Date).Limit(5).ToList();
        return latestTransactions;
    }
}
