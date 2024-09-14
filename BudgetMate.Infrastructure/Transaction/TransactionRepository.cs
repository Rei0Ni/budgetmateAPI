using System;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Transaction;
using BudgetMate.Core.Contexts;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Infrastructure;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IMapper _mapper;

    public TransactionRepository(ApplicationDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public TransactionDto AddTransaction(Transaction transaction)
    {
        _context.Transactions.InsertOne(transaction);
        return _mapper.Map<TransactionDto>(transaction);
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
}
