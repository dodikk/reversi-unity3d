using System;
using System.Linq;
using System.Collections.Generic;

namespace ReversiKit
{
    public class GreedyTurnSelector : ITurnSelector
    {
        public IReversiTurn SelectBestTurnOnBoard(
            IEnumerable<IReversiTurn> validTurns, 
            IBoardState board)
        {
            IReversiTurn result = 
                validTurns.OrderByDescending(t => t.PositionsOfFlippedItems.Count())
                          .First();

            return result;
        }
    }
}

