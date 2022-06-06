using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Importers
{
    public class ImportMissingHeadersException : Exception
    {
        public ImportMissingHeadersException(string message) : base(message)
        {
        }

        public ImportMissingHeadersException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
