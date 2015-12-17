using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public interface ITurnCalculator
	{
		IEnumerable<IReversiTurn> GetValidTurnsForBoard(IBoardState board);
	}
}

