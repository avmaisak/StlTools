using System.Collections.Generic;

namespace StlTools
{
	/// <summary>
	///  Определяет треугольные грани.
	/// </summary>
	public interface IFacet
	{
		/// <summary>
		/// Нормаль.
		/// </summary>
		public INormal Normal { get; set; }

		/// <summary>
		/// Вершины.
		/// </summary>
		public ICollection<IVertex> Vertices { get; set; }
	}
}
