
namespace backend
{
    public class SupermarketService : ISupermarketService
    {
        private static List<Supermarkets> _supermarket = new List<Supermarkets>();

        static SupermarketService()
        {
            _supermarket.Add(new Supermarkets
            {
                Id = 1,
                Name = "Lidl",
                WebsiteUrl = "https://www.lidl.hu",
            });
        }
        //public SupermarketService()
        //{
        //    _supermarket = [];
        //}
        public void Add(Supermarkets supermarkets)
        {
            _supermarket.Add(supermarkets);
        }

        public void Delete(int id)
        {
            _supermarket.RemoveAll(x => x.Id == id);
        }

        public List<Supermarkets> Get()
        {
            return _supermarket;
        }

        public Supermarkets Get(int id)
        {
            return _supermarket.Find(x => x.Id == id);
        }

        public void Update(Supermarkets supermarkets)
        {
            var oldSupermarket = Get(supermarkets.Id);

            oldSupermarket.WebsiteUrl = supermarkets.WebsiteUrl;
            oldSupermarket.Name = supermarkets.Name;
        }
    }
}
