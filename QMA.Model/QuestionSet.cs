using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model
{
    public class QuestionSet : PrimaryKeyBase
    {
        public string Name { get; set; }

        public string Notes { get; set; }
    }
}
