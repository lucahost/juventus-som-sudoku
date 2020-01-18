using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Juventus.SOM.Sudoku.Behaviors
{
    public class SudokuFieldValueValidationBehavior : Behavior<Entry>
    {
        public static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(SudokuFieldValueValidationBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidPropertyKey, value);
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = ValidateValue(e.NewTextValue);
            ((Entry)sender).TextColor = IsValid ? Color.Default : Color.Red;
        }

        private bool ValidateValue(object newText)
        {
            int iValue = 0;
            if (newText is string strValue)
            {
                if (string.IsNullOrEmpty(strValue))
                {
                    return true;
                }
                if (!int.TryParse(strValue, out iValue))
                {
                    return false;
                }
            }
            if (iValue > 0 && iValue < 10)
            {
                return true;
            }
            return false;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= HandleTextChanged;

        }
    }
}
