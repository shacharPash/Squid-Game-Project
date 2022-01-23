using Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


//using System.Windows.UI.Popups;
//using System.Drawing.Primitives.dll;

namespace Logic
{

    public class Manager
    {
        //Window myWindow;
        // Create all game objects
        Canvas myCanvas;
        Goodie goodPlayer;
        Baddie[] Baddies = new Baddie[10];
        int numOfBaddis;
        List<Coin> coinsList = new List<Coin>();

        MediaPlayer Back = new MediaPlayer();
        MediaPlayer Front = new MediaPlayer();
        MediaPlayer Shoot = new MediaPlayer();

        // for isTouch function:
        Rect rect1;
        Rect rect2;


        StackPanel btnstackPanel = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            VerticalAlignment = VerticalAlignment.Top,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        Label txtScore = new Label();
        int score;
        Button pauseBtn = new Button();
        Button restartBtn = new Button();

        Random rnd = new Random();
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool goLeft, goRight, goUp, goDown, jump;
        bool noLeft, noRight, noUp, noDown;

        bool firstGame = true;

        public Manager(Window w, Canvas c) //Ctor
        {
            //myWindow = w;
            myCanvas = c;
            goodPlayer = new Goodie();
            for (int i = 0; i < Baddies.Length; i++)
            {
                Baddies[i] = new Baddie();
            }

        }

        #region Set Up Game
        public void StartGame()
        {
            myCanvas.Focus();
            //StopAllPlayers();//dont foget to delete
            SetGameObjects();
            SetGameImages();

            if (firstGame) //make sure the GameLoop add to gameTimer.Tick only in the first game.
            {
                gameTimer.Tick += GameLoop;
                firstGame = false;
            }
            gameTimer.Interval = TimeSpan.FromMilliseconds(15);

            //PlayBadGirlSound();
            //Thread.Sleep(5000);
            PlayBackgroundMusic();

        }
        private void SetGameObjects()
        {
            score = 0;
            numOfBaddis = 10;

            myCanvas.Children.Add(btnstackPanel);

            //Set score text line:
            btnstackPanel.Children.Add(txtScore);
            txtScore.FontSize = 30;
            txtScore.FontFamily = new FontFamily("MV Boli");
            txtScore.Content = "Score: " + score;

            //Set Pause button:
            btnstackPanel.Children.Add(pauseBtn);
            pauseBtn.Width = 150;
            pauseBtn.Height = 75;
            pauseBtn.Content = "Pause";
            pauseBtn.FontSize = 30;
            pauseBtn.FontFamily = new FontFamily("MV Boli");
            //Canvas.SetTop(pauseBtn, 50);
            //Canvas.SetLeft(pauseBtn, 0);
            pauseBtn.Click += (sender, args) =>
            {
                if (pauseBtn.Content.ToString() == "Pause" && gameTimer.IsEnabled)
                {
                    gameTimer.Stop();
                    pauseBtn.Content = "Resume";
                }
                else if (pauseBtn.Content.ToString() == "Resume")
                {
                    pauseBtn.Content = "Pause";
                    gameTimer.Start();
                }
            };

            //Set Restart button:
            btnstackPanel.Children.Add(restartBtn);
            restartBtn.Width = 150;
            restartBtn.Height = 75;
            restartBtn.Content = "Restart";
            restartBtn.FontSize = 30;
            restartBtn.FontFamily = new FontFamily("MV Boli");
            //Canvas.SetTop(pauseBtn, 50);
            //Canvas.SetLeft(pauseBtn, 0);
            restartBtn.Click += (sender, args) => RertartGame();

            // Delete buttons background
            pauseBtn.BorderBrush = restartBtn.BorderBrush = Brushes.Transparent;
            pauseBtn.BorderThickness = restartBtn.BorderThickness = new Thickness(0);
            pauseBtn.Background = restartBtn.Background = Brushes.Transparent;


            //PLAYERS:
            int w = (int)Application.Current.MainWindow.Width;
            int h = (int)Application.Current.MainWindow.Height;

            //Set goodie on canvas:
            myCanvas.Children.Add(goodPlayer.body);
            Canvas.SetLeft(goodPlayer.body, rnd.Next(5, w - 100));
            Canvas.SetTop(goodPlayer.body, rnd.Next(5, h - 100));


            //set Baddies on canvas:
            for (int i = 0; i < Baddies.Length; i++)
            {
                Baddies[i].speed = rnd.Next(1, 3);
                Baddies[i].xDitection = Baddies[i].yDitection = 1;
                Canvas.SetLeft(Baddies[i].body, rnd.Next(100, w - 100));
                Canvas.SetTop(Baddies[i].body, rnd.Next(100, h - 100));
                myCanvas.Children.Add(Baddies[i].body);
                if (GetDistannse(Baddies[i].GetRect(), goodPlayer.GetRect()) < 200) //make sure the starting position of the goodPlayer doesn't collition with the new baddie
                {
                    myCanvas.Children.Remove(Baddies[i].body);
                    --i;
                    continue;
                }
                for (int j = 0; j < i; j++)
                {
                    if (GetDistannse(Baddies[i].GetRect(), Baddies[j].GetRect()) < 150) //make sure the starting position of the new baddie doesn't collition with the other baddies
                    {
                        myCanvas.Children.Remove(Baddies[i].body);
                        --i;
                        break;
                    }
                }
            }
        }

