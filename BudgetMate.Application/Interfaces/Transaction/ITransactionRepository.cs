using System;
using BudgetMate.Application.DTO.Transaction;

namespace BudgetMate.Application.Interfaces.Transaction;

public interface ITransactionRepository
{
    TransactionDto? GetTransaction(string transactionId, string userId);
    List<TransactionDto> GetAllUserTransaction(string userId);
    TransactionDto AddTransaction(Core.Entities.Transaction dto);
    TransactionDto? DeleteTransaction(string transactionId);
}
