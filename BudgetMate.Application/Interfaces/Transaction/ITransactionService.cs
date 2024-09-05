using System;
using BudgetMate.Application.DTO.Transaction;

namespace BudgetMate.Application.Interfaces.Transaction;

public interface ITransactionService
{
    TransactionDto? GetTransaction(string transactionId, string userId);
    List<TransactionDto> GetAllUserTransaction(string userId);
    Task<TransactionDto?> AddTransactionAsync(NewTransactionDto dto, string userId);
    TransactionDto? DeleteTransaction(string transactionId, string UserId);
}
