using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LibValidation
{
    public class BindableBaseWithValidation : BindableBase, INotifyDataErrorInfo
    {
        public void Validate(object value, string propertyName)
        {
            if (errorDictionary.ContainsKey(propertyName))
                errorDictionary.Remove(propertyName);

            var validationContext = new ValidationContext(this, null)
            {
                MemberName = propertyName
            };
            var validationResult = new List<ValidationResult>();
            Validator.TryValidateProperty(value, validationContext, validationResult);

            if (validationResult.Count > 0)
            {
                var errorList = validationResult.Select(v => v.ErrorMessage);
                errorDictionary.Add(propertyName, errorList);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public bool HasErrors => errorDictionary.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) &&
                errorDictionary.ContainsKey(propertyName) &&
                errorDictionary[propertyName].Count() > 0)
                return errorDictionary[propertyName];
            else
                return null;
        }

        private Dictionary<string, IEnumerable<string>> errorDictionary = new Dictionary<string, IEnumerable<string>>();
    }
}
