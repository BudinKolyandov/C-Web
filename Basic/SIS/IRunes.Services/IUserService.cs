using IRunes.Models;

namespace IRunes.Services
{
    public interface IUserService
    {
        User CreateUser(User user);


        User GetUserByUsernamAndPassword(string username, string password);

    }
}
