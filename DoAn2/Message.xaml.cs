using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    
    public partial class Message : UserControl
    {
        private string name;
        private string time;
        private string mess;

        public Message()
        {
            InitializeComponent();
        }
      
        public Message(string Name, string Time, string Mess)
        {
            InitializeComponent();
            this.name = Name;
            this.time = Time;
            this.mess = Mess;

            mess = mess.Replace("<br />", "");

            lbName.Content = name;
            lbMess.Text = mess;
            lbTime.Content = time;
        }
    }
}
