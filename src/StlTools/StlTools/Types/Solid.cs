using StlTools.Interfaces;

namespace StlTools.Types
{
	/// <summary>
	/// Файл типа ASCII STL начинается со строки: solid name
	/// где name — необязательная строка (но если name опущено, всё равно должен быть пробел после solid).
	/// </summary>
	public class Solid: ISolid
	{
		/// <summary>
		/// где name — необязательная строка (но если name опущено, всё равно должен быть пробел после solid).
		/// </summary>
		public string Name { get; set; }
	}
}
