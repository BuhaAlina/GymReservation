using GymScheduler.Domain;

namespace GymScheduler.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        void DeleteUser(User user);
        void EditUser(User user);
        bool ExistsEmail(string email);
    }
}