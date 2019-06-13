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
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Login == model.Login && u.checkPassword(model.Password));
        if (user != null)
        {
          await Authenticate(user); // аутентификация

          return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Некорректные логин и(или) пароль");
      }
      return View(model);
    }

    public async Task<IActionResult> VkAuth(string code = "", string access_token = "", string expires_in = "", string user_id = "", string email = "")
    {

      

      string secret_key = "ftOs39sriOsLSZEhTOSd";
      string API_ID = "7020055";

      var x = HttpContext.Request;

      // авторизация в два этапа сначала получаем код доступа
      // потом получаем токен


      string request = "https://oauth.vk.com/access_token?client_id=7020055&client_secret=ftOs39sriOsLSZEhTOSd&redirect_uri=http://localhost/account/vkauth&code=" + code;


      var json_string = new WebClient().DownloadString(request);
      var data = JsonConvert.DeserializeObject<Rootobject2>(json_string);
      access_token = data.access_token;
      expires_in = data.expires_in.ToString();
      user_id = data.user_id.ToString();
      email = data.email;
      int vk_uid;

      if (!int.TryParse(user_id, out vk_uid))
      {
        return RedirectToAction("login");
      }

      

      // проверяем регался ли у нас пользователь с таким vk_uid
      VkUserID vkUserID = db.vkUserIDs.Where(v => v.vk_id == vk_uid).FirstOrDefault();

      if (vkUserID != null)
      {
        // если такая запись есть, то загружаем соответствуюшего пользователя из базы
        User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.id == vkUserID.user_id);
        // и авторизуем пользователя
        await Authenticate(user); // аутентификация
        // отправляем его домой
        return RedirectToAction("Index", "Home");
      }

      //получаем остальнгые даные пользователя (имя)

      string first_name = "";
      string last_name = "";

      string api_url = "https://api.vk.com/method/users.get?user_id=" + user_id + "&v=5.52&access_token=" + access_token;
      json_string = new WebClient().DownloadString(api_url);

      var json = JsonConvert.DeserializeObject<Rootobject>(json_string);
      first_name = json.response[0].first_name;
      last_name = json.response[0].last_name;

      User newuser = new User
      {
        name = first_name + " " + last_name,
        Login = email,
        AccessLevel = 9000,
        PasswordHash = CryptoHelper.GetMD5(user_id),
        Role = db.Roles.Where(r => r.Name.Contains("DefaultUser")).FirstOrDefault()
      };

      db.Users.Add(newuser);
      db.SaveChanges();

      vkUserID = new VkUserID { user_id = newuser.id, vk_id = vk_uid };
      db.vkUserIDs.Add(vkUserID);
      db.SaveChanges();

      newuser = db.Users.Include(u => u.Role).FirstOrDefault(u=>u.id == vkUserID.user_id);
      await Authenticate(newuser); // аутентификация

      return RedirectToAction("Index", "Home");
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
          User newUser = new User
          {
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

    [HttpGet]
    public IActionResult Manage()
    {
      // получаем текущего пользователя
      User currentuser = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
      // получаем подписку
      Subscribe sub;
      // если для пользователя еще нет настроек подписки
      if (db.Subscribes.Where(s => s.Userid == currentuser.id).Count() == 0)
      {
        // создаем новую
        sub = new Subscribe();
        sub.User = currentuser;
        sub.SendNews = false;
        // добавляем в базу
        db.Add(sub);
        db.SaveChanges();
      }
      else
        // в противном случае (если подписка у пользователя есть) загружаем ее из базы данных
        sub = db.Subscribes.Include(s => s.User).FirstOrDefault(s => s.Userid == currentuser.id);
      // создаем вьюмодель для текущего юзера и подписки
      ManageAccountViewModel maVM = new ManageAccountViewModel(currentuser, sub);

      return View(maVM);
    }
    [HttpPost]
    public IActionResult Manage(ManageAccountViewModel model)
    {
      // получаем текущего пользователя
      User currentuser = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
      // получаем подписку
      Subscribe sub;
      // если для пользователя еще нет настроек подписки
      if (db.Subscribes.Where(s => s.Userid == currentuser.id).Count() == 0)
      {
        // создаем новую
        sub = new Subscribe();
        sub.User = currentuser;
        sub.SendNews = false;
        // добавляем в базу
        db.Add(sub);
        db.SaveChanges();
      }
      else
        // в противном случае (если подписка у пользователя есть) загружаем ее из базы данных
        sub = db.Subscribes.Include(s => s.User).FirstOrDefault(s => s.Userid == currentuser.id);


      //обновляем настройки подписки у пользователя
      sub.SendNews = model.EMailNewsSubscribe;

      db.Subscribes.Update(sub);
      db.SaveChanges();

      if (model.Password != null && model.Password == model.ConfirmPassword && model.Password.Length >= 3)
      {
        currentuser.PasswordHash = CryptoHelper.GetMD5(model.Password);
      }
      currentuser.Login = model.Login;
      currentuser.name = model.Name;
      db.Users.Update(currentuser);
      db.SaveChanges();

      // создаем вьюмодель для текущего юзера и подписки
      ManageAccountViewModel maVM = new ManageAccountViewModel(currentuser, sub);
      return View(maVM);
    }
  }
}