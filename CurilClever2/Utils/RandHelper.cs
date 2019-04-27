using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Utils
{
  public class RandHelper<T>
  {
    public static T RandomItem(List<T> items, Random rand)
    {
      return items[rand.Next(0, items.Count - 1)];
    }
    public static string randomPhone(Random rand)
    {
      string result = "8 ";
      string[] operators = new string[] { "915", "916", "922", "777", "926", "912", "902", "903", "908" };
      result += RandHelper<string>.RandomItem(operators.ToList(), rand);
      result += " ";
      for (int i = 0; i < 3; i++)
        result += rand.Next(0, 9).ToString();
      result += " ";

      for (int i = 0; i < 2; i++)
        result += rand.Next(0, 9).ToString();

      result += " ";
      for (int i = 0; i < 2; i++)
        result += rand.Next(0, 9).ToString();

      return result;
    }
    public static string randomEmail(Random rand, string Name, string secondName)
    {
      string result = "";

      string[] providers = new string[] { "mail", "yandex", "gmail", "bk", "pochta", "yahoo", "bing", "gmail", "yandex", "ya", "mail" };
      string[] domens = new string[] { ".ru", ".com", ".org", ".net", ".msk", ".ua" };
      string[] separs = new string[] { "_", "-", ".", "", "", "." };
      if (rand.Next() % 2 == 0)
      {
        result += Transliteration.Front(Name);
        result += RandHelper<string>.RandomItem(separs.ToList(), rand);
        result += Transliteration.Front(secondName);
      }
      else
      {
        result += Transliteration.Front(secondName);
        result += RandHelper<string>.RandomItem(separs.ToList(), rand);
        result += Transliteration.Front(Name);
      }

      int litersToRemove = rand.Next(0, 6);

      List<char> _result = result.ToCharArray().ToList();

      for (int i = 0; i < litersToRemove; i++)
      {
        _result.RemoveAt(rand.Next(0, _result.Count - 1));
      }
      result = new string(_result.ToArray());
      result += "@";
      result += RandHelper<string>.RandomItem(providers.ToList(), rand);
      result += RandHelper<string>.RandomItem(domens.ToList(), rand);

      return result;
    }
    public static string randomPassoprt(Random rand)
    {
      string[] cityes = new string[] { "Москве", "Костроме", "Архангельску", "Санкт-Питербург", "Красноярску" };
      string[] region = new string[] { "Северный", "Южноный", "Верхний", "Нижний", "Центральный", "Заводской", "Правительственный", "Складской", "Мусорный", "Западный", "Портовой", "Бансковский" };
      string[] numbers = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
      DateTime startDate = new DateTime(2002, 1, 1);
      DateTime endDate = new DateTime(2018, 12, 31);
      int range = (endDate - startDate).Days;
      DateTime GiveDate = startDate.AddDays(rand.Next(range));
      string result = "Серия ";

      result += RandHelper<string>.RandomItem(numbers.ToList(), rand);
      result += RandHelper<string>.RandomItem(numbers.ToList(), rand);
      result += " ";
      result += RandHelper<string>.RandomItem(numbers.ToList(), rand);
      result += RandHelper<string>.RandomItem(numbers.ToList(), rand);
      result += " Номер ";
      for(int i = 0; i< 6; i++)
      {
        result += RandHelper<string>.RandomItem(numbers.ToList(), rand);
      }

      result += " Выдан отделением УФМС по гор. ";
      result += RandHelper<string>.RandomItem(cityes.ToList(), rand);
      result += " по району ";
      result += RandHelper<string>.RandomItem(region.ToList(), rand);
      result += " ";
      result += GiveDate.ToLongDateString();
      return result;
    }
  }
}