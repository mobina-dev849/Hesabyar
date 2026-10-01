using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Domain.Entities.Transaction;

namespace Shared.Dtos
{
    public class CreateTransactionDto
    {
        public string CounterpartyPhoneNumber { get; set; }
        public string? Explainn { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType Type { get; set; }
    }
}
