using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StlTools.Extensions;
using StlTools.Interfaces;
using StlTools.Resources;
using StlTools.Types;

namespace StlTools
{
	/// <summary>
	/// Читатель STL из файла.
	/// </summary>
	public class StlFileReader : IStlReader<Shape>, IDisposable
	{
		/// <summary>
		/// Модель.
		/// </summary>
		private static Shape _shape;

		private FileStream _fileStream;

		public StlFileReader() => _shape = new Shape();

		/// <summary>
		/// Читать файл и преобразовать в STL формат.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public async Task<Shape> ReadAndConvertAsync(string path)
		{

			if (!File.Exists(path)) return _shape;

			_fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

			if (!_fileStream.CanRead) throw new StlToolsException(StlToolsResources.ErrIO001);

			await using (_fileStream) return _fileStream.IsBinary() ? await ReadBinary(_shape, path) : await ReadAsii(_shape, path);
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

				if (row.StartsWith(StlToolsResources.SolidToken)) shape.Name = resultRow.Replace(StlToolsResources.SolidToken, string.Empty).Trim();

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

				// ReSharper disable once PossibleNullReferenceException
				if (row == "outer loop") facet.Vertices = new List<Vertex>();

				if (row.StartsWith("vertex"))
				{
					if (!row.Contains(" ")) continue;

					var vertexData = row.Split(' ');
					// ReSharper disable once PossibleNullReferenceException
					facet.Vertices.Add(new Vertex
					{
						X = Convert.ToDouble(vertexData[1]),
						Y = Convert.ToDouble(vertexData[2]),
						Z = Convert.ToDouble(vertexData[3])
					});
				}

				if (!row.StartsWith("endfacet")) continue;

				shape.Facets.Add(facet);
				facet = null;
			}

			return shape;
		}

		/// <summary>
		/// Читать бинарный.
		/// </summary>
		/// <param name="shape"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		private static async Task<Shape> ReadBinary(Shape shape, string path)
		{
			var bytes = await File.ReadAllBytesAsync(path);
			if (bytes == null || bytes.Length == 0) return shape;

			//  Файл начинается с заголовка из 80 символов
			const int headerTreshold = 80;
			// После заголовка идет 4-байтовое беззнаковое целое число (little-endian),
			const int littleEndianTreshold = headerTreshold + 4;

			shape.Name = bytes.ToList().GetRange(0, headerTreshold).ToArray().ConvertToString().Trim();
			// После заголовка идет 4-байтовое беззнаковое целое число (little-endian),
			shape.LittleEndian = BitConverter.ToUInt32(bytes.ToList().GetRange(headerTreshold, littleEndianTreshold).ToArray());

			var payLoad = bytes.ToList();

			payLoad.RemoveRange(0, littleEndianTreshold);

			/*
			 * Каждый треугольник описывается двенадцатью 32-битными числами с плавающей запятой: 3 числа для нормали и по 3 числа на каждую из трёх вершин для координат X/Y/Z.
			 * После идут 2 байта беззнакового 'short', который называется 'attribute byte count'.
			 * В обычном файле должно быть равно нулю, так как большинство программ не понимает других значений.[6]
			   Числа с плавающей запятой представляются в виде числа IEEE с плавающей запятой и считается обратным порядком байтов, 
			   хотя это не указано в документации.
			 */
			var payLoadIndex = 0;
			shape.Facets = new List<Facet>();
			var payloadData = new List<byte>();

			foreach (var data in payLoad)
			{
				payLoadIndex++;
				payloadData.Add(data);

				// треугольник
				if (payLoadIndex % 50 != 0) continue;

				shape.Facets.Add(new Facet
				{
					Normal = new Normal
					{
						X = BitConverter.ToSingle(payloadData.Skip(0).Take(4).ToArray(), 0),
						Y = BitConverter.ToSingle(payloadData.Skip(4).Take(4).ToArray(), 0),
						Z = BitConverter.ToSingle(payloadData.Skip(8).Take(4).ToArray(), 0),
					},
					Vertices = new List<Vertex>
					{
						new Vertex {
							X = BitConverter.ToSingle(payloadData.Skip(12).Take(4).ToArray()),
							Y = BitConverter.ToSingle(payloadData.Skip(16).Take(4).ToArray()),
							Z = BitConverter.ToSingle(payloadData.Skip(20).Take(4).ToArray()),
						},
						new Vertex
						{
							X =BitConverter.ToSingle(payloadData.Skip(24).Take(4).ToArray()),
							Y = BitConverter.ToSingle(payloadData.Skip(28).Take(4).ToArray()),
							Z = BitConverter.ToSingle(payloadData.Skip(32).Take(4).ToArray()),
						},
						new Vertex
						{
							X = BitConverter.ToSingle(payloadData.Skip(36).Take(4).ToArray()),
							Y = BitConverter.ToSingle(payloadData.Skip(40).Take(4).ToArray()),
							Z = BitConverter.ToSingle(payloadData.Skip(44).Take(4).ToArray()),
						},
					}
				});

				payloadData.Clear();
			}

			return shape;
		}

		public void Dispose()
		{
			_shape = null;
			_fileStream.Dispose();
		}
	}
}
