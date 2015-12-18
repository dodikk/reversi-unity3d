using System;
using System.Diagnostics;
using System.Collections.Generic;

// TODO : Make NuGet work properly with Unity3D
// 
using System.Linq;


#if NO_UNITY
using Conditions.Guards;
#endif


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

			#if NO_UNITY
			Check.If (cellName.Length).IsBetween (2, 2);
			#endif

			char[] cellNameParts = cellName.ToCharArray ();
			int column = letterToIndex[cellNameParts[0]];
			int row = Int32.Parse(cellNameParts[1].ToString()) - 1;

			#if NO_UNITY
            Check.If (row).IsBetween(0, MatrixBoard.BOARD_SIZE - 1);
            Check.If (column).IsBetween(0, MatrixBoard.BOARD_SIZE - 1);
			#endif

			return new CellCoordinates (row, column);
		}

		public static string CoordinatesToCellName(ICellCoordinates cellPoint)
		{
			setupMappingIfNeeded();

			#if NO_UNITY
            Check.If (cellPoint.Row).IsBetween(0, MatrixBoard.BOARD_SIZE - 1);
            Check.If (cellPoint.Column).IsBetween(0, MatrixBoard.BOARD_SIZE - 1);
			#endif

			char cColumn = indexToLetter[cellPoint.Column];
			string cRow = (cellPoint.Row + 1).ToString();

			string result = cColumn.ToString () + cRow.ToString ();
			return result;
		}

        public static string PrintTurnPositions(IEnumerable<IReversiTurn> turns)
        {
            var cells = turns.Select(t => t.Position);
            return PrintCoordinates(cells);
        }

        public static string PrintCoordinates(IEnumerable<ICellCoordinates> cells)
        {
            var resultNames = cells.Select(c => BoardCoordinatesConverter.CoordinatesToCellName(c));
            string debugResultNames = String.Join("; ", resultNames.ToArray());

            return debugResultNames;
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


