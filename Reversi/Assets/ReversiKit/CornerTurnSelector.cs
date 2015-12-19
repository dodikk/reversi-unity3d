using System;
using System.Linq;
using System.Collections.Generic;

namespace ReversiKit
{
    public class CornerTurnSelector : ITurnSelector
    {
        public IReversiTurn SelectBestTurnOnBoard(
            IEnumerable<IReversiTurn> validTurns, 
            IBoardState board)
        {
            if (null == validTurns)
            {
                return null;
            }

            var cornerTurns = validTurns.Where(t => t.Position.IsCorner);
            if (null == cornerTurns || 0 == cornerTurns.Count())
            {
                return null;
            }

            return cornerTurns.First();
        }
    }
}

