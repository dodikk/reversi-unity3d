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
            throw new NotImplementedException();
        }

        private IEnumerable<IReversiTurn> _validTurns;
    }
}

