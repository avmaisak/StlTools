using System.Collections.Generic;

namespace StlTools.Interfaces
{
	/// <summary>
	///  Определяет треугольные грани.
	/// </summary>
	public interface IFacet<TNormal,TVerticles> where TNormal : INormal
	{
		/// <summary>
		/// Нормаль.
		/// </summary>
		public TNormal Normal { get; set; }

		/// <summary>
		/// Вершины.
		/// </summary>
		public ICollection<TVerticles> Vertices { get; set; }
	}
}
