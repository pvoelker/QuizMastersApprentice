using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Importers
{
    public class ImportNonmatchingDuplicatesException : Exception
    {
        public ImportNonmatchingDuplicatesException(string message) : base(message)
        {
        }

        public ImportNonmatchingDuplicatesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
