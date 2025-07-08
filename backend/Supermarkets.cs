namespace backend
{
    public class Supermarkets
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }

        public ICollection<Flyer>Flyers { get; set; } = new List<Flyer>();
    }
}
