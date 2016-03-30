using System.Windows;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Music.Play();
        }

        private void btnVsCom_Click(object sender, RoutedEventArgs e)
        {
            OfflinePlaySpace ofl = new OfflinePlaySpace();
            this.Hide();
            ofl.ShowDialog();
            this.Show();
        }

      

        private void btnVsLan_Click(object sender, RoutedEventArgs e)
        {
            PlaySpace pl = new PlaySpace();
            this.Hide();
            pl.ShowDialog();
            this.Show();
        }

        private void btnOnePlayer_Click(object sender, RoutedEventArgs e)
        {
            OnePlayer window = new OnePlayer();
            this.Hide();
            window.ShowDialog();
            this.Show();
        }       
      
        private void btnOnePlayer_Enter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtblockIntro.Text = "Thách đấu bản thân";
        }

        private void btnVsCom_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtblockIntro.Text = "Luyện tập cùng máy tính";
        }

        private void btnVsLan_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtblockIntro.Text = "Chơi online với người khác";
        }

        private void btnClose_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtblockIntro.Text = "Thoát";
        }

        private void btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            txtblockIntro.Text = "";
        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Show();
        }
    }
}
