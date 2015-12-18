using System;
using System.Collections.Generic;

namespace ReversiKit
{
    public class ReversiTurnPOD : IReversiTurn
    {
        public ReversiTurnPOD()
        {
        }

        public ICellCoordinates Position { get; set; }
        public IEnumerable<ICellCoordinates> PositionsOfFlippedItems { get; set; }
    }
}

