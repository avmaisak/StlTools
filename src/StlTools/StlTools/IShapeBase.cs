using System.Collections.Generic;

namespace StlTools
{
	/// <summary>
	/// Модель. Базовый интерфейс.
	/// </summary>
	public interface IShapeBase
	{
		/// <summary>
		/// Файл состоит из произвольного числа граней-треугольников.
		/// </summary>
		public ICollection<IFacet> Facets { get; set; }
	}
}
