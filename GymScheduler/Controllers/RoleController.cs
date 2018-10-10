using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Services;
using GymScheduler.ViewModels;
using Omu.ValueInjecter;

namespace GymScheduler.Controllers
{
    public class RoleController: Controller
    {
        private readonly IRepository<Domain.Role> roleRepository;
        //private readonly IUnitofWork unitOfWork;
        private IRoleService roleService;

        public RoleController()
        {
            var dbFactory = new DatabaseFactory();
            roleRepository = new Repository<Domain.Role>(dbFactory);
            //this.unitOfWork = new UnitofWork(dbFactory);
            roleService = new RoleService(dbFactory);

        }


        // GET: Category
        public ActionResult Index()
        {
            var roles = roleRepository.GetAll();

            IEnumerable<ViewModels.Role> roleView = roles.Select(dbrole => new ViewModels.Role()
            {
                Id = dbrole.Id,
                Name = dbrole.Name,
            });
            return View(roleView);
        }

        

        // POST: Create
        [HttpPost]
        public ActionResult Create(ViewModels.Role model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.Role();
                dbModel.InjectFrom(model);
                //roleRepository.Add(dbModel);
                roleService.AddRole(dbModel);
                //transform the object
                //unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new Domain.Role();
            var roleView = new Role();
            roleView.InjectFrom(model);
            return View(roleView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = roleRepository.GetById(id);
            var roleView = new Role();
            roleView.InjectFrom(model);
            return View(roleView);
        }


        [HttpPost]
        public ActionResult Edit(ViewModels.Role model, int id)
        {
            if (ModelState.IsValid)
            {
                var dbRole = roleRepository.GetById(id);
                dbRole.InjectFrom(model);
                //roleRepository.Update(dbRole);
                roleService.EditRole(dbRole);
                //unitOfWork.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var dbRole = roleRepository.GetById(id);
            if (dbRole == null)
            {
                return HttpNotFound();
            }

            var roleView = new Role();
            roleView.InjectFrom(dbRole);
            return View(roleView);
        }


        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Role model)
        {


            var dbRole = roleRepository.GetById(id);

            try
            {
                // roleRepository.Delete(dbRole);
                roleService.DeleteRole(dbRole, id);
                //unitOfWork.Commit();
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("Error", "Cant' delete, already used");

                model.InjectFrom(dbRole);
                return View(model);

            }


            return RedirectToAction("Index");

        }

    }
}