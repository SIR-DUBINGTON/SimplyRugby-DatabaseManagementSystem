using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SimplyRugby.MatchAndTrainingAmateur.Converters
{
    /// <summary>
    /// Class to convert a DateTime to a string in the format "yyyy-MM-dd"
    /// </summary>
    public class DateTimeToDateStringConverter : IValueConverter
    {
        /// <summary>
        /// Method to convert a DateTime to a string in the format "yyyy-MM-dd"
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("yyyy-MM-dd");
            }
            return null;
        }

        /// <summary>
        /// Method to convert a string in the format "yyyy-MM-dd" to a DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse((string)value, out DateTime date))
            {
                return date;
            }
            return null;
        }
    }
}
