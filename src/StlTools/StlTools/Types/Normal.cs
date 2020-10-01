using StlTools.Interfaces;

namespace StlTools.Types
{
	/// <summary>
	/// Нормаль.
	/// </summary>
	public class Normal : INormal
	{
		/// <summary>
		/// Координата X.
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// Координата Y.
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Координата Z.
		/// </summary>
		public double Z { get; set; }
	}
}
