using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Drawing;
//using System.Windows.UI.Popups;
//using System.Drawing.Primitives.dll;

namespace Logic
{

    public class Manager
    {
        // Create all game objects
        public Canvas myCanvas;
        public Goodie goodPlayer;
        public Baddie[] Baddies = new Baddie[10];
        public int numOfBaddis = 10;
        public List<Coin> coinsList = new List<Coin>();
        //public Coin[] coins = new Coin[20]; // maximum 20 coin on board

        Rect rect1; // for isTouch function
        Rect rect2;

        //int TickCounter = 0;

        public Label txtScore = new Label();
        public Button saveBtn = new Button();

        public int score = 0;
        Random rnd = new Random();
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool goLeft, goRight, goUp, goDown, jump;
        bool noLeft, noRight, noUp, noDown;

        public Manager(Canvas c) //Ctor
        {
            myCanvas = c;
            goodPlayer = new Goodie();
            for (int i = 0; i < Baddies.Length; i++)
            {
                Baddies[i] = new Baddie();
            }
        }




        public void canvasKeyDown(object sender, KeyEventArgs e)
        {
            //  firstAct = false;
            myCanvas.Focus();
            gameTimer.Start();
            if (e.Key == Key.J) //player jump
            {
                jump = true;
            }
            if (e.Key == Key.Left && noLeft == false) // player go left
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                goodPlayer.body.RenderTransform = new RotateTransform(90, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false) // player go right
            {
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;

                goRight = true;
                goodPlayer.body.RenderTransform = new RotateTransform(-90, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2); ;

            }

            if (e.Key == Key.Up && noUp == false) // goodPlayer.body go up
            {
                noLeft = noRight = noDown = false;
                goLeft = goRight = goDown = false;

                goUp = true;
                goodPlayer.body.RenderTransform = new RotateTransform(-180, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2); ;

            }

            if (e.Key == Key.Down && noDown == false) // goodPlayer.body go down
            {
                noLeft = noRight = noUp = false;
                goLeft = goRight = goUp = false;
                goDown = true;
                goodPlayer.body.RenderTransform = new RotateTransform(0, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2); ;

            }

        }

        public void GameSetUp()
        {
            myCanvas.Focus();
            SetGameObjects();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);
            //gameTimer.Start();
            //currentBadStep = badMoveStep;
            SetGameImages();
        }

        private void SetGameObjects()
        {
            //Set save button:
            myCanvas.Children.Add(saveBtn);
            saveBtn.Height = saveBtn.Width = 75;
            saveBtn.Content = "Save";
            saveBtn.FontSize = 30;
            //saveBtn.Background
            saveBtn.FontFamily = new FontFamily("MV Boli");
            Canvas.SetTop(saveBtn, 50);
            Canvas.SetLeft(saveBtn, 0);
            saveBtn.Click += (sender, args) => gameTimer.Stop();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            //Application.Current.MainWindow.Width;



            //Set goodie on canvas
            myCanvas.Children.Add(goodPlayer.body);
            Canvas.SetLeft(goodPlayer.body, 75);
            Canvas.SetTop(goodPlayer.body, 75);
            goodPlayer.body.Width = 40;
            goodPlayer.body.Height = 40;
            goodPlayer.speed = 6;


            //Set baddies on canvas
            int w = (int)Application.Current.MainWindow.Width;
            int h = (int)Application.Current.MainWindow.Height;

            for (int i = 0; i < Baddies.Length; i++)
            {
                myCanvas.Children.Add(Baddies[i].body);
                Canvas.SetLeft(Baddies[i].body, rnd.Next(150, w - 100));
                Canvas.SetTop(Baddies[i].body, rnd.Next(150, h - 100));
                Baddies[i].body.Width = 50;
                Baddies[i].body.Height = 50;
                //Baddies[i].body.Tag = "bad";
                Baddies[i].speed = rnd.Next(1, 3);
                Baddies[i].xDitection = Baddies[i].yDitection = 1;

            }

            //Set score text line:
            myCanvas.Children.Add(txtScore);
            txtScore.FontSize = 30;
            txtScore.FontFamily = new FontFamily("MV Boli");
            txtScore.Content = "Score: " + score;

        }

