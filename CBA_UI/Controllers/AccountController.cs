using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CBA.Models;
using CBA.Logic;
using CBA.Core;
using System.Text;
using CBA.Core.ViewModels;

namespace CBA.Controllers
{
    //[Authorize(Roles="Admin")]
    public class UserController : Controller
    {
        BranchLogic branchLogic = new BranchLogic();
        UserLogic userLogic = new UserLogic();
        public UserController()
            : this(new UserManager<User>(new UserStore<User>(new ApplicationDbContext())))
        {
        }

        public UserController(UserManager<User> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<User> UserManager { get; private set; }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var allUsers = userLogic.GetAll();
            //Utils.SendEmail("ayodejiquadri30@gmail.com","Core Banking Application", "You have been registered on the Core Banking App");
            ViewBag.foundUsers = allUsers;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Index(FindUserView model)
        {
            IList<User> foundUsers = new List<User>();

            if (ModelState.IsValid)
            {
                foundUsers = userLogic.FindUsers(model.UserName);
                if (foundUsers.Count == 0)
                {
                    ModelState.AddModelError("", "No User was found");
                }
            }
            ViewBag.foundUsers = foundUsers;
            return View(model);            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            User user = userLogic.GetById(id);
            if (user == null)            
                RedirectToAction("Index");
            
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            ViewBag.UserName = user.UserName;
            return View(new EditUserViewModel() 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OtherNames = user.OtherNames,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                BranchID = user.Branch.ID,
                IsSuperAdmin = user.IsSuperAdmin

            });
        }

        //
        // POST: /UserManagement/Edit/5
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(EditUserViewModel model)
        {
            string userID = Request.Form["Id"]; //int.Parse(collection["ID"]);
            User user = userLogic.GetById(userID);
            if (user == null)//A user with the Id does not exist. som1 may have tried to post anoda id.
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {                
                int branchID = model.BranchID;
                var branch = branchLogic.GetById(branchID);
                if (branch==null)
                {
                    ModelState.AddModelError("BranchID", "Sorry that branch does not exist");
                }
                string email = model.Email.ToLower();
                if (userLogic.IsUserExistingWithEmail(model.Email) && (!user.Email.Equals(email)))
                {
                    ModelState.AddModelError("Email", "Sorry a user exists with that Email");
                }
                if (ModelState.IsValid)
                {
                    user.Branch = branch;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.OtherNames = model.OtherNames;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    userLogic.Update(user);
                    TempData["SuccessMessage"] = "User was updated successfully";
                    return RedirectToAction("Index");
                }
            }                        
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name", model.BranchID);
            ViewBag.UserName = user.UserName;
            return View(model);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RolesEnum)));
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Branch branch;
                //using (var context = new CBAContext())
                //{
                //    branch = context.Branches.Where(x => x.Name.Equals("Opebi")).First();
                //}
                if (userLogic.IsUserExistingWithEmail(model.Email))
                {
                    ModelState.AddModelError("Email", "Sorry a user exists with that Email");
                }
                if (!userLogic.IsUserNameAvailable(model.UserName))
                {
                    ModelState.AddModelError("UserName", "Sorry that user name alredy exists ");
                }
                using (var context = new CBAContext())
                {
                    branch = context.Branches.Where(x => x.ID == model.BranchID).SingleOrDefault();
                    if (branch == null)
                    {
                        ModelState.AddModelError("BranchID", "Invalid Branch");
                    }
                }
                RolesEnum roleEnum = 0;
                if (!Enum.TryParse<RolesEnum>(model.Role,true,out roleEnum))
                {
                    ModelState.AddModelError("", "Invalid role");
                }
                if (ModelState.IsValid) 
                {
                    var userName = model.UserName.Trim().ToLower();
                    var email = model.Email.Trim().ToLower();
                    var user = new User()
                    {
                        UserName = userName,
                        Email = email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        OtherNames = model.OtherNames,
                        PhoneNumber = model.PhoneNumber,
                        BranchId = branch.ID
                    };
                    string password = System.Web.Security.Membership.GeneratePassword(7,0);
                    //var branch = branchLogic.GetById(model.BranchID);
                    // UpdateModel(user);
                    //user.Branch = new Branch();
                    //user.Branch.Name = "Lekki";
                    //user.Branch.DateCreated = user.Branch.DateModified = DateTime.Now;
                    var result = await UserManager.CreateAsync(user, password);
                    //UserManager.a
                    if (result.Succeeded)
                    {
                        var currentUser = UserManager.FindByName(user.UserName);
                        var roleresult = UserManager.AddToRole(currentUser.Id, roleEnum.ToString());

                        var msgBody = getRegisterUserEmailBody(model.FirstName, model.UserName, password, roleEnum.ToString());
                        Utils.SendEmail(model.Email, "Core Banking Application user creation", msgBody);
                        //StringBuilder sb = new StringBuilder();
                        //sb.AppendFormat("User was created with username: {0} and  Password: {1}",userName,password);
                        //TempData["SuccessMessage"] = sb.ToString();
                        //await SignInAsync(user, isPersistent: false);
                        //return RedirectToAction("Index", "Home");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
               
                //if (branch!=null)
                //{
                    
                //}

               
            }
            //ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
            //    "ID", "Name");
            // If we got this far, something failed, redisplay form
            ViewBag.AllBranches = new SelectList(branchLogic.GetAll().OrderBy(b => b.Name),
                "ID", "Name");
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(RolesEnum)));
            return View(model);
        }

        private string getRegisterUserEmailBody(string firstName, string userName, string password, string role)
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Quadri-PC\Documents\Projects\Visual Studio 2013\Projects\CBA\CBA_UI\EmailTemplates\RegisterUser.txt"); 
            try
            {
                text = text.Replace("{firstName}", firstName);
                text = text.Replace("{role}", role);
                text = text.Replace("{userName}", userName);
                text = text.Replace("{password}", password);                
            }
            catch (Exception e)
            {
                                
            }
            return text;
            //throw new NotImplementedException();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        //[Authorize(Roles = "Admin")]
        public ActionResult ChangePassword(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangePassword(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("ChangePassword");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangePassword", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}