using FluentAssertions;
using System.Text;
using Xunit;

namespace QMA.Importers.Csv.UnitTests
{
    public class ImportTests
    {
        [Fact]
        public void HappyPath()
        {
            string fakeFileContents = "Number,Text,Answer,Points" + Environment.NewLine + "1234,What color is the sky?,Blue,10";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var importer = new QuestionImporter();

            var result = importer.Import(new StreamReader(fakeMemoryStream));

            result.Should().HaveCount(1);

            var resultList = result.ToList();

            resultList[0].Number.Should().Be(1234);
            resultList[0].Text.Should().Be("What color is the sky?");
            resultList[0].Answer.Should().Be("Blue");
            resultList[0].Points.Should().Be(10);
        }

        [Fact]
        public void HappyPathMultipleLine()
        {
            string fakeFileContents = "Number,Text,Answer,Points" + Environment.NewLine +
                "1234,What color is the sky?,Blue,10" + Environment.NewLine +
                "2345,What color is coal?,Black,15" + Environment.NewLine +
                "321,What color is a 'ruby'?,Red,20";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var importer = new QuestionImporter();

            var result = importer.Import(new StreamReader(fakeMemoryStream));

            result.Should().HaveCount(3);

            var resultList = result.ToList();

            resultList[0].Number.Should().Be(1234);
            resultList[0].Text.Should().Be("What color is the sky?");
            resultList[0].Answer.Should().Be("Blue");
            resultList[0].Points.Should().Be(10);

            resultList[1].Number.Should().Be(2345);
            resultList[1].Text.Should().Be("What color is coal?");
            resultList[1].Answer.Should().Be("Black");
            resultList[1].Points.Should().Be(15);

            resultList[2].Number.Should().Be(321);
            resultList[2].Text.Should().Be("What color is a 'ruby'?");
            resultList[2].Answer.Should().Be("Red");
            resultList[2].Points.Should().Be(20);
        }

        [Fact]
        public void MissingHeaders()
        {
            string fakeFileContents = "1234,What color is the sky?,Blue,10";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var importer = new QuestionImporter();

            var act = () => importer.Import(new StreamReader(fakeMemoryStream));

            act.Should().Throw<ImportMissingHeadersException>();
        }

        [Fact]
        public void BadFields()
        {
            string fakeFileContents = "Number,Text,Answer,Points" + Environment.NewLine + "1234,What color is the sky?,Blue";
            byte[] fakeFileBytes = Encoding.UTF8.GetBytes(fakeFileContents);

            var fakeMemoryStream = new MemoryStream(fakeFileBytes);

            var importer = new QuestionImporter();

            var act = () => importer.Import(new StreamReader(fakeMemoryStream));

            act.Should().Throw<ImportFailedException>();
        }
    }
}