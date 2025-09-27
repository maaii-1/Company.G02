
namespace Company.G02.PL.Services
{
    public class Singleton : ISingleton
    {
        public Singleton()
        {
            guid = Guid.NewGuid();
        }
        public Guid guid { get; set; }

        public string GetGuid()
        {
            return guid.ToString();
        }
    }
}
