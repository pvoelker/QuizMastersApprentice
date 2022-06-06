using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
