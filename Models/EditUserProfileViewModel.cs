using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
	public class EditUserProfileViewModel
	{
		public string Id { get; set; }
		[Required(ErrorMessage ="Обязательное поле!")]
		public string UserName { get; set; }
		
		[Required(ErrorMessage ="Обязательное поле!")]
		public string UserSurname { get; set; }
		
		[Required(ErrorMessage ="Обязательное поле!")]
		public string PhoneNumber { get; set; }
		
		public IFormFile UploadedFile { get; set; }
		public string? AvatarImgPath { get; set; } = "/images/null-avatar.jpeg";
	}
}