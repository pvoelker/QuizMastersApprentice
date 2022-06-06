using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {
        }
    }
}
