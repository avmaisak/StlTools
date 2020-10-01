using System.IO;

namespace StlTools.Extensions
{
	public static class ByteExtensions
	{
		public static string ConvertToString(this byte[] bytes)
		{
			using var stream = new MemoryStream(bytes);
			using var streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
	}
}
