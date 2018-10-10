using GymScheduler.Areas.Clients.ViewModels;
using GymScheduler.Data.Infrastructure;
using GymScheduler.Services;
using System.Web.Mvc;
using Omu.ValueInjecter;

namespace GymScheduler.Areas.Clients.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly IRepository<Domain.User> _usersRepository;
        private readonly IUserService _userService;

        public MyAccountController()
        {
            var dbFactory = new DatabaseFactory();
            _usersRepository = new Repository<Domain.User>(dbFactory);
            _userService = new UserService(dbFactory);
        }

        // GET: Clients/MyAccount/Details/{id}
        [HttpGet]
        public ActionResult Details(int id)
        {
            var dbClient = _usersRepository.GetById(id);
            var viewClient = new User();
            viewClient.InjectFrom(dbClient);
            return View(viewClient);
        }

        // GET: Clients/MyAccount/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _usersRepository.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var viewClient = new User();
            viewClient.InjectFrom(user);
            viewClient.CurrentEmail = user.Email;

            return View(viewClient);
        }

        // POST: Clients/MyAccount/Edit/{id}
        [HttpPost]
        public ActionResult Edit(User model, int id)
        {
            if (ModelState.IsValid)
            {
                var dbClient = _usersRepository.GetById(id);
                var currentEmail = dbClient.Email;

                if (!_userService.ExistsEmail(model.Email))
                {
                    TryUpdateModel(dbClient);
                    _userService.EditUser(dbClient);
                }
                else if (_userService.ExistsEmail(model.Email) && model.Email == currentEmail)
                {
                    TryUpdateModel(dbClient);
                    _userService.EditUser(dbClient);
                }
                else
                {
                    ModelState.AddModelError("Email", "This email already exists");
                    return View(model);
                }
            }

            return RedirectToAction("Details", new { id = model.Id });
        }
    }
}