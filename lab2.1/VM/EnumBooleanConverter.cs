using System;
using System.Globalization;
using System.Windows.Data;

namespace lab2._1.View
{
    public enum Algoritmh { Kuznechik, Aes };
    class EnumBooleanConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Algoritmh alg = Algoritmh.Kuznechik;
            string p;
            if ((Algoritmh)value == alg)
            {
                p = "Kuznechik";
            }
            else
            {
                p = "Aes";
            }
            //return parameter.Equals(value);
            return (string)parameter == p;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Algoritmh alg = Algoritmh.Kuznechik;
            if (parameter != null && parameter.ToString() == "Kuznechik")
                alg = Algoritmh.Kuznechik;
            else if (parameter != null && parameter.ToString() == "Aes")
                alg = Algoritmh.Aes;

            return ((bool)value) ? alg : Binding.DoNothing;
        }

    }
}
