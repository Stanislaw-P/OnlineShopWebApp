using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp
{
	public enum OrderStatusViewModel
	{
		[Display(Name ="Создан")]
		Created,
		[Display(Name ="Обработан")]
		Processed,
		[Display(Name ="В пути")]
		InPath,
		[Display(Name ="Отменён")]
		Canceled,
		[Display(Name ="Доставлен")]
		Delivered
	}
}
