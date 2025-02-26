using System.Globalization;

namespace TaxiDC2.Converters;

public class BoolToColorConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool b)
		{
			return b ? Colors.Green : Color.FromArgb("#ff313131");
		}
		return Colors.SlateGrey;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}