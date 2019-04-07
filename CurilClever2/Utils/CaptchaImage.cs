using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CurilClever2.Utils
{
  public class CaptchaImage
  {
    private string text; // текст капчи
    private int width; // ширина картинки
    private int height; // высота картинки
    public Bitmap Image { get; set; } // само изображение капчи

    public CaptchaImage(string s, int width, int height)
    {
      text = s;
      this.width = width;
      this.height = height;
      GenerateImage();
    }
    // создаем изображение
    private void GenerateImage()
    {
      Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

      Graphics g = Graphics.FromImage(bitmap);
      // отрисовка строки
      List<Color> colorlist = new List<Color>();
      colorlist.Add(Color.White);
      colorlist.Add(Color.Red);
      colorlist.Add(Color.RosyBrown);
      colorlist.Add(Color.Green);
      colorlist.Add(Color.Yellow);
      colorlist.Add(Color.Silver);


      Random rand = new Random(Environment.TickCount);
      int base_x_shift = width / text.Length;
      for (int i = 0; i < text.Length; i++)
      {
        int shift_x = rand.Next(-base_x_shift / 8, base_x_shift / 8);
        int shift_y = rand.Next(0, 15);
        int font_Size = rand.Next((height - shift_y) * 3 / 8, (height - shift_y) * 2 / 3);
        Color randcolor = colorlist[rand.Next(0, colorlist.Count - 1)];
        g.DrawString(text[i].ToString(), new Font("Arial", font_Size, FontStyle.Bold),
                          new SolidBrush(randcolor), new RectangleF(i * base_x_shift + shift_x, shift_y, base_x_shift, height - shift_y));
      }
      for (int i = 0; i < 3; i++)
      {
        float randWidth = rand.Next(10, 50) / 10f;
        Color randcolor = colorlist[rand.Next(0, colorlist.Count - 1)];
        Pen randPen = new Pen(randcolor, randWidth);
        int rand_x1 = rand.Next(0, width / 4);
        int rand_x2 = rand.Next(width * 3/4, width);
        int rand_y1 = rand.Next(0, height);
        int rand_y2 = rand.Next(0, height);
        PointF p1 = new PointF(rand_x1, rand_y1);
        PointF p2 = new PointF(rand_x2, rand_y2);
        g.DrawLine(randPen, p1, p2);
      }
      g.Dispose();

      Image = bitmap;
    }

    ~CaptchaImage()
    {
      Dispose(false);
    }

    public void Dispose()
    {
      GC.SuppressFinalize(this);
      Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
        Image.Dispose();
    }
  }
}
