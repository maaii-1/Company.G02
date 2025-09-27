
namespace Company.G02.PL.Services
{
    public class ScopedService : IScopedService
    {
        public ScopedService()
        {
            guid = Guid.NewGuid();
        }
        public Guid guid { get; set ; }

        public string GetGuid()
        {
            return guid.ToString();
        }
    }
}
