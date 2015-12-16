using System;

namespace ReversiKit
{
	public class CellCoordinates : ICellCoordinates
	{
		public CellCoordinates (int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}

		public int Row {get; set;}
		public int Column {get; set;}
	}
}

