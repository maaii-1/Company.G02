namespace Company.G02.PL.Services
{
    public interface ISingleton
    {
        public Guid guid { get; set; }
        string GetGuid();
    }
}
