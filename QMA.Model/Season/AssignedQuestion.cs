using System;

namespace QMA.Model.Season
{
    public class AssignedQuestion : PrimaryKeyBase
    {
        public string TeamMemberId { get; set; }

        public string QuestionId { get; set; }
    }
}
