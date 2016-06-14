namespace SharedLibrarySocket.Interfaces
{
    public interface IUserService
    {
        bool Authorize(string userName, string password);

        string AuthorizeAndGetSession(string username, string password);
    }
}