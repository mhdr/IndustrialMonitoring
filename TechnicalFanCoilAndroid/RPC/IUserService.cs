namespace TechnicalFanCoilAndroid.RPC
{
    public interface IUserService
    {
        bool Authorize(string userName, string password);

        string AuthorizeAndGetSession(string username, string password);
    }
}