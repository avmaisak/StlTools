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
			Assert.IsTrue(shape.Facets.Count == 12);
		}

		[Test]
		public void ShapeObjectCompare()
		{
			var shapeAscii = new StlFileReader().ReadAndConvertAsync(@"E:\Github\StlTools\data\10ascii.stl").GetAwaiter().GetResult();
			var shapeBin = new StlFileReader().ReadAndConvertAsync(@"E:\Github\StlTools\data\10bin.stl").GetAwaiter().GetResult();
			
			Assert.IsTrue(shapeBin.LittleEndian == 12);
			Assert.IsTrue(shapeBin.Facets.Count == 12);
			Assert.IsTrue(shapeAscii.Facets.Count == 12);

			Assert.IsTrue(shapeAscii.Facets.FirstOrDefault().Normal.X == shapeBin.Facets.FirstOrDefault().Normal.X);
			Assert.IsTrue(shapeAscii.Facets.FirstOrDefault().Normal.Y == shapeBin.Facets.FirstOrDefault().Normal.Y);
			Assert.IsTrue(shapeAscii.Facets.FirstOrDefault().Normal.Z == shapeBin.Facets.FirstOrDefault().Normal.Z);

			for (var i = 0; i < 12; i++)
			{
				var fs = shapeAscii.Facets.ToArray()[i];
				var fb = shapeBin.Facets.ToArray()[i];

				Assert.IsTrue(fs.Vertices.Count == fb.Vertices.Count );

				var fsVerticles = shapeAscii.Facets.ToArray()[i].Vertices.ToArray();
				var fbVerticles = shapeBin.Facets.ToArray()[i].Vertices.ToArray();

				for (int j = 0; j < 3; j++)
				{
					Assert.IsTrue(fsVerticles[j].X == fbVerticles[j].X);
					Assert.IsTrue(fsVerticles[j].Y == fbVerticles[j].Y);
					Assert.IsTrue(fsVerticles[j].Z == fbVerticles[j].Z);
				}
			}

		}
	}
}