        public void SetGameImages()
        {



            // Set background:
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/Background3.jpg", UriKind.RelativeOrAbsolute));
            myCanvas.Background = backgroundImage;

            // Delete button background
            saveBtn.BorderBrush = Brushes.Transparent;
            saveBtn.BorderThickness = new Thickness(0);
            saveBtn.Background = Brushes.Transparent;

            // Set player image:
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/player456JustHead.png", UriKind.RelativeOrAbsolute));
            goodPlayer.body.Fill = playerImage;

            ImageBrush bad0Image = new ImageBrush();
            bad0Image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/bad0o.png", UriKind.RelativeOrAbsolute));

            ImageBrush bad3Image = new ImageBrush();
            bad3Image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/bad3o.png", UriKind.RelativeOrAbsolute));

            ImageBrush badGirlImage = new ImageBrush();
            badGirlImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/bad4o.png", UriKind.RelativeOrAbsolute));

            ImageBrush mostBadImage = new ImageBrush();
            mostBadImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/mostBad.png", UriKind.RelativeOrAbsolute));

            //Set Baddies images:
            Baddies[0].body.Fill = Baddies[3].body.Fill = Baddies[6].body.Fill = bad0Image;
            Baddies[1].body.Fill = Baddies[4].body.Fill = Baddies[7].body.Fill = bad3Image;
            Baddies[2].body.Fill = Baddies[5].body.Fill = Baddies[8].body.Fill = badGirlImage;
            Baddies[9].body.Fill = mostBadImage;


            ////goodPlayer.body.SetValue()
            //ImageBrush backgroundImage = new ImageBrush();
            //backgroundImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/sgBackground.jpg", UriKind.RelativeOrAbsolute));
            //myCanvas.Background = backgroundImage; //set background        

            //ImageBrush playerImage = new ImageBrush();
            //playerImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/player456.png", UriKind.RelativeOrAbsolute));
            //goodPlayer.body.Fill = playerImage; //set player image

            //Baddies[0].image = new ImageBrush();
            //Baddies[0].image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/bad0.png", UriKind.RelativeOrAbsolute));

            //Baddies[1].image = new ImageBrush();
            //Baddies[1].image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/bad3.png", UriKind.RelativeOrAbsolute));

            //Baddies[2].image = new ImageBrush();
            //Baddies[2].image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/badGirl.png", UriKind.RelativeOrAbsolute));

            //Baddies[9].image = new ImageBrush();
            //Baddies[9].image.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/mostBad.png", UriKind.RelativeOrAbsolute));

            ////set Baddies images:
            //Baddies[3].body.Fill = Baddies[6].body.Fill = Baddies[0].body.Fill = Baddies[0].image;
            //Baddies[4].body.Fill = Baddies[7].body.Fill = Baddies[1].body.Fill = Baddies[1].image;
            //Baddies[5].body.Fill = Baddies[8].body.Fill = Baddies[2].body.Fill = Baddies[2].image;


        }

        public void GameLoop(object sender, EventArgs e)
        {
            //if (firstAct) gameTimer.Stop();
            txtScore.Content = "Score: " + score;
            CreateCoins();
            PlayerMove();
            BaddiesMove();
            winCheck();
        }
        private async void CreateCoins()
        {

            if (rnd.Next(0, 75) == 25) //chance of 1/75
            {
          
                
                Coin coin;
                ImageBrush coinImage = new ImageBrush();
                switch (rnd.Next(0, 4)) // set coin image and value
                {
                    default: //case 0
                        coin = new Coin(1);
                        coinImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/Coin0.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        coin = new Coin(3);
                        coinImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/Coin3.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        coin = new Coin(5);
                        coinImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/Coin5.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        coin = new Coin(7);
                        coinImage.ImageSource = new BitmapImage(new Uri("C:/Users/spash/Desktop/לוחמים להייטקס/SGproject/Logic/images/CoinUmbrella.png", UriKind.RelativeOrAbsolute));
                        break;
                }


                coin.body.Fill = coinImage;
                Canvas.SetLeft(coin.body, rnd.Next(50, (int)Application.Current.MainWindow.Width - 100));
                Canvas.SetTop(coin.body, rnd.Next(50, (int)Application.Current.MainWindow.Height - 100));
                myCanvas.Children.Add(coin.body);
                coinsList.Add(coin);
            }


            

        }
        private void BaddiesMove()
        {
            for (int i = 0; i < Baddies.Length; i++)
            {
                //if(gameTimer < 200)
                //if (Baddies[i].body.IsVisible && (Canvas.GetLeft(Baddies[i].body) + 90 > Application.Current.MainWindow.Width
                //    || Canvas.GetLeft(Baddies[i].body) - 50 < 0) || rnd.Next(0, 25) % 25 == 0) //change diraction in case the baddie touches the left-right edge or randomly
                //    Baddies[i].xDitection *= -1;

                //if (Baddies[i].body.IsVisible && (Canvas.GetTop(Baddies[i].body) + 90 > Application.Current.MainWindow.Height
                //    || Canvas.GetTop(Baddies[i].body) - 50 < 0) || rnd.Next(0, 25) % 25 == 0) //change diraction in case the baddie touches the top-bottom edge, or randomly 
                //    Baddies[i].yDitection *= -1;
                Canvas.SetLeft(Baddies[i].body, Canvas.GetLeft(Baddies[i].body) + (Baddies[i].speed * Baddies[i].xDitection));
                Canvas.SetTop(Baddies[i].body, Canvas.GetTop(Baddies[i].body) + (Baddies[i].speed * Baddies[i].yDitection));
                if (Baddies[i].body.IsVisible)
                {
                    Baddies[i].xDitection = (Canvas.GetLeft(Baddies[i].body) > Canvas.GetLeft(goodPlayer.body) ? -1 : 1);
                    Baddies[i].yDitection = (Canvas.GetTop(Baddies[i].body) > Canvas.GetTop(goodPlayer.body) ? -1 : 1);
                }




                //else if (Baddies[i].IsVisible && Canvas.GetLeft(Baddies[i]) - 50 < 0)
                //    Canvas.SetLeft(Baddies[i], Canvas.GetLeft(Baddies[i]) + (playerSpeed / 1.5));

                CollitionCheck(i);

            }
        }

