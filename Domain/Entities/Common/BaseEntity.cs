using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common
{
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
    public abstract class BaseEntity : BaseEntity<long>
    {

    }
}
