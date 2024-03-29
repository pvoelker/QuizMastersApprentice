﻿using QMA.Model.Season;
using System;
using System.Collections.Generic;

namespace QMA.Model
{
    public class SeasonInfo : SoftDeletablePrimaryKeyBase
    {
        public string Name { get; set; }

        public string QuestionSetId { get; set; }

        public string Notes { get; set; }
    }
}
