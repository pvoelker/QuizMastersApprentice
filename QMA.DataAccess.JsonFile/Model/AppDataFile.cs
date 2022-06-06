using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess.JsonFile.Model
{
    public class AppDataFile
    {
        public string SmtpAddress { get; set; }

        public int SmtpPort { get; set; }

        public string UserName { get; set; }

        public string FromName { get; set; }

        public string FromEmail { get; set; }

        public List<DatabaseInfo> Databases { get; set; } = new List<DatabaseInfo>();
    }
}
