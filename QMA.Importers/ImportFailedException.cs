using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Importers
{
    public class ImportFailedException : Exception
    {
        public ImportFailedException(string message) : base(message)
        {
        }

        public ImportFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
