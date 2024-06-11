using System.ComponentModel.DataAnnotations;

namespace vidoeMVC.ViewModels.Categories
{
    public class CreateCategoryVM
    {
        [Required, MinLength(2, ErrorMessage = "Minimal length 2"), MaxLength(15, ErrorMessage = "Maximal length 15")]
        public string Name { get; set; }
    }
}
