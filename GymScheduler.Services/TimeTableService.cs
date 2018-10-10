using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymScheduler.Domain;
using GymScheduler.Data.Infrastructure;

namespace GymScheduler.Services
{

    public class TimeTableService : ITimeTableService
    {
        private DatabaseFactory factory;
        private IRepository<Domain.Timetable> timetableRepository;
        private IRepository<Domain.Category> categoryRepository;
        private IRepository<Domain.Studio> studioRepository;
        private IRepository<Domain.ClassType> classtypeRepository;
        private IRepository<Domain.User> userRepository;
        private IRepository<Domain.Reservation> reservationRepository;
        private readonly IUnitofWork unitofWork;

        public TimeTableService(DatabaseFactory factory)
        {
            this.factory = factory;
            timetableRepository = new Repository<Domain.Timetable>(factory);
            categoryRepository = new Repository<Domain.Category>(factory);
            studioRepository = new Repository<Domain.Studio>(factory);
            classtypeRepository = new Repository<Domain.ClassType>(factory);
            userRepository = new Repository<Domain.User>(factory);
            reservationRepository = new Repository<Domain.Reservation>(factory);
            this.unitofWork = new UnitofWork(this.factory);

        }
        public void AddTimetable(Timetable myTimetable)
        {
            timetableRepository.Add(myTimetable);
            unitofWork.Commit();
        }

        public void EditTimetable(Timetable myTimetable)
        {
            timetableRepository.Update(myTimetable);
            unitofWork.Commit();
        }

        public void DeleteTimetable(Timetable myTimetable, int id)
        {
            var check = reservationRepository.GetAll().Count(x => x.TimetableId == id);
            if (check == 0)
            {
                timetableRepository.Delete(myTimetable);
                unitofWork.Commit();
            }
            else
            {
                throw new ArgumentException();
            }

        }

    }


}
