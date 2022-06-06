using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model
{
    public abstract class PrimaryKeyBase
    {
        public string PrimaryKey { get; set; }

        public DateTimeOffset? Deleted { get; set; }
    }
}
