using System.Windows;
using System.Windows.Input;
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
            this.WindowState = WindowState.Maximized;
            MyCanvas.KeyDown += CanvasKeyDown;
            game = new Manager(gameWindow, MyCanvas);

            game.StartGame();
            
            //log.GameSetUp();
            //
            

        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            game.CanvasKeyDown(sender, e);
        }

    }
}
