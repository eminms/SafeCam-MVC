using System.ComponentModel.DataAnnotations;

namespace SafeCam.ViewModels.ProductVM
{
    public class UpdateProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is reqired")]
        [MaxLength(100, ErrorMessage = "Max length must be 100 characters"),
         MinLength(5, ErrorMessage = "Min length must be 5 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Image is required")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public int CategoryId { get; set; }
    }
}
