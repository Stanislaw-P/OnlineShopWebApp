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

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        static int instanceCounter = 0;
        public string ImagePath { get; set; } 

        public override string ToString()
        {
            return $"{Id}\n{Name}\n{Cost}";
        }
    }
}
