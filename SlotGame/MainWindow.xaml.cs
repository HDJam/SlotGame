using System.Text;
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

namespace SlotGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitReels();

            // Add function to Tick event handler
            ReelTimer.Tick += MoveColumns;
        }

        public bool Reel1Spinning = false;
        public bool Reel2Spinning = false;
        public bool Reel3Spinning = false;

        public int Reel1Speed = 10;
        public int Reel2Speed = 12;
        public int Reel3Speed = 14;

        public bool StopTimer = false;
        public bool GameEnd = false;

        public DispatcherTimer ReelTimer = new()
        {
            Interval = TimeSpan.FromSeconds(.01),
        };


        private void InitReels()
        {
            BitmapImage bitmap;

            foreach (Image image in Reel1.Children)
            {
                bitmap = new BitmapImage(new Uri(GetRandomImageUri(), UriKind.Relative));

                image.BeginInit();
                image.Source = bitmap;
                image.EndInit();
            }

            foreach (Image image in Reel2.Children)
            {
                bitmap = new BitmapImage(new Uri(GetRandomImageUri(), UriKind.Relative));

                image.BeginInit();
                image.Source = bitmap;
                image.EndInit();
            }

            foreach (Image image in Reel3.Children)
            {
                bitmap = new BitmapImage(new Uri(GetRandomImageUri(), UriKind.Relative));

                image.BeginInit();
                image.Source = bitmap;
                image.EndInit();
            }
        }

        private static string GetRandomImageUri()
        {
            int rand = new Random().Next(0, 5);

            return rand switch
            {
                0 => @"/img/Circle_Red.png",
                1 => @"/img/Circle_Green.png",
                2 => @"/img/Circle_Blue.png",
                3 => @"/img/Circle_Orange.png",
                4 => @"/img/Circle_Yellow.png",
                _ => @"/img/Circle_Blue.png",
            };
        }



        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // If reel 1 is still spinning
            if (Reel1Spinning == true)
            {
                // Stop reel
                Reel1Spinning = false;

                return;
            }

            // If reel 2 is still spinning
            if (Reel2Spinning == true)
            {
                // Stop reel
                Reel2Spinning = false;

                return;
            }


            // If reel 3 is still spinning
            if (Reel3Spinning == true)
            {
                // Stop reel
                Reel3Spinning = false;

                return;
            }

            // Reset game
            GameEnd = false;

            // Enable reels
            Reel1Spinning = true;
            Reel2Spinning = true;
            Reel3Spinning = true;

            ResultLabel.Content = "Good luck! Stop the reels!";

            // Enable timer
            ReelTimer.Start();

        }

        private void MoveColumns(object sender, EventArgs e)
        {
            // If reel spin is true, spin indefinitely
            if (Reel1Spinning)
            {
                Canvas.SetTop(Grid0x, Canvas.GetTop(Grid0x) + Reel1Speed);
                Canvas.SetTop(Grid00, Canvas.GetTop(Grid00) + Reel1Speed);
                Canvas.SetTop(Grid01, Canvas.GetTop(Grid01) + Reel1Speed);
                Canvas.SetTop(Grid02, Canvas.GetTop(Grid02) + Reel1Speed);
                if (Canvas.GetTop(Grid02) > Stage.Height)
                {
                    ResetReels(Grid0x, Grid00, Grid01, Grid02);
                }
            }
            if (Reel2Spinning)
            {
                Canvas.SetTop(Grid1x, Canvas.GetTop(Grid1x) + Reel2Speed);
                Canvas.SetTop(Grid10, Canvas.GetTop(Grid10) + Reel2Speed);
                Canvas.SetTop(Grid11, Canvas.GetTop(Grid11) + Reel2Speed);
                Canvas.SetTop(Grid12, Canvas.GetTop(Grid12) + Reel2Speed);
                if (Canvas.GetTop(Grid12) > Stage.Height)
                {
                    ResetReels(Grid1x, Grid10, Grid11, Grid12);
                }
            }
            if (Reel3Spinning)
            {
                Canvas.SetTop(Grid2x, Canvas.GetTop(Grid2x) + Reel3Speed);
                Canvas.SetTop(Grid20, Canvas.GetTop(Grid20) + Reel3Speed);
                Canvas.SetTop(Grid21, Canvas.GetTop(Grid21) + Reel3Speed);
                Canvas.SetTop(Grid22, Canvas.GetTop(Grid22) + Reel3Speed);

                if (Canvas.GetTop(Grid22) > Stage.Height)
                {
                    ResetReels(Grid2x, Grid20, Grid21, Grid22);
                }
            }

            // If reel is not at the default position keep moving until it is
            if (Reel1Spinning == false && Canvas.GetTop(Grid0x) > -75)
            {
                Canvas.SetTop(Grid0x, Canvas.GetTop(Grid0x) + Reel1Speed);
                Canvas.SetTop(Grid00, Canvas.GetTop(Grid00) + Reel1Speed);
                Canvas.SetTop(Grid01, Canvas.GetTop(Grid01) + Reel1Speed);
                Canvas.SetTop(Grid02, Canvas.GetTop(Grid02) + Reel1Speed);
                if (Canvas.GetTop(Grid02) > Stage.Height)
                {
                    ResetReels(Grid0x, Grid00, Grid01, Grid02);
                }
            }

            if (Reel2Spinning == false && Canvas.GetTop(Grid1x) > -75)
            {
                Canvas.SetTop(Grid1x, Canvas.GetTop(Grid1x) + Reel2Speed);
                Canvas.SetTop(Grid10, Canvas.GetTop(Grid10) + Reel2Speed);
                Canvas.SetTop(Grid11, Canvas.GetTop(Grid11) + Reel2Speed);
                Canvas.SetTop(Grid12, Canvas.GetTop(Grid12) + Reel2Speed);
                if (Canvas.GetTop(Grid12) > Stage.Height)
                {
                    ResetReels(Grid1x, Grid10, Grid11, Grid12);
                }
            }

            if (Reel3Spinning == false && Canvas.GetTop(Grid2x) > -75)
            {
                Canvas.SetTop(Grid2x, Canvas.GetTop(Grid2x) + Reel3Speed);
                Canvas.SetTop(Grid20, Canvas.GetTop(Grid20) + Reel3Speed);
                Canvas.SetTop(Grid21, Canvas.GetTop(Grid21) + Reel3Speed);
                Canvas.SetTop(Grid22, Canvas.GetTop(Grid22) + Reel3Speed);

                if (Canvas.GetTop(Grid22) > Stage.Height)
                {
                    ResetReels(Grid2x, Grid20, Grid21, Grid22);
                }
            }


            // Check for win
            if (Reel3Spinning == false && Canvas.GetTop(Grid2x) == -75 && GameEnd == false)
            {
                // Stop game ticks
                GameEnd = true;

                // Stop timer from running
                ReelTimer.Stop();


                // Check for a winner
                CheckForWin();
            }
        }

        private static void ResetReels(Image row1, Image row2, Image row3, Image row4)
        {
            Canvas.SetTop(row1, -75);
            Canvas.SetTop(row2, 0);
            Canvas.SetTop(row3, 75);
            Canvas.SetTop(row4, 150);

            row4.Source = row3.Source.Clone();
            row3.Source = row2.Source.Clone();
            row2.Source = row1.Source.Clone();
            row1.Source = new BitmapImage(new Uri(GetRandomImageUri(), UriKind.Relative));
        }

        private void CheckForWin()
        {
            List<int> list = new()
            {
                GetSourceColorEnum(Grid00.Source.ToString()),
                GetSourceColorEnum(Grid01.Source.ToString()),
                GetSourceColorEnum(Grid02.Source.ToString()),
                GetSourceColorEnum(Grid10.Source.ToString()),
                GetSourceColorEnum(Grid11.Source.ToString()),
                GetSourceColorEnum(Grid12.Source.ToString()),
                GetSourceColorEnum(Grid20.Source.ToString()),
                GetSourceColorEnum(Grid21.Source.ToString()),
                GetSourceColorEnum(Grid22.Source.ToString())
            };

            string winnerLines = "";

            // Check Diagonal, top left to bottom right for matches
            if (list[4].Equals(list[0]) && list[8].Equals(list[0]))
            {
                // winner
                winnerLines += "Diagonal 1\r\n";
            }

            // Check Diagonal, bottom left to top right for matches
            if (list[4].Equals(list[2]) && list[6].Equals(list[2]))
            {
                // Winner
                winnerLines += "Diagonal 2\r\n";
            }

            // Check row winner 1
            if (list[3].Equals(list[0]) && list[6].Equals(list[0]))
            {
                // Winner
                winnerLines += "Row 1\r\n";
            }

            // Check row winner 2
            if (list[4].Equals(list[1]) && list[7].Equals(list[1]))
            {
                // Winner
                winnerLines += "Row 2\r\n";
            }
            // Check row winner 3
            if (list[5].Equals(list[2]) && list[8].Equals(list[2]))
            {
                // Winner
                winnerLines += "Row 3\r\n";
            }

            if (winnerLines.Length > 0)
            {
                ResultLabel.Content = $"You won!\r\nWinning lines:\r\n{winnerLines}";

                //MessageBox.Show($"You won!\r\nWinning lines:\r\n{ winnerLines }", "Winner");
                return;
            }

            if (winnerLines.Length == 0)
            {
                ResultLabel.Content = "Loser!";
                //MessageBox.Show("LOSER");
                return;
            }
        }

        private static int GetSourceColorEnum(string source)
        {
            if (source.Contains((string)Shapes.Red.ToString()))
            {
                return (int)Shapes.Red;
            }
            if (source.Contains((string)Shapes.Green.ToString()))
            {
                return (int)Shapes.Green;
            }
            if (source.Contains((string)Shapes.Blue.ToString()))
            {
                return (int)Shapes.Blue;
            }
            if (source.Contains((string)Shapes.Orange.ToString()))
            {
                return (int)Shapes.Orange;
            }
            if (source.Contains((string)Shapes.Yellow.ToString()))
            {
                return (int)Shapes.Yellow;
            }

            // Default out of array
            return 99;
        }

        enum Shapes
        {
            Red = 0,
            Green = 1,
            Blue = 2,
            Orange = 3,
            Yellow = 4
        }

        private void PlayArea_Click(object sender, MouseButtonEventArgs e)
        {
            PlayButton_Click(sender, e);
        }
    }
}