using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public long CreatorId { get; set; }
        public User Creator { get; set; }   // Navigation Property

        public long CounterpartyId { get; set; }
        public User Counterparty { get; set; }   // Navigation Property

        public string Title { get; set; } = string.Empty;
        public string? Explainn { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? StatusDate { get; set; }

        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }

        public enum TransactionType
        {
            [Display(Name ="پرداخت")]
            Payment = 1,
            [Display(Name ="دریافت")]
            Receipt = 2
        }

        public enum TransactionStatus
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}
