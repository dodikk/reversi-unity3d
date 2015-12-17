using System;

namespace ReversiKit
{
	public interface ITurnValidator
	{
		// TODO : maybe IEnumerable<IreversiTurn> should be used
		bool IsValidPositionForTurnOnBoard(ICellCoordinates turnPosition, IBoardState board);
	}
}

