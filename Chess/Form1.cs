using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Chess
{
    public partial class Form1 : Form
    {
        public Image chessSprites;
        public int[,] map = new int[8, 8]
        {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
        };

        public Button[,] butts = new Button[8, 8];

        public static int currPlayer;

        public bool check = false, checkMate;

        public Button prevButton;

        public bool isMoving = false;

        public Form1()
        {
            InitializeComponent();

            chessSprites = new Bitmap("D:\\Chess\\Chess\\Sprites\\chess.png");

            //button1.BackgroundImage = part;

            Init();
        }

        public void Init()
        {
            map = new int[8, 8]
            {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
            };

            currPlayer = 1;
            CreateMap();
        }

        public void CreateMap()
        {
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j] = new Button();

                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j*50,i*50);

                    switch (map[i, j]/10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0+150*(map[i,j]%10-1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;
                        case 2:
                            Image part1 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part1);
                            g1.DrawImage(chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (map[i, j] % 10-1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part1;
                            break;
                    }
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(OnFigurePress);
                    this.Controls.Add(butt);

                    butts[i, j] = butt;
                }
            }
            CloseSteps();
        }
        
        public void OnFigurePress(object sender, EventArgs e)
        {
            if (prevButton != null)
                CloseSteps();

            Button pressedButton = sender as Button;

            //pressedButton.Enabled = false;
            
            if (map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] != 0 && map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]/10 == currPlayer)
            {
                CloseSteps();
                pressedButton.BackColor = Color.Red;
                DeactivateAllButtons();
                pressedButton.Enabled = true;
                ShowSteps(pressedButton.Location.Y / 50, pressedButton.Location.X / 50, map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50]);
                if (isMoving)
                {
                    CloseSteps();
                    ActivateAllButtons();
                    isMoving = false;
                }
                else
                    isMoving = true;
            }else
            {
                if (isMoving)
                {
                    map[pressedButton.Location.Y / 50, pressedButton.Location.X / 50] = map[prevButton.Location.Y / 50, prevButton.Location.X / 50];
                    map[prevButton.Location.Y / 50, prevButton.Location.X / 50] = 0;
                    pressedButton.BackgroundImage = prevButton.BackgroundImage;
                    prevButton.BackgroundImage = null;
                    isMoving = false;
                    CloseSteps();
                    ActivateAllButtons();
                    SwitchPlayer();
                    

                    if (checkBoard(map) == 1)
                        MessageBox.Show("Шах чорному гравцеві", "Повідомлення");

                    else if(checkBoard(map) == 2)
                        MessageBox.Show("Шах білому гравцеві", "Повідомлення");
                }
            }
           
            prevButton = pressedButton;
        }

        public void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
        {
            int dir = currPlayer == 1 ? 1 : -1;
            switch (currFigure%10)
            {
                case 6:
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                        {
                            if(IcurrFigure == 1 && dir == 1)
                            {
                                butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                                butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                            }
                            else if(IcurrFigure == 6 && dir == -1)
                            {
                                butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                                butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                            }
                            else
                            {
                                butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                                butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                            }
                        }
                    }
                    
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure + 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != currPlayer)
                        {
                            if (map[IcurrFigure + 1 * dir, JcurrFigure + 1] % 10 == 1)
                            {
                                check = true; 
                            }
                            else
                            {
                                butts[IcurrFigure + 1 * dir, JcurrFigure + 1].BackColor = Color.Yellow;
                                butts[IcurrFigure + 1 * dir, JcurrFigure + 1].Enabled = true;
                            }
                        }
                    }
                    if (InsideBorder(IcurrFigure + 1 * dir, JcurrFigure - 1))
                    {
                        if (map[IcurrFigure + 1 * dir, JcurrFigure - 1] != 0 && map[IcurrFigure + 1 * dir, JcurrFigure - 1] / 10 != currPlayer)
                        {
                            if (map[IcurrFigure + 1 * dir, JcurrFigure - 1] % 10 == 1)
                            {
                                check = true; 
                            }
                            else
                            {
                                butts[IcurrFigure + 1 * dir, JcurrFigure - 1].BackColor = Color.Yellow;
                                butts[IcurrFigure + 1 * dir, JcurrFigure - 1].Enabled = true;
                            }
                        }
                    }
                    break;
                case 5:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    break;
                case 3:
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 2:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                    ShowDiagonal(IcurrFigure, JcurrFigure);
                    break;
                case 1:
                    ShowVerticalHorizontal(IcurrFigure, JcurrFigure,true);
                    ShowDiagonal(IcurrFigure, JcurrFigure,true);
                    break;
                case 4:
                    ShowHorseSteps(IcurrFigure, JcurrFigure);
                    break;
            }
        }

        public void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
        {
            if (InsideBorder(IcurrFigure - 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure - 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure - 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure + 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure + 1);
            }
            if (InsideBorder(IcurrFigure + 2, JcurrFigure - 1))
            {
                DeterminePath(IcurrFigure + 2, JcurrFigure - 1);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure + 2))
            {
                DeterminePath(IcurrFigure +1, JcurrFigure + 2);
            }
            if (InsideBorder(IcurrFigure - 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure - 1, JcurrFigure -2);
            }
            if (InsideBorder(IcurrFigure + 1, JcurrFigure - 2))
            {
                DeterminePath(IcurrFigure +1, JcurrFigure -2);
            }
        }

        public void DeactivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    butts[i, j].Enabled = false;
                }
            }
        }

        public void ActivateAllButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    
                    butts[i, j].Enabled = true;
                }
            }
        }

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure,bool isOneStep=false)
        {
            int j = JcurrFigure + 1;
            for(int i = IcurrFigure-1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j <7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        public void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure,bool isOneStep=false)
        {
            //int e, h;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (InsideBorder(i, JcurrFigure))
                {
                    if (!DeterminePath(i, JcurrFigure))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure + 1; j < 8; j++)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
            for (int j = JcurrFigure - 1; j >= 0; j--)
            {
                if (InsideBorder(IcurrFigure, j))
                {
                    if (!DeterminePath(IcurrFigure, j))
                        break;
                }
                if (isOneStep)
                    break;
            }
        }

        public bool DeterminePath(int IcurrFigure,int j)
        {
            if (map[IcurrFigure, j] == 0)
            {
                butts[IcurrFigure, j].BackColor = Color.Yellow;
                butts[IcurrFigure, j].Enabled = true;
            }
            else
            {
                if (map[IcurrFigure, j] / 10 != currPlayer)
                {
                    if (map[IcurrFigure, j] % 10 == 1)
                    {
                        check = true;
                    }
                    else
                    {
                        butts[IcurrFigure, j].BackColor = Color.Yellow;
                        butts[IcurrFigure, j].Enabled = true;
                        check = false;
                    }    
                }
                return false;
            }
            return true;
        }

        public static bool InsideBorder(int ti,int tj)
        {
            if (ti >= 8 || tj >= 8 || ti < 0 || tj < 0)
                return false;
            return true;
        }

        public void CloseSteps()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if((i + j) % 2 == 0)
                    {
                        butts[i, j].BackColor = Color.Gray;
                    }
                    else
                    {
                        butts[i, j].BackColor = Color.White;
                    }
                    
                }
            }
        }

        public void SwitchPlayer()
        {
            if (currPlayer == 1)
                currPlayer = 2;
            else currPlayer = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            Init();
        }

        public static int checkBoard(int[,] board)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board[i, j] == 21)
                    {
                        if (lookFor4(board, 14, i, j))
                            return 1;
                        if (lookFor6(board, 16, i, j))
                            return 1;
                        if (lookFor5(board, 15, i, j))
                            return 1;
                        if (lookFor3(board, 13, i, j))
                            return 1;
                        if (lookFor2(board, 12, i, j))
                            return 1;
                        if (lookFor1(board, 11, i, j))
                            return 1;
                    }

                    if (board[i, j] == 11)
                    {
                        if (lookFor4(board, 24, i, j))
                            return 2;
                        if (lookFor6(board, 26, i, j))
                            return 2;
                        if (lookFor5(board, 25, i, j))
                            return 2;
                        if (lookFor3(board, 23, i, j))
                            return 2;
                        if (lookFor2(board, 22, i, j))
                            return 2;
                        if (lookFor1(board, 11, i, j))
                            return 2;
                    }
                }
            }
            return 0;
        }

        public static bool lookFor1(int[,] board, int c, int i, int j)
        {
            int[] x = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] y = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int k = 0; k < 8; k++)
            {
                int m = i + x[k];
                int n = j + y[k];

                if (InsideBorder(m, n) && board[m, n] == c)
                    return true;
            }
            return false;
        }

        public static bool lookFor2(int[,] board, int c, int i, int j)
        {
            if (lookFor3(board, c, i, j) || lookFor5(board, c, i, j))
                return true;

            return false;
        }

        public static bool lookFor3(int[,] board, int c, int i, int j)
        {
            int k = 0;
            while (InsideBorder(i + ++k, j + k))
            {
                if (board[i + k, j + k] == c)
                    return true;
                if (board[i + k, j + k] != 0)
                    break;
            }
            k = 0;
            while (InsideBorder(i + ++k, j - k))
            {
                if (board[i + k, j - k] == c)
                    return true;
                if (board[i + k, j - k] != 0)
                    break;
            }
            k = 0;
            while (InsideBorder(i - ++k, j + k))
            {
                if (board[i - k, j + k] == c)
                    return true;
                if (board[i - k, j + k] != 0)
                    break;
            }
            k = 0;
            while (InsideBorder(i - ++k, j - k))
            {
                if (board[i - k, j - k] == c)
                    return true;
                if (board[i - k, j - k] != 0)
                    break;
            }
            return false;
        }

        public static bool lookFor5(int[,] board, int c, int i, int j)
        {
            int k = 0;
            while (InsideBorder(i + ++k, j))
            {
                if (board[i + k, j] == c)
                    return true;
                if (board[i + k, j] != 0)
                    break;
            }

            k = 0;
            while (InsideBorder(i + --k, j))
            {
                if (board[i + k, j] == c)
                    return true;
                if (board[i + k, j] != 0)
                    break;
            }

            k = 0;
            while (InsideBorder(i, j + ++k))
            {
                if (board[i, j + k] == c)
                    return true;
                if (board[i, j + k] != 0)
                    break;
            }

            k = 0;
            while (InsideBorder(i, j + --k))
            {
                if (board[i, j + k] == c)
                    return true;
                if (board[i, j + k] != 0)
                    break;
            }
            return false;
        }

        public static bool lookFor4(int[,] board, int c, int i, int j)
        {
            int[] x = { 2, 2, -2, -2, 1, 1, -1, -1 };
            int[] y = { 1, -1, 1, -1, 2, -2, 2, -2 };

            for (int k = 0; k < 8; k++)
            {
                int m = i + x[k];
                int n = j + y[k];

                if (InsideBorder(m, n) && board[m, n] == c)
                    return true;
            }
            return false;
        }

        public static bool lookFor6(int[,] board, int c, int i, int j)
        {
            if (currPlayer == 1)
            {
                if (InsideBorder(i + 1, j - 1) && board[i + 1, j - 1] == 26)
                    return true;

                if (InsideBorder(i + 1, j + 1) && board[i + 1, j + 1] == 26)
                    return true;
            }
            else
            {
                if (InsideBorder(i - 1, j - 1) && board[i - 1, j - 1] == 16)
                    return true;
                if (InsideBorder(i - 1, j + 1) && board[i - 1, j + 1] == 16)
                    return true;
            }
            return false;
        }
    }
}
