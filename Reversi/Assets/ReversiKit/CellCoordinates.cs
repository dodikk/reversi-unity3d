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

		#endregion
	}
}

