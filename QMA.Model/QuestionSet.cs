using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model
{
    public class QuestionSet : SoftDeletablePrimaryKeyBase
    {
        public string Name { get; set; }

        public string Notes { get; set; }
    }
}
