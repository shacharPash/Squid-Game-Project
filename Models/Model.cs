using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Models
{
    public class Model
    {
        public Model() => body = new Rectangle();
        public int speed { get; set; }
        public Rectangle body { get; set; }

        public virtual Rect GetRect(int r = 0)
        {
            return new Rect(Canvas.GetLeft(body) - r, Canvas.GetTop(body) - r, body.Width + r, body.Height + r);
        }


        //public ImageBrush image { get; set; }
    }
}
