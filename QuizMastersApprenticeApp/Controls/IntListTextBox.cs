
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace QuizMastersApprenticeApp.Controls
{
    public class IntListTextBox : TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            try
            {
                IntList = ParseIntList(Text);
                IsValidIntList = true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                IsValidIntList = false;
            }

            base.OnTextChanged(e);
        }

        public static readonly DependencyProperty IsValidIntListProperty = DependencyProperty.Register(nameof(IsValidIntList), typeof(bool), typeof(IntListTextBox),
            new FrameworkPropertyMetadata(false));

        public bool IsValidIntList
        {
            get { return (bool)GetValue(IsValidIntListProperty); }
            set { SetValue(IsValidIntListProperty, value); }
        }

        public static readonly DependencyProperty IntListProperty = DependencyProperty.Register(nameof(IntList), typeof(List<int>), typeof(IntListTextBox),
            new FrameworkPropertyMetadata(null));

        public List<int> IntList
        {
            get { return (List<int>)GetValue(IntListProperty) ?? new List<int>(); }
            set { SetValue(IntListProperty, value); }
        }

        static private List<int> ParseIntList(string text)
        {
            var retVal = new List<int>();

            if (string.IsNullOrEmpty(text))
            {
                return retVal;
            }
            else
            {
                var items = text.Split(',');
                foreach (var item in items)
                {
                    if (item.Contains('-'))
                    {
                        var subItems = item.Split('-');
                        if (subItems.Length == 2)
                        {
                            int beginIntVal;
                            if (int.TryParse(subItems[0], out beginIntVal) == true)
                            {
                                int endIntVal;
                                if (int.TryParse(subItems[1], out endIntVal) == true)
                                {
                                    if (beginIntVal <= endIntVal)
                                    {
                                        for (int i = beginIntVal; i <= endIntVal; i++)
                                        {
                                            AddWhenIfNotExists(retVal, i);
                                        }
                                    }
                                    else
                                    {
                                        throw new ParseIntListException("Beginning int value is not less than or equal to end value");
                                    }
                                }
                                else
                                {
                                    throw new ParseIntListException($"Unable to parse end value: {subItems[1]}");
                                }
                            }
                            else
                            {
                                throw new ParseIntListException($"Unable to parse beginning value: {subItems[0]}");
                            }
                        }
                        else
                        {
                            throw new ParseIntListException($"Range must contain two values separated by a hypen: {item}");
                        }
                    }
                    else
                    {
                        int intVal;
                        if (int.TryParse(item, out intVal) == true)
                        {
                            AddWhenIfNotExists(retVal, intVal);
                        }
                        else
                        {
                            throw new ParseIntListException($"Unable to parse single (non-range) value: {item}");
                        }
                    }
                }

                retVal.Sort();
                return retVal;
            }
        }

        static private void AddWhenIfNotExists(List<int> list, int value)
        {
            if (list.Contains(value) == false)
            {
                list.Add(value);
            }
        }
    }

    public class ParseIntListException : Exception
    {
        public ParseIntListException()
        {
        }

        public ParseIntListException(string message) : base(message)
        {
        }
    }
}
