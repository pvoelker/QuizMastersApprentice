  using System;

namespace QMA.Model
{
    /// <summary>
    /// Information for a question and the associated answer
    /// </summary>
    public class ImportQuestion
    {
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
        /// The point value of the question
        /// </summary>
        public int Points { get; set; }
    }
}
