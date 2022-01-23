using System.Windows.Media;

namespace Models
{
    public class Baddie: Model
    {
        public Baddie()
        {
            body.Tag = "bad";
            body.Width = 50;
            body.Height = 50;
            body.Fill = Brushes.Red;
        }
        public int xDitection { get; set; }
        public int yDitection { get; set; }
    }
}
