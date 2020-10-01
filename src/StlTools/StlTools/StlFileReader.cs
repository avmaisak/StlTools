using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using StlTools.Extensions;
using StlTools.Interfaces;
using StlTools.Types;

namespace StlTools
{
	/// <summary>
	/// Читатель STL из файла.
	/// </summary>
	public class StlFileReader : IStlReader<Shape>
	{
		/// <summary>
		/// Модель.
		/// </summary>
		private static Shape _shape;

		public StlFileReader() => _shape = new Shape();

		/// <summary>
		/// Читать файл и преобразовать в STL формат.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<Shape> ReadAndConvertAsync(string path)
		{

			if (!File.Exists(path)) return _shape;

			var objStream = new FileStream(path, FileMode.Open, FileAccess.Read);

			if (!objStream.CanRead) throw new StlToolsException(Resources.StlToolsResources.ErrIO001);

			await using (objStream) return objStream.IsBinary() ? ReadBin(_shape, objStream) : await ReadAsii(_shape, path);
		}

		/// <summary>
		/// Читать ASCII.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		private static async Task<Shape> ReadAsii(Shape shape, string path)
		{
			var result = await File.ReadAllTextAsync(path);

			if (string.IsNullOrWhiteSpace(result)) return shape;

			var resultArray = result.Split(Environment.NewLine);

			if (resultArray.Length == 0) return shape;

			shape.Facets = new List<Facet>();
			var facet = new Facet();

			foreach (var resultRow in resultArray)
			{
				if (string.IsNullOrWhiteSpace(resultRow)) continue;

				var row = resultRow.Trim();
				if (row.Contains(".")) row = row.Replace(".", ",");

				if (row.StartsWith(Resources.StlToolsResources.SolidToken))
				{
					shape.Name = resultRow.Replace(Resources.StlToolsResources.SolidToken, string.Empty).Trim();
				}

				if (row.StartsWith("facet"))
				{
					facet = new Facet();
					var facetStartArr = row.Split(' ');
					facet.Normal = new Normal
					{
						X = Convert.ToDouble(facetStartArr[2]),
						Y = Convert.ToDouble(facetStartArr[3]),
						Z = Convert.ToDouble(facetStartArr[4])
					};
				}

				if (row == "outer loop")
				{
					// ReSharper disable once PossibleNullReferenceException
					facet.Vertices = new List<Vertex>();
				}


				if (row.StartsWith("vertex"))
				{
					var vertexData = row.Split(' ');
					facet.Vertices.Add(new Vertex
					{
						X = Convert.ToDouble(vertexData[1]), 
						Y = Convert.ToDouble(vertexData[2]), 
						Z = Convert.ToDouble(vertexData[3])
					});
				}


				if (row.StartsWith("endfacet"))
				{
					shape.Facets.Add(facet);
					facet = null;
				}
			}

			return shape;
		}

		/// <summary>
		/// Читать бинарный.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="stream"></param>
		/// <returns></returns>
		private static Shape ReadBin(Shape shape, Stream stream)
		{
			return _shape;
		}

	}
}
