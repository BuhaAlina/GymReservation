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
    public class UserController : Controller
    {
        private readonly IRepository<Domain.User> _usersRepository;
        private readonly IRepository<Domain.Role> _rolesRepository;
        private readonly IUserService _userService;

        public UserController()
        {
            var dbFactory = new DatabaseFactory();
            _usersRepository = new Repository<Domain.User>(dbFactory);
            _rolesRepository = new Repository<Domain.Role>(dbFactory);
            _userService = new UserService(dbFactory);
        }

        // GET: Users
        public ActionResult Index()
        {
            var dbUsers = _usersRepository.GetAll();
            IEnumerable<User> users = dbUsers.Select(c => new User()
            {
                Id = c.Id,
                RoleId = c.RoleId,
                RoleName = c.Role.Name,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                StreetName = c.StreetName,
                StreetNo = c.StreetNo,
                City = c.City,
                Country = c.Country,
                TelephoneNo = c.TelephoneNo,
                IsActive = c.IsActive

            });
            return View(users.OrderBy(u => u.IsActive).ThenBy(u => u.FirstName));
        }

        // GET: Create
        [HttpGet]
        public ActionResult Create()
        {
            var model = new User();


            var roles = _rolesRepository.GetAll();
            var roleList = roles.Select(r => new SelectListItem()
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
                .ToList();

            ViewBag.Role = roleList;

            return View(model);
        }

        // POST: Create
        [HttpPost]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                var dbModel = new Domain.User();
                dbModel.InjectFrom(model);

                _userService.AddUser(dbModel);


                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _usersRepository.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var viewUser = new User()
            {
                Id = user.Id,
                RoleId = user.RoleId,
                RoleName = user.Role.Name,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                StreetName = user.StreetName,
                StreetNo = user.StreetNo,
                City = user.City,
                Country = user.Country,
                TelephoneNo = user.TelephoneNo,
                IsActive = user.IsActive

            };
            var roles = _rolesRepository.GetAll();
            var roleList = roles.Select(r => new SelectListItem()
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
                .ToList();

            ViewBag.Role = roleList;
            viewUser.InjectFrom(user);
            viewUser.CurrentEmail = user.Email;

            return View(viewUser);
        }


        [HttpPost]
        public ActionResult Edit(User model, int id)
        {
            var roles = _rolesRepository.GetAll();
            var roleList = roles.Select(r => new SelectListItem()
            {
                Value = r.Id.ToString(),
                Text = r.Name
            })
                .ToList();

            ViewBag.Role = roleList;

            if (ModelState.IsValid)
            {
                var dbUser = _usersRepository.GetById(id);
                var currentEmail = dbUser.Email;

                if (!_userService.ExistsEmail(model.Email))
                {
                    TryUpdateModel(dbUser);
                    _userService.EditUser(dbUser);
                }
                if (_userService.ExistsEmail(model.Email) && model.Email == currentEmail)
                {
                    TryUpdateModel(dbUser);
                    _userService.EditUser(dbUser);
                }
                else
                {
                    ModelState.AddModelError("Email", "This email already exists");
                    return View(model);
                }

            }

            return RedirectToAction("Index");
        }


        // GET: Delete
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = _usersRepository.GetById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var viewUser = new User();

            viewUser.InjectFrom(user);

            return View(viewUser);
        }

        // POST: Delete
        [HttpPost]
        public ActionResult Delete(User model, int id)
        {
            var user = _usersRepository.GetById(id);
            user.InjectFrom(model);

            _userService.DeleteUser(user);

            return RedirectToAction("Index");
        }

        public JsonResult IsEmailValid(string email, string currentEmail)
        {
            var users = _usersRepository.GetAll();
            var emails = users.Select(u => u.Email);

            if (email == currentEmail)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            if (_userService.ExistsEmail(email))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
