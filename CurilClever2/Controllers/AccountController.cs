using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reCAPTCHA.AspNetCore;
using CurilClever2.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace CurilClever2.Controllers
{

  public class AccountController : Controller
  {
    private CleverDBContext db;
    private IRecaptchaService _recaptcha;

    public AccountController(CleverDBContext context, IRecaptchaService recaptcha)
    {
      db = context;
      _recaptcha = recaptcha;
    }
    public IActionResult AccessDenied()
    {
      return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
      if (ModelState.IsValid)
      {
        User user = await db.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.checkPassword(model.Password));
        if (user != null)
        {
          await Authenticate(user); // аутентификация

          return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
      }
      return View(model);
    }

    public FileContentResult GetCapturePicture(string hash)
    {
      CaptureModel cm = db.CaptureModels.Find(hash);
      if (cm == null) return null;

      CaptchaImage img = new CaptchaImage(cm.code, 120, 50);
      MemoryStream ms = new MemoryStream();
      img.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
      return new FileContentResult(ms.GetBuffer(), "image/jpeg");
    }
    public IActionResult GetCaptureBlock()
    {
      RegisterModel rm = new RegisterModel();
      CaptureModel cm = new CaptureModel();
      cm.code = rm.code;
      cm.hashstring = rm.CaptureHash;
      if (db.CaptureModels.Find(cm.hashstring) == null)
      {
        db.CaptureModels.Add(cm);
        db.SaveChanges();
      }
      return PartialView(rm);
    }
    [HttpGet]
    public IActionResult Register()
    {
      RegisterModel rm = new RegisterModel();
      CaptureModel cm = new CaptureModel();
      cm.code = rm.code;
      cm.hashstring = rm.CaptureHash;
      if (db.CaptureModels.Find(cm.hashstring) == null)
      {
        db.CaptureModels.Add(cm);
        db.SaveChanges();
      }
      return View(rm);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {

      var recaptcha = await _recaptcha.Validate(Request);
      if (!recaptcha.success)
      {
        ModelState.AddModelError("Recaptcha", "Неопознанная ошибка при обработке запроса на сервер гугл, попробуйте позднее!");
        return View(model);
      }
      if (!model.CheckUserCaptureinput())
        ModelState.AddModelError("CaptureUserInput", "неправильно введена капча ");
      if (ModelState.IsValid)
      {
        User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
        if (user == null)
        {
          Role role = db.Roles.Where(r => r.Name.Contains("DefaultUser")).FirstOrDefault();
          // добавляем пользователя в бд
          User newUser = new User {
            name = model.Name,
            Login = model.Login,
            PasswordHash = CryptoHelper.GetMD5(model.Password),
            AccessLevel = 9000,
            Role = role
          };

          db.Users.Add(newUser);
          await db.SaveChangesAsync();

          await Authenticate(newUser); // аутентификация

          return RedirectToAction("Index", "Home");
        }
        else
          ModelState.AddModelError("", "Некорректные имя, логин и(или) пароль");
      }
      return View(model);
    }

    private async Task Authenticate(User user)
    {
      // создаем один claim
      var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
        };
      // создаем объект ClaimsIdentity
      ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
      // установка аутентификационных куки
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Index", "Home");
    }
  }
}