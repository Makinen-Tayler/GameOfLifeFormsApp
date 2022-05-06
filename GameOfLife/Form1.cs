using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GameOfLife
{
    public partial class GameWindow : Form
    {


        public List<Rectangle> listRec = new List<Rectangle>();
        Timer MyTimer = new Timer();

        public GameWindow()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void GameWindow_Load(object sender, EventArgs e)
        {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            Globals.cols = this.Width / Globals.resolution;
            Globals.rows = this.Height / Globals.resolution;


            Globals.grid = Globals.MakeArr(Globals.cols, Globals.rows);
            Random rand = new Random();
            for (var i = 0; i < Globals.cols; i++)
            {
                for (var j = 0; j < Globals.rows; j++)
                {
                    Globals.grid[i, j] = rand.Next(0, 2);
                }
            }
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();            
        }

        private void GameWindow_Paint(object sender, PaintEventArgs e)
        {


            Graphics g = e.Graphics;
            Brush drawingBrush = new SolidBrush(Color.Black);
            Pen drawingPen = new Pen(Color.White);

            for (int i = 0; i < Globals.cols; i++)
            {
                for (int j = 0; j < Globals.rows; j++)
                {
                    var x = i * Globals.resolution;
                    var y = j * Globals.resolution;
                    Rectangle rect = new Rectangle(x, y, Globals.resolution - 1, Globals.resolution - 1);

                    if (Globals.grid[i,j] == 1)
                    {
                        //g.DrawRectangle(drawingPen, rect);
                        g.FillRectangle(drawingBrush, rect);

                    }
                    else
                    {
                        Brush drawingBrush2 = new SolidBrush(Color.DarkOliveGreen);
                        //g.DrawRectangle(drawingPen, rect);
                        g.FillRectangle(drawingBrush2, rect);
                    }


                }
            }

            var next = Globals.MakeArr(Globals.cols, Globals.rows);

            for (int i = 0; i < Globals.cols; i++)
            {
                for (int j = 0; j < Globals.rows; j++)
                {
                    var sum = 0;
                    var state = Globals.grid[i,j];
                    // Count live neighbors!
                    var neighbors = countNeighbors(Globals.grid, i, j);

                    if (state == 0 && neighbors == 3)
                    {
                        next[i,j] = 1;
                    }
                    else if (state == 1 && (neighbors < 2 || neighbors > 3))
                    {
                        next[i,j] = 0;
                    }
                    else
                    {
                        next[i,j] = state;
                    }
                }
            }
            Globals.grid = next;
            MyTimer.Interval = 125; //refresh

            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();
        }

        public int countNeighbors(int[,] grid, int x, int y)
        {
            var sum = 0;
            for(var i = -1; i < 2; i++)
            {
                for(var j = -1; j < 2; j++)
                {
                    var col = (x + i + Globals.cols) % Globals.cols;
                    var row = (y + j + Globals.rows) % Globals.rows;
                    sum += grid[col,row];
                }
            }
            sum -= grid[x, y];
            return sum;
        }
    }
}
