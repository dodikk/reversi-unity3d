using System;
using System.Diagnostics;


namespace ReversiKit
{
	public class BoardCoordinatesConverter
	{
		private BoardCoordinatesConverter ()
		{
		}

		public static ICellCoordinates CellNameToCoordinates(string cellName)
		{
			Debug.Assert(2 == cellName.Length);


			// TODO : rewrite if the invariant does not work for Unicode
			char[] cellNameParts = cellName.ToCharArray ();
			int row = cellNameParts[0] - 'A';
			int column = cellNameParts [0] - '0';

			Debug.Assert (row >= 0);
			Debug.Assert (row < 8);

			Debug.Assert (column >= 0);
			Debug.Assert (column < 8);

			return new CellCoordinates (row, column);
		}

		public static string CoordinatesToCellName(ICellCoordinates cellPoint)
		{
			Debug.Assert (cellPoint.Row >= 0);
			Debug.Assert (cellPoint.Row < 8);

			Debug.Assert (cellPoint.Column >= 0);
			Debug.Assert (cellPoint.Column < 8);

			char cRow = (char)(cellPoint.Row + (int)'A');
			char cColumn = (char)(cellPoint.Column + (int)'0');

			string result = cRow.ToString () + cColumn.ToString ();
			return result;
		}
	}
}


