using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class PageViewModel
  {
    public int PageNumber { get; private set; }
    public int TotalPages { get; private set; }
    public PageViewModel()
    {

    }
    /// <summary>
    /// Создает новый объект типа pageViewModel 
    /// </summary>
    /// <param name="count"> количество повторяющихся элементов, которые предполагается размещать на страницах</param>
    /// <param name="pageNumber"> номер текущей страницы</param>
    /// <param name="pageSize"> количество повторяющихся элементов на одной странице</param>
    public PageViewModel(int count, int pageNumber, int pageSize)
    {
      PageNumber = pageNumber;
      // получаем колчество страниц округлением до наименьшего целого в сторону увеличения
      TotalPages = (int) (Math.Ceiling(count / (double)pageSize));
    }

    public bool HasPreviousPage
    {
      get
      {
        return (PageNumber > 1);
      }
    }

    public bool HasNextPage
    {
      get
      {
        return (PageNumber < TotalPages);
      }
    }

  }
}
