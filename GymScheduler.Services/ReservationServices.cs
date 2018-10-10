using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Domain;
using Omu.ValueInjecter;

namespace GymScheduler.Services
{
    public class ReservationServices : IReservationServices
    {
        private DatabaseFactory dbFactory;
        private IRepository<Domain.Reservation> reservationRepository;
        private IRepository<Domain.Timetable> timetableRepository;
        private readonly IUnitofWork unitofWork;

        public ReservationServices(DatabaseFactory factory)
        {
            this.dbFactory = factory;
            this.reservationRepository = new Repository<Domain.Reservation>(dbFactory);
            this.timetableRepository = new Repository<Domain.Timetable>(dbFactory);
            this.unitofWork = new UnitofWork(dbFactory);
        }

        public void AddReservation(Reservation myReservation)
        {
            reservationRepository.Add(myReservation);
            unitofWork.Commit();
        }

        public void EditReservation(Reservation myReservation)
        {
            reservationRepository.Update(myReservation);
            unitofWork.Commit();
        }

        public void DeleteReservation(Reservation myReservation)
        {
            reservationRepository.Delete(myReservation);
            unitofWork.Commit();
        }

        public bool CapacityCheck(int timetableId)
        {
            var capacity = timetableRepository.GetById(timetableId).Studio.Capacity;
            var reservationCount = reservationRepository.GetAll().Count(x => x.TimetableId == timetableId);
            return reservationCount < capacity;
        }

        public bool DuplicateRegistration(int userId, int timetableId) 
        {
            var duplicate = reservationRepository.GetAll()
                .Count(x => x.UserId == userId && x.TimetableId == timetableId);
            return duplicate > 0;
        }

        
    }
}