        private void CollitionCheck(int i)
        {
            // Check if the player touches the baddies:
            if (isTouch(Baddies[i].body, goodPlayer.body))
                GameOver($"You failed, your score is: {score}");

            // chech if the baddies touch each other:
            for (int j = i + 1; j < Baddies.Length; j++)

                if (isTouch(Baddies[j].body, Baddies[i].body))
                {
                    Baddies[j].body.Visibility = Visibility.Collapsed; //Delete one baddie
                    ++score;
                    --numOfBaddis;
                }
        }

        public bool isTouch(Rectangle a, Rectangle b)
        {
            if (a.IsVisible && b.IsVisible)
            {
                rect1 = new Rect(Canvas.GetLeft(a) + 10, Canvas.GetTop(a) + 10, a.Width - 10, a.Height - 10);
                rect2 = new Rect(Canvas.GetLeft(b) + 10, Canvas.GetTop(b) + 10, b.Width - 10, b.Height - 10);
                return (rect1.IntersectsWith(rect2));
            }
            return false;
            //return false;
            //return (Rectangle.Intersect(b, a) != Rectangle.Empty);
            //return (a.IsVisible && b.IsVisible && Math.Abs((Canvas.GetLeft(a) - Canvas.GetLeft(b))) < Math.Min(a.Width,b.Width) 
            //  && Math.Abs((Canvas.GetTop(a) - Canvas.GetTop(b))) < Math.Min(a.Height,b.Height));
        }

        public void PlayerMove()
        {
            if (jump)
            {
                Canvas.SetLeft(goodPlayer.body, rnd.Next(50, (int)Application.Current.MainWindow.Width - 50));
                Canvas.SetTop(goodPlayer.body, rnd.Next(50, (int)Application.Current.MainWindow.Height - 50));
                jump = false;
            }
            if (goRight)
            {
                Canvas.SetLeft(goodPlayer.body, Canvas.GetLeft(goodPlayer.body) + goodPlayer.speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(goodPlayer.body, Canvas.GetLeft(goodPlayer.body) - goodPlayer.speed);
            }
            if (goUp)
            {
                Canvas.SetTop(goodPlayer.body, Canvas.GetTop(goodPlayer.body) - goodPlayer.speed);
            }
            if (goDown)
            {
                Canvas.SetTop(goodPlayer.body, Canvas.GetTop(goodPlayer.body) + goodPlayer.speed);
            }

            if (goDown && Canvas.GetTop(goodPlayer.body) + 90 > Application.Current.MainWindow.Height)
            {
                noDown = true;
                goDown = false;
            }

            if (goUp && Canvas.GetTop(goodPlayer.body) < 5)
            {
                noUp = true;
                goUp = false;
            }

            if (goLeft && Canvas.GetLeft(goodPlayer.body) < 5)
            {
                noLeft = true;
                goLeft = false;

            }

            if (goRight && Canvas.GetLeft(goodPlayer.body) + 65 > Application.Current.MainWindow.Width)
            {
                noRight = true;
                goRight = false;
            }



            foreach (var c in coinsList)
            {
                if (isTouch(c.body, goodPlayer.body))
                {
                    c.body.Visibility = Visibility.Collapsed;
                    score += c.value;
                }
            }


        }

        public void winCheck()
        {

            //for (int i = 0; i < Baddies.Length; i++)
            //    if (Baddies[i].body.IsVisible && !oneBaddieLeft)
            //        oneBaddieLeft = true;

            if (numOfBaddis <= 1) GameOver($"You win! your score is: {score}");
        }
        public void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBox.Show(message, "Front man says");

            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

        }
    }
}
