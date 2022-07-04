using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using QMA.ViewModel.Observables.Practice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMA.ViewModel.Provider
{
    public class PracticeReportEmail : IDisposable
    {
        private SmtpClient _client = null;

        public PracticeReportEmail()
        {
        }

        public async Task ConnectAsync(string host, int port, string username, string password)
        {
            _client = new SmtpClient();

            await _client.ConnectAsync(host, port, SecureSocketOptions.Auto);

            await _client.AuthenticateAsync(username, password);
        }

        public async Task SendMessageAsync(MimeMessage message)
        {
            if(_client == null)
            {
                throw new NullReferenceException("Connect must be called first");
            }

            await _client.SendAsync(message);
        }

        public async Task SendPracticeReportAsync(DateTimeOffset now,
            string seasonName,
            string fromName, string fromEmail,
            string parentName, string parentEmail,
            ObservablePracticeQuizzer quizzer,
            int totalQuestionsAsked,
            IEnumerable<ObservablePracticeQuestion> justLearned,
            IEnumerable<ObservablePracticeQuestion> noAnswers)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(parentName, parentEmail));
            message.Subject = $"{seasonName} - Quizzing Practice at {now.ToString("g")}";
            message.Body = new TextPart("html")
            {
                Text = BuildMessageBody(quizzer, totalQuestionsAsked, justLearned, noAnswers)
            };

            await SendMessageAsync(message);
        }

        private string BuildMessageBody(ObservablePracticeQuizzer quizzer,
            int totalQuestionsAsked,
            IEnumerable<ObservablePracticeQuestion> justLearned,
            IEnumerable<ObservablePracticeQuestion> noAnswers)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"<p>{quizzer.FirstName} has completed a practice.</p>");
            builder.AppendLine($"<p>A total of {totalQuestionsAsked} questions were asked.</p>");
            builder.AppendLine();
            var unqiueJustLearned = justLearned.Distinct();
            if (unqiueJustLearned.Any())
            {
                builder.AppendLine("<p><b>Questions just learned about:</b></p>");
                builder.AppendLine("<ul>");
                foreach (var item in unqiueJustLearned)
                {
                    builder.AppendLine($"<li>#{item.Number} --- {item.Text}</li>");
                }
                builder.AppendLine("</ul>");
            }
            else
            {
                builder.AppendLine("<p><i>No new questions were learned</i></p>");
            }
            builder.AppendLine();
            var unqiueNoAnswers = noAnswers.Distinct();
            if (unqiueNoAnswers.Any())
            {
                builder.AppendLine("<p><b>Questions not answered:</b></p>");
                builder.AppendLine("<ul>");
                foreach (var item in unqiueNoAnswers)
                {
                    builder.AppendLine($"<li>#{item.Number} --- {item.Text}</li>");
                }
                builder.AppendLine("</ul>");
            }
            else
            {
                builder.AppendLine("<p><i>No questions were not answers</i></p>");
            }
            builder.AppendLine();
            var unqiueCorrectQuestions = quizzer.CorrectQuestions.Distinct();
            if (unqiueCorrectQuestions.Any())
            {
                builder.AppendLine($"<p><b>Questions answered correctly by {quizzer.FirstName}:</b></p>");
                builder.AppendLine("<ul>");
                foreach (var item in unqiueCorrectQuestions)
                {
                    builder.AppendLine($"<li>#{item.Number} --- {item.Text}</li>");
                }
                builder.AppendLine("</ul>");
            }
            else
            {
                builder.AppendLine($"<p><i>No correct answers were given during practice by {quizzer.FirstName}</i></p>");
            }
            builder.AppendLine();
            var unqiueWrongQuestions = quizzer.WrongQuestions.Distinct();
            if (unqiueWrongQuestions.Any())
            {
                builder.AppendLine($"<p><b>Questions answered wrong by {quizzer.FirstName}:</b></p>");
                builder.AppendLine("<ul>");
                foreach (var item in unqiueWrongQuestions)
                {
                    builder.AppendLine($"<li>#{item.Number} --- {item.Text}</li>");
                }
                builder.AppendLine("</ul>");
            }
            else
            {
                builder.AppendLine($"<p><i>No wrong answers were given during practice by {quizzer.FirstName}</i></p>");
            }
            builder.AppendLine();
            if (quizzer.AssignedQuestions.Any())
            {
                builder.AppendLine($"<p><b>Questions assigned to {quizzer.FirstName}:</b></p>");
                builder.AppendLine("<ul>");
                foreach (var item in quizzer.AssignedQuestions)
                {
                    builder.AppendLine($"<li>#{item.Number} --- {item.Text}</li>");
                }
                builder.AppendLine("</ul>");
            }
            else
            {
                builder.AppendLine($"<p><i>No questions are assigned to {quizzer.FirstName}</i></p>");
            }
            builder.AppendLine($"<p><i>Assigned Questions</i> are questions that the quizzer is expected to focus on to know so other quizzers on the team can focus on other questions.</p>");

            return builder.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_client != null)
            {
                if (disposing)
                {
                    _client.Disconnect(true);

                    _client.Dispose();
                }

                _client = null;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
