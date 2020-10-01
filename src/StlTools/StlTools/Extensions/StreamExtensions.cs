using System.IO;

namespace StlTools.Extensions
{
	public static class StreamExtensions
	{
		/// <summary>
		/// Является бинарным.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static bool IsBinary(this Stream stream)
		{
			var bFlag = true;
			// Iterate through stream & check ASCII value of each byte.
			for (var nPosition = 0; nPosition < stream.Length; nPosition++)
			{
				var a = stream.ReadByte();

				if (!(a >= 0 && a <= 127)) break;
				if (stream.Position == (stream.Length)) bFlag = false;
			}
			return bFlag;
		}
	}
}
