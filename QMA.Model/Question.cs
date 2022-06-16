  using System;

namespace QMA.Model
{
    public class Question : DeleteablePrimaryKeyBase
    {
        public string QuestionSetId { get; set; }

        /// <summary>
        /// Number of the question
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// The text for the question
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The answer for the question
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        ///  Points for the question
        /// </summary>
        public int Points { get; set; } = 0;

        /// <summary>
        /// Notes for the question
        /// </summary>
        public string Notes { get; set; }
    }
}
