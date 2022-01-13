using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Models
{
    
    public class Coin : Model
    {
        public Coin(int v)
        {
            body.Width = body.Height = 40;
            value = v;
        }
        public int value { get; set; }
        
    }
}
