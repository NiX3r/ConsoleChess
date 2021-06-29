using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleChess.ChessEnums;

namespace ConsoleChess
{
    class CheckFigureInstance
    {

        public int PosX { get; set; }
        public int PoxY { get; set; }
        public Figure CheckFigure { get; set; }

        public CheckFigureInstance(int x, int y, Figure figure)
        {
            PosX = x;
            PoxY = y;
            CheckFigure = figure;
        }

    }
}
