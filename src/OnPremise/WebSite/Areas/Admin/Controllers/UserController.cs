using BrockAllen.MembershipReboot;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Authorization.Mvc;
using Thinktecture.IdentityServer.Repositories;
using Thinktecture.IdentityServer.Web.Areas.Admin.ViewModels;

namespace Thinktecture.IdentityServer.Web.Areas.Admin.Controllers
{
    [ClaimsAuthorize(Constants.Actions.Administration, Constants.Resources.Configuration)]
    public class UserController : Controller
    {
        [Import]
        public IUserManagementRepository UserManagementRepository { get; set; }
        UserAccountService userAccountService;

        public UserController()
        {
            Container.Current.SatisfyImportsOnce(this);
            this.userAccountService = DependencyResolver.Current.GetService<UserAccountService>();
        }

        public UserController(IUserManagementRepository userManagementRepository, UserAccountService userAccountService)
        {
            this.UserManagementRepository = userManagementRepository;
            this.userAccountService = userAccountService;
        }

        public ActionResult Index(string filter = null, int page = 0, int size = 20)
        {

            var vm = new UsersViewModel(this.userAccountService, page, size, filter);
            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string action, UserDeleteModel[] list)
        {
            if (action == "new") return Create();
            if (action == "delete") return Delete(list);

            ModelState.AddModelError("", Resources.UserController.InvalidAction);
            var vm = new UsersViewModel(this.userAccountService);

            return View("Index", vm);
        }

        public ActionResult Create()
        {
            var rolesvm = new UserRolesViewModel(UserManagementRepository, String.Empty);
            var vm = new UserInputModel();
            vm.Roles = rolesvm.RoleAssignments;
            return View("Create", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userName = SecuritySettings.Instance.EmailIsUsername ? model.Email : model.UserName;

                    this.UserManagementRepository.CreateUser(userName, model.Password, model.Email);
                    if (model.Roles != null)
                    {
                        var roles = model.Roles.Where(x => x.InRole).Select(x => x.Role);
                        if (roles.Any())
                        {
                            this.UserManagementRepository.SetRolesForUser(model.Email, roles);
                        }
                    }
                    TempData["Message"] = Resources.UserController.UserCreated;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorCreatingUser);
                }
            }

            return View("Create", model);
        }

        public ActionResult Detail(string username)
        {
            var user = userAccountService.GetByUsername(username);
            var vm = new UserViewModel
            {
                User = user
            };

            return this.View(vm);
        }

        private ActionResult Delete(UserDeleteModel[] list)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var name in list.Where(x => x.Delete).Select(x => x.Username))
                    {
                        this.UserManagementRepository.DeleteUser(name);
                    }
                    TempData["Message"] = Resources.UserController.UsersDeleted;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorDeletingUser);
                }
            }
            return Index();
        }

        public ActionResult Roles(string username)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, username);
            return View("Roles", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(string username, UserRoleAssignment[] roleAssignments)
        {
            var vm = new UserRolesViewModel(this.UserManagementRepository, username);
            if (ModelState.IsValid)
            {
                try
                {
                    var currentRoles =
                        roleAssignments.Where(x => x.InRole).Select(x => x.Role);
                    this.UserManagementRepository.SetRolesForUser(username, currentRoles);
                    TempData["Message"] = Resources.UserController.RolesAssignedSuccessfully;
                    return RedirectToAction("Roles", new { username });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", Resources.UserController.ErrorAssigningRoles);
                }
            }

            return View("Roles", vm);
        }

        public new ActionResult Profile(string username)
        {
            var vm = new UserProfileViewModel(username);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public new ActionResult Profile(string username, ProfilePropertyInputModel[] profileValues)
        {
            var vm = new UserProfileViewModel(username, profileValues);

            if (vm.UpdateProfileFromValues(ModelState))
            {
                TempData["Message"] = Resources.UserController.ProfileUpdated;
                return RedirectToAction("Profile", new { username });
            }

            return View(vm);
        }

        public ActionResult ChangePassword(string username)
        {
            UserPasswordModel model = new UserPasswordModel();
            model.Username = username;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(UserPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.UserManagementRepository.SetPassword(model.Username, model.Password);
                    TempData["Message"] = Resources.UserController.ProfileUpdated;
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "Error updating password");
                }
            }

            return View("ChangePassword", model);
        }
    }
}
