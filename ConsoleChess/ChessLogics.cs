using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleChess.ChessEnums;

namespace ConsoleChess
{
    class ChessLogics
    {
        
        private Figure[,] board;
        private Status status;
        List<CheckFigureInstance> checkFigures;

        public ChessLogics()
        {

            board = new Figure[8, 8];
            status = Status.WhitePlaying;
            checkFigures = new List<CheckFigureInstance>();

            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {
                    board[x, y] = Figure.EmptyBox;
                }
            }

            // Setup black board - on top
            for (int x = 0; x < 8; x++)
                board[x, 1] = Figure.BlackPawn;
            board[0, 0] = board[7, 0] = Figure.BlackRook;
            board[1, 0] = board[6, 0] = Figure.BlackKnight;
            board[2, 0] = board[5, 0] = Figure.BlackBishop;
            board[3, 0] = Figure.BlackQueen;
            board[4, 0] = Figure.BlackKing;

            // Setup white board - on bottom
            for (int x = 0; x < 8; x++)
                board[x, 6] = Figure.WhitePawn;
            board[0, 7] = board[7, 7] = Figure.WhiteRook;
            board[1, 7] = board[6, 7] = Figure.WhiteKnight;
            board[2, 7] = board[5, 7] = Figure.WhiteBishop;
            board[3, 7] = Figure.WhiteQueen;
            board[4, 7] = Figure.WhiteKing;

            PrintBoard();

        }
        
        public Status GetStatus()
        {
            return status;
        }

