using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using System.IO;
using System.Net;
using System.Threading;


namespace PlantomaticVM
{
    [ContentProperty("Source")]
    public class GetImageResource : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null) return null;
            return ImageSource.FromResource(Source);
        }
    }

    // TODO THIS METHOD IS NOT CURRENTLY USED. KEEPING IT HERE NOW JUST IN CASE I EVER NEED IT
    // Takes a number representing a count of plants and a max count of plants and returns a height. 
    // Used on the shopping list page to make the boxes representing the number of plants proportionally 
    // taller if there are more plants with flowers in a given month
    public class CalcBoxHeight : IMarkupExtension
    {
        public double count { set; get; }
        public double maxCount { set; get; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (maxCount == 0 || count == 0)
                return (double) 1; //so that empty months get some kind of indicator

            //Why times 64? Because that is the number of units we want the tallest box to be
            return (double)((count / maxCount) * 64);
        }

    }

    public class CountToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return (double) value * 64 + 1; //+1 so that even empty months show something
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return (double) value / 64;
        }

    }

    // Takes an object, and if the object exists then it returns False. If the object is null, it returns True. 
    // One use: If nothing is selected in the list of plants, then set the visible state of the "Select an plant to see detail" label to True.
    public class ObjectToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return true;

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes an object, and if the object exists then it returns True. If the object is null, it returns False. 
    // One use: If nothing is selected in the list of plants, then set the visible state of the Show Detail panel to False.
    public class ObjectToBoolConverterOpposite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    // Takes a list, and if the list has at least one element then it returns TRUE. If the list is null or has zero elements, it returns True. 
    // One use: If the list of plants is empty, then set the visible state of the Detail panel label to False.
    public class ListToBoolConverterOpposite : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;

            return ((IList)value).Count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

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


/*
 * This is here because apparently Xamarin.Forms doesn't provide support for all of .NET, so the 
 * synchronous web request function isn't available. 
 * 
 * To properly work as an extension, this needed to be in a separate namespace and also be static.
 * 
 * See this for more info: http://stackoverflow.com/questions/17187347/webheadercollection-httpwebrequest-on-xamarin
 * 
 */

namespace WebUtility
{
    public static class WebUtility
    {
        public static WebResponse GetResponse(this WebRequest request)
        {
            ManualResetEvent evt = new ManualResetEvent(false);
            WebResponse response = null;
            request.BeginGetResponse((IAsyncResult ar) =>
            {
                response = request.EndGetResponse(ar);
                evt.Set();
            }, null);
            evt.WaitOne();
            return response as WebResponse;
        }

        public static Stream GetRequestStream(this WebRequest request)
        {
            ManualResetEvent evt = new ManualResetEvent(false);
            Stream requestStream = null;
            request.BeginGetRequestStream((IAsyncResult ar) =>
            {
                requestStream = request.EndGetRequestStream(ar);
                evt.Set();
            }, null);
            evt.WaitOne();
            return requestStream;
        }
    }
}
