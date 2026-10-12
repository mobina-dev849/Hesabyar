using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Domain.Entities.Transaction;

namespace Shared.Dtos
{
    public class TransactionDto
    {
        public long Id { get; set; }

        public long CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string CreatorPhoneNumber { get; set; }

        public long CounterpartyId { get; set; }
        public string CounterpartyName { get; set; }
        public string CounterpartyPhoneNumber { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Explainn { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? StatusDate { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
