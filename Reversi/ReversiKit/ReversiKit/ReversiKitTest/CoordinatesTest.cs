using System;
using NUnit.Framework;
using ReversiKit;

namespace ReversiKitTest
{
	[TestFixture]
	public class CoordinatesTest
	{

		#region Text To Point
		[Test]
		public void TestValidTextToField()
		{
			var received = BoardCoordinatesConverter.CellNameToCoordinates ("C5");

			Assert.AreEqual(4, received.Row);
			Assert.AreEqual(2, received.Column);
		}

		[Test]
		public void TestInvalidNameCausesException()
		{
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CellNameToCoordinates ("R8");
			};

			Assert.Catch(failure, "invalid column name");
		}

		[Test]
		public void TestZeroColumnCausesException()
		{
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CellNameToCoordinates ("A0");
			};

			Assert.Catch(failure, "invalid row name");
		}

		[Test]
		public void TestLargeColumnInNameCausesException()
		{
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CellNameToCoordinates ("A9");
			};

			Assert.Catch(failure, "invalid row name");
		}

		[Test]
		public void TestLomgNameCausesException()
		{
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CellNameToCoordinates ("A12");
			};

			Assert.Catch(failure, "invalid column name");
		}
		#endregion


		#region Point to Text
		[Test]
		public void TestValidFieldToText()
		{
			var position = new CellCoordinates (4, 2);
			string received = BoardCoordinatesConverter.CoordinatesToCellName (position);

			Assert.AreEqual("C5", received);
		}

		[Test]
		public void TestNegativeColumnIndexCausesException()
		{
			var position = new CellCoordinates(3, -1);

			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CoordinatesToCellName (position);
			};

			Assert.Catch(failure, "invalid column name");
		}

		[Test]
		public void TestNegativeRowCausesException()
		{
			var position = new CellCoordinates(-1, 0);

			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CoordinatesToCellName (position);
			};

			Assert.Catch(failure, "invalid row name");
		}

		[Test]
		public void TestLargeRowCausesException()
		{
			var position = new CellCoordinates(9, 0);
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CoordinatesToCellName (position);
			};

			Assert.Catch(failure, "invalid row name");
		}

		[Test]
		public void TestLargeColumnCausesException()
		{
			var position = new CellCoordinates(0, 9);
			TestDelegate failure = delegate() 
			{
				BoardCoordinatesConverter.CoordinatesToCellName (position);
			};

			Assert.Catch(failure, "invalid row name");
		}
		#endregion

	}
}

