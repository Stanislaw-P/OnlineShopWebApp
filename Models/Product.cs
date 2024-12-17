namespace OnlineShopWebApp.Models
{
    public class Product
    {
        public Product(string name, decimal cost, string description)
        {
            Name = name;
            Cost = cost;
            Description = description;
            Id = instanceCounter;
            instanceCounter++;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal Cost { get; }
        public string Description { get; }
        static int instanceCounter = 0;

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}\n{Description}";
        }
    }
}
