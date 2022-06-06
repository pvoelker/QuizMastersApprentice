using QMA.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMA.DataAccess
{
    public interface IAppDataRepository : IDisposable
    {
        List<DatabaseInfo> GetDatabases();

        void SetDatabases(List<DatabaseInfo> value);

        string SmtpAddress { get; set; }

        int SmtpPort { get; set; }

        string UserName { get; set; }

        string FromName { get; set; }

        string FromEmail { get; set; }

        void Save();
    }
}
