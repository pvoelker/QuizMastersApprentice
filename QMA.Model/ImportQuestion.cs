  using System;

namespace QMA.Model
{
    /// <summary>
    /// Information for a question and the associated answer
    /// </summary>
    public class ImportQuestion : IEquatable<ImportQuestion>
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

        public override bool Equals(object obj) => this.Equals(obj as ImportQuestion);

        public bool Equals(ImportQuestion p)
        {
            if (p is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (GetType() != p.GetType())
            {
                return false;
            }

            return (Number == p.Number) && (Text == p.Text) && (Answer == p.Answer) && (Points == p.Points);
        }

        public override int GetHashCode() => (Number, Text, Answer, Points).GetHashCode();

        public static bool operator ==(ImportQuestion lhs, ImportQuestion rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ImportQuestion lhs, ImportQuestion rhs) => !(lhs == rhs);
    }
}
