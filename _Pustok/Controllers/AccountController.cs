using _Pustok.DAL;
using _Pustok.Models;
using _Pustok.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _Pustok.Controllers
{
    public class AccountController : Controller
    {
        private readonly PustokContext _pustokContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(PustokContext pustokContext,UserManager<AppUser> userManager)
        {
            this._pustokContext = pustokContext;
            this._userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVM memberVM)
        {
            if(!ModelState.IsValid)
                return View();

            //AppUser user = _pustokContext.AppUsers.FirstOrDefault(x => x.NormalizedUserName == memberVM.Username.ToUpper());
           

            if(await _userManager.FindByNameAsync(memberVM.Username) != null)
            {
                ModelState.AddModelError("Username", "User alrady exist");
                return View();
            }
            else if(await _userManager.FindByEmailAsync(memberVM.Email) != null)
            {
                ModelState.AddModelError("Email", "Email alrady exist");
                return View();
            }

            AppUser user = new AppUser
            {
                UserName = memberVM.Username,
                Email = memberVM.Email,
                Fullname = memberVM.Fullname,
            };
           var result = await _userManager.CreateAsync(user, memberVM.Password);

            if (!result.Succeeded)
            {
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }

                return View();
            }

            return RedirectToAction("login");
        }
    }
}
