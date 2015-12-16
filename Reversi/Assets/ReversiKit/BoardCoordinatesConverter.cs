using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace ReversiKit
{
	public class BoardCoordinatesConverter
	{
		private BoardCoordinatesConverter ()
		{
		}

		public static ICellCoordinates CellNameToCoordinates(string cellName)
		{
			setupMappingIfNeeded();
			Debug.Assert(2 == cellName.Length);


			char[] cellNameParts = cellName.ToCharArray ();
			int row = letterToIndex[cellNameParts[0]];
			int column = Int32.Parse(cellNameParts[1].ToString()) - 1;

			Debug.Assert (row >= 0);
			Debug.Assert (row < 8);

			Debug.Assert (column >= 0);
			Debug.Assert (column < 8);

			return new CellCoordinates (row, column);
		}

		public static string CoordinatesToCellName(ICellCoordinates cellPoint)
		{
			setupMappingIfNeeded();
			
			Debug.Assert (cellPoint.Row >= 0);
			Debug.Assert (cellPoint.Row < 8);

			Debug.Assert (cellPoint.Column >= 0);
			Debug.Assert (cellPoint.Column < 8);

			char cRow = indexToLetter[cellPoint.Row];
			char cColumn = Convert.ToChar(cellPoint.Column);

			string result = cRow.ToString () + cColumn.ToString ();
			return result;
		}

		private static void setupMappingIfNeeded()
		{
			if (null == letterToIndex)
			{
				letterToIndex = new Dictionary<char, int>();
				letterToIndex.Add('A', 0);
				letterToIndex.Add('B', 1);
				letterToIndex.Add('C', 2);
				letterToIndex.Add('D', 3);
				letterToIndex.Add('E', 4);
				letterToIndex.Add('F', 5);
				letterToIndex.Add('G', 6);
				letterToIndex.Add('H', 7);
			}


			if (null == indexToLetter)
			{
				indexToLetter = new char[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'};
			}
		}

		private static Dictionary<char, int> letterToIndex;
		private static char[] indexToLetter;
	}
}


