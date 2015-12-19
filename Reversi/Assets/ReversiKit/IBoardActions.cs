using System;

namespace ReversiKit
{
    public interface IBoardActions
    {
        void ApplyTurn(IReversiTurn turn);
        void PassTurn();

        bool IsTurnOfBlackPlayer { set; }

        void TryConsumeCellByBlackPlayer(ICellCoordinates cellPosition);
        void TryConsumeCellByWhitePlayer(ICellCoordinates cellPosition);
        void TryConsumeCellByBlackPlayer(ICellCoordinates cellPosition, bool isBlackPlayer);

        void TryConsumeNamedCellByBlackPlayer(string cellName, bool isBlackPlayer);
        void TryConsumeNamedCellByBlackPlayer(string cellName);
        void TryConsumeNamedCellByWhitePlayer(string cellName);
    }
}

