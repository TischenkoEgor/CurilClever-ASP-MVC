using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using reCAPTCHA.AspNetCore;

namespace CurilClever2
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      string connection = Configuration.GetConnectionString("DefaultConnection");
      services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));
      services.AddTransient<IRecaptchaService, RecaptchaService>();
      services.AddTransient<IStringLocalizer, SiteStringLocalizer>();

      services.AddLocalization(options => options.ResourcesPath = "Resources");

      services.AddDbContext<CleverDBContext>(options => options.UseSqlServer(connection));
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options => //CookieAuthenticationOptions
              {
                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
              });
      services.Configure<CookiePolicyOptions>(options =>
        {
          // This lambda determines whether user consent for non-essential cookies is needed for a given request.
          options.CheckConsentNeeded = context => true;
          options.MinimumSameSitePolicy = SameSiteMode.None;
        });
      services.Configure<RequestLocalizationOptions>(options =>
      {
        var supportedCultures = new[]
        {
          new CultureInfo("en-US"),
          new CultureInfo("ru-RU"),
        };

        options.DefaultRequestCulture = new RequestCulture("ru");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
      });


      services.AddMvc()
        .AddViewLocalization() // добавляем локализацию представлений
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }
      // конфигурируем доступные языковые культуры
      var supportedCultures = new[]
      {
          new CultureInfo("en-US"),
          new CultureInfo("ru-RU"),
      };
      // подключаем сервис отвечающий за отслеживания выбранного языка локализации
      app.UseRequestLocalization(new RequestLocalizationOptions
      {
        // культура по умолчанию
        DefaultRequestCulture = new RequestCulture("ru-RU"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
      });
      app.UseStaticFiles();
      app.UseCookiePolicy();
      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                   name: "areas",
                   template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
