using System;
using System.Collections.Generic;

namespace ReversiKit
{
	public class TurnCalculator : ITurnCalculator
	{
		public TurnCalculator()
		{
		}
			
		#region ITurnCalculator
		public IEnumerable<IReversiTurn> GetValidTurnsForBoard(IBoardState board)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}

