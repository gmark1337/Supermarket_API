namespace backend
{
    public interface IFlyerService
    {
        void SetAll(List<Flyer> pages);
        List<Flyer> Get();

    }
}
