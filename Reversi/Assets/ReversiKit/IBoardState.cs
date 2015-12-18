using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public interface IBoardState
	{
		bool IsTurnOfBlackPlayer { get; }

		bool IsCellFree(ICellCoordinates position);
		bool IsCellTakenByBlack(ICellCoordinates position);
		bool IsCellTakenByWhite(ICellCoordinates position);

		bool IsCellTakenByCurrentPlayer(ICellCoordinates position);
		bool IsCellTakenByInactivePlayer(ICellCoordinates position);

		IEnumerable<ICellCoordinates> GetNeighboursForCell(ICellCoordinates position);
		IEnumerable<ICellCoordinates> GetEmptyEnemyNeighbours();

        int NumberOfBlackPieces { get; }
        int NumberOfWhitePieces { get; }
        int NumberOfFreeCells { get; }


        void ApplyTurn(IReversiTurn turn);
        void PassTurn();
	}
}

