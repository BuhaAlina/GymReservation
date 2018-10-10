using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;
using System;
using System.Linq;

namespace GymScheduler.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _usersRepository;
        private readonly IUnitofWork _unitofWork;

        public UserService(IDatabaseFactory factory)
        {
            _usersRepository = new Repository<User>(factory);
            _unitofWork = new UnitofWork(factory);
        }

        public void AddUser(User user)
        {
            _usersRepository.Add(user);
            _unitofWork.Commit();
        }

        public void EditUser(User user)
        {
            _usersRepository.Update(user);
            _unitofWork.Commit();
        }

        public void DeleteUser(User user)
        {
            _usersRepository.Delete(user);
            _unitofWork.Commit();
        }

        public bool ExistsEmail(string email)
        {
            var users = _usersRepository.GetAll();
            var emails = users.Select(u => u.Email);

            if (emails.Any(item => string.Equals(email, item, StringComparison.CurrentCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }
    }
}
