using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngCalculator.MvvmCore
{
    public abstract class ViewModelBase : ObservableObject, IDataErrorInfo
    {
        public string this[string columnName] => OnValidate(columnName);

        public string Error => throw new NotSupportedException();

        protected virtual string OnValidate(string propertyName)
        {
            ValidationContext context = new(this)
            {
                MemberName = propertyName
            };

            Collection<ValidationResult> results = new();

            bool isValid = Validator.TryValidateObject(this, context, results, true);

            if (!isValid)
            {
                ValidationResult result = results.SingleOrDefault(p => p.MemberNames.Any(memberName => memberName == propertyName));

                return result?.ErrorMessage;
            }

            return null;
        }
    }
}
