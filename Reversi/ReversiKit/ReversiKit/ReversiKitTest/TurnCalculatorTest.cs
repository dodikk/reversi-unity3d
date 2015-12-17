using System;
using NUnit.Framework;
using ReversiKit;
using System.Linq;

namespace ReversiKitTest
{
	[TestFixture]
	public class TurnCalculatorTest
	{
		private MatrixBoard    _initialBoard;
		private TurnCalculator _sut;

		[SetUp]
		public void SetUp()
		{
			this._sut = new TurnCalculator();

			this._initialBoard = new MatrixBoard();
			{
				this._initialBoard.TryConsumeNamedCellByBlackPlayer("D4");
				this._initialBoard.TryConsumeNamedCellByBlackPlayer("E5");

				this._initialBoard.TryConsumeNamedCellByWhitePlayer("E4");
				this._initialBoard.TryConsumeNamedCellByWhitePlayer("D5");

				this._initialBoard.IsTurnOfBlackPlayer = true;
			}
		}

		[TearDown]
		public void TearDown()
		{
			this._initialBoard = null;
			this._sut		   = null;
		}


		[Test]
		public void TestTurnsForInitialState()
		{
			var turns = this._sut.GetValidTurnsForBoard(this._initialBoard);
			Assert.IsNotNull(turns);

			var turnsCount = turns.Count();
			Assert.AreEqual(4, turnsCount);

			IReversiTurn turn = null;
			ICellCoordinates position = null;
			ICellCoordinates flippedCell = null;

			{
				position = BoardCoordinatesConverter.CellNameToCoordinates("C5");
				turn =  turns.Where
				(
					t => (t.Position.Row == position.Row &&
						t.Position.Column == position.Column)
				).First();

				Assert.IsNotNull(turn);
				Assert.AreEqual(1, turn.PositionsOfFlippedItems.Count());

				flippedCell = turn.PositionsOfFlippedItems.First();
				Assert.IsNotNull(flippedCell);

				string flippedCellName = BoardCoordinatesConverter.CoordinatesToCellName(flippedCell);
				Assert.AreEqual("C5", flippedCellName);
			}
		}
	


	}
}

