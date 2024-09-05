using System;
using System.Transactions;
using AutoMapper;
using BudgetMate.Application.DTO.Transaction;

namespace BudgetMate.Application.Configuration;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Transaction, 
            TransactionDto>().ReverseMap();
    }
}
