using Windows.Globalization.NumberFormatting;

namespace ERP.Client.Formatter
{
    public class DoubleFormatter : INumberFormatter
    {
        public string Format(long value)
        {
            return "0";
        }

        public string Format(ulong value)
        {
            return "0";
        }

        public string Format(double value)
        {
            return value.ToString("0");
        }
    }
}
