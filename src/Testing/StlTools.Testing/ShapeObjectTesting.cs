using System.Linq;
using NUnit.Framework;
using StlTools.Types;

namespace StlTools.Testing
{
	public class ShapeObjectTesting
	{
		private Shape _shape;

		[Test]
		public void ShapeObjectIsNotNull()
		{
			_shape = new Shape();
			Assert.IsNotNull(_shape);
		}
		
		[Test]
		public void ShapeObjectIsBad()
		{
			_shape = new Shape();
			Assert.IsTrue(_shape.BadShape);
		}

		[Test]
		public void ShapeObjectNullableChecksIsBinary()
		{
			_shape = new Shape();
			Assert.IsFalse(_shape.IsAscii);
		}

		[Test]
		public void ShapeObjectNullableChecksIsAscii()
		{
			_shape = new Shape();
			Assert.IsFalse(_shape.IsAscii);
		}

		[Test]
		[TestCase("10ascii")]
		public void ShapeObjectReadAscii(string e)
		{
			using var reader = new StlFileReader();
			var shape = reader.ReadAndConvertAsync(@"E:\Github\StlTools\data\10ascii.stl").GetAwaiter().GetResult();
			Assert.IsTrue(shape.Name == e);
			Assert.IsTrue(shape.Facets != null);
			Assert.IsTrue(shape.Facets.Count == 12);
			var firstFacet = shape.Facets.FirstOrDefault();
			Assert.IsNotNull(firstFacet);
		}

		[Test]
		[TestCase("OpenSCAD_Model")]
		public void ShapeObjectReadAsciiNetWeavingNeedleKit(string e)
		{
			using var reader = new StlFileReader();
			var shape = reader.ReadAndConvertAsync(@"E:\Github\StlTools\data\NetWeavingNeedleKit.stl").GetAwaiter().GetResult();
			Assert.IsTrue(shape.Name == e);
			Assert.IsTrue(shape.Facets != null);
			Assert.IsTrue(shape.Facets.Count == 7876);
			var firstFacet = shape.Facets.FirstOrDefault();
			Assert.IsNotNull(firstFacet);
		}

		[Test]
		[TestCase("solid 10bin")]
		public void ShapeObjectReadBin(string e)
		{
			using var reader = new StlFileReader();
			var shape = reader.ReadAndConvertAsync(@"E:\Github\StlTools\data\10bin.stl").GetAwaiter().GetResult();
			Assert.IsTrue(shape.Name == e);
			Assert.IsTrue(shape.LittleEndian == 12);
			Assert.Pass();
		}
	}
}
