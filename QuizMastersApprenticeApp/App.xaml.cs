using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuizMastersApprenticeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string AppDataFileName { get { return "appData.json"; } }

        public string AppName { get { return "QMA"; } }

        public string AppDataFolder { get; private set; }

        public string CommonDocFolder { get; private set; }

        public string UserDocFolder { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
#endif

            SetupAppDataFolder();
            SetupCommonDocFolder();
            SetupUserDocFolder();

            base.OnStartup(e);
        }

        private void SetupAppDataFolder()
        {
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            AppDataFolder = Path.Combine(AppDataFolder, AppName);

            Directory.CreateDirectory(AppDataFolder);
        }

        private void SetupCommonDocFolder()
        {
            CommonDocFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            CommonDocFolder = Path.Combine(CommonDocFolder, AppName);

            Directory.CreateDirectory(CommonDocFolder);
        }

        private void SetupUserDocFolder()
        {
            UserDocFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            UserDocFolder = Path.Combine(UserDocFolder, AppName);

            Directory.CreateDirectory(UserDocFolder);
        }
    }


#if DEBUG
    public class DebugTraceListener : TraceListener
    {
        public override void Write(string? message)
        {
            // Do nothing
        }

        public override void WriteLine(string? message)
        {
            Debugger.Break();
        }
    }
#endif
}
