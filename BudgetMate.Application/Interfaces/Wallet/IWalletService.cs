using System;
using BudgetMate.Application.DTO.Wallet;

namespace BudgetMate.Application.Interfaces.Wallet;

public interface IWalletService
{
    dynamic GetWallet(string UserId);

}
