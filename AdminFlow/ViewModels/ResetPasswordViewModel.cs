using System.ComponentModel.DataAnnotations;

namespace AdminFlow.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parola eşleşmiyor")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}