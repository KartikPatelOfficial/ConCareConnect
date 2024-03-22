using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LaboratoryPanelWPF.ViewModels;

public class FormViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private Dictionary<string, List<string>> Errors { get; }

    public FormViewModel()
    {
        Errors = new Dictionary<string, List<string>>();
    }

    // Validate properties using decorated attributes and/or a validation delegate. 
    // The validation delegate is optional.
    public bool ValidateProperty<TValue>(
      TValue value,
      Func<TValue, (bool IsValid, IEnumerable<string> ErrorMessages)> validationDelegate = null,
      [CallerMemberName] string propertyName = null)
    {
        // Clear previous errors of the current property to be validated 
        Errors.Remove(propertyName);
        OnErrorsChanged(propertyName);

        var isValueValid = ValidatePropertyUsingAttributes(value, propertyName);
        if (validationDelegate != null)
        {
            isValueValid |= ValidatePropertyUsingDelegate(value, validationDelegate, propertyName);
        }

        return isValueValid;
    }

    // Validate properties using decorated attributes. 
    public bool ValidatePropertyUsingAttributes<TValue>(TValue value, string propertyName)
    {
        // Check if property is decorated with validation attributes
        // using reflection
        var validationAttributes = GetType()
          .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
          ?.GetCustomAttributes(typeof(ValidationAttribute)) ?? new List<Attribute>();

        // Validate using attributes if present
        if (!validationAttributes.Any()) return true;

        var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
        var validationResults = new List<ValidationResult>();
        if (Validator.TryValidateProperty(value, validationContext, validationResults)) return true;

        foreach (var attributeValidationResult in validationResults)
        {
            AddError(propertyName, attributeValidationResult.ErrorMessage);
        }

        return false;
    }

    // Validate properties using the delegate. 
    public bool ValidatePropertyUsingDelegate<TValue>(
      TValue value,
      Func<TValue, (bool IsValid, IEnumerable<string> ErrorMessages)> validationDelegate,
      string propertyName)
    {
        // Validate using the delegate
        (var isValid, var errorMessages) = validationDelegate.Invoke(value);
        if (isValid) return true;

        // Store the error messages of the failed validation
        foreach (var errorMessage in errorMessages)
        {
            AddError(propertyName, errorMessage);
        }

        return false;
    }

    // Adds the specified error to the errors collection if it is not 
    // already present, inserting it in the first position if 'isWarning' is 
    // false. Raises the ErrorsChanged event if the Errors collection changes. 
    // A property can have multiple errors.
    public void AddError(string propertyName, string errorMessage, bool isWarning = false)
    {
        if (!Errors.TryGetValue(propertyName, out var propertyErrors))
        {
            propertyErrors = new List<string>();
            Errors[propertyName] = propertyErrors;
        }

        if (propertyErrors.Contains(errorMessage)) return;
        if (isWarning)
        {
            // Move warnings to the end
            propertyErrors.Add(errorMessage);
        }
        else
        {
            propertyErrors.Insert(0, errorMessage);
        }
        OnErrorsChanged(propertyName);
    }

    public bool PropertyHasErrors(string propertyName) => Errors.TryGetValue(propertyName, out var propertyErrors) && propertyErrors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    // Returns all errors of a property. If the argument is 'null' instead of the property's name, 
    // then the method will return all errors of all properties.
    public System.Collections.IEnumerable GetErrors(string propertyName)
      => string.IsNullOrWhiteSpace(propertyName)
        ? Errors.SelectMany(entry => entry.Value)
        : Errors.TryGetValue(propertyName, out var errors)
          ? errors
          : new List<string>();

    // Returns if the view model has any invalid property
    public bool HasErrors => Errors.Any();

    protected virtual void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
