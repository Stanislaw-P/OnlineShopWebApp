namespace OnlineShopWebApp
{
	public class RandomCounter : ICounter
	{
		static Random rnd = new Random();
		private int _value;
		public RandomCounter()
		{
			_value = rnd.Next(100);
		}

		public int Value => _value;
	}
}
