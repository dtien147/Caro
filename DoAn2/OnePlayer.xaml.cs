using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for OnePlayer.xaml
    /// </summary>
    public partial class OnePlayer : Window
    {
       
        readonly private bool win = true;
        private bool flag = true;

        private bool newGame = true;
        public OnePlayer()
        {
            InitializeComponent();
           
        }

        private void Window_MouseLeftButonUp(object sender, MouseButtonEventArgs e)
        {
            if (newGame == true)
            {
                Point p = e.GetPosition(chessBoard);
                if (p.X >= 0 && p.X <= 600 && p.Y >= 0 && p.Y <= 600)
                {
                    int x = int.Parse(((int) p.X/50).ToString());
                    int y = int.Parse(((int) p.Y/50).ToString());



                    chessBoard.veQuanCo(new Point(x, y), flag);

                    txtbloxkStepInfo.Text = "Nước đi mới nhất: ";
                    txtbloxkStepInfo.Text += (y + 1).ToString() + " " + (x + 1).ToString();

                    if (chessBoard.isWin(x, y) || chessBoard.isWin(x + 1, y) || chessBoard.isWin(x + 2, y) ||
                        chessBoard.isWin(x - 1, y) || chessBoard.isWin(x - 2, y) || chessBoard.isWin(x, y + 1) ||
                        chessBoard.isWin(x, y + 2) || chessBoard.isWin(x, y - 1) || chessBoard.isWin(x, y - 2) ||
                        chessBoard.isWin(x + 2, y + 2) || chessBoard.isWin(x + 1, y + 1) ||
                        chessBoard.isWin(x - 1, y - 1) || chessBoard.isWin(x - 2, y - 2) ||
                        chessBoard.isWin(x + 1, y - 1) || chessBoard.isWin(x - 1, y + 1) ||
                        chessBoard.isWin(x - 2, y + 2) || chessBoard.isWin(x + 2, y - 2))
                    {


                        if (
                            MessageBox.Show("Trò chơi kết thúc. Bạn có muốn chơi ván mới không?", "Kết thúc",
                                MessageBoxButton.YesNo) ==
                            MessageBoxResult.Yes)
                        {
                            flag = true;
                            chessBoard.clearBoard();

                            txtbloxkStepInfo.Text = "Nước đi mới nhất: ";
                            txtbloxkStepInfo.Text = "";
                        }

                        else
                        {
                            newGame = false;
                            txtbloxkNewGame.Text = "Chưa bắt đầu ván mới";
                        }
                    }



                    else flag = !flag;
                }


            }

        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn chơi ván mới không?", "Xác nhận", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                newGame = true;
                flag = true;
                chessBoard.clearBoard();
                         
                txtbloxkStepInfo.Text = "Nước đi mới nhất: ";
                txtbloxkStepInfo.Text = "";
            }

            else
            {
                newGame = false;
                txtbloxkNewGame.Text = "Chưa bắt đầu ván mới";
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Show();
        }
    }    
}
