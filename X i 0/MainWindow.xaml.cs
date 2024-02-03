using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System;

namespace Xi0
{
    public partial class MainWindow : Window
    {
        private Button[] buttons;
        private bool isPlayerOneTurn;
        private bool isGameEnded;

        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
            ResetGame();
        }

        private void InitializeButtons()
        {
            buttons = new Button[]
            {
                btn1, btn2, btn3,
                btn4, btn5, btn6,
                btn7, btn8, btn9
            };

            foreach (Button button in buttons)
            {
                button.Click += OnButtonClick;
            }
        }

        private void ResetGame()
        {
            isPlayerOneTurn = true;
            isGameEnded = false;
            lblResult.Content = "";

            foreach (Button button in buttons)
            {
                button.IsEnabled = true;
                button.Content = "";
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (isGameEnded)
                return;

            Button button = sender as Button;
            button.Content = "X";
            button.IsEnabled = false;
            isPlayerOneTurn = !isPlayerOneTurn;
            CheckWin();

            if (!isPlayerOneTurn)
            {
                Robot();
                CheckWin();
            }
        }

        private void Robot()
        {
            if (isGameEnded)
                return;

            Random random = new Random();
            int index = random.Next(1, 9);
            Button button = FindName("btn" + index) as Button;


            if (button.Content != null && button.IsEnabled) 
            {
                button.IsEnabled = false;
                button.Content = "O";
                CheckWin();
            }
            else
            {
                Robot();
            }
        }

        private void CheckWin()
        {
            string[,] board = new string[3, 3];

            for (int i = 0; i < buttons.Length; i++)
            {
                int row = i / 3;
                int col = i % 3;

                board[row, col] = buttons[i].Content.ToString();
            }

            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != "")
                {
                    EndGame(board[i, 0]);
                    return;
                }

                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != "")
                {
                    EndGame(board[0, i]);
                    return;
                }
            }

            if ((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != "") ||
                (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ""))
            {
                EndGame(board[1, 1]);
                return;
            }

            if (!board.Cast<string>().Any(cell => cell == ""))
            {
                EndGame("Ничья");
                return;
            }
        }

        private void EndGame(string winner)
        {
            isGameEnded = true;

            foreach (Button button in buttons)
            {
                button.IsEnabled = false;
            }

            lblResult.Content = $"Выиграл игрок {winner}!";
        }

        private void OnRestartClick(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }
    }
}