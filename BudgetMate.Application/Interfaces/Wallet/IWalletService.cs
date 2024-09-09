using System;
using BudgetMate.Application.DTO.Wallet;

namespace BudgetMate.Application.Interfaces.Wallet;

public interface IWalletService
{
    WalletDto GetWallet(string UserId);

}
