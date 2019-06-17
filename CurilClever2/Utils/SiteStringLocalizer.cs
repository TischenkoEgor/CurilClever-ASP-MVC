﻿using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2
{
  public class SiteStringLocalizer : IStringLocalizer
  {
    Dictionary<string, Dictionary<string, string>> resources;
    // ключи ресурсов
    const string HEADER = "Header";
    const string MESSAGE = "Message";
    const string Male = "Male";
    const string Female = "Female";

    public SiteStringLocalizer  ()
    {
      // словарь для английского языка
      Dictionary<string, string> enDict = new Dictionary<string, string>
      {
        {HEADER, "Welcome" },
        {MESSAGE, "Hello World!" },
        {Male, "Male" },
        {Female, "Female" }
      };
      // словарь для русского языка
      Dictionary<string, string> ruDict = new Dictionary<string, string>
      {
        {HEADER, "Добо пожаловать" },
        {MESSAGE, "Привет мир!" },
        {Male, "Муж." },
        {Female, "Жен." }
      };
     
      // создаем словарь ресурсов
      resources = new Dictionary<string, Dictionary<string, string>>
      {
        {"en", enDict },
        {"ru", ruDict }
      };
    }
    // по ключу выбираем для текущей культуры нужный ресурс
    public LocalizedString this[string name]
    {
      get
      {
        var currentCulture = CultureInfo.CurrentUICulture;
        string val = "";
        if (resources.ContainsKey(currentCulture.Name))
        {
          if (resources[currentCulture.Name].ContainsKey(name))
          {
            val = resources[currentCulture.Name][name];
          }
        }
        return new LocalizedString(name, val);
      }
    }

    public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
      throw new NotImplementedException();
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
      return this;
    }
  }

}
