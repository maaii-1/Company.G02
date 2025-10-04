using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUSer> _userManager;

        public AccountController(UserManager<AppUSer> userManager)
        {
            _userManager = userManager;
        }

        #region SignUp

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if(user is null)
                {
                     user = await _userManager.FindByEmailAsync(model.Email);
                    if(user is null)
                    {
                        user = new AppUSer()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            UserName = model.UserName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,

                        };

                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                    }
                }

                ModelState.AddModelError("", "Invalid Username Or Password!");

            }


            return View(model);
        }

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid)
            {
               var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {

                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid Email Or Password!");

            }
            
            return View();
        }


        #endregion
    }
}
