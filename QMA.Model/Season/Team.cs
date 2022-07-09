using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.Model.Season
{
    public class Team : SoftDeletablePrimaryKeyBase
    {
        public string SeasonId { get; set; }

        public string Name { get; set; }

        public int? MaxPointValue { get; set; }

        public string Notes { get; set; }

        public IEnumerable<string> QuizzerIds { get; set; }
    }
}
