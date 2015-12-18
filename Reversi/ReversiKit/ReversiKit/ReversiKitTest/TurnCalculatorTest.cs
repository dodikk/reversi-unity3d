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
			this._initialBoard = new MatrixBoard();
			{
				this._initialBoard.TryConsumeNamedCellByBlackPlayer("D4");
				this._initialBoard.TryConsumeNamedCellByBlackPlayer("E5");

				this._initialBoard.TryConsumeNamedCellByWhitePlayer("E4");
				this._initialBoard.TryConsumeNamedCellByWhitePlayer("D5");

				this._initialBoard.IsTurnOfBlackPlayer = true;
			}

			this._sut = new TurnCalculator();
		}

		[TearDown]
		public void TearDown()
		{
			this._initialBoard = null;
			this._sut		   = null;
		}



		// 8 x x x x x x x x
		// 7 x x x x x x x x
		// 6 x x T x x x x x
		// 5 x T W B x x x x
		// 4 x x B W T x x x
		// 3 x x x T x x x x
		// 2 x x x x x x x x
		// 1 x x x x x x x x
		//   A B C D E F G H
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
				Assert.AreEqual("D5", flippedCellName);
			}

			{
				position = BoardCoordinatesConverter.CellNameToCoordinates("D6");
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
				Assert.AreEqual("D5", flippedCellName);
			}

			{
				position = BoardCoordinatesConverter.CellNameToCoordinates("F4");
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
				Assert.AreEqual("E4", flippedCellName);
			}

			{
				position = BoardCoordinatesConverter.CellNameToCoordinates("E3");
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
				Assert.AreEqual("E4", flippedCellName);
			}
		}
	

        // 8 x x x x x x x x
        // 7 x x x x x x x x
        // 6 x x x x x x x x
        // 5 x x x x x x x x
        // 4 x x x x x x x x
        // 3 x x x x x x x x
        // 2 x x x x x x x x
        // 1 x x x x x x x x
        //   A B C D E F G H
        [Test]
        public void TestTurnsResultIsNullForEmptyBoard()
        {
            var board = new MatrixBoard();
            var sut = new TurnCalculator();

            var turns = sut.GetValidTurnsForBoard(board);
            Assert.IsNull(turns);
        }


        // 8 W x x W x x x W
        // 7 x x x B x x B x
        // 6 x x x B x B x x
        // 5 x x x B B x x x
        // 4 W B B T B B B W
        // 3 x x B B x x x x
        // 2 x B x B x x x x
        // 1 W x x W x x x W
        //   A B C D E F G H
        [Test]
        public void TestMultipleDirections()
        {
            var sut = new TurnCalculator();
            var board = new MatrixBoard();
            {
                board.IsTurnOfBlackPlayer = false;
                board.TryConsumeNamedCellByWhitePlayer("A1");
                board.TryConsumeNamedCellByWhitePlayer("H1");
                board.TryConsumeNamedCellByWhitePlayer("A8");
                board.TryConsumeNamedCellByWhitePlayer("H8");
                board.TryConsumeNamedCellByWhitePlayer("A4");
                board.TryConsumeNamedCellByWhitePlayer("H4");
                board.TryConsumeNamedCellByWhitePlayer("D1");
                board.TryConsumeNamedCellByWhitePlayer("D8");


                // Diagonal
                board.TryConsumeNamedCellByBlackPlayer("B2");
                board.TryConsumeNamedCellByBlackPlayer("C3");
                board.TryConsumeNamedCellByBlackPlayer("E5");
                board.TryConsumeNamedCellByBlackPlayer("F6");
                board.TryConsumeNamedCellByBlackPlayer("G7");

                // Horizontal
                board.TryConsumeNamedCellByBlackPlayer("B4");
                board.TryConsumeNamedCellByBlackPlayer("C4");
                board.TryConsumeNamedCellByBlackPlayer("E4");
                board.TryConsumeNamedCellByBlackPlayer("F4");
                board.TryConsumeNamedCellByBlackPlayer("G4");

                // Vertical
                board.TryConsumeNamedCellByBlackPlayer("D2");
                board.TryConsumeNamedCellByBlackPlayer("D3");
                board.TryConsumeNamedCellByBlackPlayer("D5");
                board.TryConsumeNamedCellByBlackPlayer("D6");
                board.TryConsumeNamedCellByBlackPlayer("D7");
            }


            var result = sut.GetValidTurnsForBoard(board);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            IReversiTurn turn = result.First();
            string strPosition = BoardCoordinatesConverter.CoordinatesToCellName(turn.Position);
            Assert.AreEqual("D4", strPosition);

            Assert.AreEqual(15, turn.PositionsOfFlippedItems.Count());

            var sortedFlips = 
                turn.PositionsOfFlippedItems.OrderBy(c =>
                {
                    return BoardCoordinatesConverter.CoordinatesToCellName(c);
                });
            string strSortedFlips = BoardCoordinatesConverter.PrintCoordinates(sortedFlips);
            string expectedFlips = "B2; B4; C3; C4; D2; D3; D5; D6; D7; E4; E5; F4; F6; G4; G7";

            Assert.AreEqual(expectedFlips, strSortedFlips);
        }

        // 8 B x x B x x x B
        // 7 x x x B x x B x
        // 6 x x x B x B x x
        // 5 x x x B B x x x
        // 4 B B B T B B B B
        // 3 x x B B x x x x
        // 2 x B x B x x x x
        // 1 B x x B x x x B
        //   A B C D E F G H
        [Test]
        public void TestNoFlipsIfBoardEndReached()
        {
            var sut = new TurnCalculator();
            var board = new MatrixBoard();
            {
                board.IsTurnOfBlackPlayer = false;
                board.TryConsumeNamedCellByBlackPlayer("A1");
                board.TryConsumeNamedCellByBlackPlayer("H1");
                board.TryConsumeNamedCellByBlackPlayer("A8");
                board.TryConsumeNamedCellByBlackPlayer("H8");
                board.TryConsumeNamedCellByBlackPlayer("A4");
                board.TryConsumeNamedCellByBlackPlayer("H4");
                board.TryConsumeNamedCellByBlackPlayer("D1");
                board.TryConsumeNamedCellByBlackPlayer("D8");


                // Diagonal
                board.TryConsumeNamedCellByBlackPlayer("B2");
                board.TryConsumeNamedCellByBlackPlayer("C3");
                board.TryConsumeNamedCellByBlackPlayer("E5");
                board.TryConsumeNamedCellByBlackPlayer("F6");
                board.TryConsumeNamedCellByBlackPlayer("G7");

                // Horizontal
                board.TryConsumeNamedCellByBlackPlayer("B4");
                board.TryConsumeNamedCellByBlackPlayer("C4");
                board.TryConsumeNamedCellByBlackPlayer("E4");
                board.TryConsumeNamedCellByBlackPlayer("F4");
                board.TryConsumeNamedCellByBlackPlayer("G4");

                // Vertical
                board.TryConsumeNamedCellByBlackPlayer("D2");
                board.TryConsumeNamedCellByBlackPlayer("D3");
                board.TryConsumeNamedCellByBlackPlayer("D5");
                board.TryConsumeNamedCellByBlackPlayer("D6");
                board.TryConsumeNamedCellByBlackPlayer("D7");
            }


            var result = sut.GetValidTurnsForBoard(board);
            Assert.IsNull(result);
        }


        // 8 W x x x x x x x
        // 7 x B x x x x x x
        // 6 x x B x x x x x
        // 5 x x x B x x x x
        // 4 x x x x B x x x
        // 3 x x x x x T x x
        // 2 x x x x x x B x
        // 1 x x x x x x x W
        //   A B C D E F G H
        [Test]
        public void TestReverseDiagonalFlips()
        {
            var sut = new TurnCalculator();
            var board = new MatrixBoard();
            {
                board.IsTurnOfBlackPlayer = false;

                board.TryConsumeNamedCellByWhitePlayer("A8");
                board.TryConsumeNamedCellByWhitePlayer("H1");

                board.TryConsumeNamedCellByBlackPlayer("B7");
                board.TryConsumeNamedCellByBlackPlayer("C6");
                board.TryConsumeNamedCellByBlackPlayer("D5");
                board.TryConsumeNamedCellByBlackPlayer("E4");
                board.TryConsumeNamedCellByBlackPlayer("G2");
            }

            var result = sut.GetValidTurnsForBoard(board);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());

            IReversiTurn turn = result.First();
            string strPosition = BoardCoordinatesConverter.CoordinatesToCellName(turn.Position);
            Assert.AreEqual("F3", strPosition);

            Assert.AreEqual(5, turn.PositionsOfFlippedItems.Count());

            var sortedFlips = 
                turn.PositionsOfFlippedItems.OrderBy(c =>
                {
                    return BoardCoordinatesConverter.CoordinatesToCellName(c);
                });
            string strSortedFlips = BoardCoordinatesConverter.PrintCoordinates(sortedFlips);
            string expectedFlips = "B7; C6; D5; E4; G2";

            Assert.AreEqual(expectedFlips, strSortedFlips);
        }

		// 8 x x x x x x x x
		// 7 x x x x x x x x
		// 6 x x x x x x x x
		// 5 x x x x x x x x
		// 4 x x x x x x x x
		// 3 x x x x x x x x
		// 2 x x x x x x x x
		// 1 x x x x x x x x
		//   A B C D E F G H


	}
}

