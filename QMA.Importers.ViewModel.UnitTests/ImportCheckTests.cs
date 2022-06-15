using FluentAssertions;
using Moq;
using QMA.DataAccess;
using QMA.ViewModel.Observables;
using Xunit;

namespace QMA.Importers.ViewModel.UnitTests
{
    public class ImportCheckTests
    {
        private readonly string _questionSetId = Guid.NewGuid().ToString();

        [Fact]
        public void HappyPath()
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 1, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 1,
                    Text = "What color is the sky?",
                    Answer = "Blue",
                    Points = 10
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 2, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 2,
                    Text = "What is 1 + 1?",
                    Answer = "2",
                    Points = 15
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 3, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 3,
                    Text = "Is water wet?",
                    Answer = "Yes",
                    Points = 30
                }
            });

            var importCheck = new QuestionImportCheck();

            var importedData = new List<ObservableImportQuestion>
            {
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 1,
                        Text = "What color is the sky?",
                        Answer = "Blue",
                        Points = 10
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 2,
                        Text = "What is 1 + 1?",
                        Answer = "2",
                        Points = 15
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 3,
                        Text = "Is water wet?",
                        Answer = "Yes",
                        Points = 30
                    }),
            };

            importCheck.CheckAgainstQuestionSet(importedData, repositoryMock.Object, _questionSetId);

            importedData.Should().HaveCount(3);
            importedData.Should().NotContain(x => x.HasParseError == true);
            importedData.Should().OnlyContain(x => x.AlreadyExists == true);
        }

        [Fact]
        public void MismatchedQuestion()
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 1, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 1,
                    Text = "What color is the ocean?",
                    Answer = "Blue",
                    Points = 10
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 2, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 2,
                    Text = "What is 1 + 1?",
                    Answer = "2",
                    Points = 15
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 3, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 3,
                    Text = "Is water wet?",
                    Answer = "Yes",
                    Points = 30
                }
            });

            var importCheck = new QuestionImportCheck();

            var importedData = new List<ObservableImportQuestion>
            {
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 1,
                        Text = "What color is the sky?",
                        Answer = "Blue",
                        Points = 10
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 2,
                        Text = "What is 1 + 1?",
                        Answer = "2",
                        Points = 15
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 3,
                        Text = "Is water wet?",
                        Answer = "Yes",
                        Points = 30
                    }),
            };

            importCheck.CheckAgainstQuestionSet(importedData, repositoryMock.Object, _questionSetId);

            importedData.Should().HaveCount(3);
            importedData[0].ParseError.Should().Be("Question Mismatch");
            importedData[0].AlreadyExists.Should().BeTrue();
            importedData[1].HasParseError.Should().BeFalse();
            importedData[1].AlreadyExists.Should().BeTrue();
            importedData[2].HasParseError.Should().BeFalse();
            importedData[2].AlreadyExists.Should().BeTrue();
        }

        [Fact]
        public void MismatchedAnswer()
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 1, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 1,
                    Text = "What color is the sky?",
                    Answer = "Purple",
                    Points = 10
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 2, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 2,
                    Text = "What is 1 + 1?",
                    Answer = "2",
                    Points = 15
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 3, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 3,
                    Text = "Is water wet?",
                    Answer = "Yes",
                    Points = 30
                }
            });

            var importCheck = new QuestionImportCheck();

            var importedData = new List<ObservableImportQuestion>
            {
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 1,
                        Text = "What color is the sky?",
                        Answer = "Blue",
                        Points = 10
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 2,
                        Text = "What is 1 + 1?",
                        Answer = "2",
                        Points = 15
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 3,
                        Text = "Is water wet?",
                        Answer = "Yes",
                        Points = 30
                    }),
            };

            importCheck.CheckAgainstQuestionSet(importedData, repositoryMock.Object, _questionSetId);

            importedData.Should().HaveCount(3);
            importedData[0].ParseError.Should().Be("Answer Mismatch");
            importedData[0].AlreadyExists.Should().BeTrue();
            importedData[1].HasParseError.Should().BeFalse();
            importedData[1].AlreadyExists.Should().BeTrue();
            importedData[2].HasParseError.Should().BeFalse();
            importedData[2].AlreadyExists.Should().BeTrue();
        }

        [Fact]
        public void MismatchedPoints()
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 1, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 1,
                    Text = "What color is the sky?",
                    Answer = "Blue",
                    Points = 11
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 2, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 2,
                    Text = "What is 1 + 1?",
                    Answer = "2",
                    Points = 15
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 3, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 3,
                    Text = "Is water wet?",
                    Answer = "Yes",
                    Points = 30
                }
            });

            var importCheck = new QuestionImportCheck();

            var importedData = new List<ObservableImportQuestion>
            {
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 1,
                        Text = "What color is the sky?",
                        Answer = "Blue",
                        Points = 10
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 2,
                        Text = "What is 1 + 1?",
                        Answer = "2",
                        Points = 15
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 3,
                        Text = "Is water wet?",
                        Answer = "Yes",
                        Points = 30
                    }),
            };

            importCheck.CheckAgainstQuestionSet(importedData, repositoryMock.Object, _questionSetId);

            importedData.Should().HaveCount(3);
            importedData[0].ParseError.Should().Be("Points Mismatch");
            importedData[0].AlreadyExists.Should().BeTrue();
            importedData[1].HasParseError.Should().BeFalse();
            importedData[1].AlreadyExists.Should().BeTrue();
            importedData[2].HasParseError.Should().BeFalse();
            importedData[2].AlreadyExists.Should().BeTrue();
        }

        [Fact]
        public void MultipleQuestionsWithSameNumber()
        {
            var repositoryMock = new Mock<IQuestionRepository>();
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 1, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 1,
                    Text = "What color is the sky?",
                    Answer = "Blue",
                    Points = 10
                },
                new Model.Question
                {
                    Number = 1,
                    Text = "Do I have the same number?",
                    Answer = "Yes",
                    Points = 15
                }
            });
            repositoryMock.Setup(e => e.GetByQuestionNumber(_questionSetId, 3, false)).Returns(new List<Model.Question>
            {
                new Model.Question
                {
                    Number = 3,
                    Text = "Is water wet?",
                    Answer = "Yes",
                    Points = 30
                }
            });

            var importCheck = new QuestionImportCheck();

            var importedData = new List<ObservableImportQuestion>
            {
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 1,
                        Text = "What color is the sky?",
                        Answer = "Blue",
                        Points = 10
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 2,
                        Text = "What is 1 + 1?",
                        Answer = "2",
                        Points = 15
                    }),
                new ObservableImportQuestion(
                    new Model.ImportQuestion
                    {
                        Number = 3,
                        Text = "Is water wet?",
                        Answer = "Yes",
                        Points = 30
                    }),
            };

            importCheck.CheckAgainstQuestionSet(importedData, repositoryMock.Object, _questionSetId);

            importedData.Should().HaveCount(3);
            importedData[0].ParseError.Should().Be("Multiples Found");
            importedData[0].AlreadyExists.Should().BeTrue();
            importedData[1].HasParseError.Should().BeFalse();
            importedData[1].AlreadyExists.Should().BeFalse();
            importedData[2].HasParseError.Should().BeFalse();
            importedData[2].AlreadyExists.Should().BeTrue();
        }
    }
}