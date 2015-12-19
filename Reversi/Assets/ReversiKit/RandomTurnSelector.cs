using System;
using System.Linq;
using System.Collections.Generic;


namespace ReversiKit
{
    public class RandomTurnSelector : ITurnSelector
    {
        public IReversiTurn SelectBestTurnOnBoard(
            IEnumerable<IReversiTurn> validTurns, 
            IBoardState board)
        {
            Random rand = new Random();
            int index = rand.Next(0, validTurns.Count());
        
            IReversiTurn result = validTurns.ElementAt(index);
            return result;
        }
    }
}

