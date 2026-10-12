using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos
{
    public class UpdateTransacitonDto
    {
       // public long Id { get; set; }
        public string? Title { get; set; }
        public string? Explainn { get; set; }
        public decimal Amount { get; set; }
        //public DateTime? StatusDate { get; set; }
    }
}
