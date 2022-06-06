using QMA.Model;
using System;
using System.Collections.Generic;

namespace QMA.Importers
{
    public interface IQuestionImporter
    {
        IEnumerable<ImportQuestion> Import();
    }
}
