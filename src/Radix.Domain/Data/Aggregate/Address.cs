﻿using Radix.Data;
using Radix.Domain.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Domain.Data.Aggregate
{
    public record Address
    {
        public required Id Id { get; init; }
        public TenantId TenantId { get; set; } = TenantId.Default;
    }

    
}
