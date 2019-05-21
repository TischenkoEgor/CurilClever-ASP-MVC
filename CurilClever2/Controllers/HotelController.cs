﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.Utils;
using CurilClever2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurilClever2.Controllers
{
  [Authorize(Roles = "Admin, Moderator, Manager")]
  public class HotelController : Controller
  {
    private CleverDBContext db;
    public HotelController(CleverDBContext _db)
    {
      db = _db;
    }
    public IActionResult Index()
    {
      return View(db.Hotels.OrderByDescending(x => x.id).ToList());
    }
    [HttpGet]
    public IActionResult CreateHotel()
    {
      return View();
    }
    [HttpPost]
    public IActionResult CreateHotel(Hotel hotel)
    {
      try
      {
        db.Hotels.Add(hotel);
        db.SaveChanges();
        News news = new News();
        news.TextShort = "добавлен новый отель";
        news.TextFull = "В базу данных добавлен новый отель!Его параметры: <br>";
        news.TextFull += "Название " + hotel.Name + "<br>";
        news.TextFull += "Адрес " + hotel.Addres + "<br>";
        news.TextFull += "Звезд " + hotel.StarsRate + "<br>";
        news.TextFull += "Стоимость ночи " + hotel.Price + "<br>";
        news.TextFull += "Дата добавления " + news.Created.ToLongDateString() + " " + news.Created.ToLongTimeString() + "<br>";
        news.ObjectUrl = "/Hotel/EditHotel/" + hotel.id.ToString();

        news.User = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();

        db.News.Add(news);
        db.SaveChanges();

        new Task((x) =>
        {
          MailSender.SendNewsToSubscribers(news, (List<Subscribe>)x);
        }, db.Subscribes.Include(x => x.User).ToList()).Start();

        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {

      }
      return View(hotel);
    }
    [HttpGet]
    public IActionResult EditHotel(int? id)
    {
      if (id == null)
        return RedirectToAction("index");
      Hotel hotel = db.Hotels.Where(h => h.id == id).FirstOrDefault();
      if(hotel == null)
        return RedirectToAction("index");

      return View(hotel);
    }
    [HttpPost]
    public IActionResult EditHotel(Hotel hotel)
    {
      db.Hotels.Update(hotel);
      db.SaveChanges();
      return RedirectToAction("Index");
    }
    public IActionResult DeleteHotel(int? id, int page)
    {
      if (id != null)
      {
        Hotel hotel = db.Hotels.Where(h => h.id == id).FirstOrDefault();
        if (hotel != null)
        {
          db.Hotels.Remove(hotel);
          db.SaveChanges();
        }
      }
      return RedirectToAction("GetTableOfHotels", new { page=page});
    }
    public IActionResult GetTableOfHotels(int page = 1)
    {
      int pageSize = 10;   // количество элементов на странице
      IQueryable<Hotel> source = db.Hotels;
      var count = source.Count();
      var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
      if (page > 1 && items.Count == 0)
      {
        items = source.Skip((page - 2) * pageSize).Take(pageSize).ToList();
      }
      PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
      HotelPageViewModel viewModel = new HotelPageViewModel
      {
        PageViewModel = pageViewModel,
        Hotels = items
      };
      return PartialView(viewModel);
    }
  }
}