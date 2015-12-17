using System;

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

		#region IBoardState
		public bool IsTurnOfBlackPlayer { get; set; }

		public  bool IsCellFree(ICellCoordinates position)
		{
			return (FREE_CELL == this._cells[position.Row, position.Column]);
		}

		public bool IsCellTakenByBlack(ICellCoordinates position)
		{
			return (TAKEN_BY_BLACK == this._cells[position.Row, position.Column]);
		}

		public bool IsCellTakenByWhite(ICellCoordinates position)
		{
			return (TAKEN_BY_WHITE == this._cells[position.Row, position.Column]);
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


		private int[,] _cells;

		private const int BOARD_SIZE = 8;

		private const int FREE_CELL = 0;
		private const int TAKEN_BY_WHITE = 1;
		private const int TAKEN_BY_BLACK = -1;
	}
}

