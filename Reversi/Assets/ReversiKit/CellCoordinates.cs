using System;

namespace ReversiKit
{
    public class CellCoordinates : ICellCoordinates, ICloneable
	{
		public CellCoordinates (int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}


        public object Clone()
        {
            CellCoordinates result = new CellCoordinates(this.Row, this.Column);
            return result;
        }

		#region Equality
		public override bool Equals(object other)
		{
			CellCoordinates otherCell = other as CellCoordinates;
			if (null == otherCell)
			{
				return false;
			}

			bool rowEqual = (this.Row == otherCell.Row);
			bool colEqual = (this.Column == otherCell.Column);

			return rowEqual && colEqual;
		}

		public override  int GetHashCode()
		{
			int rowHash = this.Row.GetHashCode();
			int columnHash = this.Column.GetHashCode();

			return (10 * rowHash) + columnHash;
		}
		#endregion

		#region ICellCoordinates
		public int Row {get; set;}
		public int Column {get; set;}

        public bool IsBlack 
        {
            get
            {
                int sumOfCoordinates = this.Column + this.Row;
                bool result = (0 == (sumOfCoordinates % 2));

                return result;
            }
        }

        public bool IsWhite 
        {
            get
            {
                return !this.IsBlack;
            }
        }

        public bool IsCorner 
        { 
            get
            {
                bool isA1 = (this.Row == 0                           || this.Column == 0);
                bool isA8 = (this.Row == MatrixBoard.BOARD_MAX_INDEX || this.Column == 0);
                bool isH1 = (this.Row == 0                           || this.Column == MatrixBoard.BOARD_MAX_INDEX);
                bool isH8 = (this.Row == MatrixBoard.BOARD_MAX_INDEX || this.Column == MatrixBoard.BOARD_MAX_INDEX);

                return isA1 || isA8 || isH1 || isH8;
            }
        }
            
        public bool IsBorder 
        { 
            get
            {
                bool isBottom = (this.Row == 0);
                bool isTop = (this.Row == MatrixBoard.BOARD_MAX_INDEX);
                bool isLeft = (this.Column == 0);
                bool isRight = (this.Column == MatrixBoard.BOARD_MAX_INDEX);

                return isBottom || isTop || isLeft || isRight;
            }
        }

		#endregion
	}
}

