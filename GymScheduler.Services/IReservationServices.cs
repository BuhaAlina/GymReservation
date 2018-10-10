using GymScheduler.Domain;



namespace GymScheduler.Services
{
    public interface IReservationServices
    {
        void DeleteReservation(Reservation myReservation);
        void EditReservation(Reservation myReservation);
        void AddReservation(Reservation myReservation);
        bool CapacityCheck(int timetableId);
        bool DuplicateRegistration(int userId, int timetableId);
    }
}