using System;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.DTO.Wallet;

namespace BudgetMate.Application.Interfaces.Dashboard;

public interface IDashboardService
{
    List<TransactionDto> GetLatestTransactions(string UserId);
    WalletDto GetWallet(string UserId);
}
