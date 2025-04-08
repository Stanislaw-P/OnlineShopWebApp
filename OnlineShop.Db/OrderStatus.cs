using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp
{
	public enum OrderStatus
	{
		Created,
		Processed,
		InPath,
		Canceled,
		Delivered
	}
}
