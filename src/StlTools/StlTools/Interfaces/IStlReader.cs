using System.Threading.Tasks;

namespace StlTools.Interfaces
{
	/// <summary>
	/// Читатель STL из файла.
	/// </summary>
	public interface IStlReader<T>
	{
		/// <summary>
		/// Читать файл и преобразовать в STL формат.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		Task<T> ReadAndConvertAsync(string path);
	}
}
