using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model
{
    public abstract class SoftDeletablePrimaryKeyBase : PrimaryKeyBase
    {
        public DateTimeOffset? Deleted { get; set; }
    }
}
