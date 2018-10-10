using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymScheduler.Services
{
    public class ClassTypeService : IClassTypeService
    {
        private IRepository<Domain.ClassType> classRepository;
        private readonly IUnitofWork unitofWork;

        private IRepository<Domain.Category> categRepository;

        private IRepository<Domain.Timetable> timetableRepository;
       
        private DatabaseFactory factory;

        public ClassTypeService(DatabaseFactory factory)
        {
            this.factory = factory;
            this.classRepository = new Repository<Domain.ClassType>(factory);
            this.categRepository = new Repository<Domain.Category>(factory);
            this.timetableRepository = new Repository<Domain.Timetable>(factory);
            this.unitofWork = new UnitofWork(this.factory);

        }

        public void AddClassType(ClassType myClass)
        {
            classRepository.Add(myClass);

            //transform the object
            unitofWork.Commit();
        }
        public void EditClassType(ClassType myClass)
    {
            classRepository.Update(myClass);
            unitofWork.Commit();
        }
        public void DeleteClassType(ClassType myClass, int id)
        {

            var isUsed = timetableRepository.GetAll().FirstOrDefault(x => x.ClassTypeId == id);

            if (isUsed == null)
            {
                classRepository.Delete(myClass);
                unitofWork.Commit();
            }
            else
            {
                throw new ArgumentException();
            }
        }

    }
}
