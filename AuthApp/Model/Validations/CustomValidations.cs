using System.ComponentModel.DataAnnotations;

namespace AuthApp.Model.Validations
{
    public class CustomValidations : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var loginModel = (LoginModel)validationContext.ObjectInstance;
        //    if (loginModel.UserName.Contains("String"))
        //    {
        //        return new ValidationResult("Name contains invalid string");
        //    }
        //    return ValidationResult.Success;
        //}
    }
}
