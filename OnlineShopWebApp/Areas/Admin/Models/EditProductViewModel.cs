using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
    public class EditProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Range(1, 100000, ErrorMessage = "Цена должна быть в пределех от 1 до 100 000 руб!")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        public string? Description { get; set; }

        public List<string> ImagesPaths { get; set; }

        public IFormFile[]? UploadedFiles { get; set; }
        public byte[] ConcurrenceToken { get; set; }
    }
}
