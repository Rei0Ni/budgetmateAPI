using System;
using AutoMapper;
using BudgetMate.Application.DTO.Invoice;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Invoice;
using BudgetMate.Application.Interfaces.Transaction;
using BudgetMate.Application.Interfaces.Wallet;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace BudgetMate.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IWalletRepository _walletRepository;
    private readonly IInvoiceService _invoiceService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository repository,
                              IWalletRepository walletRepository,
                              IInvoiceService invoiceService,
                              UserManager<ApplicationUser> userManager,
                              IMapper mapper)
    {
        _repository = repository;
        _walletRepository = walletRepository;
        _invoiceService = invoiceService;
        _userManager = userManager;
        _mapper = mapper;
    }
    public async Task<TransactionDto?> AddTransactionAsync(NewTransactionDto dto, string userId)
    {
        var User = await _userManager.FindByIdAsync(userId);
        if (User != null)
        {
            var transaction = new Transaction()
            {
                User = User,
                UserId = User.Id,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Description = dto.Description,
                Date = dto.Date ?? DateTime.Now
            };
            var newTransaction = _repository.AddTransaction(transaction);
            if (newTransaction != null)
            {
                if (dto.Invoice != null)
                {
                    var InvoicePath = _invoiceService.AddInvoice(dto.Invoice, newTransaction);
                    
                    if (InvoicePath == null)
                    {
                        Log.Error("Couldn't save Invoice");
                    }

                    Log.Information(InvoicePath);
                    newTransaction = await _repository.UpdateTransactionInvoiceAsync(newTransaction.Id.ToString(), InvoicePath);
                }

                // {
                _walletRepository.ModifyWallet(userId, _mapper.Map<TransactionDto>(newTransaction));
                var returnedTransaction = _mapper.Map<TransactionDto>(newTransaction);
                return returnedTransaction;
                // }
            }
        }
        return null;
    }

    public TransactionDto? DeleteTransaction(string transactionId, string UserId)
    {
        var transaction = _repository.GetTransaction(transactionId, UserId);
        if (transaction != null)
        {
            _invoiceService.DeleteInvoice(transactionId);
            _walletRepository.RemoveTransactionFromWallet(UserId, transaction.Amount, transaction.Type);
            return _repository.DeleteTransaction(transactionId);
        }
        return null;
    }

    public AllTransactionsDto GetAllUserTransaction(string userId)
    {
        var AllTransactionsDto = new AllTransactionsDto();
        AllTransactionsDto.Transactions = _repository.GetAllUserTransaction(userId);
        return AllTransactionsDto;
    }

    public TransactionDto? GetTransaction(string transactionId, string userId)
    {
        var transaction = _repository.GetTransaction(transactionId, userId);
        return transaction;
    }
}
