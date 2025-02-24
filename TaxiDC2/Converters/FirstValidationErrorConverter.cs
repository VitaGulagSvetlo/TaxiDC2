﻿using System.Collections;
using System.Globalization;

namespace TaxiDC2.Converters
{
	public class FirstValidationErrorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ICollection<string> errors = value as ICollection<string>;
			return errors != null && errors.Count > 0 ? errors.ElementAt(0) : null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	public class FirstErrorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is IEnumerable errors)
			{
				foreach (var error in errors)
				{
					if (error != null)
					{
						return error.ToString();
					}
				}
			}
			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}