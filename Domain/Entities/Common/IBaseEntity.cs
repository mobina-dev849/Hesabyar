using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common
{
    public interface IBaseEntity<TKay>
    {
        public TKay Id { get; set; }
    }
}
