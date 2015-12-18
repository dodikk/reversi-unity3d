using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#if NO_UNITY
using Conditions.Guards;
#endif

namespace ReversiKit
{
	public class MatrixBoard : IBoardState
	{
		public MatrixBoard ()
		{
			this.IsTurnOfBlackPlayer = true;

			this._cells = new int[BOARD_SIZE, BOARD_SIZE];
			this.ZeroCells ();
		}



		private void ZeroCells()
		{
			for (int row = 0; row != BOARD_SIZE; ++row)
				for (int col = 0; col != BOARD_SIZE; ++col) 
				{
					this._cells [row, col] = FREE_CELL;
				}
		}

        // TODO : optimize.
        // reduce memory footprint
        private IEnumerable<ICellCoordinates> FlattenCells()
        {
            if (null != this._flattenCells)
            {
                return this._flattenCells;
            }

            this._flattenCells = new ICellCoordinates[BOARD_SIZE * BOARD_SIZE];
            for (int row = 0; row != BOARD_SIZE; ++row)
                for (int col = 0; col != BOARD_SIZE; ++col) 
                {
                    this._flattenCells[row * BOARD_SIZE + col] = new CellCoordinates(row, col);
                }

            return this._flattenCells;
        }


		#region IBoardState
		public bool IsTurnOfBlackPlayer { get; set; }

		public  bool IsCellFree(ICellCoordinates position)
		{
            #if NO_UNITY
            Check.If(position.Row   ).IsBetween(0, BOARD_MAX_INDEX);
            Check.If(position.Column).IsBetween(0, BOARD_MAX_INDEX);
            #endif

			return (FREE_CELL == this._cells[position.Row, position.Column]);
		}

		public bool IsCellTakenByBlack(ICellCoordinates position)
		{
            #if NO_UNITY
            Check.If(position.Row).IsBetween(0, BOARD_MAX_INDEX);
            Check.If(position.Column).IsBetween(0, BOARD_MAX_INDEX);
            #endif

			return (TAKEN_BY_BLACK == this._cells[position.Row, position.Column]);
		}

		public bool IsCellTakenByWhite(ICellCoordinates position)
		{
            #if NO_UNITY
            Check.If(position.Row   ).IsBetween(0, BOARD_MAX_INDEX);
            Check.If(position.Column).IsBetween(0, BOARD_MAX_INDEX);
            #endif

			return (TAKEN_BY_WHITE == this._cells[position.Row, position.Column]);
		}

		public bool IsCellTakenByInactivePlayer(ICellCoordinates position)
		{
			if (this.IsTurnOfBlackPlayer)
			{
				return this.IsCellTakenByWhite(position);
			}
			else
			{
				return this.IsCellTakenByBlack(position);
			}
		}

        public bool IsCellTakenByCurrentPlayer(ICellCoordinates position)
		{
			if (this.IsTurnOfBlackPlayer)
			{
				return this.IsCellTakenByBlack(position);
			}
			else
			{
				return this.IsCellTakenByWhite(position);
			}
		}


		public IEnumerable<ICellCoordinates> GetEmptyEnemyNeighbours()
		{
			IEnumerable<ICellCoordinates> enemyCells = 
                this.FlattenCells()
                    .Where(c => this.IsCellTakenByInactivePlayer(c));

            IEnumerable<ICellCoordinates> result = 
				enemyCells.SelectMany(c => this.GetNeighboursForCell(c))
						  .Where(c => this.IsCellFree(c))
					      .Distinct();


            string debugResultNames = BoardCoordinatesConverter.PrintCoordinates(result);
            Debug.WriteLine(debugResultNames);

			return result;
		}

		public IEnumerable<ICellCoordinates> GetNeighboursForCell(ICellCoordinates position)
		{
			var result = new List<ICellCoordinates>();

            if (0 != position.Row)
            {
                if (0 != position.Column)
                {
                    result.Add(new CellCoordinates(position.Row - 1, position.Column - 1));
                }
                    
                result.Add(new CellCoordinates(position.Row - 1, position.Column));

                if (position.Column < BOARD_MAX_INDEX)
                {
                    result.Add(new CellCoordinates(position.Row - 1, position.Column + 1));
                }
            }

            {
                if (0 != position.Column)
                {
                    result.Add(new CellCoordinates(position.Row, position.Column - 1));
                }

                if (position.Column < BOARD_MAX_INDEX)
                {
                    result.Add(new CellCoordinates(position.Row, position.Column + 1));
                }
            }

            if (position.Row < BOARD_MAX_INDEX)
            {
                if (0 != position.Column)
                {
                    result.Add(new CellCoordinates(position.Row + 1, position.Column - 1));
                }

                result.Add(new CellCoordinates(position.Row + 1, position.Column));

                if (position.Column < BOARD_MAX_INDEX)
                {
                    result.Add(new CellCoordinates(position.Row + 1, position.Column + 1));
                }
            }


            #if NO_UNITY
            foreach (ICellCoordinates c in result)
            {
                Check.If(c.Row   ).IsBetween(0, BOARD_MAX_INDEX);
                Check.If(c.Column).IsBetween(0, BOARD_MAX_INDEX);
            }
            #endif

			return result;
		}
		#endregion

		#region Mutable
		public void TryConsumeCellByBlackPlayer(ICellCoordinates cellPosition)
		{
			this.TryConsumeCellByBlackPlayer (cellPosition, true);
		}

		public void TryConsumeCellByWhitePlayer(ICellCoordinates cellPosition)
		{
			this.TryConsumeCellByBlackPlayer (cellPosition, false);
		}

		public void TryConsumeCellByBlackPlayer(ICellCoordinates cellPosition, bool isBlackPlayer)
		{
			if (!IsCellFree (cellPosition)) 
			{
				throw new ArgumentOutOfRangeException ("cellPosition", cellPosition, "Cell already taken.");
			}


            #if NO_UNITY
            Check.If(cellPosition.Row   ).IsBetween(0, BOARD_MAX_INDEX);
            Check.If(cellPosition.Column).IsBetween(0, BOARD_MAX_INDEX);
            #endif
			this._cells [cellPosition.Row, cellPosition.Column] = isBlackPlayer ? TAKEN_BY_BLACK : TAKEN_BY_WHITE;
		}
		#endregion

		#region Mutable Text
		public void TryConsumeNamedCellByBlackPlayer(string cellName, bool isBlackPlayer)
		{
			ICellCoordinates cellPosition = BoardCoordinatesConverter.CellNameToCoordinates(cellName);
			this.TryConsumeCellByBlackPlayer(cellPosition, isBlackPlayer);
		}

		public void TryConsumeNamedCellByBlackPlayer(string cellName)
		{
			this.TryConsumeNamedCellByBlackPlayer(cellName, true);
		}

		public void TryConsumeNamedCellByWhitePlayer(string cellName)
		{
			this.TryConsumeNamedCellByBlackPlayer(cellName, false);
		}
		#endregion


        // state of the board
		private int[,] _cells;

        // a bunch of constants for LINQ
        // TODO : optimize
        private ICellCoordinates[] _flattenCells;

		public const int BOARD_SIZE = 8;
        public const int BOARD_MAX_INDEX = BOARD_SIZE - 1;

		private const int FREE_CELL = 0;
		private const int TAKEN_BY_WHITE = 1;
		private const int TAKEN_BY_BLACK = -1;
	}
}

