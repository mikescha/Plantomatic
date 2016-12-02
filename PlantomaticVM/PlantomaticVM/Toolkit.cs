using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using System.Collections;

namespace PlantomaticVM
{
    public class ListToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            return ((IList)value).Count == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class BoolToStringConverter: IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }

    [ContentProperty("Items")]
    public class ObjectToIndexConverter<T> : IValueConverter
    {
        public IList<T> Items { set; get; }

        public ObjectToIndexConverter()
        {
            Items = new List<T>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is T) || Items == null)
                return -1;

            return Items.IndexOf((T)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;

            if (index < 0 || Items == null || index >= Items.Count)
                return null;

            return Items[index];
        }
    }
}