﻿using System;
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
        User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.checkPassword(model.Password));
        if (user != null)
        {
          await Authenticate(model.Login); // аутентификация

          return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
      }
      return View(model);
    }
    [HttpGet]
    public IActionResult Register()
    {
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
      var recaptcha = await _recaptcha.Validate(Request);
      if (!recaptcha.success)
      {
        ModelState.AddModelError("Recaptcha", "There was an error validating recatpcha. Please try again!");
        return View(model);
      }

      if (ModelState.IsValid)
      {
        User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
        if (user == null)
        {
          // добавляем пользователя в бд
          db.Users.Add(new User { name = model.Name, Login = model.Login, PasswordHash = CryptoHelper.GetMD5(model.Password), AccessLevel = 9000 });
          await db.SaveChangesAsync();

          await Authenticate(model.Login); // аутентификация

          return RedirectToAction("Index", "Home");
        }
        else
          ModelState.AddModelError("", "Некорректные имя, логин и(или) пароль");
      }
      return View(model);
    }

    private async Task Authenticate(string userName)
    {
      // создаем один claim
      var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
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