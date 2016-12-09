using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using System.Collections;

namespace PlantomaticVM
{
    // Takes a list, and if the list has at least one element then it returns False. If the list is null or has zero elements, it returns True. 
    // One use: If the list of plants is empty, then set the visible state of the "No matching plants" label to True.
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

    // Takes a boolean, and returns back TrueText if the parameter was True, or FalseText otherwise. 
    // One use: If the shopping cart state is False, then show the text, "Add to Cart"
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


    // TODO DO I NEED THIS?
    public class IntToStringConverter : IValueConverter
    {
        public string TrueText { set; get; }
        public string FalseText { set; get; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0 ? TrueText : FalseText;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }


    //Takes a list of objects, and returns the one at a specified position.
    //One use: In the Filters page XAML, if the user picks the 3rd string in the filter, then this gives back the 3rd object in the associated list
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