namespace OnlineShopWebApp.Models
{
    public class Product
    {
        public Product(string name, decimal cost, string description, string imagePath)
        {
            Name = name;
            Cost = cost;
            Description = description;
            Id = instanceCounter;
            ImagePath = imagePath;
            instanceCounter++;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal Cost { get; }
        public string Description { get; }
        static int instanceCounter = 0;
        public string ImagePath { get; } 

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}";
        }
    }
}
