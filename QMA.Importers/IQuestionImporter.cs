using QMA.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace QMA.Importers
{
    /// <summary>
    /// Interface for a question importer
    /// </summary>
    public interface IQuestionImporter
    {
        /// <summary>
        /// Imports questions from a provided stream reader
        /// </summary>
        /// <param name="reader">The stream to import the questions from</param>
        /// <returns>The imported questions</returns>
        IEnumerable<ImportQuestion> Import(StreamReader reader);
    }
}
