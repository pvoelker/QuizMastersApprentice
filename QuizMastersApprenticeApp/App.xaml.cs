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

            SetupUnhandledExceptionHandling();

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

        private void SetupUnhandledExceptionHandling()
        {
            // Catch exceptions from all threads in the AppDomain
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception, "AppDomain.CurrentDomain.UnhandledException", false);

            // Catch exceptions from each AppDomain that uses a task scheduler for async operations
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                ShowUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException", false);

            // Catch exceptions from a single specific UI dispatcher thread
            Dispatcher.UnhandledException += (sender, args) =>
            {
                // If we are debugging, let Visual Studio handle the exception and take us to the code that threw it.
                if (!Debugger.IsAttached)
                {
                    args.Handled = true;
                    ShowUnhandledException(args.Exception, "Dispatcher.UnhandledException", true);
                }
            };
        }

        private void ShowUnhandledException(Exception e, string unhandledExceptionType, bool promptUserForShutdown)
        {
            var messageBoxTitle = $"An unexpected error occurred: {unhandledExceptionType}";
            var messageBoxMessage = $"Exception information:\n\n{e}";
            var messageBoxButtons = MessageBoxButton.OK;

            if (promptUserForShutdown)
            {
                messageBoxMessage += "\n\nIt is recommended that the application be closed. Do you want to close the application?";
                messageBoxButtons = MessageBoxButton.YesNo;
            }

            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                Current.Shutdown();
            }
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
