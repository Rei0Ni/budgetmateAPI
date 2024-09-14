using System;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.DTO.Wallet;
using BudgetMate.Application.Interfaces.Dashboard;
using BudgetMate.Application.Interfaces.Wallet;

namespace BudgetMate.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashTransactionRepository _dashTransactionRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public DashboardService(
        IDashTransactionRepository dashTransactionRepository, 
        IWalletRepository walletRepository,
        IMapper mapper)
    {
        _dashTransactionRepository = dashTransactionRepository;
        _walletRepository = walletRepository;
        _mapper = mapper;
    }
    public List<TransactionDto> GetLatestTransactions(string UserId)
    {
        var latestTransactions = _dashTransactionRepository.GetLatestTransactions(UserId);
        return _mapper.Map<List<TransactionDto>>(latestTransactions);
    }

    public WalletDto GetWallet(string UserId)
    {
        var wallet = _walletRepository.GetWallet(UserId);
        return _mapper.Map<WalletDto>(wallet);
    }
}
