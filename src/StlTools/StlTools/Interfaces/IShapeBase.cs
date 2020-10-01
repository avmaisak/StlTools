using System.Collections.Generic;

namespace StlTools.Interfaces
{
	/// <summary>
	/// Модель. Базовый интерфейс.
	/// </summary>
	public interface IShapeBase<T>
	{
		/// <summary>
		/// Файл состоит из произвольного числа граней-треугольников.
		/// </summary>
		public ICollection<T> Facets { get; set; }
	}
}
