using System;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Transaction;
using BudgetMate.Application.Interfaces.Wallet;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BudgetMate.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IWalletRepository _walletRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public TransactionService(ITransactionRepository repository, 
                              IWalletRepository walletRepository,
                              UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _walletRepository = walletRepository;
        _userManager = userManager;
    }
    public async Task<TransactionDto?> AddTransactionAsync(NewTransactionDto dto, string userId)
    {
        var User = await _userManager.FindByIdAsync(userId);
        if (User != null)
        {
            var transaction = new Transaction(){
                User = User,
                UserId = User.Id,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,
                Date = dto.Date
            };
            var newTransaction = _repository.AddTransaction(transaction);
            _walletRepository.ModifyWallet(userId, newTransaction);
            return newTransaction;
        }
        return null;
    }

    public TransactionDto? DeleteTransaction(string transactionId, string UserId)
    {
        var transaction = _repository.GetTransaction(transactionId, UserId);
        if (transaction != null)
        {
            return _repository.DeleteTransaction(transactionId);
        }
        return null;
    }

    public List<TransactionDto> GetAllUserTransaction(string userId)
    {
        return _repository.GetAllUserTransaction(userId);
    }

    public TransactionDto? GetTransaction(string transactionId, string userId)
    {
        return _repository.GetTransaction(transactionId, userId);
    }
}
