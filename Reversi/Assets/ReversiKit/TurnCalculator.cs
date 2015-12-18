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
		#region ITurnCalculator
		public IEnumerable<IReversiTurn> GetValidTurnsForBoard(IBoardState board)
		{
			var turnCandidates = board.GetEmptyEnemyNeighbours();

			var result = new List<IReversiTurn>();
			foreach (ICellCoordinates cell in turnCandidates)
			{
				var turn = this.TurnForCellOnBoard(cell, board);
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

        private IReversiTurn TurnForCellOnBoard(
            ICellCoordinates turnCandidate, 
            IBoardState board)
		{
			var cellNeighbours = board.GetNeighboursForCell(turnCandidate);
			var directions = cellNeighbours.Where(n =>
			{
                return board.IsCellTakenByInactivePlayer(n);
			});


            var allFlippedCells = new List<ICellCoordinates>();
            foreach (ICellCoordinates singleDirection in directions)
            {
                IEnumerable<ICellCoordinates> flippedCells = 
                    this.FlippedCellsForDirectionOfTurnOnBoard(
                        singleDirection, 
                        turnCandidate,
                        board);

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

        private IEnumerable<ICellCoordinates> FlippedCellsForDirectionOfTurnOnBoard(
            ICellCoordinates direction, 
            ICellCoordinates turnCandidate,
            IBoardState      board)
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
                if (board.IsCellTakenByInactivePlayer(current))
                {
                    var currentClone = current.Clone() as CellCoordinates;
                    result.Add(currentClone);
                }
                else if (board.IsCellTakenByCurrentPlayer(current))
                {
                    isMyColourFound = true;
                    break;
                }
                else // empty cell
                {
                    break;
                }

                current.Row += rowIncrement;
                current.Column += columnIncrement;
            }
                
            if (!isMyColourFound)
            {
                return null;
            }

            return result;
        }
	}
}

