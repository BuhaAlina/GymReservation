using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Services;
using GymScheduler.Domain;
using Omu.ValueInjecter;
using GymScheduler.Areas.Clients.ViewModels;


namespace GymScheduler.Areas.Clients.Controllers
{
    public class TimeTableClientController : Controller
    {
        private IRepository<Domain.Timetable> timetableRepository;
        
        private ITimeTableService timeTableService;

        public TimeTableClientController()
        {
            var dbFactory = new DatabaseFactory();
            this.timetableRepository = new Repository<Domain.Timetable>(dbFactory);
          
            timeTableService = new TimeTableService(dbFactory);

        }
        // GET: Clients/TimeTableClient
        [HttpGet]
        public ActionResult Index()
        {
            var timetables = timetableRepository.GetAll();
            var timetableView = new Timetable();
            timetableView.InjectFrom(timetables);
           
            return View(timetableView);
          
        }
    }
}       