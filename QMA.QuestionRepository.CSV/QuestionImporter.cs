using CsvHelper;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace QMA.Importers.Csv
{
    /// <summary>
    /// Question importer for Comma-Separated Value (CSV) text
    /// </summary>
    public class QuestionImporter : IQuestionImporter
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public QuestionImporter()
        {
        }

        /// <inheritdoc />
        /// <exception cref="ImportFailedException">Import failed, see inner exception</exception>
        /// <exception cref="ImportMissingHeadersException">Import failed due to missing or bad headers, see inner exception</exception>
        public IEnumerable<ImportQuestion> Import(StreamReader reader)
        {
            try
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ImportQuestion>();

                    return records.ToList();
                }
            }
            catch (HeaderValidationException ex)
            {
                throw new ImportMissingHeadersException($"Import failed due to missing or bad headers. Expected headers: {nameof(ImportQuestion.Number)}, {nameof(ImportQuestion.Text)}, {nameof(ImportQuestion.Answer)}, {nameof(ImportQuestion.Points)}", ex);
            }
            catch (Exception ex)
            {
                throw new ImportFailedException("Import failed", ex);
            }
        }
    }
}