        public void PrintBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    switch (board[x, y])
                    {
                        case Figure.EmptyBox:
                            Console.Write("[00] ");
                            break;
                        case Figure.WhitePawn:
                            Console.Write("[wP] ");
                            break;
                        case Figure.BlackPawn:
                            Console.Write("[bP] ");
                            break;
                        case Figure.WhiteRook:
                            Console.Write("[wR] ");
                            break;
                        case Figure.BlackRook:
                            Console.Write("[bR] ");
                            break;
                        case Figure.WhiteKnight:
                            Console.Write("[wK] ");
                            break;
                        case Figure.BlackKnight:
                            Console.Write("[bK] ");
                            break;
                        case Figure.WhiteBishop:
                            Console.Write("[wB] ");
                            break;
                        case Figure.BlackBishop:
                            Console.Write("[bB] ");
                            break;
                        case Figure.WhiteQueen:
                            Console.Write("[wQ] ");
                            break;
                        case Figure.BlackQueen:
                            Console.Write("[bQ] ");
                            break;
                        case Figure.WhiteKing:
                            Console.Write("[wK] ");
                            break;
                        case Figure.BlackKing:
                            Console.Write("[bK] ");
                            break;
                    }
                }
                Console.WriteLine("\n");
            }
        }

        public MoveReturns Move(string from, string to)
        {

            if (from.Count() != 2 && to.Count() != 2)
                return MoveReturns.BadInputParameters;

            int fromX = 0, fromY = 0, toX = 0, toY = 0;
            try
            {
                switch (from.ToArray()[0])
                {
                    case 'a':
                        fromX = 0;
                        break;
                    case 'b':
                        fromX = 1;
                        break;
                    case 'c':
                        fromX = 2;
                        break;
                    case 'd':
                        fromX = 3;
                        break;
                    case 'e':
                        fromX = 4;
                        break;
                    case 'f':
                        fromX = 5;
                        break;
                    case 'g':
                        fromX = 6;
                        break;
                    case 'h':
                        fromX = 7;
                        break;
                }
                switch (from.ToArray()[1])
                {
                    case '1':
                        fromY = 7;
                        break;
                    case '2':
                        fromY = 6;
                        break;
                    case '3':
                        fromY = 5;
                        break;
                    case '4':
                        fromY = 4;
                        break;
                    case '5':
                        fromY = 3;
                        break;
                    case '6':
                        fromY = 2;
                        break;
                    case '7':
                        fromY = 1;
                        break;
                    case '8':
                        fromY = 0;
                        break;
                }
                switch (to.ToArray()[0])
                {
                    case 'a':
                        toX = 0;
                        break;
                    case 'b':
                        toX = 1;
                        break;
                    case 'c':
                        toX = 2;
                        break;
                    case 'd':
                        toX = 3;
                        break;
                    case 'e':
                        toX = 4;
                        break;
                    case 'f':
                        toX = 5;
                        break;
                    case 'g':
                        toX = 6;
                        break;
                    case 'h':
                        toX = 7;
                        break;
                }
                switch (to.ToArray()[1])
                {
                    case '1':
                        toY = 7;
                        break;
                    case '2':
                        toY = 6;
                        break;
                    case '3':
                        toY = 5;
                        break;
                    case '4':
                        toY = 4;
                        break;
                    case '5':
                        toY = 3;
                        break;
                    case '6':
                        toY = 2;
                        break;
                    case '7':
                        toY = 1;
                        break;
                    case '8':
                        toY = 0;
                        break;
                }
            }
            catch
            {
                return MoveReturns.BadInputParameters;
            }

            MoveReturns output = Move(fromX, fromY, toX, toY);
            return output;

        }

        private MoveReturns Move(int fromX, int fromY, int toX, int toY)
        {
            if (status == Status.WhitePlaying || status == Status.BlackCheckWhite)
            {
                if (status == Status.WhitePlaying)
                {
                    if (board[fromX, fromY] == Figure.WhitePawn)
                    {
                        if (PawnMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhiteRook)
                    {
                        if (RookMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhiteBishop)
                    {
                        if (BishopMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhiteKnight)
                    {
                        if (KnightMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhiteKing)
                    {
                        if (KingMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhiteQueen)
                    {
                        if (QueenMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.BlackPawn ||
                        board[fromX, fromY] == Figure.BlackRook ||
                        board[fromX, fromY] == Figure.BlackKnight ||
                        board[fromX, fromY] == Figure.BlackBishop ||
                        board[fromX, fromY] == Figure.BlackQueen ||
                        board[fromX, fromY] == Figure.BlackKing)
                        return MoveReturns.TryToPlayAsEnemy;
                    return MoveReturns.BoxIsEmpty;
                }
                else
                {
                    return MoveReturns.NothingHappened;
                }
            }
            else if (status == Status.BlackPlaying || status == Status.WhiteCheckBlack)
            {
                if (status == Status.BlackPlaying)
                {
                    if (board[fromX, fromY] == Figure.BlackPawn)
                    {
                        if (PawnMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if(board[fromX, fromY] == Figure.BlackRook)
                    {
                        if (RookMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.BlackBishop)
                    {
                        if (BishopMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.BlackKnight)
                    {
                        if (KnightMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.BlackKing)
                    {
                        if (KingMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.BlackQueen)
                    {
                        if (QueenMove(fromX, fromY, toX, toY))
                        {
                            CheckStatus();
                            switch (status)
                            {
                                case Status.WhiteCheckBlack:
                                    return MoveReturns.WhiteCheckBlack;
                                case Status.BlackCheckWhite:
                                    return MoveReturns.BlackCheckWhite;
                                default:
                                    return MoveReturns.MoveDone;
                            }
                        }
                        else
                            return MoveReturns.TryToCheatMove;
                    }
                    if (board[fromX, fromY] == Figure.WhitePawn ||
                        board[fromX, fromY] == Figure.WhiteRook ||
                        board[fromX, fromY] == Figure.WhiteKnight ||
                        board[fromX, fromY] == Figure.WhiteBishop ||
                        board[fromX, fromY] == Figure.WhiteQueen ||
                        board[fromX, fromY] == Figure.WhiteKing)
                        return MoveReturns.TryToPlayAsEnemy;
                    return MoveReturns.BoxIsEmpty;
                }
                else
                {
                    return MoveReturns.NothingHappened;
                }
            }
            else
            {
                return MoveReturns.GameAlreadyEnded;
            }
        }

        private bool CheckOutIndex(int x, int y)
        {
            if (x >= 0 && x <= 7 && y >= 0 && y <=7)
                return false;
            return true;
        }

        private bool CheckCheckmate(int kingX, int kingY)
        {

            if(countCheckFigure == 1)
            {

            }
            else if (countCheckFigure > 1)
            {

            }
            else
            {
                // something really messed up
            }

            return false;
        }

        private void CheckStatus()
        {
            int wKingX = -1, wKingY = -1, bKingX = - 1, bKingY = - 1;
            for(int y = 0; y < 8; y++)
            {
                for(int x = 0; x < 8; x++)
                {

                    if(board[x, y] == Figure.WhiteKing)
                    {
                        wKingX = x;
                        wKingY = y;
                        // checks pawns check
                        if (!CheckOutIndex(x - 1, y + 1)) { if (board[x - 1, y + 1] == Figure.BlackPawn) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-1, y+1, board[x-1, y+1])); } }
                        if (!CheckOutIndex(x + 1, y + 1)) { if (board[x + 1, y + 1] == Figure.BlackPawn) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+1, y+1, board[x+1, y+1])); } }
                        // checks + lines
                        for(int z = x - 1; z > - 1; z--)
                        {
                            if(!CheckOutIndex(z, y)) { if (board[z, y] == Figure.BlackQueen || board[z, y] == Figure.BlackRook) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(z, y, board[z, y])); } }
                        }
                        for(int z = y - 1; z > - 1; z--)
                        {
                            if (!CheckOutIndex(x, z)) { if (board[x, z] == Figure.BlackQueen || board[x, z] == Figure.BlackRook) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x, z, board[x, z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(z, y)) { if (board[z, y] == Figure.BlackQueen || board[z, y] == Figure.BlackRook) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(z, y, board[z, y])); } }
                        }
                        for (int z = y + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x, z)) { if (board[x, z] == Figure.BlackQueen || board[x, z] == Figure.BlackRook) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x, z, board[x, z])); } }
                        }
                        // checks X lines
                        for(int z = x - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(x - z, y - z)) { if (board[x - z, y - z] == Figure.BlackQueen || board[x - z, y - z] == Figure.BlackBishop) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-z, y-z, board[x-z, y-z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x + z, y - z)) { if (board[x + z, y - z] == Figure.BlackQueen || board[x + z, y - z] == Figure.BlackBishop) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+z, y-z, board[x+z, y-z])); } }
                        }
                        for (int z = x - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(x - z, y + z)) { if (board[x - z, y + z] == Figure.BlackQueen || board[x - z, y + z] == Figure.BlackBishop) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-z, y+z, board[x-z, y+z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x + z, y + z)) { if (board[x + z, y + z] == Figure.BlackQueen || board[x + z, y + z] == Figure.BlackBishop) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+z, y+z, board[x+z, y+z])); } }
                        }
                        // checks knight moves
                        if (!CheckOutIndex(x - 2, y + 1)) { if (board[x - 2, y + 1] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-2, y+1, board[x-2, y+1])); } }
                        if (!CheckOutIndex(x - 2, y - 1)) { if (board[x - 2, y - 1] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-2, y-1, board[x-2, y-1])); } }
                        if (!CheckOutIndex(x - 1, y - 2)) { if (board[x - 1, y - 2] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-1, y-2, board[x-1, y-2])); } }
                        if (!CheckOutIndex(x + 1, y - 2)) { if (board[x + 1, y - 2] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+1, y-2, board[x+1, y-2])); } }
                        if (!CheckOutIndex(x + 2, y + 1)) { if (board[x + 2, y + 1] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+2, y+1, board[x+2, y+1])); } }
                        if (!CheckOutIndex(x + 2, y - 1)) { if (board[x + 2, y - 1] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+2, y-1, board[x+2, y-1])); } }
                        if (!CheckOutIndex(x + 1, y + 2)) { if (board[x + 1, y + 2] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x+1, y+2, board[x+1, y+2])); } }
                        if (!CheckOutIndex(x - 1, y + 2)) { if (board[x - 1, y + 2] == Figure.BlackKnight) { status = Status.BlackCheckWhite; checkFigures.Add(new CheckFigureInstance(x-1, y+2, board[x-1, y+2])); } }
                    }

                    if (board[x, y] == Figure.BlackKing)
                    {
                        bKingX = x;
                        bKingY = y;
                        // checks pawns check
                        if (!CheckOutIndex(x - 1, y - 1)) { if (board[x - 1, y - 1] == Figure.WhitePawn) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x-1, y-1, board[x-1, y-1])); } }
                        if (!CheckOutIndex(x + 1, y - 1)) { if (board[x + 1, y - 1] == Figure.WhitePawn) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x+1, y-1, board[x+1, y-1])); } }
                        // checks + lines
                        for (int z = x - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(z, y)) { if (board[z, y] == Figure.WhiteQueen || board[z, y] == Figure.WhiteRook) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(z, y, board[z, y])); } }
                        }
                        for (int z = y - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(x, z)) { if (board[x, z] == Figure.WhiteQueen || board[x, z] == Figure.WhiteRook) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x, z, board[x, z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(z, y)) { if (board[z, y] == Figure.WhiteQueen || board[z, y] == Figure.WhiteRook) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(z, y, board[z, y])); } }
                        }
                        for (int z = y + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x, z)) { if (board[x, z] == Figure.WhiteQueen || board[x, z] == Figure.WhiteRook) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x, z, board[x, z])); } }
                        }
                        // checks X lines
                        for (int z = x - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(x - z, y - z)) { if (board[x - z, y - z] == Figure.WhiteQueen || board[x - z, y - z] == Figure.WhiteBishop) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - z, y - z, board[x - z, y - z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x + z, y - z)) { if (board[x + z, y - z] == Figure.WhiteQueen || board[x + z, y - z] == Figure.WhiteBishop) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + z, y - z, board[x + z, y - z])); } }
                        }
                        for (int z = x - 1; z > -1; z--)
                        {
                            if (!CheckOutIndex(x - z, y + z)) { if (board[x - z, y + z] == Figure.WhiteQueen || board[x - z, y + z] == Figure.WhiteBishop) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - z, y + z, board[x - z, y + z])); } }
                        }
                        for (int z = x + 1; z < 8; z++)
                        {
                            if (!CheckOutIndex(x + z, y + z)) { if (board[x + z, y + z] == Figure.WhiteQueen || board[x + z, y + z] == Figure.WhiteBishop) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + z, y + z, board[x + z, y + z])); } }
                        }
                        // checks knight moves
                        if (!CheckOutIndex(x - 2, y + 1)) { if (board[x - 2, y + 1] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - 2, y + 1, board[x - 2, y + 1])); } }
                        if (!CheckOutIndex(x - 2, y - 1)) { if (board[x - 2, y - 1] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - 2, y - 1, board[x - 2, y + 1])); } }
                        if (!CheckOutIndex(x - 1, y - 2)) { if (board[x - 1, y - 2] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - 1, y - 2, board[x - 1, y - 2])); } }
                        if (!CheckOutIndex(x + 1, y - 2)) { if (board[x + 1, y - 2] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + 1, y - 2, board[x + 1, y - 2])); } }
                        if (!CheckOutIndex(x + 2, y + 1)) { if (board[x + 2, y + 1] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + 2, y + 1, board[x + 2, y + 1])); } }
                        if (!CheckOutIndex(x + 2, y - 1)) { if (board[x + 2, y - 1] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + 2, y - 1, board[x + 2, y - 1])); } }
                        if (!CheckOutIndex(x + 1, y + 2)) { if (board[x + 1, y + 2] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x + 1, y + 2, board[x + 1, y + 2])); } }
                        if (!CheckOutIndex(x - 1, y + 2)) { if (board[x - 1, y + 2] == Figure.WhiteKnight) { status = Status.WhiteCheckBlack; checkFigures.Add(new CheckFigureInstance(x - 1, y + 2, board[x - 1, y + 2])); } }
                    }

                    // checks if it is not checkmate
                    if (status == Status.BlackCheckWhite && CheckCheckmate(wKingX, wKingY))
                    {
                        
                    }
                    if(status == Status.WhiteCheckBlack && CheckCheckmate(bKingX, bKingY))
                    {

                    }

                }
            }

            if (status == Status.WhitePlaying)
                status = Status.BlackPlaying;
            else
                status = Status.WhitePlaying;
        }

        private int GetAbsoluteValue(int value)
        {
            if (value < 0)
                return (value + (-2 * value));
            else
                return value;
        }

        private bool QueenMove(int fromX, int fromY, int toX, int toY)
        {
            bool canMove = true;
            if (fromX == toX || fromY == toY)
            {
                // Check if figure can move - nothing in way
                if (fromX == toX)
                {
                    if (fromY < toY)
                    {
                        for (int y = fromY + 1; y < toY; y++)
                        {
                            if (board[toX, y] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int y = toY + 1; y < fromY; y++)
                        {
                            if (board[toX, y] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (fromX < toX)
                    {
                        for (int x = fromX + 1; x < toX; x++)
                        {
                            if (board[x, toY] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int x = toX + 1; x < fromX; x++)
                        {
                            if (board[x, toY] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                    }
                }

                if (canMove)
                {
                    if (board[fromX, fromY] == Figure.WhiteQueen)
                    {
                        board[fromX, fromY] = Figure.EmptyBox;
                        // Checks toX and toY figure
                        if (board[toX, toY] == Figure.WhiteBishop ||
                            board[toX, toY] == Figure.WhiteKing ||
                            board[toX, toY] == Figure.WhiteKnight ||
                            board[toX, toY] == Figure.WhitePawn ||
                            board[toX, toY] == Figure.WhiteQueen ||
                            board[toX, toY] == Figure.WhiteRook)
                        {
                            return false;
                        }
                        board[toX, toY] = Figure.WhiteQueen;
                        return true;
                    }
                    if (board[fromX, fromY] == Figure.BlackQueen)
                    {
                        board[fromX, fromY] = Figure.EmptyBox;
                        // Checks toX and toY figure
                        if (board[toX, toY] == Figure.BlackBishop ||
                            board[toX, toY] == Figure.BlackKing ||
                            board[toX, toY] == Figure.BlackKnight ||
                            board[toX, toY] == Figure.BlackPawn ||
                            board[toX, toY] == Figure.BlackQueen ||
                            board[toX, toY] == Figure.BlackRook)
                        {
                            return false;
                        }
                        board[toX, toY] = Figure.BlackQueen;
                        return true;
                    }
                    return false;
                }
            }
            if(GetAbsoluteValue(fromX - toX) == GetAbsoluteValue(fromY - toY))
            {
                // Move left up
                if (fromX > toX && fromY > toY)
                {
                    for (int i = toX + 1; i < fromX; i++)
                    {
                        if (board[toX + i, toY + i] != Figure.EmptyBox)
                        {
                            canMove = false;
                            break;
                        }
                    }
                }
                // Move right up
                if (fromX < toX && fromY > toY)
                {
                    for (int i = fromX + 1; i < toX; i++)
                    {
                        if (board[toX - i, toY + i] != Figure.EmptyBox)
                        {
                            canMove = false;
                            break;
                        }
                    }
                }
                // Move left down
                if (fromX > toX && fromY < toY)
                {
                    for (int i = toX + 1; i < fromX; i++)
                    {
                        if (board[toX + i, toY - i] != Figure.EmptyBox)
                        {
                            canMove = false;
                            break;
                        }
                    }
                }
                // Move right down
                if (fromX < toX && fromY < toY)
                {
                    for (int i = toX + 1; i < fromX; i++)
                    {
                        if (board[toX - i, toY - i] != Figure.EmptyBox)
                        {
                            canMove = false;
                            break;
                        }
                    }
                }
                if (canMove)
                {
                    if(board[fromX, fromY] == Figure.WhiteQueen)
                    {
                        // Checks toX and toY figure
                        board[fromX, fromY] = Figure.EmptyBox;
                        if (board[toX, toY] == Figure.WhiteBishop ||
                            board[toX, toY] == Figure.WhiteKing ||
                            board[toX, toY] == Figure.WhiteKnight ||
                            board[toX, toY] == Figure.WhitePawn ||
                            board[toX, toY] == Figure.WhiteQueen ||
                            board[toX, toY] == Figure.WhiteRook)
                        {
                            return false;
                        }
                        board[toX, toY] = Figure.WhiteQueen;
                        return true;
                    }
                    if (board[fromX, fromY] == Figure.BlackQueen)
                    {
                        // Checks toX and toY figure
                        board[fromX, fromY] = Figure.EmptyBox;
                        if (board[toX, toY] == Figure.BlackBishop ||
                            board[toX, toY] == Figure.BlackKing ||
                            board[toX, toY] == Figure.BlackKnight ||
                            board[toX, toY] == Figure.BlackPawn ||
                            board[toX, toY] == Figure.BlackQueen ||
                            board[toX, toY] == Figure.BlackRook)
                        {
                            return false;
                        }
                        board[toX, toY] = Figure.BlackQueen;
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private bool KnightMove(int fromX, int fromY, int toX, int toY)
        {
            // Checks who plays and if it's correct figure
            if (status == Status.WhitePlaying && board[fromX, fromY] == Figure.WhiteKnight)
            {
                // Checks if move cause check
                // insert

                // Checks movement correction
                if((fromX - 2 == toX && fromY + 1 == toY) ||
                   (fromX - 2 == toX && fromY - 1 == toY) ||
                   (fromX - 1 == toX && fromY - 2 == toY) ||
                   (fromX + 1 == toX && fromY - 2 == toY) ||
                   (fromX + 2 == toX && fromY + 1 == toY) ||
                   (fromX + 2 == toX && fromY - 1 == toY) ||
                   (fromX - 1 == toX && fromY + 2 == toY) ||
                   (fromX + 1 == toX && fromY + 2 == toY))
                {
                    board[fromX, fromY] = Figure.EmptyBox;
                    board[toX, toY] = Figure.WhiteKnight;
                    return true;
                }
            }
            if (status == Status.BlackPlaying && board[fromX, fromY] == Figure.BlackKnight)
            {
                // Checks if move cause check
                // insert

                // Checks movement correction
                if ((fromX - 2 == toX && fromY + 1 == toY) ||
                   (fromX - 2 == toX && fromY - 1 == toY) ||
                   (fromX - 1 == toX && fromY - 2 == toY) ||
                   (fromX + 1 == toX && fromY - 2 == toY) ||
                   (fromX + 2 == toX && fromY + 1 == toY) ||
                   (fromX + 2 == toX && fromY - 1 == toY) ||
                   (fromX - 1 == toX && fromY + 2 == toY) ||
                   (fromX + 1 == toX && fromY + 2 == toY))
                {
                    board[fromX, fromY] = Figure.EmptyBox;
                    board[toX, toY] = Figure.BlackKnight;
                    return true;
                }
            }
            return false;
        }

        private bool KingMove(int fromX, int fromY, int toX, int toY)
        {
            if((toX + 1 == fromX && toY + 1 == fromY) ||
               (toX == fromX && toY + 1 == fromY) ||
               (toX - 1 == fromX && toY + 1 == fromY) ||
               (toX + 1 == fromX && toY == fromY) ||
               (toX == fromX && toY == fromY) ||
               (toX - 1 == fromX && toY == fromY) ||
               (toX + 1 == fromX && toY - 1 == fromY) ||
               (toX == fromX && toY - 1 == fromY) ||
               (toX - 1 == fromX && toY - 1 == fromY))
            {
                // Checks if move cause check
                // insert

                // Split black and white figure
                if(board[fromX, fromY] == Figure.WhiteKing)
                {
                    if (board[toX, toY] == Figure.WhiteBishop ||
                       board[toX, toY] == Figure.WhiteKnight ||
                       board[toX, toY] == Figure.WhitePawn ||
                       board[toX, toY] == Figure.WhiteQueen ||
                       board[toX, toY] == Figure.WhiteRook)
                        return false;

                    board[toX, toY] = Figure.WhiteKing;
                    board[fromX, fromY] = Figure.EmptyBox;
                    return true;

                }
                if(board[fromX, fromY] == Figure.BlackKing)
                {
                    if (board[toX, toY] == Figure.BlackBishop ||
                       board[toX, toY] == Figure.BlackKnight ||
                       board[toX, toY] == Figure.BlackPawn ||
                       board[toX, toY] == Figure.BlackQueen ||
                       board[toX, toY] == Figure.BlackRook)
                        return false;

                    board[toX, toY] = Figure.BlackKing;
                    board[fromX, fromY] = Figure.EmptyBox;
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool BishopMove(int fromX, int fromY, int toX, int toY)
        {
            // Checks who plays and if it's correct figure
            if (status == Status.WhitePlaying && board[fromX, fromY] == Figure.WhiteBishop)
            {

                // Checks if move cause check
                // insert

                // Checks toX and toY figure
                if(board[toX, toY] == Figure.WhiteBishop ||
                    board[toX, toY] == Figure.WhiteKing ||
                    board[toX, toY] == Figure.WhiteKnight ||
                    board[toX, toY] == Figure.WhitePawn ||
                    board[toX, toY] == Figure.WhiteQueen ||
                    board[toX, toY] == Figure.WhiteRook)
                {
                    return false;
                }

                // Checks movement correction
                if (GetAbsoluteValue(fromX - toX) == GetAbsoluteValue(fromY - toY))
                {
                    // Move left up
                    if(fromX>toX && fromY>toY)
                    {
                        bool canMove = true;
                        for(int i = toX+1; i < fromX; i++)
                        {
                            if(board[toX+i, toY+i] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteBishop;
                            return true;
                        }
                    }
                    // Move right up
                    if(fromX<toX && fromY>toY)
                    {
                        bool canMove = true;
                        for (int i = fromX + 1; i < toX; i++)
                        {
                            if (board[toX - i, toY + i] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteBishop;
                            return true;
                        }
                    }
                    // Move left down
                    if(fromX>toX && fromY<toY)
                    {
                        bool canMove = true;
                        for (int i = toX + 1; i < fromX; i++)
                        {
                            if (board[toX + i, toY - i] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteBishop;
                            return true;
                        }
                    }
                    // Move right down
                    if(fromX<toX && fromY<toY)
                    {
                        bool canMove = true;
                        for (int i = toX + 1; i < fromX; i++)
                        {
                            if (board[toX - i, toY - i] != Figure.EmptyBox)
                            {
                                canMove = false;
                                break;
                            }
                        }
                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteBishop;
                            return true;
                        }
                    }

                    return false;
                }
                return false;
            }
            if (status == Status.BlackPlaying && board[fromX, fromY] == Figure.BlackBishop)
            {

                // Checks if move cause check
                // insert

                // Checks toX and toY figure
                if (board[toX, toY] == Figure.BlackBishop ||
                    board[toX, toY] == Figure.BlackKing ||
                    board[toX, toY] == Figure.BlackKnight ||
                    board[toX, toY] == Figure.BlackPawn ||
                    board[toX, toY] == Figure.BlackQueen ||
                    board[toX, toY] == Figure.BlackRook)
                {
                    return false;
                }

                // Checks movement correction
                if (fromX == toX || fromY == toY)
                {
                    // Split to horizontal move
                    if (fromX == toX)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if (fromY < toY)
                        {
                            for (int y = fromY + 1; y < toY; y++)
                            {
                                Console.Write(y + " ");
                                if (board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = toY + 1; y < fromY; y++)
                            {
                                Console.Write(y + " ");
                                if (board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackBishop;
                            return true;
                        }
                        else
                            return false;

                    }
                    // Split to vertical move
                    if (fromY == toY)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if (fromX < toX)
                        {
                            for (int x = fromX + 1; x < toX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int x = toX + 1; x < fromX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackBishop;
                            return true;
                        }
                        else
                            return false;

                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private bool RookMove(int fromX, int fromY, int toX, int toY)
        {
            // Checks who plays and if it's correct figure
            if (status == Status.WhitePlaying && board[fromX, fromY] == Figure.WhiteRook)
            {

                // Checks if move cause check
                // insert

                // Checks movement correction
                if (fromX == toX || fromY == toY)
                {  
                    // Split to horizontal move
                    if (fromX == toX)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if(fromY < toY)
                        {
                            for(int y = fromY+1; y < toY; y++)
                            {
                                Console.Write(y + " ");
                                if(board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = toY+1; y < fromY; y++)
                            {
                                Console.Write(y + " ");
                                if (board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteRook;
                            return true;
                        }
                        else
                            return false;

                    }
                    // Split to vertical move
                    if (fromY == toY)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if (fromX < toX)
                        {
                            for (int x = fromX+1; x < toX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int x = toX+1; x < fromX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhiteRook;
                            return true;
                        }
                        else
                            return false;

                    }
                    return false;
                }
                return false;
            }
            if (status == Status.BlackPlaying && board[fromX, fromY] == Figure.BlackRook)
            {

                // Checks if move cause check
                // insert

                // Checks movement correction
                if (fromX == toX || fromY == toY)
                {
                    // Split to horizontal move
                    if (fromX == toX)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if (fromY < toY)
                        {
                            for (int y = fromY + 1; y < toY; y++)
                            {
                                Console.Write(y + " ");
                                if (board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int y = toY + 1; y < fromY; y++)
                            {
                                Console.Write(y + " ");
                                if (board[toX, y] != Figure.EmptyBox)
                                {
                                    Console.Write("found\n");
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackRook;
                            return true;
                        }
                        else
                            return false;

                    }
                    // Split to vertical move
                    if (fromY == toY)
                    {
                        // Check if figure can move - nothing in way
                        bool canMove = true;
                        if (fromX < toX)
                        {
                            for (int x = fromX + 1; x < toX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int x = toX + 1; x < fromX; x++)
                            {
                                if (board[x, toY] != Figure.EmptyBox)
                                {
                                    canMove = false;
                                    break;
                                }
                            }
                        }

                        if (canMove)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackRook;
                            return true;
                        }
                        else
                            return false;

                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        private bool PawnMove(int fromX, int fromY, int toX, int toY)
        {
            if(status == Status.WhitePlaying && board[fromX, fromY] == Figure.WhitePawn)
            {

                // Checks if move cause check
                // insert

                if ((fromY == toY+1 && ( toX+1 == fromX || toX == fromX || toX-1 == fromX)) || (fromY == toY+2 && fromX == toX))
                {
                    if(fromX == toX)
                    {
                        if(board[toX, toY] == Figure.EmptyBox && fromY == 1 && fromY == toY + 2)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhitePawn;
                            return true;
                        }
                        if(board[toX, toY] == Figure.EmptyBox)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhitePawn;
                            return true;
                        }
                    }
                    else
                    {
                        if (board[toX, toY] == Figure.BlackPawn ||
                        board[toX, toY] == Figure.BlackRook ||
                        board[toX, toY] == Figure.BlackKnight ||
                        board[toX, toY] == Figure.BlackBishop ||
                        board[toX, toY] == Figure.BlackQueen)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhitePawn;
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
            if(status == Status.BlackPlaying && board[fromX, fromY] == Figure.BlackPawn)
            {

                // Checks if move cause check
                // insert

                if ((fromY == toY-1 && (toX + 1 == fromX || toX == fromX || toX - 1 == fromX)) || (fromY == toY-2 && fromX == toX))
                {
                    if (fromX == toX)
                    {
                        if (board[toX, toY] == Figure.EmptyBox && fromY == 6 && fromY == toY-2)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.WhitePawn;
                            return true;
                        }
                        if (board[toX, toY] == Figure.EmptyBox)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackPawn;
                            return true;
                        }
                    }
                    else
                    {
                        if (board[toX, toY] == Figure.WhitePawn ||
                        board[toX, toY] == Figure.WhiteRook ||
                        board[toX, toY] == Figure.WhiteKnight ||
                        board[toX, toY] == Figure.WhiteBishop ||
                        board[toX, toY] == Figure.WhiteQueen)
                        {
                            board[fromX, fromY] = Figure.EmptyBox;
                            board[toX, toY] = Figure.BlackPawn;
                            return true;
                        }
                    }
                    return false;
                }
                return false;
            }
            return false;
        }
        
    }
}
