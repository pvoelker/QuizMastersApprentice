using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuizMastersApprenticeApp.Controls
{
    public partial class DataGridRowButtons : UserControl
    {
        public DataGridRowButtons()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsDeletedProperty = DependencyProperty.Register(nameof(IsDeleted), typeof(bool), typeof(DataGridRowButtons),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsDeleted
        {
            get { return (bool)GetValue(IsDeletedProperty); }
            set { SetValue(IsDeletedProperty, value); }
        }

        public static readonly DependencyProperty IsNotDeletedProperty = DependencyProperty.Register(nameof(IsNotDeleted), typeof(bool), typeof(DataGridRowButtons),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsNotDeleted
        {
            get { return (bool)GetValue(IsNotDeletedProperty); }
            set { SetValue(IsNotDeletedProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(DataGridRowButtons));
        
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty RestoreCommandProperty = DependencyProperty.Register(nameof(RestoreCommand), typeof(ICommand), typeof(DataGridRowButtons));

        public ICommand RestoreCommand
        {
            get { return (ICommand)GetValue(RestoreCommandProperty); }
            set { SetValue(RestoreCommandProperty, value); }
        }
    }
}