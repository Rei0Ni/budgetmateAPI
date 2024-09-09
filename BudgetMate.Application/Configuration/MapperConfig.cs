using System;
using System.Transactions;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;
using BudgetMate.Application.DTO.Wallet;
using BudgetMate.Core.Entities;

namespace BudgetMate.Application.Configuration;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Core.Entities.Transaction, 
            TransactionDto>().ReverseMap();

        CreateMap<Wallet,
            WalletDto>().ReverseMap();
    }
}
