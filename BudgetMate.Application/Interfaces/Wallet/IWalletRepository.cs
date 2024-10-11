using BudgetMate.Application.DTO.Transaction;

namespace BudgetMate.Application.Interfaces.Wallet;

public interface IWalletRepository
{
    void AddWallet(string UserId);
    void RemoveWallet(string UserId);
    void ModifyWallet(string UserId, TransactionDto? transaction);
    Core.Entities.Wallet GetWallet(string UserId);
    List<Core.Entities.Transaction> GetTodayTransactions(string UserId);
}
