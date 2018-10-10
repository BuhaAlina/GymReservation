using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;

namespace GymScheduler.Services
{
   

    public class StudioService : IStudioService
    {
        private DatabaseFactory factory;
        private readonly IRepository<Domain.Studio> studioRepository;
        private readonly IRepository<Domain.Timetable> timetableRepository;
        private readonly IUnitofWork unitofWork;

        public StudioService(DatabaseFactory factory)
        {
            this.factory = factory;
            this.studioRepository = new Repository<Studio>(factory);
            this.unitofWork = new UnitofWork(this.factory);
        }

        public void AddStudio(Studio myStudio)
        {
            studioRepository.Add(myStudio);
            unitofWork.Commit();
        }

        public void EditStudio(Studio myStudio)
        {
            studioRepository.Update(myStudio);
            unitofWork.Commit();
        }
        public void DeleteStudio(Studio myStudio, int id)
        {
            var check = timetableRepository.GetAll().Count(x => x.StudioId == id);
            if (check == 0)
            {
                studioRepository.Delete(myStudio);
                unitofWork.Commit();
            }
            else
            {
                throw new ArgumentException();
            }

        }

    }
}
