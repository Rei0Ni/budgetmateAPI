using System;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Core.Entities;
using MongoDB.Driver;

namespace BudgetMate.Application.Interfaces.Transaction;

public interface ITransactionRepository
{
    Task<bool> UserHasAccessToTransactionAsync(string UserId, string transactionId);
    TransactionDto? GetTransaction(string transactionId, string userId);
    TransactionDto GetTransactionByInvoiceId(string InvoiceId);
    List<TransactionDto> GetAllUserTransaction(string userId);
    Task<Core.Entities.Transaction> UpdateTransactionInvoiceAsync(string TransactionId, string InvoicePath);
    Core.Entities.Transaction AddTransaction(Core.Entities.Transaction dto);
    TransactionDto? DeleteTransaction(string transactionId);
}
