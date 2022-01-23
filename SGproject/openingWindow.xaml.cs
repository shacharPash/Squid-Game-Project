using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace SGproject
{
    /// <summary>
    /// Interaction logic for openingWindow1.xaml
    /// </summary>
    public partial class openingWindow1 : Window
    {
        public openingWindow1()
        {
            InitializeComponent();

            openingWindow.WindowState = WindowState.Maximized;

            //Set background:
            ImageBrush openingBackgroundImage = new ImageBrush();
            openingBackgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/openingBackground2.jpg", UriKind.RelativeOrAbsolute));
            openingGrid.Background = openingBackgroundImage;

            SoundPlayer PinkSoldierSound = new SoundPlayer("../../sounds/dramaticOpeningSound.wav");
            PinkSoldierSound.Play();

            //Create StackPannel for the buttons
            StackPanel btnstackPanel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // create buttons:
            Button start = new Button();
            Button exit = new Button();
            Button rules = new Button();

            //add buttons to stack panel:
            btnstackPanel.Children.Add(rules);
            btnstackPanel.Children.Add(start);
            btnstackPanel.Children.Add(exit);

            //Add Stack panel to Grid:
            openingGrid.Children.Add(btnstackPanel);


            //design the buttons
            start.Content = "Start";
            exit.Content = "Exit";
            rules.Content = "Rules";
            rules.Height = start.Height = exit.Height = 100;
            rules.Width = start.Width = exit.Width = 300;
            rules.FontSize = exit.FontSize = 50;
            start.FontSize = 75;
            rules.FontFamily = start.FontFamily = exit.FontFamily = new FontFamily("MV Boli");
            rules.Foreground = start.Foreground = exit.Foreground = Brushes.White;


            //delete buttons Background:
            rules.BorderBrush = start.BorderBrush = exit.BorderBrush = Brushes.Transparent;
            rules.BorderThickness = start.BorderThickness = exit.BorderThickness = new Thickness(0);
            rules.Background = start.Background = exit.Background = Brushes.Transparent;

            //to see the text when the button changes color:
            start.MouseEnter += (sender, args) => start.Foreground = Brushes.Black;
            rules.MouseEnter += (sender, args) => rules.Foreground = Brushes.Black;
            exit.MouseEnter += (sender, args) => exit.Foreground = Brushes.Black;

            start.MouseLeave += (sender, args) => start.Foreground = Brushes.White;
            rules.MouseLeave += (sender, args) => rules.Foreground = Brushes.White;
            exit.MouseLeave += (sender, args) => exit.Foreground = Brushes.White;

            //define Click fonctions:           

            RulesWindow rulesWindow = new RulesWindow();// create only one rules window 
            rules.Click += (sender, args) => { rulesWindow.Show(); };

            start.Click += (sender, args) =>
            {
                PinkSoldierSound.Stop();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                openingWindow.Close();
                rulesWindow.Close();

            };

            exit.Click += (sender, args) => Application.Current.Shutdown();



            //savedGames.Click += (sender, args) =>
            //{
            //    if (System.IO.File.Exists("file.txt"))
            //    {
            //        string xaml = System.IO.File.ReadAllText("file.txt");
            //        object content = System.Windows.Markup.XamlReader.Parse(xaml);

            //        MainWindow mainWindow = new MainWindow();
            //        mainWindow.Content =  content;
            //        mainWindow.Show();
            //        openingWindow.Close();

            //        //MainWindow m = new MainWindow();
            //        //foreach (var x in m.MyCanvas.Children)
            //        // mainWindow.MyCanvas.Children.Add((UIElement)x);

            //        //mainWindow.MyCanvas.KeyDown += MyCanvas_KeyDown;
            //        //mainWindow.Show();

            //        //mainWindow.Focus();

            //    }
            //};




            //openingGrid.Children[0] = savedGames;
            //openingGrid.Children[1] = start;
            //openingGrid.Children[2] = exit;

            rules.HorizontalAlignment = HorizontalAlignment.Left;
            //openingGrid.HorizontalAlignment = HorizontalAlignment.Center;
            //Canvas.SetLeft(start, (Canvas.GetLeft(exit) + Canvas.GetLeft(savedGames)) / 2);


            //savedGames.HorizontalAlignment = HorizontalAlignment.Left;
            //start.HorizontalAlignment = HorizontalAlignment.Center;
            //exit.HorizontalAlignment = HorizontalAlignment.Right;













        }



        //private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
