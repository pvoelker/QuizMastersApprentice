using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using QMA.DataAccess;
using QMA.ViewModel.Observables.Practice;
using QMA.ViewModel.Provider;
using QMA.ViewModel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace QMA.ViewModel.Practice
{
    public class PracticeReport : ObservableObject
    {
        private IMessageBoxService _messageBoxService;

        public PracticeReport(
            IMessageBoxService messageBoxService,
            string seasonName,
            ObservableCollection<ObservablePracticeQuizzer> practiceQuizzers,
            int totalQuestionsAsked,
            ObservableCollection<ObservablePracticeQuestion> noAnswerQuestions,
            ObservableCollection<ObservablePracticeQuestion> justLearningQuestions,
            IAppDataRepository appDataRepository)
        {
            if (practiceQuizzers == null)
            {
                throw new ArgumentNullException(nameof(practiceQuizzers));
            }
            
            _messageBoxService = messageBoxService;

            SmtpAddress = appDataRepository.SmtpAddress;
            SmtpPort = appDataRepository.SmtpPort;
            UserName = appDataRepository.UserName;
            FromName = appDataRepository.FromName;
            FromEmail = appDataRepository.FromEmail;

            PracticeQuizzers = practiceQuizzers;

            _totalQuestionsAsked = totalQuestionsAsked;

            NoAnswerQuestions = noAnswerQuestions;

            JustLearningQuestions = justLearningQuestions;

            Initialize = new RelayCommand(() =>
            {
            });

            SendReports = new RelayCommand(() =>
            {
                var now = DateTime.Now;

                using (var email = new PracticeReportEmail())
                {
                    try
                    {
                        IsSending = true;

                        email.Connect(SmtpAddress, SmtpPort, UserName, Password);
                        foreach (var item in PracticeQuizzers)
                        {
                            if (item.ParentEmail != null)
                            {
                                try
                                {
                                    email.SendPracticeReport(now,
                                        seasonName,
                                        FromName, FromEmail,
                                        item.ParentFullName, item.ParentEmail,
                                        item,
                                        TotalQuestionsAsked,
                                        JustLearningQuestions,
                                        NoAnswerQuestions);

                                    item.ReportSent = true;
                                }
                                catch (Exception ex)
                                {
                                    item.ReportError = ex.ToString();
                                }
                            }
                        }

                        Close();
                    }
                    catch (Exception ex)
                    {
                        _messageBoxService.ShowError(ex.Message);
                    }
                    finally
                    {
                        IsSending = false;
                    }
                }
            });

            Closing = new RelayCommand<CancelEventArgs>((CancelEventArgs e) =>
            {
                appDataRepository.SmtpAddress = SmtpAddress;
                appDataRepository.SmtpPort = SmtpPort;
                appDataRepository.UserName = UserName;
                appDataRepository.FromName = FromName;
                appDataRepository.FromEmail = FromEmail;

                appDataRepository.Save();
            });
        }

        public ObservableCollection<ObservablePracticeQuizzer> PracticeQuizzers { get; } = null;

        public ObservableCollection<ObservablePracticeQuestion> NoAnswerQuestions { get; } = null;

        public ObservableCollection<ObservablePracticeQuestion> JustLearningQuestions { get; } = null;

        private int _totalQuestionsAsked = 0;
        public int TotalQuestionsAsked
        {
            get => _totalQuestionsAsked;
        }

        private string _smtpAddress;
        public string SmtpAddress
        {
            get => _smtpAddress;
            set=> SetProperty(ref _smtpAddress, value);
        }

        private int _smtpPort = 587;
        public int SmtpPort
        {
            get => _smtpPort;
            set => SetProperty(ref _smtpPort, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _fromName;
        public string FromName
        {
            get => _fromName;
            set => SetProperty(ref _fromName, value);
        }

        private string _fromEmail;
        public string FromEmail
        {
            get => _fromEmail;
            set => SetProperty(ref _fromEmail, value);
        }

        private bool _isSending;
        public bool IsSending
        {
            get => _isSending;
            set => SetProperty(ref _isSending, value);
        }

        #region Commands

        public IRelayCommand Initialize { get; }

        public IRelayCommand SendReports { get; }

        public IRelayCommand<CancelEventArgs> Closing { get; }

        #endregion

        #region Bindable events

        public event EventHandler Closed;
        private void Close()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }

        #endregion
    }
}
