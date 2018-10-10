using GymScheduler.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymScheduler.ViewModels;
using Omu.ValueInjecter;
using GymScheduler.Services;



namespace GymScheduler.Controllers
{
    public class ClassTypeController : Controller
    {
        private IRepository<Domain.ClassType> classRepository;
       // private readonly IUnitofWork unitofWork;
        private IRepository<Domain.Category> categRepository;
        private IClassTypeService classServices;
        //private ClassTypeContext db;
        
        public ClassTypeController()
        {
            var dbFactory = new DatabaseFactory();
            classRepository = new Repository<Domain.ClassType>(dbFactory);
            categRepository = new Repository<Domain.Category>(dbFactory);
            // this.unitofWork = new UnitofWork(dbFactory);
            classServices = new ClassTypeService(dbFactory);
            //ClassTypeContext db = new ClassTypeContext();
        }
        
        // GET: ClassTypes
        public ActionResult Index(string option, string search)
        {


           var classes = classRepository.GetAll();
           var categ = categRepository.GetAll();
           IEnumerable<ViewModels.ClassType> classView = classes.Select(dbclass => new ViewModels.ClassType()
             {
                 Id = dbclass.Id,
                 Name = dbclass.Name,
                 Duration = dbclass.Duration,
                 CategoryId = dbclass.CategoryId,
                 CategoryName = dbclass.Category.Name

             });

            var categList = categ.Select(db => new SelectListItem()
            {
                Value = db.Id.ToString(),
                Text = db.Name

            }).ToList();

                     

            ViewBag.Category = categList;
            return View(classView);
            
        }


        // POST: Create
        [HttpPost]
        public ActionResult Create(ViewModels.ClassType model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.ClassType();
                           
                dbModel.InjectFrom(model);
                //classRepository.Add(dbModel);
                classServices.AddClassType(dbModel);
                //transform the object
                //unitofWork.Commit();
                return RedirectToAction("Index");
            }
            return View(model);
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ClassType();

            var categ = categRepository.GetAll();
            var categList = categ.Select(db => new SelectListItem()
            {
                Value = db.Id.ToString(),
                Text = db.Name

            }).ToList();

            ViewBag.Category = categList;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dbClass = classRepository.GetById(id);
            var classView = new ClassType();
            var categ = categRepository.GetAll();
            var categList = categ.Select(db => new SelectListItem()
            {
                Value = db.Id.ToString(),
                Text = db.Name

            }).ToList();

            ViewBag.Category = categList;

            classView.InjectFrom(dbClass);
            return View(classView);
        }


        [HttpPost]
        public ActionResult Edit(ViewModels.ClassType model, int id)
        {
            if (ModelState.IsValid)
            {
              var dbClass = classRepository.GetById(id);
              dbClass.InjectFrom(model);
                //classRepository.Update(dbClass);
                classServices.EditClassType(dbClass);
              //unitofWork.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var dbClass = classRepository.GetById(id);
            if (dbClass == null)
            {
                return HttpNotFound();
            }
            var classView = new ClassType();
            classView.InjectFrom(dbClass);
            return View(classView);
        }


        [HttpPost]
        public ActionResult Delete( int id,ViewModels.ClassType model)
        {
            var dbClass = classRepository.GetById(id);
            
            try
            {
                //classRepository.Delete(dbClass);                
                classServices.DeleteClassType(dbClass, id);
                // unitofWork.Commit();
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("Error", "Cant' delete, already used");
                model.InjectFrom(dbClass);
                return View(model);
            }
            return RedirectToAction("Index");
            
        }

        
       
    }
}