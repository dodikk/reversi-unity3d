using System;
using System.Collections.Generic;

namespace ReversiKit
{
    public class CompositeTurnSelector : ITurnSelector
    {
        public CompositeTurnSelector(ITurnSelector[] selectorsSortedByPriorityAsc)
        {
            this._selectorsSortedByPriorityAsc = selectorsSortedByPriorityAsc;
        }

        public IReversiTurn SelectBestTurnOnBoard(
            IEnumerable<IReversiTurn> validTurns, 
            IBoardState board)
        {
            if (null == validTurns)
            {
                return null;
            }

            int selectorsCount = this._selectorsSortedByPriorityAsc.Length;
            for (int selectorIndex = 0; selectorIndex < selectorsCount; ++selectorIndex)
            {
                ITurnSelector currentSelector = this._selectorsSortedByPriorityAsc[selectorIndex];
                IReversiTurn currentSuggestion = currentSelector.SelectBestTurnOnBoard(validTurns, board);

                if (null != currentSuggestion)
                {
                    return currentSuggestion;
                }
            }

            return null;
        }

        ITurnSelector[] _selectorsSortedByPriorityAsc;
    }
}

