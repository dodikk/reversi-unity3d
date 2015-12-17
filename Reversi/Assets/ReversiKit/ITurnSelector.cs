using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public interface ITurnSelector
	{
		IReversiTurn SelectBestTurnOnBoard(IEnumerable<IReversiTurn> validTurns, IBoardState board);
	}
}

