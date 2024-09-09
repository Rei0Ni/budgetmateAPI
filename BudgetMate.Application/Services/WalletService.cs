using System;
using AutoMapper;
using BudgetMate.Application.DTO.Wallet;
using BudgetMate.Application.Interfaces.Wallet;

namespace BudgetMate.Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository walletRepository, IMapper mapper)
    {
        _walletRepository = walletRepository;
        _mapper = mapper;
    }
    public WalletDto GetWallet(string UserId)
    {
        return _mapper.Map<WalletDto>(_walletRepository.GetWallet(UserId));
    }
}
