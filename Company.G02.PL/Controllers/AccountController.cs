using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUSer> _userManager;
        private readonly SignInManager<AppUSer> _signInManager;

        public AccountController(UserManager<AppUSer> userManager, SignInManager<AppUSer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                        var result =  await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if(result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Email Or Password!");

            }
            
            return View();
        }


        #endregion

        #region SignOut

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn)); // Redirect back to login page
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgotPasswordDto model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {

                    // Generate Token
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    // Create URL
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);


                    // Create Email

                    var emailBody = $@"
                                    <p>Dear {user.UserName},</p>
                                    <p>We received a request to reset your password for your account associated with this email address.</p>
                                    <p>To reset your password, please click the link below:</p>
                                    <p>
                                        <a href='{url}' style='display:inline-block;padding:10px 15px;
                                        background-color:#007bff;color:#fff;text-decoration:none;
                                        border-radius:5px;'>Reset Password</a>
                                    </p>
                                    <p>If you did not request this, you can safely ignore this email. 
                                    Your password will remain unchanged.</p>
                                    <p>Thank you,<br/>Support Team</p>";

                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password Request",
                        Body = emailBody
                    };

                    var flag = EmailSetting.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourInbox");
                    }
                }

            }
            ModelState.AddModelError("", "Invalid Reset Password");

            return View("ForgotPassword", model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            if(ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if(email is null ||  token is null)
                {
                    return BadRequest("Invalid password reset request.");
                }

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }

                }
                ModelState.AddModelError("", "Invalid password reset request.");
            }
            return View();
        }


        #endregion


        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
