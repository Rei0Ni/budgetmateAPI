using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Core.Entities;

namespace BudgetMate.Application.Interfaces.Wallet;

public interface IWalletRepository
{
    void AddWallet(string UserId);
    void RemoveWallet(string UserId);
    void ModifyWallet(string UserId, TransactionDto? transaction);
    void RemoveTransactionFromWallet(string UserId, decimal amount, TransactionType type);
    Core.Entities.Wallet GetWallet(string UserId);
    List<Core.Entities.Transaction> GetTodayTransactions(string UserId);
}
