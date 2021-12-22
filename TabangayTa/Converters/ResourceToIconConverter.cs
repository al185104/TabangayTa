using System;
using System.Globalization;
using TabangayTa.Helpers;
using Xamarin.Forms;

namespace TabangayTa.Converters
{
    public class ResourceToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string))
            {
                throw new InvalidOperationException("The target must be a String!");
            }

            if (string.IsNullOrEmpty(value as string))
                return IconFont.HelpCircle;

            string icon = (string)value;

            string ret = IconFont.HelpCircle;

            switch (icon)
            {
                case ResourceType.ChargingStation:
                    ret = IconFont.BatteryCharging70;
                    break;
                case ResourceType.DrinkingWater:
                    ret = IconFont.Water;
                    break;
                case ResourceType.SignalWiFi:
                    ret = IconFont.Signal;
                    break;
                case ResourceType.ClinicsPharmacies:
                    ret = IconFont.Pharmacy;
                    break;
                case ResourceType.ATMRemittance:
                    ret = IconFont.Atm;
                    break;
                case ResourceType.Gas:
                    ret = IconFont.GasStation;
                    break;
                case ResourceType.WaterStation:
                    ret = IconFont.WaterPump;
                    break;
                case ResourceType.Grocery:
                    ret = IconFont.Cart;
                    break;
                default:
                    ret = IconFont.HelpCircle;
                    break;
            }

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
