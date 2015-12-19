using System;

namespace ReversiKit
{
	public interface ICellCoordinates
	{
		int Row {get;}
		int Column {get;}
        bool IsBlack {get;}
        bool IsWhite {get;}

        bool IsCorner { get; }
        bool IsBorder { get; }
	}
}

