﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

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
			Check.If (row).IsBetween(0, 7);
			Check.If (column).IsBetween(0, 7);
			#endif

			return new CellCoordinates (row, column);
		}

		public static string CoordinatesToCellName(ICellCoordinates cellPoint)
		{
			setupMappingIfNeeded();

			#if NO_UNITY
			Check.If (cellPoint.Row).IsBetween(0, 7);
			Check.If (cellPoint.Column).IsBetween(0, 7);
			#endif

			char cColumn = indexToLetter[cellPoint.Row];
			char cRow = Convert.ToChar(cellPoint.Column);

			string result = cColumn.ToString () + cRow.ToString ();
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


