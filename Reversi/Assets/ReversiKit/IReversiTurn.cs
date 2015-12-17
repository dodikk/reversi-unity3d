using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public interface IReversiTurn
	{
		ICellCoordinates Position { get; }
		IEnumerable<ICellCoordinates> PositionsOfFlippedItems { get; }
	}
}

