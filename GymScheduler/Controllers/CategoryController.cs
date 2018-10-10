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
    public class CategoryController : Controller
    {

        private readonly IRepository<Domain.Category> categoryRepository;
       // private readonly IUnitofWork unitOfWork;
        private ICategoryService categoryService;


        public CategoryController()
        {
            var dbFactory = new DatabaseFactory();
            categoryRepository = new Repository<Domain.Category>(dbFactory);
            //this.unitOfWork = new UnitofWork(dbFactory);
            categoryService = new CategoryService(dbFactory);
        }


        // GET: Category
        public ActionResult Index()
        {
            var categories = categoryRepository.GetAll();

            IEnumerable<ViewModels.Category> categView = categories.Select(dbcateg => new ViewModels.Category()
            {
                Id = dbcateg.Id,
                Name = dbcateg.Name,
            });
            return View(categView);
        }


        // POST: Create
        [HttpPost]
        public ActionResult Create(ViewModels.Category model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.Category();
                dbModel.InjectFrom(model);
               // categoryRepository.Add(dbModel);
                categoryService.AddCategory(dbModel);
                //transform the object
                //unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new Domain.Category();
            var categView = new Category();
            categView.InjectFrom(model);
            return View(categView);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = categoryRepository.GetById(id);
            var categView = new Category();
            categView.InjectFrom(category);
            return View(categView);
        }


        [HttpPost]
        public ActionResult Edit(ViewModels.Category model,int id)
        {
            if (ModelState.IsValid)
            {
                var dbCategory = categoryRepository.GetById(id);
                dbCategory.InjectFrom(model);
                //categoryRepository.Update(dbCategory);
                categoryService.EditCategory(dbCategory);
                //unitOfWork.Commit();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var dbCategory = categoryRepository.GetById(id);
            if (dbCategory == null)
            {
                return HttpNotFound();
            }

            var categView = new Category();
            categView.InjectFrom(dbCategory);
            return View(categView);
        }


        [HttpPost]
        public ActionResult Delete(int id, ViewModels.Category model)
        {
            
            
                var dbCategory = categoryRepository.GetById(id);

            try
            {
                // categoryRepository.Delete(dbCategory);
                categoryService.DeleteCategory(dbCategory,id);
                // unitOfWork.Commit();
            }
            catch (ArgumentException e)
            {
                ModelState.AddModelError("Error", "Cant' delete, already used");
                
                return View(model);

            }
            

            return RedirectToAction("Index");

        }



    }
}