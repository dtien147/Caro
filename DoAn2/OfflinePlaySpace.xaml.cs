using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for OfflinePlaySpace.xaml
    /// </summary>
    public partial class OfflinePlaySpace : Window
    {
        private bool gameStart;
        private bool currPlayer;
        private bool player;
        private bool pause;
        private Random rd;
        private AI ai;

        private Random rnd = new Random();

        public OfflinePlaySpace()
        {
            InitializeComponent();
            gameStart = false;
            ai = new AI(12, 7);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (gameStart == true && currPlayer == true)
            {
                Point p = e.GetPosition(gomokuBoard);
                if (p.X >= 0 && p.X <= 600 && p.Y >= 0 && p.Y <= 600 && gameStart && currPlayer)
                {
                    gomokuBoard.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        Point newPoint = new Point(p.X/50, p.Y/50);

                        int x = (int) newPoint.X;
                        int y = (int) newPoint.Y;

                        bool check = gomokuBoard.veQuanCo(newPoint, true);
                        if (check)
                        {
                            if (gomokuBoard.isWin(x, y) || gomokuBoard.isWin(x + 1, y) || gomokuBoard.isWin(x + 2, y) ||
                                gomokuBoard.isWin(x - 1, y) || gomokuBoard.isWin(x - 2, y) ||
                                gomokuBoard.isWin(x, y + 1) || gomokuBoard.isWin(x, y + 2) ||
                                gomokuBoard.isWin(x, y - 1) || gomokuBoard.isWin(x, y - 2) ||
                                gomokuBoard.isWin(x + 2, y + 2) || gomokuBoard.isWin(x + 1, y + 1) ||
                                gomokuBoard.isWin(x - 1, y - 1) || gomokuBoard.isWin(x - 2, y - 2) ||
                                gomokuBoard.isWin(x + 1, y - 1) || gomokuBoard.isWin(x - 1, y + 1) ||
                                gomokuBoard.isWin(x - 2, y + 2) || gomokuBoard.isWin(x + 2, y - 2))
                            {
                                if (MessageBox.Show("Bạn đã thắng! Tạo ván mới?", "Kết thúc", MessageBoxButton.YesNo) ==
                                    MessageBoxResult.Yes)
                                {
                                    newGame();                                   
                                    txtbloxkStepInfo.Text = "Nước đi mới nhất: ";
                                }

                                else
                                {
                                    gameStart = false;
                                    txtblockMoreInfo.Text = "Trò chơi kết thúc";
                                    currPlayer = true;
                                }

                            }

                            else
                            {
                             
                                    txtbloxkStepInfo.Text = "Nước đi mới nhất: " + (y + 1).ToString() + " " +
                                                            (x + 1).ToString();
                                    txtblockMoreInfo.Text = "";                                

                                currPlayer = false;
                            }                            
                        }


                    }

                        ));




                    if (!currPlayer)
                    {
                        Thread thread = new Thread(new ThreadStart(computerMove));
                        thread.IsBackground = true;
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                    }
                }

                else
                    gameStart = false;


            }
        }

        private void computerMove()
        {
            Point newPoint = ai.Solve(ref gomokuBoard, 'o');
            gomokuBoard.Dispatcher.Invoke(new Action(() =>
            {
                gomokuBoard.veQuanCo(newPoint, currPlayer);


                int x = (int) newPoint.X;
                int y = (int) newPoint.Y;
                if (gomokuBoard.isWin(x, y) || gomokuBoard.isWin(x + 1, y) || gomokuBoard.isWin(x + 2, y) ||
                    gomokuBoard.isWin(x - 1, y) || gomokuBoard.isWin(x - 2, y) ||
                    gomokuBoard.isWin(x, y + 1) || gomokuBoard.isWin(x, y + 2) ||
                    gomokuBoard.isWin(x, y - 1) || gomokuBoard.isWin(x, y - 2) ||
                    gomokuBoard.isWin(x + 2, y + 2) || gomokuBoard.isWin(x + 1, y + 1) ||
                    gomokuBoard.isWin(x - 1, y - 1) || gomokuBoard.isWin(x - 2, y - 2) ||
                    gomokuBoard.isWin(x + 1, y - 1) || gomokuBoard.isWin(x - 1, y + 1) ||
                    gomokuBoard.isWin(x - 2, y + 2) || gomokuBoard.isWin(x + 2, y - 2))
                {
                    if (MessageBox.Show("Bạn đã thua! Tạo ván mới?", "Kết thúc", MessageBoxButton.YesNo) ==
                        MessageBoxResult.Yes)
                    {
                        newGame();
                        gomokuBoard.Dispatcher.Invoke(new Action(() =>
                        {
                            txtbloxkStepInfo.Text = "Nước đi mới nhất: ";


                        }));
                    }

                    else
                    {
                        gameStart = false;
                        txtblockMoreInfo.Text = "Trò chơi kết thúc";

                    }

                }

                else
                {

                    txtbloxkStepInfo.Text = "Nước đi mới nhất: " + (y + 1).ToString() + " " + (x + 1).ToString();
                    txtblockMoreInfo.Text = "";


                    currPlayer = true;
                }
            }));


        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc chắn muốn chơi ván mới không", "Xác nhận", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
                newGame();

            else gameStart = false;
        }



        private void newGame()
        {
            gameStart = true;
            if (rbtnTb.IsChecked == true)
                ai = new AI(12, 7);
            if (rbtnDe.IsChecked == true)
                ai = new AI(12, 5);
            if (rbtnKho.IsChecked == true)
                ai = new AI(12, 10);
            gomokuBoard.Dispatcher.Invoke(new Action(() =>
            {
                gomokuBoard.clearBoard();
            }));
            rd = new Random();
            int i = rd.Next(0, 1000);

            currPlayer = false;

            if (!currPlayer)
            {
                gomokuBoard.Dispatcher.Invoke(new Action(() =>
                {
                    gomokuBoard.veQuanCo(new Point(rd.Next(5, 8), rd.Next(5, 8)), currPlayer);
                    currPlayer = true;
                }));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Show();
        }
    }
}
