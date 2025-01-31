using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class Product
    {
        public Product()
        {
            Id = instanceCounter;
            instanceCounter++;
        }

        public Product(string name, int cost, string description, string imagePath) : this()
        {
            Name = name;
            Cost = cost;
            Description = description;
            ImagePath = imagePath;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Название должно содержать от 4 до 30 символов!")]
		public string Name { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [Range(0, 10000000, ErrorMessage = "Минимальная стоимость 0 руб.")]
		public int Cost { get; set; }

        [Required(ErrorMessage = "Обязательное поле!")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "Описание должно содержать от 4 до 200 символов!")]
        public string Description { get; set; }

        public string ImagePath { get; set; } = "none"; // Нужно, чтобы работала валидация без ошибок

        static int instanceCounter = 0;

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}";
        }
    }
}
