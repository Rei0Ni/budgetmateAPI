using System;
using System.Security.Cryptography.X509Certificates;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Wallet;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure.Wallet;

public class WalletRepository : IWalletRepository
{
    private readonly ApplicationDBContext _context;

    public WalletRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public void AddWallet(string UserId)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(UserId)).FirstOrDefault();
        if (User == null)
        {

        }
        _context.Wallets.InsertOne(new Core.Entities.Wallet() { User = User!, UserId = User!.Id });
    }

    public Core.Entities.Wallet GetWallet(string UserId)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(UserId)).FirstOrDefault();
        if (User == null)
        {

        }
        var wallet = _context.Wallets.Find(x => x.UserId == User!.Id).FirstOrDefault();
        return wallet;
    }

    public List<Transaction> GetTodayTransactions(string UserId){
        var User = _context.Users.Find(x => x.Id == new Guid(UserId)).FirstOrDefault();

        var startDate = DateTime.UtcNow.AddDays(-1);
        var endDate = DateTime.UtcNow;

        var Transactions = _context.Transactions
            .Find(t => t.UserId == new Guid(UserId) && t.Date >= startDate && t.Date < endDate)
            .ToList();

        return Transactions;
    }

    public void ModifyWallet(string UserId, TransactionDto? transaction)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(UserId)).FirstOrDefault();
        if (User == null)
        {

        }
        var wallet = _context.Wallets.Find(x => x.UserId == User!.Id).FirstOrDefault();
        var filter = Builders<Core.Entities.Wallet>.Filter.Eq(wallet => wallet.UserId, User!.Id);
        if (transaction!.Type == TransactionType.INCOME)
        {
            var update = Builders<Core.Entities.Wallet>.Update
                    .Set(wallet => wallet.Balance, wallet.Balance + transaction.Amount)
                    .Set(wallet => wallet.Income, wallet.Income + transaction.Amount);
            _context.Wallets.UpdateOneAsync(filter, update);
        }
        else if (transaction!.Type == TransactionType.EXPENSE)
        {
            var update = Builders<Core.Entities.Wallet>.Update
                    .Set(wallet => wallet.Balance, wallet.Balance - transaction.Amount)
                    .Set(wallet => wallet.Expense, wallet.Expense + transaction.Amount);
            _context.Wallets.UpdateOneAsync(filter, update);
        }
    }

    public void RemoveWallet(string UserId)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(UserId)).FirstOrDefault();
        if (User == null)
        {

        }
        _context.Wallets.DeleteOne(x => x.UserId == User!.Id);
    }
}
