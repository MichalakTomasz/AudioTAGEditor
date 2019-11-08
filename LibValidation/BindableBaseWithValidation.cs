using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LibValidation
{
    /// <summary>
    /// You have to use validation attributes (from System.ComponentModel.DataAnnotations namespace) before properties which should be checked 
    /// and add "UpdateSourceTrigger=PropertyChanged" with binding into validated field from view
    /// </summary>
    public abstract class BindableBaseWithValidation : 
        BindableBase, INotifyDataErrorInfo
    {
        public virtual void Validate<T>(T value, string propertyName)
        {
            if (errorDictionary.ContainsKey(propertyName))
                errorDictionary.Remove(propertyName);

            var validationContext = new ValidationContext(this)
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
        }

        public bool HasErrors => errorDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public void OnErrorChanged([CallerMemberName] string propertyName = null)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected override bool SetProperty<T>(
            ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            Validate(value, propertyName);
            return base.SetProperty(ref storage, value, propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (!string.IsNullOrWhiteSpace(propertyName) &&
                errorDictionary.ContainsKey(propertyName) &&
                errorDictionary[propertyName].Count() > 0)
                return errorDictionary[propertyName];
            else
                return null;
        }

        private Dictionary<string, IEnumerable<string>> errorDictionary = 
            new Dictionary<string, IEnumerable<string>>();
    }
}
