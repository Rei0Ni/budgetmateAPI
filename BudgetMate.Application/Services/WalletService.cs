using System;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.DTO.Wallet;
using BudgetMate.Application.Interfaces.Wallet;

namespace BudgetMate.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public WalletService(
        IWalletRepository walletRepository,
        IMapper mapper
    )
    {
        _walletRepository = walletRepository;
        _mapper = mapper;
    }
    public dynamic GetWallet(string UserId)
    {
        var wallet = _mapper.Map<WalletDto>(_walletRepository.GetWallet(UserId));
        var Transactions = _mapper.Map<List<TransactionDto>>(_walletRepository.GetTodayTransactions(UserId));
        return new { Wallet = wallet, Transactions = Transactions };
    }
}
