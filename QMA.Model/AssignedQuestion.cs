  using System;

namespace QMA.Model
{
    public class AssignedQuestion : PrimaryKeyBase
    {
        public string QuestionId { get; set; }

        public string QuizzerId { get; set; }
    }
}
