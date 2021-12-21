using System.Threading.Tasks;

namespace TabangayTa.Services.Settings
{
    public interface ISettingsService
    {
        #region 
        string AuthAccessToken { get; set; }
        string AuthIdToken { get; set; }
        bool UseMocks { get; set; }
        string IdentityEndpointBase { get; set; }
        string GatewayShoppingEndpointBase { get; set; }
        string GatewayMarketingEndpointBase { get; set; }
        bool UseFakeLocation { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        bool AllowGpsLocation { get; set; }
        #endregion

        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        double GetValueOrDefault(string key, double defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
        Task AddOrUpdateValue(string key, double value);
    }
}
