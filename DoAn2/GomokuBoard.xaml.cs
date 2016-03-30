using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace DoAn2
{
    /// <summary>
    /// Interaction logic for GomokuBoard.xaml
    /// </summary>
    public partial class GomokuBoard : UserControl
    {
        private char[,] matrix;
        readonly private bool win = true;
        public char[,] Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
            }
        }

        public GomokuBoard()
        {
            InitializeComponent();
   //         StreamResourceInfo sri = System.Windows.Application.GetResourceStream(
   //new Uri(@"pack://application:,,,/Images/leaf.cur", UriKind.Absolute));

   //         this.Cursor = new Cursor(sri.Stream);
            Matrix = new char[12, 12];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Matrix[i, j] = ' ';
                }
            }
        }

        

        /// <summary>
        /// Vẽ quân cờ tại vị trí point và thêm ký hiệu quân cờ đó vào ma trận matrix
        /// </summary>
        /// <param name="point">vị trí vẽ</param>
        /// <param name="flag">ký hiệu quân cờ x hay o</param>
        /// <returns></returns>
        public bool veQuanCo(Point point, bool flag)
        {
            if (Matrix[(int)point.X, (int)point.Y] == ' ')
            {
                Image im = new Image();
                im.VerticalAlignment = VerticalAlignment.Center;
                im.HorizontalAlignment = HorizontalAlignment.Center;
                im.Stretch = Stretch.None;
                im.Height = 50;
                im.Width = 50;

                BitmapImage mark = new BitmapImage();
                mark.BeginInit();

                if (flag)
                {
                    mark.UriSource = new Uri("images/x.png", UriKind.Relative);
                    Matrix[(int)point.X, (int)point.Y] = 'x';
                }
                else
                {
                    mark.UriSource = new Uri("images/o.png", UriKind.Relative);
                    Matrix[(int)point.X, (int)point.Y] = 'o';
                }
                mark.EndInit();

                im.Source = mark;
                Canvas.SetTop(im, (int)point.Y * 50);
                Canvas.SetLeft(im, (int)point.X * 50);
               
                    mainCanvas.Children.Add(im);
              
                return true;
            }
            return false;
        }

        /// <summary>
        /// xóa trắng bàn cờ và mảng quân cờ
        /// </summary>
        public void clearBoard()
        {
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Matrix[i, j] = ' ';
                }
            }
        }

      

        private bool lookLeft(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (col_pos > 0 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos,col_pos - 1] == Matrix[row_pos,col_pos])
                            --col_pos;
                        else return !win;
                    }

                    else return !win;
                }
                catch (Exception)
                {

                    return !win;
                }

            }

            return win;
        }

        private bool lookRight(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (col_pos < 12 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos,col_pos + 1] == Matrix[row_pos,col_pos])
                            ++col_pos;
                        else return !win;
                    }
                    else return !win;
                }
                catch (Exception)
                {

                    return !win;
                }

            }
            return win;
        }

        private bool lookUp(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (row_pos > 0 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos - 1,col_pos] == Matrix[row_pos,col_pos])
                            --row_pos;
                        else return !win;
                    }

                    else return !win;
                }
                catch (Exception)
                {
                    return !win;
                }


            }
            return win;

        }

        private bool lookDown(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if ( row_pos < 12 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos + 1,col_pos] == Matrix[row_pos,col_pos])
                            ++row_pos;
                        else return !win;
                    }

                    else return !win;
                }

                catch (Exception)
                {
                    return !win;
                }
            }
            return win;
        }

        private bool lookUpLeft(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (row_pos > 0 && col_pos > 0 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos - 1,col_pos - 1] == Matrix[row_pos,col_pos])
                        {
                            --row_pos;
                            --col_pos;
                        }
                        else return !win;
                    }

                    else return !win;
                }
                catch (Exception)
                {
                    return !win;
                }
            }

            return win;
        }

        private bool lookUpRight(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if ( row_pos > 0 && col_pos < 12 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos - 1,col_pos + 1] == Matrix[row_pos,col_pos])
                        {
                            --row_pos;
                            ++col_pos;
                        }
                        else return !win;
                    }


                    else return !win;
                }
                catch (Exception)
                {
                    return !win;
                }
            }

            return win;
        }

        private bool lookDownLeft(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (row_pos < 12 && col_pos > 0 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos + 1,col_pos - 1] == Matrix[row_pos,col_pos])
                        {
                            ++row_pos;
                            --col_pos;
                        }
                        else return !win;
                    }

                    else return !win;
                }
                catch (Exception)
                {
                    return !win;
                }
            }

            return win;
        }

        private bool lookDownRight(int row_pos, int col_pos)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (row_pos < 12 && col_pos < 12 && Matrix[row_pos,col_pos] != ' ')
                    {

                        if (Matrix[row_pos + 1,col_pos + 1] == Matrix[row_pos,col_pos])
                        {
                            ++row_pos;
                            ++col_pos;
                        }
                        else return !win;
                    }

                    else return !win;

                }
                catch (Exception)
                {
                    return !win;
                }
            }

            return win;
        }

        public bool isWin(int row_pos, int col_pos)
        {
            if (lookLeft(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookRight(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookDown(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookUp(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookUpLeft(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookUpRight(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookDownLeft(row_pos, col_pos) == win)
            {
                return win;
            }

            if (lookDownRight(row_pos, col_pos) == win)
            {
                return win;
            }

            return !win;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StreamResourceInfo sri = System.Windows.Application.GetResourceStream(
            new Uri(@"pack://application:,,,/Images/leaf.cur", UriKind.Absolute));

            this.Cursor = new Cursor(sri.Stream);
        }
    }
}
