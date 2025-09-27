
namespace Company.G02.PL.Services
{
    public class TransientSevrvice : ITransientSevrvice
    {
        public TransientSevrvice()
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
