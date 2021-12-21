﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TabangayTa.Helpers;
using TabangayTa.Models;
using TabangayTa.Services.RequestProvider;
using Xamarin.Forms;

namespace TabangayTa.Services.ResourcePins
{
    public class ResourcePinService : IResourcePinService
    {
        private const string ApiUrlResourcePin = "resourcepin";
        private IRequestProvider requestProvider => DependencyService.Get<IRequestProvider>();

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
