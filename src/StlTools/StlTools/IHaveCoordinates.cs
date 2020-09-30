namespace StlTools
{
	/// <summary>
	///  Имеются координаты.
	/// </summary>
	public interface IHaveCoordinates
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
