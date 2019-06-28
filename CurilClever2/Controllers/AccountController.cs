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

    private string VkAuthCallBackUrl = "";

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
      VkAuthCallBackUrl = "http://";
      VkAuthCallBackUrl += HttpContext.Request.Host;
      VkAuthCallBackUrl += "/account/vkauth";

      ViewData["callbackurl"] = VkAuthCallBackUrl;
      return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
      VkAuthCallBackUrl = "http://";
      VkAuthCallBackUrl += HttpContext.Request.Host;
      VkAuthCallBackUrl += "/account/vkauth";

      ViewData["callbackurl"] = VkAuthCallBackUrl;

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

    public async Task<IActionResult> VkAuth(string code = "")
    {
      VkAuthCallBackUrl = "http://";
      VkAuthCallBackUrl += HttpContext.Request.Host; //название домена, берется из ссылки
      VkAuthCallBackUrl += "/account/vkauth";

      string secret_key = "ftOs39sriOsLSZEhTOSd";
      string VkApp_ID = "7020055";

      // переменные для данных от вконтакте
      string access_token = "";
      string expires_in = "";
      int user_id;
      string email = "";

      // авторизация в два этапа сначала получаем код доступа
      // потом получаем токен

      // урл для запроса токена доступа из API Vkontakte 
      string request = "https://oauth.vk.com/access_token";
      request += "?client_id=" + VkApp_ID;
      request += "&client_secret=" + secret_key;
      // адрес коллбэка нужен только для того, что бы ВК сравнил его с прошлым коллбэком 
      // и убедился что этот запрос делаем мы, а не левый верблюд, который мимо проходил
      request += "&redirect_uri=" + VkAuthCallBackUrl;
      request += "&code=" + code;

      // загружаем коментарии в формате JSON
      string json_string = new WebClient().DownloadString(request);
      // распаковываем JSON в соотвествующий объект
      VkAccessTokenData data = JsonConvert.DeserializeObject<VkAccessTokenData>(json_string);

      // получаем данные из JSON объекта
      access_token = data.access_token;
      expires_in = data.expires_in.ToString();
      user_id = data.user_id;
      email = data.email;

      // проверяем есть ли уже в базе пользователь с такой почтой
      User existuser = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Login == email);
      if(existuser != null)
      {
        // если такой пользователь нашелся
        // авторизуем пользователя
        await Authenticate(existuser); 
        // отправляем его домой
        return RedirectToAction("Index", "Home");
      }

      // если пользователя с такой почтой нет, то СОЗДАЕМ НоооОВОГО В НАШЕЙ БАЗЕ блэт
      // получаем остальнгые даные пользователя (имя)

      string first_name = "";
      string last_name = "";

      // урл для запроса данных из API Vkontakte 
      string api_url = "https://api.vk.com/method/users.get?user_id=" + user_id + "&v=5.59&access_token=" + access_token;
      // загружаем коментарии в формате JSON
      json_string = new WebClient().DownloadString(api_url);
      // распаковываем JSON в объект
      VkusersData json = JsonConvert.DeserializeObject<VkusersData>(json_string);

      // получаем данные из JSON объекта
      first_name = json.response[0].first_name;
      last_name = json.response[0].last_name;

      // создаем нового юзера и заполняем его поля
      User newuser = new User
      {
        name = first_name + " " + last_name,
        Login = email,
        AccessLevel = 9000,
        PasswordHash = CryptoHelper.GetMD5("ololololololokek"),
        Role = db.Roles.Where(r => r.Name.Contains("DefaultUser")).FirstOrDefault()
      };

      // добавлчем его в базу
      db.Users.Add(newuser);
      // сохраняем изменения в базе
      db.SaveChanges();

      // и авторизуем пользователя
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
      var x = User.Identity;
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