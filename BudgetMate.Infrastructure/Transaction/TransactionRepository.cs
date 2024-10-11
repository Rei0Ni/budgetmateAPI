using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Transaction;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IMapper _mapper;
    private readonly IHostEnvironment _env;

    public TransactionRepository(
        ApplicationDBContext context,
        IMapper mapper,
        IHostEnvironment environment)
    {
        _env = environment;
        _context = context;
        _mapper = mapper;
    }
    public Transaction AddTransaction(Transaction transaction)
    {
        _context.Transactions.InsertOne(transaction);
        return transaction;
    }

    public TransactionDto? DeleteTransaction(string transactionId)
    {
        var transaction = _context.Transactions.FindOneAndDelete(x => x.Id == new Guid(transactionId));
        return _mapper.Map<TransactionDto>(transaction);
    }

    public List<TransactionDto> GetAllUserTransaction(string userId)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(userId)).FirstOrDefault();
        var transactions = _context.Transactions.Find(x => x.UserId == User.Id).SortByDescending(x => x.Date).ToList();
        return _mapper.Map<List<TransactionDto>>(transactions);
    }

    public TransactionDto? GetTransaction(string transactionId, string userId)
    {
        var User = _context.Users.Find(x => x.Id == new Guid(userId)).FirstOrDefault();
        var transaction = _context.Transactions
            .Find(x => x.UserId == new Guid(userId) && x.Id == new Guid(transactionId))
            .FirstOrDefault();
        return _mapper.Map<TransactionDto>(transaction);
    }

    public TransactionDto GetTransactionByInvoiceId(string InvoiceId)
    {
        var transaction = _context.Transactions
            .Find(x => x.Invoice == $"api/Invoice/{InvoiceId}")
            .FirstOrDefault();
        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<Transaction> UpdateTransactionInvoiceAsync(string TransactionId, string InvoicePath)
    {
        // var transaction = _context.Transactions.Find(x => x.Id == new Guid(TransactionId)).FirstOrDefault();
        var filter = Builders<Transaction>.Filter.Eq(transaction => transaction.Id, new Guid(TransactionId));
        var update = Builders<Transaction>.Update
                    .Set(transaction => transaction.Invoice, InvoicePath);
        await _context.Transactions.UpdateOneAsync(filter, update);
        return _context.Transactions.Find(x => x.Id == new Guid(TransactionId)).FirstOrDefault();
    }

    public async Task<bool> UserHasAccessToTransactionAsync(string UserId, string transactionId)
    {
        return (await _context.Transactions.FindAsync(x => x.UserId == new Guid(UserId) && x.Id == new Guid(transactionId))).Any();
    }
}
