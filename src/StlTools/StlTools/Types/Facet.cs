using System.Collections.Generic;
using StlTools.Interfaces;

namespace StlTools.Types
{
	/// <summary>
	/// Определяет треугольные грани.
	/// </summary>
	public class Facet: IFacet<Normal, Vertex>
	{
		/// <summary>
		/// Нормаль.
		/// </summary>
		public Normal Normal { get; set; }

		/// <summary>
		/// Вершины.
		/// </summary>
		public ICollection<Vertex> Vertices { get; set; }
	}
}
