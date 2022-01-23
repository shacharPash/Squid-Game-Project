using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Models
{
    public class Goodie : Model
    {


        public bool stright { get; set; } //check if the image of the oodie is vertical stright and not horizontal
        public Goodie()
        {
            stright = true;
            body.Tag = "good";
            body.Width = 20;
            body.Height = 55;
            speed = 6;
            body.Fill = Brushes.Green;
        }

        public override Rect GetRect(int r = 0)
        {
            if (stright/*(body.RenderTransform as RotateTransform).Angle % 180 == 0*/) // in cases that the player in the starting position or 180 degrees upside down
                return new Rect(Canvas.GetLeft(body) - r, Canvas.GetTop(body) - r, body.Width + r, body.Height + r);
            return new Rect(Canvas.GetLeft(body) - r + body.Width / 2 - body.Height / 2, Canvas.GetTop(body) - r + body.Height / 2 - body.Width / 2, body.Height + r, body.Width + r);
        }
    }
}
