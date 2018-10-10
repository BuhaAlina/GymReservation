using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Services
{
    public class RoleService :IRoleService
    {

        private DatabaseFactory factory;
        private IRepository<Domain.Role> roleRepository;
        private IRepository<Domain.User> userRepository;
        private readonly IUnitofWork unitOfWork;

        public RoleService(DatabaseFactory factory)
        {

            this.factory = factory;
            this.roleRepository = new Repository<Domain.Role>(factory);
            this.userRepository = new Repository<Domain.User>(factory);
            this.unitOfWork = new UnitofWork(this.factory);

        }

        public void AddRole(Role myRole)
        {
            roleRepository.Add(myRole);
            unitOfWork.Commit();
        }

        public void EditRole(Role myRole)
        {
            roleRepository.Update(myRole);
            unitOfWork.Commit();
        }
        public void DeleteRole(Role myRole, int id)
        {

            var isUsed = userRepository.GetAll().FirstOrDefault(x => x.RoleId == id);
            if (isUsed == null)
            {
                roleRepository.Delete(myRole);
                unitOfWork.Commit();
            }
            else
            {
                throw new ArgumentException();
            }


        }
    }
}
