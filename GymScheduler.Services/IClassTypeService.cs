using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Services
{
    public interface IClassTypeService
    {
        void AddClassType(ClassType myCateg);
        void EditClassType(ClassType myCateg);
        void DeleteClassType(ClassType myCateg, int id);

    }
}
