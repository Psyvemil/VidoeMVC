using System.ComponentModel.DataAnnotations;

namespace vidoeMVC.ViewModels.Accounts
{
    public class ForgotPasswordVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
