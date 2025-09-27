namespace Company.G02.PL.Services
{
    public interface ITransientSevrvice
    {
        public Guid guid { get; set; }
        string GetGuid();
    }
}
