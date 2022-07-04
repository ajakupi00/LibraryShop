using System.ComponentModel.DataAnnotations;



namespace PRA.Models
{
    public class PasswordModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [RegularExpressionAttribute(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+])[A-Za-z\d][A-Za-z\d!@#$%^&*()_+]{7,19}$")]

        public string Password { get; set; }



        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}