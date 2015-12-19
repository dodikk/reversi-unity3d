using System;

namespace ReversiKit
{
	public interface ITurnValidator
	{
		bool IsValidPositionForTurnOnBoard(ICellCoordinates turnPosition, IBoardState board);
	}
}

