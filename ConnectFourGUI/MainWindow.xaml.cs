// Dharma Bellamkonda
// Connect Four GUI

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace ConnectFourGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int circleSize = 80;

        private ConnectFourBoard board;
        private DispatcherTimer animationTimer;
        private bool inputLock;

        private Side currentSide;
        private Ellipse currentCircle;
        private int currentColumn;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            inputLock = true;
            board = new ConnectFourBoard(6, 7);
            currentSide = Side.Red;
            animationTimer = new DispatcherTimer();
            animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            animationTimer.Start();
            GameCanvas.Children.Clear();
            DrawBackground();
            inputLock = false;
            EnableAllInsertButtons();
        }

        private void InsertButton_Click(int column)
        {
            if (inputLock == false)
            {
                bool success = board.Insert(currentSide, column);
                if (success)
                {
                    currentColumn = column;
                    DrawCircle(currentSide, column);
                    AfterTurn();
                }
            }

        }

        #region InsertButton_Click Methods
        private void InsertButton0_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(0);
        }

        private void InsertButton1_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(1);
        }

        private void InsertButton2_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(2);
        }

        private void InsertButton3_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(3);
        }

        private void InsertButton4_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(4);
        }

        private void InsertButton5_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(5);
        }

        private void InsertButton6_Click(object sender, RoutedEventArgs e)
        {
            InsertButton_Click(6);
        }
        #endregion

        private void DrawBackground()
        {
            for (int row = 0; row < board.GameBoard.GetLength(0); row++)
            {
                for (int column = 0; column < board.GameBoard.GetLength(1); column++)
                {
                    Rectangle square = new Rectangle();
                    square.Height = circleSize;
                    square.Width = circleSize;
                    square.Fill = (column % 2 == 0) ? Brushes.White : Brushes.LightGray;
                    Canvas.SetBottom(square, circleSize * row);
                    Canvas.SetRight(square, circleSize * column);
                    GameCanvas.Children.Add(square);
                }
            }
        }

        private void DrawCircle(Side side, int col)
        {
            inputLock = true;

            Ellipse circle = new Ellipse();
            circle.Height = circleSize;
            circle.Width = circleSize;
            circle.Fill = (side == Side.Red) ? Brushes.Red : Brushes.Black;
            Canvas.SetTop(circle, 0);
            Canvas.SetLeft(circle, col * 80);
            GameCanvas.Children.Add(circle);
            currentCircle = circle;
            animationTimer.Tick += DropCircleAnimation;
        }

        private void DropCircleAnimation(object sender, EventArgs e)
        {
            int dropLength = circleSize * (board.GameBoard.GetLength(1) - 1 - board.PiecesInCol(currentColumn));
            int dropRate = 40;
            if (Canvas.GetTop(currentCircle) < dropLength)
            {
                Canvas.SetTop(currentCircle, Canvas.GetTop(currentCircle) + dropRate);
            }
            else
            {
                animationTimer.Tick -= DropCircleAnimation;
                inputLock = false;
            }
              
        }

        private void AfterTurn()
        {
            Side winner = board.Winner();

            if (winner != Side.None)
            {
                StatusText.Text = String.Format("{0} player Wins!", currentSide);
                DisableAllInsertButtons();
            }
            else if (board.Tied())
            {
                StatusText.Text = "Tied game!";
                DisableAllInsertButtons();
            }
            else
            {
                currentSide = (currentSide == Side.Black) ? Side.Red : Side.Black;
                StatusText.Text = String.Format("{0}'s Turn", currentSide);
            }
        }

        private void DisableAllInsertButtons()
        {
            InsertButton0.IsEnabled = false;
            InsertButton1.IsEnabled = false;
            InsertButton2.IsEnabled = false;
            InsertButton3.IsEnabled = false;
            InsertButton4.IsEnabled = false;
            InsertButton5.IsEnabled = false;
            InsertButton6.IsEnabled = false;
        }

        private void EnableAllInsertButtons()
        {
            InsertButton0.IsEnabled = true;
            InsertButton1.IsEnabled = true;
            InsertButton2.IsEnabled = true;
            InsertButton3.IsEnabled = true;
            InsertButton4.IsEnabled = true;
            InsertButton5.IsEnabled = true;
            InsertButton6.IsEnabled = true;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}
