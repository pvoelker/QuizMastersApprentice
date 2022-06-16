using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model.Season
{
    public class TeamMember : PrimaryKeyBase
    {
        public string TeamId { get; set; }

        public string QuizzerId { get; set; }
    }
}
