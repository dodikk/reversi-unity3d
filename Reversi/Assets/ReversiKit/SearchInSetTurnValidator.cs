using System;
using System.Linq;
using System.Collections.Generic;

namespace ReversiKit
{
    public class SearchInSetTurnValidator : ITurnValidator
    {
        public SearchInSetTurnValidator(IEnumerable<IReversiTurn> validTurns)
        {
            this._validTurns = validTurns;
        }

        public bool IsValidPositionForTurnOnBoard(
            ICellCoordinates turnPosition, 
            IBoardState board)
        {
            var matchingTurns = this._validTurns.Where(t =>
            {
                return t.Position.Equals(turnPosition);
            });

            return (0 != matchingTurns.Count());
        }

        private IEnumerable<IReversiTurn> _validTurns;
    }
}

