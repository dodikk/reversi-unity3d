using System;
using System.Linq;
using System.Collections.Generic;

#if NO_UNITY
using Conditions.Guards;
#endif

namespace ReversiKit
{
	public class TurnCalculator : ITurnCalculator
	{
		public TurnCalculator(IBoardState board)
		{
			this._board = board;
		}
			
		#region ITurnCalculator
		public IEnumerable<IReversiTurn> GetValidTurnsForBoard(IBoardState board)
		{
			var turnCandidates = this._board.GetEmptyEnemyNeighbours();

			var result = new List<IReversiTurn>();
			foreach (ICellCoordinates cell in turnCandidates)
			{
				var turn = this.TurnForCell(cell);
                if (null == turn)
                {
                    continue;
                }

				result.Add(turn);
			}

            bool isNoValidTurnsLeft = (0 == result.Count);
            if (isNoValidTurnsLeft)
            {
                return null;
            }

			return result;
		}
		#endregion

        private IReversiTurn TurnForCell(ICellCoordinates turnCandidate)
		{
			var cellNeighbours = this._board.GetNeighboursForCell(turnCandidate);
			var directions = cellNeighbours.Where(n =>
			{
                return this._board.IsCellTakenByInactivePlayer(n);
			});


            var allFlippedCells = new List<ICellCoordinates>();
            foreach (ICellCoordinates singleDirection in directions)
            {
                IEnumerable<ICellCoordinates> flippedCells = 
                    this.FlippedCellsForDirectionOfTurn(singleDirection, turnCandidate);

                if (null != flippedCells)
                {
                    allFlippedCells.AddRange(flippedCells);
                }
            }


            bool isNoDirectionFlippedEnemyItems = (0 == allFlippedCells.Count);
            if (isNoDirectionFlippedEnemyItems)
            {
                return null;
            }
			
            var result = new ReversiTurnPOD();
            {
                result.Position = turnCandidate;
                result.PositionsOfFlippedItems = allFlippedCells;
            }

            return result;
		}

        private IEnumerable<ICellCoordinates> FlippedCellsForDirectionOfTurn(
            ICellCoordinates direction, 
            ICellCoordinates turnCandidate)
        {
            var result = new List<ICellCoordinates>();

            int rowIncrement    = direction.Row    - turnCandidate.Row   ;
            int columnIncrement = direction.Column - turnCandidate.Column;

            #if NO_UNITY
            Check.If(Math.Abs(rowIncrement   )).IsBetween(0, 1);
            Check.If(Math.Abs(columnIncrement)).IsBetween(0, 1);
            #endif


            CellCoordinates current = new CellCoordinates(direction.Row, direction.Column);
            bool isMyColourFound = false;
            while (current.Row    >= 0 && current.Row    < MatrixBoard.BOARD_SIZE &&
                   current.Column >= 0 && current.Column < MatrixBoard.BOARD_SIZE)
            {
                if (this._board.IsCellTakenByInactivePlayer(current))
                {
                    var currentClone = current.Clone() as CellCoordinates;
                    result.Add(currentClone);
                }
                else if (this._board.IsCellTakenByCurrentPlayer(current))
                {
                    isMyColourFound = true;
                    break;
                }
                else // empty cell
                {
                    break;
                }
            }
                
            if (!isMyColourFound)
            {
                return null;
            }

            return result;
        }

		private IBoardState _board;
	}
}

