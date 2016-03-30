using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace DoAn2
{
    class Element
    {
        private Point position;
        private int value;

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }

        public Point Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public Element(int x,int y,int val)
        {
            this.Position = new Point(x, y);
            this.Value = val;
        }

        public void setPosition (int x, int y)
        {
            this.Position = new Point(x, y);
        }
    }

    class AI
    {
        #region Properties
        const int MAX = 2147483647;
        private int size;
        private Random rand;

        private int[] attackRate = { 0, 1, 10, 100, 1000 };
        private int[] defenseRate = { 0, 3, 30, 300, 2500 };

        private string[] caseX = { @"\sxx\s", @"\sxxxo", @"oxxx\s", @"\sxxx\s", @"\sxxxxo", @"oxxxx\s", @"\sxxxx\s", @"xxxxx" };
        private string[] caseO = { @"\soo\s", @"\sooox", @"xooo\s", @"\sooo\s", @"\soooox", @"xoooo\s", @"\soooo\s", @"ooooo" };
        private int[] casePoint = { 6, 4, 4, 12, 30, 30, 3000, 10000 };  // Điểm tương ứng với từng trường hợp

        private int[,] value;
        

        private int maxDepth;

        private char playerSymbol;
        private char computerSymbol;

        private  int branch;
        #endregion

        
        /// <summary>
        /// Constructor khởi tạo đối tượng của class AI
        /// </summary>
        /// <param name="size"> Độ lớn của bàn cờ</param>
        /// <param name="depth"> Độ sâu tối đa thuật toán chạy </param>
        public AI(int size,int depth)
        {
            this.size = size;
            this.maxDepth = depth;
            rand = new Random();
            branch = 3;
            value = new int[size, size]; // Lưu trữ giá trị của các ô trên bàn cờ
        }

        /// <summary>
        /// Khởi tạo giá trị ban đầu cho ma trận giá trị
        /// </summary>
        private void InitializeValue()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    value[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Đánh giá các ô trong bàn cờ hiện tại
        /// </summary>
        /// <param name="board"> Đối tượng lưu trữ trạng tái hiện tại của bàn cờ </param>
        /// <param name="player"> Người chơi đang chơi trong lượt này </param>
        private void evalueteBoard(ref GomokuBoard board,char player)
        {
            InitializeValue();  //Khởi tạo gia trị ban đầu

            int row, col;

            int countComputer, countPlayer;

            // Đánh giá theo hàng ngang trong bàn cờ
            for (row = 0; row < size ; row++)
            {
                for (col = 0;col < size - 4; col++)
                {
                    countComputer = 0;  // Giá trị ban đầu 
                    countPlayer = 0;  // Giá trị ban đầu

                    for (int i = 0; i < 5; i++)
                    {
                        if (board.Matrix[row, col + i] == computerSymbol)  // Nếu quân tại ô đó là của người chơi hiện tại
                            countComputer++;                               // thì tăng số đếm của người chơi 1
                        if (board.Matrix[row,col+i] == playerSymbol)       // nếu không phải của người chơi hiện tại 
                            countPlayer++;                                 // thì tăng quân của người chơi thứ 2
                    }

                    // Nếu tìm được một dòng chỉ chứa quân người chơi hoặc máy thì
                    if (countPlayer * countComputer == 0 && countPlayer!=countComputer)
                        for (int i = 0; i < 5; i++)
                        {
                            if (board.Matrix[row, col + i] == ' ') // Tìm được ô trống trong 5 ô chứa quân cờ
                            {
                                if (countComputer == 0)  // Nếu các quân cờ đó là của người chơi 1
                                {
                                    if (player == computerSymbol) // Nếu nước đi hiện tại là của máy tính
                                        value[row, col + i] += attackRate[countPlayer];
                                    else  // nếu nước đi hiện tại là của người chơi
                                        value[row, col + i] += defenseRate[countPlayer];
                                }
                                else  // Nếu các quân đó là của máy tính
                                {
                                    if (player == playerSymbol) // Nếu nước đi hiện tại là của người chơi
                                        value[row, col + i] += attackRate[countComputer];
                                    else  // nếu nước đi hiện tại là của máy tính
                                        value[row, col + i] += defenseRate[countComputer];
                                }

                                if (countComputer == 4 || countPlayer == 4)  // Nếu đã có 4 quân cùng loại trên hàng
                                    value[row, col + i] = value[row, col + i] * 2;  // Thì nhân đôi giá trị ô đó
                            }
                        }
                }
            }

            // Đánh giá theo cột dọc
            for (row = 0; row < size - 4; row++)
            {
                for (col = 0; col < size; col++)
                {
                    countComputer = 0;  // Giá trị ban đầu 
                    countPlayer = 0;  // Giá trị ban đầu

                    for (int i = 0; i < 5; i++)
                    {
                        if (board.Matrix[row + i, col] == computerSymbol)  // Nếu quân tại ô đó là của người chơi hiện tại
                            countComputer++;                               // thì tăng số đếm của người chơi 1
                        if (board.Matrix[row + i, col] == playerSymbol)       // nếu không phải của người chơi hiện tại 
                            countPlayer++;                                 // thì tăng quân của người chơi thứ 2
                    }

                    // Nếu tìm được một dòng chỉ chứa quân người chơi hoặc máy thì
                    if (countPlayer * countComputer == 0 && countPlayer != countComputer)
                        for (int i = 0; i < 5; i++)
                        {
                            if (board.Matrix[row + i, col] == ' ') // Tìm được ô trống trong 5 ô chứa quân cờ
                            {
                                if (countComputer == 0)  // Nếu các quân cờ đó là của người chơi 1
                                {
                                    if (player == computerSymbol) // Nếu nước đi hiện tại là của máy tính
                                        value[row + i, col] += attackRate[countPlayer];
                                    else  // nếu nước đi hiện tại là của người chơi
                                        value[row + i, col] += defenseRate[countPlayer];
                                }
                                else  // Nếu các quân đó là của máy tính
                                {
                                    if (player == playerSymbol) // Nếu nước đi hiện tại là của người chơi
                                        value[row + i, col] += attackRate[countComputer];
                                    else  // nếu nước đi hiện tại là của máy tính
                                        value[row + i, col] += defenseRate[countComputer];
                                }

                                if (countComputer == 4 || countPlayer == 4)  // Nếu đã có 4 quân cùng loại trên hàng
                                    value[row + i, col] = value[row + i, col] * 2;  // Thì nhân đôi giá trị ô đó
                            }
                        }
                }
            }

            // Đánh giá theo đường chéo (Song song với đường chéo chính)
            // Tương tự như dòng và cột
            for (row = 0; row < size - 4; row++)
            {
                for (col = 0; col < size-4; col++)
                {
                    countComputer = 0;
                    countPlayer = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        if (board.Matrix[row + i, col + i] == computerSymbol)
                            countComputer++;
                        if (board.Matrix[row + i, col + i] == playerSymbol)
                            countPlayer++;
                    }

                    // Nếu tìm được 5 ô liên tiếp chỉ chứa quân của người hoặc máy
                    if (countPlayer * countComputer == 0 && countPlayer != countComputer)
                        for (int i = 0; i < 5; i++)
                        {
                            if (board.Matrix[row + i, col + i] == ' ')
                            {
                                if (countComputer == 0)
                                {
                                    if (player == computerSymbol)
                                        value[row + i, col + i] += attackRate[countPlayer];
                                    else
                                        value[row + i, col + i] += defenseRate[countPlayer];
                                }
                                else
                                {
                                    if (player == playerSymbol)
                                        value[row + i, col + i] += attackRate[countComputer];
                                    else
                                        value[row + i, col + i] += defenseRate[countComputer];
                                }

                                if (countComputer == 4 || countPlayer == 4)
                                    value[row + i, col + i] = value[row + i, col + i] * 2;
                            }

                        }
                }
            }

            // Đánh giá đường chéo song song với đường chéo phụ
            for (row = 4; row < size ; row++)
            {
                for (col = 0; col < size - 4; col++)
                {
                    countComputer = 0;
                    countPlayer = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        if (board.Matrix[row - i, col + i] == computerSymbol)
                            countComputer++;
                        if (board.Matrix[row - i, col + i] == playerSymbol)
                            countPlayer++;
                    }

                    // Nếu tìm được 5 ô liên tiếp chỉ chứa quân của người hoặc máy
                    if (countPlayer * countComputer == 0 && countPlayer != countComputer)
                        for (int i = 0; i < 5; i++)
                        {
                            if (board.Matrix[row - i, col + i] == ' ')
                            {
                                if (countComputer == 0)
                                {
                                    if (player == computerSymbol)
                                        value[row - i, col + i] += attackRate[countPlayer];
                                    else
                                        value[row - i, col + i] += defenseRate[countPlayer];
                                }
                                else
                                {
                                    if (player == playerSymbol)
                                        value[row - i, col + i] += attackRate[countComputer];
                                    else
                                        value[row - i, col + i] += defenseRate[countComputer];
                                }

                                if (countComputer == 4 || countPlayer == 4)
                                    value[row - i, col + i] = value[row - i, col + i] * 2;
                            }

                        }
                }
            }
        }

        /// <summary>
        /// Tìm vị trí có giá trị lớn nhất trong mảng value
        /// nếu có nhiều vị trí cùng giá trị thì chọn ngẫu nhiên
        /// 1 trong các vị trí đó. 
        /// </summary>
        /// <returns></returns>
        private Element findMax()
        {
            Point point;
            List<Element> list = new List<Element>();
            int max = -MAX;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (max < value[i, j])
                    {
                        max = value[i, j];
                        point = new Point(i, j);
                        list.Clear();
                        list.Add(new Element((int)point.X, (int)point.Y, max));
                    }
                    else
                        if (value[i, j] == max)
                        {
                            point = new Point(i, j);
                            list.Add(new Element((int)point.X, (int)point.Y, max));
                        }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                value[(int)list[i].Position.X, (int)list[i].Position.Y] = 0;
            }

            return list[rand.Next(0, list.Count)];
        }

        private int Eval(ref GomokuBoard board)
        {
            string totalCase = "";

            // Đưa các trường hợp trên bàn cờ vào chuỗi totalCase theo hàng và cột
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                    totalCase += board.Matrix[i, j];
                totalCase += ";";

                for (int j = 0; j < size; j++)
                    totalCase += board.Matrix[j, i];
                totalCase += ";";
            }

            // Đưa các trường hợp trên đường chéo vào chuỗi
            for (int i = 0; i < size - 4; i++)
            {
                for (int j = 0; j < size - i; j++)
                    totalCase += board.Matrix[j, i + j];
                totalCase += ";";
            }

            for (int i = size - 5; i > 0; i--)
            {
                for (int j = 0; j < size - i; j++)
                    totalCase += board.Matrix[i + j, j];
                totalCase += ";";
            }

            for (int i = 4; i < size; i++)
            {
                for (int j = 0; j <= i; j++)
                    totalCase += board.Matrix[i - j, j];
                totalCase += ";";
            }

            for (int i = size - 5; i > 0; i--)
            {
                for (int j = size - 1; j >= i; j--)
                    totalCase += board.Matrix[j, i + size - j - 1];
                totalCase += ";\n";
            }

            //Đoạn này tham khảo code
            Regex regex1, regex2;
            int temp = 0;
            for (int i = 0; i < caseX.Length; i++)
            {
                regex1 = new Regex(caseX[i]);
                regex2 = new Regex(caseO[i]);
                if (computerSymbol == 'o')
                {
                    temp += casePoint[i] * regex2.Matches(totalCase).Count;
                    temp -= casePoint[i] * regex1.Matches(totalCase).Count;
                }
                else
                {
                    temp -= casePoint[i] * regex2.Matches(totalCase).Count;
                    temp += casePoint[i] * regex1.Matches(totalCase).Count;
                }
            }
            return temp;
        }

        private int MinValue(ref GomokuBoard board, int alpha, int beta, int depth)
        {
            int val = Eval(ref board);

            if (depth >= maxDepth || Math.Abs(val) > 3000)
                return val;

            evalueteBoard(ref board, playerSymbol);
            List<Element> list = new List<Element>();

            for (int i = 0; i < branch; i++)
            {
                list.Add(findMax());
                if ( list[i].Value > 1550)
                    break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = playerSymbol;
                beta = Math.Min(beta, MaxValue(ref board, alpha, beta, depth + 1));
                board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = ' ';

                if (alpha >= beta)
                    break;
            }
            return beta;
        }

        private int MaxValue(ref GomokuBoard board, int alpha, int beta, int depth)
        {
            int val = Eval(ref board);
            if (depth >= maxDepth || Math.Abs(val) > 3000)
                return val;
            evalueteBoard(ref board, computerSymbol);

            List<Element> list = new List<Element>();
            for (int i = 0; i < branch; i++)
            {
                list.Add(findMax());
                if (list[i].Value > 1550)
                    break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = computerSymbol;
                alpha = Math.Max(alpha, MinValue(ref board, alpha, beta, depth + 1));
                board.Matrix[(int)list[i].Position.X,(int)list[i].Position.Y] = ' ';

                if (alpha > beta)
                    break;
            }
            return alpha;
        }

        private int NegaScout(ref GomokuBoard board, Element element, int alpha, int beta, int depth)
        {
            int val = Eval(ref board);

            if (depth >= maxDepth || Math.Abs(val) > 3000)
                return val;

            int b = beta;
            int a;

            if (depth % 2 == 0)
                evalueteBoard(ref board, playerSymbol);
            else
                evalueteBoard(ref board, computerSymbol);

            List<Element> list = new List<Element>();

            for (int i = 0; i < branch; i++)
            {
                list.Add(findMax());
                if ( list[i].Value > 1538)
                    break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (depth % 2 == 0)
                    board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = computerSymbol;
                else
                    board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = playerSymbol;

                a = -NegaScout(ref board, list[i], -b, -alpha, depth + 1);

                if (a > alpha && a < beta && i != 0)
                    a = -NegaScout(ref board, list[i], -beta, -alpha, depth + 1);

                board.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = ' ';
                alpha = Math.Max(a, alpha);

                if (alpha >= beta)
                    break;
                b = alpha + 1;
            }
            return alpha;
        }


        public Point Solve(ref GomokuBoard board, char Player)
        {
            Point temp;

            GomokuBoard b = new GomokuBoard();

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    b.Matrix[i, j] = board.Matrix[i, j];

            computerSymbol = Player;
            if (Player == 'o')
                playerSymbol = 'x';
            else
                playerSymbol = 'o';

            evalueteBoard(ref b, computerSymbol);

            List<Element> list = new List<Element>();

            for (int i = 0; i < branch; i++)
            {
                list.Add(findMax());
                if (list[i].Value > 1550)
                    break;
            }

            int maxTemp = -MAX;

            List<Element> ListChoose = new List<Element>();

            for (int i = 0; i < list.Count; i++)
            {
                temp = new Point(list[i].Position.X,list[i].Position.Y);

                b.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = computerSymbol;
                int t = MinValue(ref b, -MAX, MAX, 0);

                if (maxTemp < t)
                {
                    maxTemp = t;
                    ListChoose.Clear();
                    ListChoose.Add(list[i]);
                }
                else if (maxTemp == t)
                {
                    ListChoose.Add(list[i]);
                }
                b.Matrix[(int)list[i].Position.X, (int)list[i].Position.Y] = ' ';
            }

            return ListChoose[rand.Next(0, ListChoose.Count)].Position;
        }

    }
}
