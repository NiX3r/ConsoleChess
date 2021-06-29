using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleChess
{
    class ChessEnums
    {

        public enum Status
        {
            WhitePlaying,
            BlackPlaying,
            WhiteCheckBlack,
            BlackCheckWhite,
            WhiteCheckmateBlack,
            BlackCheckmateWhite,
            Draw,
            GameStopped
        }

        public enum Figure
        {
            BlackPawn,
            BlackBishop,
            BlackKnight,
            BlackRook,
            BlackQueen,
            BlackKing,
            EmptyBox,
            WhitePawn,
            WhiteBishop,
            WhiteKnight,
            WhiteRook,
            WhiteQueen,
            WhiteKing
        }

        public enum MoveReturns
        {
            MoveDone,
            BadInputParameters,
            TryToPlayAsEnemy,
            TryToCheatMove,
            GameAlreadyEnded,
            NothingHappened,
            BoxIsEmpty,
            StillGotCheck,
            WhiteCheckBlack,
            BlackCheckWhite,
            WhiteCheckmateBlack,
            BlackCheckmateWhite
        }

    }
}
