using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Services;
//using GymScheduler.ViewModels;
using Omu.ValueInjecter;
//using Category = GymScheduler.Domain.Category;

namespace GymScheduler.Controllers
{
    public class TimeTableController : Controller
    {
        private IRepository<Domain.Timetable> timetableRepository;
        private IRepository<Domain.Category> categoryRepository;
        private IRepository<Domain.Studio> studioRepository;
        private IRepository<Domain.ClassType> classtypeRepository;
        private IRepository<Domain.User> userRepository;
       // private readonly IUnitofWork unitofWork;
        private ITimeTableService timeTableService;
       
        public TimeTableController()
        {
            var dbFactory = new DatabaseFactory();
            this.timetableRepository = new Repository<Domain.Timetable>(dbFactory);
            this.categoryRepository = new Repository<Domain.Category>(dbFactory);
            this.studioRepository = new Repository<Domain.Studio>(dbFactory);
            this.classtypeRepository = new Repository<Domain.ClassType>(dbFactory);
            this.userRepository = new Repository<Domain.User>(dbFactory);
            timeTableService = new TimeTableService(dbFactory);

        }

        // GET: TimetTable
        public ActionResult Index()
        {
            var timetables = timetableRepository.GetAll();

            IEnumerable<ViewModels.Timetable> timetablesView = timetables.Select(t => new ViewModels.Timetable()
            {
                Id = t.Id,
                StartTime = t.StartTime,
               // CategoryName = t.Category.Name,
                StudioName = t.Studio.Name,
                ClassName = t.ClassType.Name,
                UserName = t.User.FirstName + " " + t.User.LastName,
                Date = t.Date,
                isActive = t.IsActive
            });
            return View(timetablesView);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ViewModels.Timetable();

            var category = categoryRepository.GetAll();
            var categoryList = category.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name })
                .ToList();
            ViewBag.Category = categoryList;

            var studio = studioRepository.GetAll();
            var studioList = studio.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name }).ToList();
            ViewBag.Studio = studioList;

            var classtype = classtypeRepository.GetAll();
            var classtypeList = classtype.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name })
                .ToList();
            ViewBag.ClassType = classtypeList;

            var user = userRepository.GetAll().Where(x=>x.RoleId==2);
            var userList = user.Select(t =>
                    new SelectListItem() { Value = t.Id.ToString(), Text = t.FirstName + " " + t.LastName.ToString() })
                .ToList();
            ViewBag.User = userList;

            return View(model);
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(ViewModels.Timetable model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.Timetable();
                dbModel.InjectFrom(model);
                //dbModel.StartTime = model.StartTime;
                dbModel.Date = model.Date;
                dbModel.IsActive = model.isActive;
                // timetableRepository.Add(dbModel);
                // unitofWork.Commit();
                timeTableService.AddTimetable(dbModel);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var timetable = timetableRepository.GetById(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }

            var timetableView = new ViewModels.Timetable()
            {
                Id = timetable.Id,
                StartTime = timetable.StartTime,
                //CategoryId = timetable.CategoryId,
                StudioId = timetable.StudioId,
                ClassTypeId = timetable.ClassTypeId,
                UserId = timetable.UserId,
               // CategoryName = timetable.Category.Name,
                StudioName = timetable.Studio.Name,
                ClassName = timetable.ClassType.Name,
                UserName  = timetable.User.FirstName + "" + timetable.User.LastName,
                Date = timetable.Date,
               isActive = timetable.IsActive

            };

            var category = categoryRepository.GetAll();
            var categoryList = category.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name })
                .ToList();
            ViewBag.Category = categoryList;


            var studio = studioRepository.GetAll();
            var studioList = studio.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name }).ToList();
            ViewBag.Studio = studioList;

            var classtype = classtypeRepository.GetAll();
            var classtypeList = classtype.Select(t => new SelectListItem() { Value = t.Id.ToString(), Text = t.Name })
                .ToList();
            ViewBag.ClassType = classtypeList;

            var user = userRepository.GetAll().Where(x=>x.RoleId==2);
            var userList = user.Select(t =>
                    new SelectListItem() { Value = t.Id.ToString(), Text = t.FirstName + " " + t.LastName.ToString() })
                .ToList();
            ViewBag.User = userList;

            //timetableView.InjectFrom(timetable);
            return View(timetableView);
        }




        [HttpPost]
        public ActionResult Edit(ViewModels.Timetable model, int id)
        {
            if (ModelState.IsValid)
            {
                var dbModel = timetableRepository.GetById(id);
                dbModel.StartTime = model.StartTime;
               // dbModel.CategoryId = model.CategoryId;
                dbModel.StudioId = model.StudioId;
                dbModel.ClassTypeId = model.ClassTypeId;
                dbModel.UserId = model.UserId;
                dbModel.Date = model.Date;
               dbModel.IsActive = model.isActive;
                //timetableRepository.Update(dbModel);
                //unitofWork.Commit();

                timeTableService.EditTimetable(dbModel);
            }

            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {

            var timetableDel = timetableRepository.GetById(id);

            if (timetableDel == null)
            {
                return HttpNotFound();
            }
            var timetableView = new ViewModels.Timetable()
            {
                StartTime = timetableDel.StartTime,
                //CategoryName = timetableDel.Category.Name,
                StudioName = timetableDel.Studio.Name,
                ClassName = timetableDel.ClassType.Name,
                UserName = timetableDel.User.FirstName + " " + timetableDel.User.LastName,
                Date = timetableDel.Date

            };

            // delView.InjectFrom(timetableDel);
            return View(timetableView);
        }


        [HttpPost]
        public ActionResult Delete(ViewModels.Timetable model, int id)
        {
            Domain.Timetable timetable = timetableRepository.GetById(id);
        
            try
            {
                timeTableService.DeleteTimetable(timetable, id);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError("Error", "Cant' delete is already used");

                var timetableView = new ViewModels.Timetable()
                {
                    StartTime = timetable.StartTime,
                    //CategoryName = timetableDel.Category.Name,
                    StudioName = timetable.Studio.Name,
                    ClassName = timetable.ClassType.Name,
                    UserName = timetable.User.FirstName + " " + timetable.User.LastName,
                    Date = timetable.Date

                };
             
                return View(timetableView);
                
            }


            return RedirectToAction("Index");

        }


        [HttpGet]
        public JsonResult IsDateValid(DateTime date)
        {
            if (date.Date >= DateTime.Now.Date)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}