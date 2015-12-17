using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public interface IReversiTurn
	{
		ICellCoordinates position { get; }
		IEnumerable<ICellCoordinates> positionsOfFlippedItems { get; }
	}
}

