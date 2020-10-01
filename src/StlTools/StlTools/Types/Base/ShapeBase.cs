using System;
using System.Collections.Generic;
using StlTools.Interfaces;
using StlTools.Resources;

namespace StlTools.Types.Base
{
	/// <summary>
	/// Универсальный тип модели. Содердит свойства как бинарного так и ASCII формата.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ShapeBase<T> : IShapeAscii<T>, IShapeBinary<T>
	{
		/// <summary>
		/// Файл типа ASCII STL начинается со строки: solid name
		/// где name — необязательная строка (но если name опущено, всё равно должен быть пробел после solid).
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Заголовок.
		/// Файл начинается с заголовка из 80 символов. (который обычно игнорируется, но не должен начинаться с 'solid', так как с этой последовательности начинается файл ASCII STL).
		/// </summary>
		public string Header { get; set; }

		/// <summary>
		/// 4-байтовое беззнаковое целое число (little-endian), указывающее количество треугольных граней в данном файле.
		/// </summary>
		public uint LittleEndian { get; set; }

		/// <summary>
		/// Файл состоит из произвольного числа граней-треугольников.
		/// </summary>
		public ICollection<T> Facets { get; set; }

		/// <summary>
		/// Является бинарным.
		/// </summary>
		public bool IsBinary =>
			Header != null &&
			string.IsNullOrWhiteSpace(Header) &&
			Header.Contains(StlToolsResources.SolidToken) &&
			!Header.StartsWith(StlToolsResources.SolidToken) &&
			LittleEndian > 0 &&
			Facets != null &&
			Facets.Count > 0 &&
			Facets.Count == LittleEndian;

		/// <summary>
		/// Является ASCII.
		/// </summary>
		public bool IsAscii =>
			!string.IsNullOrWhiteSpace(Name) &&
			string.IsNullOrWhiteSpace(Header) &&
			LittleEndian == 0 &&
			Facets != null &&
			Facets?.Count > 0;

		/// <summary>
		/// Модель некорректна.
		/// </summary>
		public bool BadShape =>
			string.IsNullOrWhiteSpace(Name) && 
			Facets == null || 
			Facets.Count == 0;
	}
}
