namespace StlTools.Interfaces
{
	/// <summary>
	/// Файл типа ASCII STL начинается со строки: solid name
	/// где name — необязательная строка (но если name опущено, всё равно должен быть пробел после solid).
	/// </summary>
	public interface ISolid
	{
		/// <summary>
		/// Файл типа ASCII STL начинается со строки: solid name
		/// где name — необязательная строка (но если name опущено, всё равно должен быть пробел после solid).
		/// </summary>
		public string Name { get; set; }
	}
}
