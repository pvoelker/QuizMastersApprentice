using FluentAssertions;
using QMA.Helpers;
using QMA.Model;
using System.IO;
using System.Linq;
using Xunit;

namespace QMA.Importers.BibleFactPak.UnitTests
{
    public class ImportTests
    {
        [Fact]
        public void HappyPath()
        {
            var importText = "Question: Question #1 for 10 pointsWhat is sin? (#275)Sin is refusing to do God's will as revealed by His Word and His Spirit.(1 John 3:4; James 4:17)Question: Question #2 for 15 pointsWhat does \"testament\" mean? (#4)Covenant [contract or agreement]Question: Question #3 for 20 pointsWho wrote more books of the Bible than any other person? (#15)PaulQuestion: Question #4 for 10 pointsWho was king over Judea when Jesus was born? (#176)Herod(Matthew 2:3)Question: Question #5 for 10 pointsWhat happened to the Egyptians who tried to follow the Israelites through the Red Sea? (#79)They all drowned.(Exodus 14:23–28)";

            var importer = new QuestionImporter();

            var result = importer.Import(new StreamReader(importText.ToStream()));

            result.Should().NotBeEmpty();
            result.Should().HaveCount(5);
            var resultList = result.ToList();
            resultList[0].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 275,
                Points = 10,
                Text = "What is sin?",
                Answer = "Sin is refusing to do God's will as revealed by His Word and His Spirit.(1 John 3:4; James 4:17)"
            });
            resultList[1].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 4,
                Points = 15,
                Text = "What does \"testament\" mean?",
                Answer = "Covenant [contract or agreement]"
            });
            resultList[2].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 15,
                Points = 20,
                Text = "Who wrote more books of the Bible than any other person?",
                Answer = "Paul"
            });
        }

        [Fact]
        public void FireBibePageNumber()
        {
            var importText = "Question: Question #8 for 10 pointsWhy did God reject Saul as king of Israel? (#131)He rejected God's command.(1 Samuel 15:22–23)Question: Question #9 for 10 pointsWho was the slave Paul won to the Lord while in prison? (#269)Onesimus(Philemon 10–13)Fire Bible Page Number: 1509Question: Question #10 for 10 pointsWho was the mother of Joseph and Benjamin? (#56)Rachel(Genesis 30:22–24; 35:16–18)";

            var importer = new QuestionImporter();

            var result = importer.Import(new StreamReader(importText.ToStream()));

            result.Should().NotBeEmpty();
            result.Should().HaveCount(3);
            var resultList = result.ToList();
            resultList[0].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 131,
                Points = 10,
                Text = "Why did God reject Saul as king of Israel?",
                Answer = "He rejected God's command.(1 Samuel 15:22–23)"
            });
            resultList[1].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 269,
                Points = 10,
                Text = "Who was the slave Paul won to the Lord while in prison?",
                Answer = "Onesimus(Philemon 10–13)"
            });
            resultList[2].Should().BeEquivalentTo(new ImportQuestion
            {
                Number = 56,
                Points = 10,
                Text = "Who was the mother of Joseph and Benjamin?",
                Answer = "Rachel(Genesis 30:22–24; 35:16–18)"
            });
        }

        [Fact]
        public void EmptyImport()
        {
            var importer = new QuestionImporter();

            var result = importer.Import(new StreamReader(string.Empty.ToStream()));

            result.Should().BeEmpty();
        }
    }
}