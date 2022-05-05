using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public static class Globals
    {
        public static int cols { get; set; }
        public static int rows { get; set; }
        public static int resolution = 40;
        public static int[,] grid = MakeArr(cols, rows);


        public static int[,] MakeArr(int cols, int rows)
        {
            int[,] arr = new int[cols, rows];
            return arr;
        }


    }


}
