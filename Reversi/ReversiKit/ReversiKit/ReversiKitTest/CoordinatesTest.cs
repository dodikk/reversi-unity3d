using System;
using NUnit.Framework;
using ReversiKit;

namespace ReversiKitTest
{
	[TestFixture]
	public class CoordinatesTest
	{
		[Test]
		public void TestValidTextToField()
		{
			var received = BoardCoordinatesConverter.CellNameToCoordinates ("C5");

			Assert.AreEqual(received.Row, 4);
			Assert.AreEqual(received.Column, 2);
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
		public void TestLargeColumnCausesException()
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
	}
}

