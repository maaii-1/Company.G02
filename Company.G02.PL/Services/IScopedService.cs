namespace Company.G02.PL.Services
{
    public interface IScopedService
    {
        public Guid guid { get; set; }
        string GetGuid();
    }
}
