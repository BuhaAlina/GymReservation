using GymScheduler.Domain;

namespace GymScheduler.Services
{
    public interface ITimeTableService
    {
        void AddTimetable (Timetable myTimetable);
        void EditTimetable(Timetable myTimetable);
        void DeleteTimetable(Timetable myTimetable,int id);
    }
}