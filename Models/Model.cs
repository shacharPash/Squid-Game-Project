using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Models
{
    public class Model
    {
        public Model() => body = new Rectangle();
        public int speed { get; set; }
        public Rectangle body { get; set; }
        //public ImageBrush image { get; set; }
    }
}
