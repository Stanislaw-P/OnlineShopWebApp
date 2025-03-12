using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Название должно содержать от 4 до 30 символов!")]
		public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Range(0, 10000000, ErrorMessage = "Минимальная стоимость 0 руб.")]
		public decimal Cost { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "Описание должно содержать от 4 до 200 символов!")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Изображение обязательно!")]
        public IFormFile UploadedFile { get; set; }
        public string ImagePath { get; set; } = "/images/image-null.png"; // Нужно, чтобы работала валидация без ошибок
    }
}
