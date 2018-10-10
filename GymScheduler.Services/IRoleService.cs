using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Services
{
    public interface IRoleService
    {
        void AddRole(Role myRole);
        void EditRole(Role myRole);
        void DeleteRole(Role myRole, int id);
    }
}
