using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ConsoleChess.ChessEnums;

namespace ConsoleChess
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessLogics chl = new ChessLogics();

            while (true)
            {
                string line = Console.ReadLine();
                MoveReturns mr =  chl.Move(line.Split(' ')[0], line.Split(' ')[1]);

                switch (mr)
                {
                    case MoveReturns.BadInputParameters:
                        Console.Write("Bad input");
                        break;
                    case MoveReturns.BoxIsEmpty:
                        Console.Write("Box is empty");
                        break;
                    case MoveReturns.GameAlreadyEnded:
                        Console.Write("Game already ended");
                        break;
                    case MoveReturns.MoveDone:
                        Console.Write("Move done successfully");
                        break;
                    case MoveReturns.NothingHappened:
                        Console.Write("Developer returns");
                        break;
                    case MoveReturns.TryToCheatMove:
                        Console.Write("Hey cheater! You try to cheat! Move again");
                        break;
                    case MoveReturns.TryToPlayAsEnemy:
                        Console.Write("Hey why are u trying to play as enemy? Move again");
                        break;
                    case MoveReturns.BlackCheckWhite:
                        Console.Write("Black check white!");
                        break;
                    case MoveReturns.WhiteCheckBlack:
                        Console.Write("White check black!");
                        break;
                }

                Thread.Sleep(1500);

                Console.Clear();
                chl.PrintBoard();

            }
        }
    }
}
