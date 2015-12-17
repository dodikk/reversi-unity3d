using System;

namespace ReversiKit
{
	public interface IBoardState
	{
		bool IsTurnOfBlackPlayer { get; }

		bool IsCellFree(ICellCoordinates position);
		bool IsCellTakenByBlack(ICellCoordinates position);
		bool IsCellTakenByWhite(ICellCoordinates position);
	}
}

