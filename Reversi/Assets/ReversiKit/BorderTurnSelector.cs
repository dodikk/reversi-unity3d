using System;
using System.Linq;
using System.Collections.Generic;


namespace ReversiKit
{
    public class BorderTurnSelector : ITurnSelector
    {
        public IReversiTurn SelectBestTurnOnBoard(
            IEnumerable<IReversiTurn> validTurns, 
            IBoardState board)
        {
            if (null == validTurns)
            {
                return null;
            }

            var cornerTurns = validTurns.Where(t => t.Position.IsBorder);
            if (null == cornerTurns || 0 == cornerTurns.Count())
            {
                return null;
            }

            return cornerTurns.First();
        }
    }
}

