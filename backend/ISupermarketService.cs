namespace backend
{
    public interface ISupermarketService
    {
        void Add(Supermarkets supermarkets);

        void Delete(int id);

        List<Supermarkets> Get();

        Supermarkets Get(int id);

        void Update(Supermarkets supermarkets);
    }
}
