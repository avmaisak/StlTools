using System.IO;
using System.Threading.Tasks;

namespace StlTools
{
	/// <summary>
	/// Читатель файлов.
	/// </summary>
	public interface IFileReader
	{
		/// <summary>
		/// Выполняет асинхронной чтений файла.
		/// </summary>
		/// <param name="stream">Поток.</param>
		/// <returns></returns>
		public Task ReadFileStreamAsync(Stream stream);
	}
}
