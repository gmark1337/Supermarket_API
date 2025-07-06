
using System.Text.Json;

namespace backend
{
    public class FlyerService : IFlyerService
    {
        private  List<Flyer> _lidl;

        public List<Flyer> Get()
        {
            return _lidl;
        }

        public void SetAll(List<Flyer> pages)
        {
            _lidl = pages;
        }
    }
}
