using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Services;
using GymScheduler.ViewModels;
using Omu.ValueInjecter;

namespace GymScheduler.Controllers
{
    public class StudioController : Controller
    {
        private readonly IRepository<Domain.Studio> studioRepository;
        private readonly IUnitofWork unitofWork;
        private IStudioService studioService;

        public StudioController()
        {
            var dbFactory = new DatabaseFactory();
            studioRepository = new Repository<Domain.Studio>(dbFactory);
            unitofWork = new UnitofWork(dbFactory);
            studioService= new StudioService(dbFactory);
        }

        // GET: Studios
        public ActionResult Index()
        {
            var studios = studioRepository.GetAll();

            IEnumerable<Studio> studiosView = studios.Select(s => new Studio()
            {
                Id = s.Id,
                Name = s.Name,
                Capacity = s.Capacity

            });

            return View(studiosView);

        }

        // GET : Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new Studio();
            return View(model);
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(Studio model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.Studio();
                dbModel.InjectFrom(model);
                //_studioRepository.Add(dbModel);
               // _unitofWork.Commit();
                studioService.AddStudio(dbModel);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET : Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var studio = studioRepository.GetById(id);
            var studioView = new Studio();
            studioView.InjectFrom(studio);
            return View(studioView);
        }


        // POST : Edit
        [HttpPost]
        public ActionResult Edit(Studio model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.Studio();
                dbModel.InjectFrom(model);
                // _studioRepository.Update(dbModel);
                //  _unitofWork.Commit();
                studioService.EditStudio(dbModel);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET : Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {

           var studio = studioRepository.GetById(id);

            if (studio == null)
            {
                return HttpNotFound();
            }
            var studioView = new Studio();
            studioView.InjectFrom(studio);
            return View(studioView);
        }

        // POST : Delete
        [HttpPost]
        public ActionResult Delete(Studio model, int id)
        {
            Domain.Studio studio = studioRepository.GetById(id);
            try
            {
                studioService.DeleteStudio(studio,id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Cant' delete is already used");

                var studioView= new ViewModels.Studio();
                {
                    studioView.InjectFrom(studio);
                   
                }

                return View(studioView);
            }
            
            //unitofWork.Commit();
           // studioService.DeleteStudio(studio);
            return RedirectToAction("Index");

        }
    }
}