using System.Collections.Generic;
using System.Text;

namespace Spectrum.Reports.Exporters
{
	public static class CsvHelper
	{
		public static string BuildCsv(IEnumerable<string> headers, IEnumerable<string[]> rows)
		{
			var builder = new StringBuilder();

			if (headers != null)
				builder.AppendLine(BuildRow(headers));

			if (rows != null)
			{
				foreach (var row in rows)
					builder.AppendLine(BuildRow(row));
			}

			return builder.ToString();
		}

		private static string BuildRow(IEnumerable<string> values)
		{
			if (values == null)
				return string.Empty;

			var escapedValues = new List<string>();
			foreach (var value in values)
				escapedValues.Add(Escape(value));

			return string.Join(",", escapedValues);
		}

		private static string Escape(string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0)
				return "\"" + value.Replace("\"", "\"\"") + "\"";

			return value;
		}
	}
}
