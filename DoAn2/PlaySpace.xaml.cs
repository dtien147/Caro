using System;
using System.Windows;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;
using System.Configuration;
using System.Text.RegularExpressions;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for PlaySpace.xaml
    /// </summary>
    public partial class PlaySpace : Window
    {
        private Socket socket;
        private Thread thread;
        private Message newMess = new Message();
        private AI ai;

        public PlaySpace()
        {
            InitializeComponent();
            ai = new AI(12, 8);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sever = ConfigurationManager.AppSettings["sever"];
            //socket = IO.Socket("ws://gomoku-lajosveres.rhcloud.com:8000");
            socket = IO.Socket(sever);
            thread = new Thread(new ParameterizedThreadStart(socketManager));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(socket);
        }
  
        private void socketManager(object obj)
        {
            Socket sk = (Socket)obj;
            string name;
            
            //Xu ly su kien chat 
            sk.On("ChatMessage", (data) =>
            {
                if (((Newtonsoft.Json.Linq.JObject)data)["from"] == null)
                {
                    name = "Sever";
                }
                else
                    name = ((Newtonsoft.Json.Linq.JObject)data)["from"].ToString();
                

                string mess = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                string time = DateTime.Now.ToString("hh:mm:ss tt");

                newMess.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    newMess = new Message(name, time, mess);
                }));
                listBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    listBox.Items.Add(newMess);
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }));
                
                if (((Newtonsoft.Json.Linq.JObject)data)["message"].ToString() == "Welcome!")
                {
                    sk.Emit("ConnectToOtherPlayer");
                }

                Regex regex = new Regex("You are the first player");
                bool condition = false;
                rbtnAIplay.Dispatcher.Invoke(new Action(() =>
                {
                    if (rbtnAIplay.IsChecked == true)
                        condition = true;
                    else
                        condition = false;
                }));
                if (regex.Matches(mess).Count > 0 && condition)
                {
                    Random rd = new Random();
                    Point p = new Point(rd.Next(4, 8), rd.Next(4, 8));
                    sk.Emit("MyStepIs", JObject.FromObject(new { row = (int)p.Y, col = (int)p.X }));
                }

            });

            //Xu ly su kien nhan nuoc di moi
            sk.On("NextStepIs", (data) =>
            {
                int x = int.Parse((((Newtonsoft.Json.Linq.JObject)data)["col"].ToString()));
                int y = int.Parse((((Newtonsoft.Json.Linq.JObject)data)["row"].ToString()));
                if (((Newtonsoft.Json.Linq.JObject)data)["player"].ToString() == "0")
                {
                    chessBoard.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        chessBoard.veQuanCo(new Point(x, y), true);
                    }));
                }
                else
                    chessBoard.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        chessBoard.veQuanCo(new Point(x, y), false);
                        if (rbtnAIplay.IsChecked == true)
                        {
                            Thread t = new Thread(new ParameterizedThreadStart(computerMove));
                            t.IsBackground = true;
                            t.SetApartmentState(ApartmentState.STA);
                            t.Start(sk);
                        }
                    }));
            });

            sk.On("EndGame", (data) =>
            {
                string mess = ((Newtonsoft.Json.Linq.JObject)data)["message"].ToString();
                string time = DateTime.Now.ToString("hh:mm:ss tt");
                newMess.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    newMess = new Message("Sever", time, mess);
                }));


                listBox.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    listBox.Items.Add(newMess);
                    listBox.SelectedIndex = listBox.Items.Count - 1;
                    listBox.ScrollIntoView(listBox.SelectedItem);
                }));

                chessBoard.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    if (MessageBox.Show("New Game?", "Game Over", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        sk.Emit("ConnectToOtherPlayer");
                        chessBoard.clearBoard();
                    }
                    else this.Close();
                }));
            });
        }

        private void btnChage_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                socket.Emit("MyNameIs", "Guest");
                txtName.Text = "Guest";
            }
            else
                socket.Emit("MyNameIs", txtName.Text);
        }

        private void Window_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(chessBoard);
            if (p.X>=0 && p.X <=600 && p.Y >=0 && p.Y <= 600)
                socket.Emit("MyStepIs", JObject.FromObject(new { row = (int)p.Y / 50, col = (int)p.X / 50 }));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Application.Current.Shutdown();
        }

        private void txtMess_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtMess.Text == "Type your message here...")
                txtMess.Text = "";
        }


        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (txtMess.Text != "Type your message here...")
            {
                socket.Emit("ChatMessage", txtMess.Text);
                txtMess.Text = "";
            }
        }

        private void txtMess_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (txtMess.Text == "")
                txtMess.Text = "Type your message here...";
        }

        private void txtName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (txtName.Text == "")
                {
                    socket.Emit("MyNameIs", "Guest");
                    txtName.Text = "Guest";
                }
                else
                    socket.Emit("MyNameIs", txtName.Text);
            }
        }

        private void txtMess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (txtMess.Text != "Type your message here...")
                {
                    socket.Emit("ChatMessage", txtMess.Text);
                    txtMess.Text = "";
                }
            }
        }

        private void computerMove(object sk)
        {
            Point p = ai.Solve(ref chessBoard, 'x');
            Socket socket = (Socket)sk;
            socket.Emit("MyStepIs", JObject.FromObject(new { row = (int)p.Y, col = (int)p.X }));
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings.Show();
        }
    }
}
