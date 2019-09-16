using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace ValidationDataAnnotations
{
    public class ValidationLib : BindableBase, INotifyDataErrorInfo
    {
        public bool HasErrors => _errorDictionary.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            var validationResult = new List<ValidationResult>
        }

        private Dictionary<string, IEnumerable<string>> _errorDictionary = new Dictionary<string, IEnumerable<string>>();
    }
}
