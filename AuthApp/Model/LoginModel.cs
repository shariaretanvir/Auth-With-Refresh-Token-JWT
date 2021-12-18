using AuthApp.Model.Validations;
using System.ComponentModel.DataAnnotations;

namespace AuthApp.Model
{
    public class LoginModel
    {
        //[CustomValidations()]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
