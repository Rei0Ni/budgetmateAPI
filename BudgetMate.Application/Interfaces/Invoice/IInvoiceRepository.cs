using System;

namespace BudgetMate.Application.Interfaces.Invoice;

public interface IInvoiceRepository
{
    Core.Entities.Invoice GetByTransactionId(string TransactionId);
    Core.Entities.Invoice AddInvoice(Core.Entities.Invoice Invoice);
}
