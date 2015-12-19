using System;

namespace ReversiKit
{
    public interface IBoardActions
    {
        void ApplyTurn(IReversiTurn turn);
        void PassTurn();
    }
}

