using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model
{
    public class Quizzer : SoftDeletablePrimaryKeyBase
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ParentFullName { get; set; }

        public string ParentEmail { get; set; }

        public string Notes { get; set; }
    }
}
