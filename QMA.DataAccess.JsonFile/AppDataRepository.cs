using QMA.DataAccess.JsonFile.Model;
using QMA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QMA.DataAccess.JsonFile
{
    public class AppDataRepository : IAppDataRepository
    {
        private string _fileName;

        private bool _disposedValue;

        private AppDataFile _model;

        public AppDataRepository(string fileName)
        {
            _fileName = fileName;

            if (File.Exists(_fileName))
            {
                var text = File.ReadAllText(_fileName);
                _model = JsonSerializer.Deserialize<AppDataFile>(text);
            }
            else
            {
                _model = new AppDataFile();
            }
        }

        public List<DatabaseInfo> GetDatabases()
        {
            return _model.Databases;
        }

        public void SetDatabases(List<DatabaseInfo> value)
        {
            _model.Databases = value;
        }

        public string SmtpAddress
        {
            get => _model.SmtpAddress;
            set => _model.SmtpAddress = value;
        }

        public int SmtpPort
        {
            get => _model.SmtpPort;
            set => _model.SmtpPort = value;
        }

        public string UserName
        {
            get => _model.UserName;
            set => _model.UserName = value;
        }

        public string FromName
        {
            get => _model.FromName;
            set => _model.FromName = value;
        }

        public string FromEmail
        {
            get => _model.FromEmail;
            set => _model.FromEmail = value;
        }

        public void Save()
        {
            var text = JsonSerializer.Serialize(_model, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_fileName, text);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Save();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
