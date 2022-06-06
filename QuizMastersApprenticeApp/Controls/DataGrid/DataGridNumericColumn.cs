using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuizMastersApprenticeApp.Controls.DataGrid
{
    /// <summary>
    /// Data grid cell that only supports interger numbers.
    /// </summary>
    public class DataGridNumericColumn : DataGridTextColumn
    {
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            var textBox = editingElement as TextBox;

            if (textBox != null) textBox.PreviewTextInput += OnPreviewTextInput;

            return base.PrepareCellForEdit(editingElement, editingEventArgs);
        }

        private void OnPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            int value;

            if (!int.TryParse(e.Text, out value))
                e.Handled = true;
        }
    }
}