        private void SetGameImages()
        {



            // Set background:
            ImageBrush backgroundImage = new ImageBrush();
            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Background3.jpg"));
            myCanvas.Background = backgroundImage;

            // Set player image:
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player456.png"));
            goodPlayer.body.Fill = playerImage;

            ImageBrush bad0Image = new ImageBrush();
            bad0Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/bad0o.png"));

            ImageBrush bad3Image = new ImageBrush();
            bad3Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/bad3o.png"));

            ImageBrush badGirlImage = new ImageBrush();
            badGirlImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/bad4o.png"));

            ImageBrush mostBadImage = new ImageBrush();
            mostBadImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/mostBad.png"));

            //Set Baddies images:
            Baddies[0].body.Fill = Baddies[3].body.Fill = Baddies[6].body.Fill = bad0Image;
            Baddies[1].body.Fill = Baddies[4].body.Fill = Baddies[7].body.Fill = bad3Image;
            Baddies[2].body.Fill = Baddies[5].body.Fill = Baddies[8].body.Fill = badGirlImage;
            Baddies[9].body.Fill = mostBadImage;

        }
        #endregion
        private void GameLoop(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            CreateCoins();
            GoodieMove();
            BaddiesMove();
            CheckAllIntersects();
            WinCheck();
        }

        public void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            //firstAct = false;
            myCanvas.Focus();

            if (e.Key == Key.J) //player jump
            {
                gameTimer.Start();
                jump = true;
            }
            if (e.Key == Key.Left && noLeft == false) // player go left
            {
                gameTimer.Start();
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                goodPlayer.body.RenderTransform = new RotateTransform(90, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2);
                goodPlayer.stright = false;


            }

            if (e.Key == Key.Right && noRight == false) // player go right
            {
                gameTimer.Start();
                noLeft = noUp = noDown = false;
                goLeft = goUp = goDown = false;

                goRight = true;
                goodPlayer.body.RenderTransform = new RotateTransform(-90, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2);
                goodPlayer.stright = false;

            }

            if (e.Key == Key.Up && noUp == false) // goodPlayer.body go up
            {
                gameTimer.Start();
                noLeft = noRight = noDown = false;
                goLeft = goRight = goDown = false;

                goUp = true;
                goodPlayer.body.RenderTransform = new RotateTransform(-180, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2);
                goodPlayer.stright = true;

            }

            if (e.Key == Key.Down && noDown == false) // goodPlayer.body go down
            {
                gameTimer.Start();
                noLeft = noRight = noUp = false;
                goLeft = goRight = goUp = false;
                goDown = true;
                goodPlayer.body.RenderTransform = new RotateTransform(0, goodPlayer.body.Width / 2, goodPlayer.body.Height / 2); ;
                goodPlayer.stright = true;
            }

        }

