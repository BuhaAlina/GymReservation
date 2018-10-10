using GymScheduler.Data.Infrastructure;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymScheduler.Domain;
using GymScheduler.Services;
using GymScheduler.ViewModels;

namespace GymScheduler.Controllers
{
    public class ReservationController : Controller
    {
        private IRepository<Domain.Reservation> reservationRepository;
        private IRepository<Domain.User> userRepository;
        private IRepository<Domain.Timetable> timetableRepository;
        //private IRepository<Domain.Studio> studioRepository;
        private IReservationServices reservationServices;

        public ReservationController()
        {
            var dbFactory = new DatabaseFactory();
            this.reservationRepository = new Repository<Domain.Reservation>(dbFactory);
            this.userRepository = new Repository<Domain.User>(dbFactory);
            this.timetableRepository = new Repository<Domain.Timetable>(dbFactory);
            //this.studioRepository = new Repository<Domain.Studio>(dbFactory);
            this.reservationServices = new ReservationServices(dbFactory);
        }

        // GET: Reservation
        public ActionResult Index()
        {
            var reservations = reservationRepository.GetAll();
            IEnumerable<ViewModels.Reservation> reservationView = reservations.Select(x => new ViewModels.Reservation()
            {
                Id = x.Id,
                Class = x.Timetable.ClassType.Name,
                Time = x.Timetable.StartTime,
                Date = x.Timetable.Date,
                FullName = x.User.FirstName + " " + x.User.LastName

            });
            return View(reservationView);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ViewModels.Reservation();


            var users = userRepository.GetAll().Where(x=>x.RoleId==3);
            var usersList = users.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.FirstName.ToString() + " " + x.LastName.ToString() }).ToList();
            ViewBag.Users = usersList;

            var timetables = timetableRepository.GetAll();
            var timetablesList = timetables.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.ClassType.Name.ToString() + " " + x.Date.ToShortDateString() + " " + x.StartTime.ToString() }).ToList();
            ViewBag.Timetables = timetablesList;
            //var classes = timetableRepository.GetAll();
            //var classesList = classes.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.ClassType.Name.ToString()}).ToList();
            //ViewBag.Classes = classesList;
            //var dates = timetableRepository.GetAll();
            //var datesList = dates.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text =x.Date.ToShortDateString()}).ToList();
            //ViewBag.Dates = datesList;
            //var startTime = timetableRepository.GetAll();
            //var StartTimeList = startTime.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text =x.StartTime.ToString() }).ToList();
            //ViewBag.StartTime = StartTimeList;
            //model.TimetableIdCopy = model.TimetableId;

            return View(model);
        }


        [HttpPost]
        public ActionResult Create(ViewModels.Reservation model)
        {
            var users = userRepository.GetAll().Where(x => x.RoleId == 3);
            var usersList = users.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.FirstName.ToString() + " " + x.LastName.ToString() }).ToList();
            ViewBag.Users = usersList;

            var timetables = timetableRepository.GetAll();
            var timetablesList = timetables.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.ClassType.Name.ToString() + " " + x.Date.ToShortDateString() + " " + x.StartTime.ToString() }).ToList();
            ViewBag.Timetables = timetablesList;

            if (ModelState.IsValid)
            {
                if (reservationServices.CapacityCheck(model.TimetableId))
                {
                    if (reservationServices.DuplicateRegistration(model.UserId, model.TimetableId) == false)
                    {
                        var dbModel = new Domain.Reservation();
                        dbModel.InjectFrom(model);
                        reservationServices.AddReservation(dbModel);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("UserId", "Already booked! ");
                    }

                }
                else
                {
                    ModelState.AddModelError("TimetableId", "Capacity full! ");
                }
            }
            return View(model);
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            var reservation = reservationRepository.GetById(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            var editReservation = new ViewModels.Reservation();


            var users = userRepository.GetAll().Where(x => x.RoleId == 3);
            var usersList = users.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.FirstName + " " + x.LastName.ToString() }).ToList();
            ViewBag.Users = usersList;

            var timetable = timetableRepository.GetAll();
            var selectList = timetable.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.ClassType.Name + " " + x.Date.ToShortDateString() + " " + x.StartTime.ToString() }).ToList();
            ViewBag.Timetable = selectList;

            return View(editReservation);
        }

        [HttpPost]
        public ActionResult Edit(ViewModels.Reservation model, int id)
        {
            if (ModelState.IsValid)
            {
                var dbReservation = reservationRepository.GetById(id);
                dbReservation.InjectFrom(model);
                reservationServices.EditReservation(dbReservation);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var reservationDel = reservationRepository.GetById(id);
            if (reservationDel == null)
            {
                return HttpNotFound();
            }
            var delName = new ViewModels.Reservation()
            {
                Time = reservationDel.Timetable.StartTime,
                FullName = reservationDel.User.FirstName + " " + reservationDel.User.LastName,
                Date = reservationDel.Timetable.Date,
            };
            return View(delName);
        }

        [HttpPost]
        public ActionResult Delete(ViewModels.Reservation model, int id)
        {
            Domain.Reservation reservation = reservationRepository.GetById(id);

            reservationServices.DeleteReservation(reservation);
            return RedirectToAction("Index");

        }

        

        public JsonResult CapacityCheck(int timetableId)
        {

            if (reservationServices.CapacityCheck(timetableId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DuplicityCheck(int TimetableId, int userId)
        {
            if (!reservationServices.DuplicateRegistration(userId, TimetableId))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}