using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Logic;

namespace SGproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        Manager game;
        public MainWindow()
        {
            //this.WindowState = WindowState.Maximized;
            
            InitializeComponent();
            
            Canvas c = MyCanvas;
            game = new Manager(c);
            game.GameSetUp();
            //log.GameSetUp();        
             
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            game.canvasKeyDown(sender, e);
        }

    }
}