        private void CreateCoins()
        {
            // creating new coin generation process.

            if (rnd.Next(0, 75) == 25) //chance of 1/75
            {


                Coin coin;
                ImageBrush coinImage = new ImageBrush();
                switch (rnd.Next(0, 4)) // set coin image and value
                {
                    default: //case 0
                        coin = new Coin(1);
                        coinImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Coin0.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 1:
                        coin = new Coin(3);
                        coinImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Coin3.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 2:
                        coin = new Coin(5);
                        coinImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/Coin5.png", UriKind.RelativeOrAbsolute));
                        break;
                    case 3:
                        coin = new Coin(7);
                        coinImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/CoinUmbrella.png", UriKind.RelativeOrAbsolute));
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
                Canvas.SetLeft(Baddies[i].body, Canvas.GetLeft(Baddies[i].body) + (Baddies[i].speed * Baddies[i].xDitection));
                Canvas.SetTop(Baddies[i].body, Canvas.GetTop(Baddies[i].body) + (Baddies[i].speed * Baddies[i].yDitection));
                if (Baddies[i].body.IsVisible)
                {
                    Baddies[i].xDitection = (Canvas.GetLeft(Baddies[i].body) > Canvas.GetLeft(goodPlayer.body) ? -1 : 1);
                    Baddies[i].yDitection = (Canvas.GetTop(Baddies[i].body) > Canvas.GetTop(goodPlayer.body) ? -1 : 1);
                }




                //else if (Baddies[i].IsVisible && Canvas.GetLeft(Baddies[i]) - 50 < 0)
                //    Canvas.SetLeft(Baddies[i], Canvas.GetLeft(Baddies[i]) + (playerSpeed / 1.5));


            }
        }
        private void GoodieMove()
        {
            if (jump)
            {
                Canvas.SetLeft(goodPlayer.body, rnd.Next(50, (int)Application.Current.MainWindow.Width - 50));
                Canvas.SetTop(goodPlayer.body, rnd.Next(50, (int)Application.Current.MainWindow.Height - 50));
                jump = false;
            }

            if (goDown && /*Canvas.GetTop(goodPlayer.body) + (goodPlayer.body.Height)*/ goodPlayer.GetRect().Y + goodPlayer.GetRect().Height + goodPlayer.speed > myCanvas.ActualHeight /*Application.Current.MainWindow.Height*/)
            {
                Canvas.SetTop(goodPlayer.body, myCanvas.ActualHeight - (goodPlayer.body.Height));
                noDown = true;
                goDown = false;
            }

            if (goUp && /*Canvas.GetTop(goodPlayer.body) - goodPlayer.speed*/ goodPlayer.GetRect().Y - goodPlayer.speed < 1)
            {
                Canvas.SetTop(goodPlayer.body, 0);
                noUp = true;
                goUp = false;
            }

            if (goLeft && /*Canvas.GetLeft(goodPlayer.body) - (goodPlayer.body.Height / 2)*/ goodPlayer.GetRect().X - goodPlayer.speed < 1)
            {
                Canvas.SetLeft(goodPlayer.body, goodPlayer.body.Height / 2 - goodPlayer.body.Width / 2);
                noLeft = true;
                goLeft = false;

            }

            if (goRight && /*Canvas.GetLeft(goodPlayer.body) + (goodPlayer.body.Height / 2)*/ goodPlayer.GetRect().X + goodPlayer.GetRect().Width + goodPlayer.speed > myCanvas.ActualWidth /*- 5*/ /*Application.Current.MainWindow.Width*/)
            {
                Canvas.SetLeft(goodPlayer.body, myCanvas.ActualWidth - (goodPlayer.body.Width + goodPlayer.body.Height) / 2); /*goodPlayer.body.ActualHeight + goodPlayer.body.ActualWidth - 2*/
                //Canvas.SetRight(goodPlayer.body, goodPlayer.body.ActualWidth + 200);
                //myCanvas.bin
                noRight = true;
                goRight = false;
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









        }

        private double GetDistannse(Rect rect1, Rect rect2)
        {
            return Math.Sqrt(Math.Pow((rect1.X + rect1.Width / 2) - (rect2.X + rect2.Width / 2), 2)
                           + Math.Pow((rect1.Y + rect1.Height / 2) - (rect2.Y + rect2.Height / 2), 2));
        }
        private void CheckAllIntersects()
        {
            foreach (var c in coinsList) //Check if the goodie touchs a coin
            {
                if (AreTouching(c, goodPlayer, -5))
                {
                    PlayCoinCollectSound();
                    c.body.Visibility = Visibility.Collapsed;
                    score += c.value;

                }
            }

            for (int i = 0; i < Baddies.Length; i++)
            {
                // Check if the player touches the baddies:
                if (AreTouching(Baddies[i], goodPlayer, -10))
                {
                    PlayLoseSound();
                    GameOver($"You lose, your score is: {score}");
                }
                for (int j = i + 1; j < Baddies.Length; j++) // chech if the baddies touch each other
                {
                    if (AreTouching(Baddies[j], Baddies[i], -10))
                    {
                        PlayShootSound();
                        Baddies[j].body.Visibility = Visibility.Collapsed; //Delete one baddie
                        ++score;
                        --numOfBaddis;
                    }
                }
            }
        }
        private bool AreTouching(Model a, Model b, int distance = 0)
        {
            if (a.body.IsVisible && b.body.IsVisible)
            {
                rect1 = a.GetRect(distance);
                rect2 = b.GetRect(distance);
                return (rect1.IntersectsWith(rect2));
            }
            return false; //else


            //if (a.IsVisible && b.IsVisible)
            //{
            //    rect1 = new Rect(Math.Max(Canvas.GetLeft(a) - radius, 0), Math.Max(Canvas.GetTop(a) - radius, 0),
            //                       Math.Min(a.Width + radius, myCanvas.ActualWidth - Canvas.GetLeft(a)), Math.Min(a.Height + radius, myCanvas.ActualHeight - Canvas.GetTop(a)));

            //    rect2 = new Rect(Math.Max(Canvas.GetLeft(b) - radius, 0), Math.Max(Canvas.GetTop(b) - radius, 0),
            //                     Math.Min(b.Width + radius, myCanvas.ActualWidth - Canvas.GetLeft(b)), Math.Min(b.Height + radius, myCanvas.ActualHeight - Canvas.GetTop(b)));
            //    return (rect1.IntersectsWith(rect2));
            //}
            //return false; //else

            //return false;
            //return (Rectangle.Intersect(b, a) != Rectangle.Empty);
            //return (a.IsVisible && b.IsVisible && Math.Abs((Canvas.GetLeft(a) - Canvas.GetLeft(b))) < Math.Min(a.Width,b.Width) 
            //  && Math.Abs((Canvas.GetTop(a) - Canvas.GetTop(b))) < Math.Min(a.Height,b.Height));
        }

        private void WinCheck()
        {

            //for (int i = 0; i < Baddies.Length; i++)
            //    if (Baddies[i].body.IsVisible && !oneBaddieLeft)
            //        oneBaddieLeft = true;

            if (numOfBaddis <= 1)
            {
                PlayWinSound();
                GameOver($"You win! your score is: {score}");
            }
        }
        private void GameOver(string message)
        {
            gameTimer.Stop();
            MessageBoxResult result = MessageBox.Show(message + "\n\nDo you want to try again?", "Front man says", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    RertartGame();
                    break;
                case MessageBoxResult.No:
                    Application.Current.Shutdown();
                    break;
            }

            //if (result == MessageBoxResult.Yes)
            //    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);




        }
        private void RertartGame()
        {
            gameTimer.Stop();
            btnstackPanel.Children.Clear();
            myCanvas.Children.Clear();
            for (int i = 0; i < Baddies.Length; i++)
            {
                Baddies[i] = new Baddie();
            }
            goodPlayer = new Goodie();

            goRight = goUp = goDown = goLeft = false;
            noRight = noUp = noDown = noLeft = false;

            StartGame();
        }




        #region PlaySoundsMethods

        private void PlayBackgroundMusic()
        {
            Back.Open(new Uri("../../sounds/fluteGameSound2.wav", UriKind.Relative));
            Back.Volume = 0.4;
            Back.Play();
        }

        private void PlayCoinCollectSound()
        {
            Front.Open(new Uri("../../sounds/CoinCollect2.wav", UriKind.Relative));
            Front.Play();
        }

        private void PlayBadGirlSound()
        {
            Back.Open(new Uri("../../sounds/badGirlSound2.wav", UriKind.Relative));
            Back.Play();
        }

        private void PlayWinSound()
        {
            Back.Open(new Uri("../../sounds/CrowdWinSound.wav", UriKind.Relative));
            Back.Play();
        }

        private void PlayLoseSound()
        {
            Back.Open(new Uri("../../sounds/CrowdLoseSound2.wav", UriKind.Relative));
            Back.Play();
        }
        private void PlayShootSound()
        {
            Shoot.Open(new Uri("../..//sounds/gunShot.wav", UriKind.Relative));
            Shoot.Play();
        }
        private void StopAllPlayers()
        {
            Back.Pause();
            Front.Pause();
            Shoot.Pause();
        }
        #endregion



    }
}
