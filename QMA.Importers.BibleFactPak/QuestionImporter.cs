 using QMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QMA.Importers.BibleFactPak
{
    public class QuestionImporter : IQuestionImporter
    {
        private string _text;

        public QuestionImporter(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            _text = text;
        }

        /// <exception cref="ImportFailedException">Import failed, see inner exception</exception>

        public IEnumerable<ImportQuestion> Import()
        {
            try
            {
                if(String.IsNullOrWhiteSpace(_text))
                {
                    return Enumerable.Empty<ImportQuestion>();
                }

                var questions = _text.Split(new string[] { "Question: " }, StringSplitOptions.RemoveEmptyEntries);

                var retVal = new List<ImportQuestion>();

                var pointsRegEx = new Regex(@"^Question #\d+ for (\d+) points", RegexOptions.Compiled);

                var questionNumberRegEx = new Regex(@" \(#(\d+)\)", RegexOptions.Compiled);

                foreach (var question in questions)
                {
                    var newQuestion = new ImportQuestion();

                    var match = pointsRegEx.Match(question);
                    var pointsText = match.Groups[1].Value;

                    newQuestion.Points = int.Parse(pointsText);

                    var question2 = question.Substring(match.Groups[0].Value.Length);

                    match = questionNumberRegEx.Match(question2);
                    var questionNumberText = match.Groups[1].Value;

                    newQuestion.Number = int.Parse(questionNumberText);

                    question2 = question2.Replace(match.Groups[0].Value, "|");

                    var questionAnswer = question2.Split('|');

                    newQuestion.Text = questionAnswer[0].Trim();
                    newQuestion.Answer = questionAnswer[1].Trim();

                    // Clear out Fire Bible page number
                    newQuestion.Answer = Regex.Replace(newQuestion.Answer, @"Fire Bible Page Number: (\d+)", "");

                    retVal.Add(newQuestion);
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw new ImportFailedException("Import failed", ex);
            }
        }
    }
}
