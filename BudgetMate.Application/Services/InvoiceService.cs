using System;
using AutoMapper;
using BudgetMate.Application.DTO.Invoice;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.Interfaces.Invoice;
using BudgetMate.Application.Interfaces.Transaction;
using BudgetMate.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace BudgetMate.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _repository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IHostingEnvironment _env;
    private readonly string InvoicesPath = String.Empty;

    public InvoiceService(
        IInvoiceRepository repository,
        ITransactionRepository transactionRepository,
        IMapper mapper,
        IHostingEnvironment environment)
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _env = environment;
        InvoicesPath = $"{_env.ContentRootPath}/Invoices/";
    }
    public string? AddInvoice(IFormFile Invoice, Transaction transaction)
    {
        if (SaveInvoice(Invoice, transaction.Id.ToString()))
        {
            return $"api/Invoice/{transaction.Id}";
        }
        return null;
    }

    public void DeleteInvoice(string transactionId)
    {
        RemoveInvoice(transactionId);
    }

    public async Task<Byte[]?> GetInvoiceAsync(string UserId, string InvoiceId)
    {
        var transaction = _transactionRepository.GetTransactionByInvoiceId(InvoiceId);
        var hasAccess = await _transactionRepository.UserHasAccessToTransactionAsync(UserId, transaction.Id.ToString());
        if (!hasAccess)
        {
            return null;
        }

        var FileName = Directory.GetFiles(InvoicesPath, $"{InvoiceId}.*");

        if (FileName.Length > 0)
        {
            var invoicePath = InvoicesPath + FileName[0].Split('/').Last();
            var file = await File.ReadAllBytesAsync(invoicePath);
            return file;
        }
        return null;
    }


    private bool SaveInvoice(IFormFile Invoice, string TransactionId)
    {
        try
        {
            if (Invoice.Length > 0)
            {
                if (!Directory.Exists(InvoicesPath))
                {
                    Directory.CreateDirectory(InvoicesPath);
                }
                using (FileStream fileStream = File.Create(InvoicesPath + TransactionId + Path.GetExtension(Invoice.FileName)))
                {
                    Invoice.CopyTo(fileStream);
                    fileStream.Flush();
                }
            }
            return true;
        }
        catch (System.Exception ex)
        {
            Log.Error($"an error happened while saving invoice for transaction with id of \"{TransactionId}\"\n{ex}");
            return false;
        }

    }

    private bool RemoveInvoice(String transactionId)
    {
        try
        {
            var files = Directory.EnumerateFiles(InvoicesPath, transactionId + ".*");
            var filePath = files.FirstOrDefault();

            if (filePath != null && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return true;
        }
        catch (System.Exception ex)
        {
            Log.Error($"an error happened while Deleting invoice for transaction with id of \"{transactionId}\"\n{ex}");
            return false;
        }
    }
}
