using System.Net;
using System.Threading.Tasks;
using TabangayTa.Helpers;
using TabangayTa.Models;
using TabangayTa.Models.Request;
using TabangayTa.Models.Resp;
using TabangayTa.Services.RequestProvider;
using Xamarin.Forms;

namespace TabangayTa.Services.ResourcePins
{
    public class ResourcePinService : IResourcePinService
    {
        private const string ApiUrlResourcePin = "resourcepin";
        private IRequestProvider requestProvider => DependencyService.Get<IRequestProvider>();

        public async Task<ResourceResponseModel> AddResourcePins(ResourceRequestModel resource)
        {
            ResourceResponseModel result = null;
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseIdentityEndpoint, $"{ApiUrlResourcePin}");
            try
            {
                // needs cleanup
                object ret = await requestProvider.PostAsync<object>(uri, resource, "1fa75cb628cf7c7a79585996f8264395");
                if(ret != null)
                {
                    result = new ResourceResponseModel()
                    {
                        response = new Models.Resp.Response { 
                            status = HttpStatusCode.OK.ToString()
                        }
                    };
                }
            }
            catch (HttpRequestExceptionEx ex) when (ex.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {
                await App.Current.MainPage.DisplayAlert("OOS", "Something went wrong in updating online.\nPlease contact your administrator.", "Back");
                result = null;
            }

            return result;
        }

        public async Task<ResourcePinModel> GetResourcePins(int limit, int cursor = 0, bool descending = false)
        {
            ResourcePinModel result;

            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseIdentityEndpoint, $"{ApiUrlResourcePin}?limit={limit}&cursor={cursor}&descending{descending}");
            try
            {
                result = await requestProvider.GetAsync<ResourcePinModel>(uri);
            }
            catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound)
            {
                await App.Current.MainPage.DisplayAlert("OOS", "Something went wrong in updating online.\nPlease contact your administrator.", "Back");
                result = null;
            }

            return result;
        }
    }
}
