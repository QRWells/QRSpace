namespace QRSpace.Server.Services
{
    public interface IUserIdGenerator
    {
        ulong NextId();
    }
}