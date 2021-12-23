using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TabangayTa.Helpers;
using TabangayTa.Models;
using TabangayTa.Services.ResourcePins;
using TabangayTa.Views;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TabangayTa.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private IResourcePinService ResourcePinService => DependencyService.Get<IResourcePinService>();
        private ResourcePinModel resourcePin;
        private CancellationTokenSource cts;

        #region Properties
        private string selectedResourceLogo;

        public string SelectedResourceLogo
        {
            get { return selectedResourceLogo; }
            set { SetProperty(ref selectedResourceLogo, value); }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set { SetProperty(ref map, value); }
        }

        private MapViewStateEnum stateEnum;

        public MapViewStateEnum StateEnum
        {
            get { return stateEnum; }
            set { SetProperty(ref stateEnum, value); }
        }
        #endregion

        #region Commands
        public ICommand ChangeStateCommand { get; set; }
        public ICommand SelectResourceCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            Title = "My Map";
            Map = new Map();
            Map.Pins.Clear();
            Map.MapClicked += async (sender, e) => {
                await Shell.Current.GoToAsync($"{nameof(AddNewResourcePinPage)}?{nameof(AddNewResourcePinViewModel.Lat)}={e.Position.Latitude}&{nameof(AddNewResourcePinViewModel.Lng)}={e.Position.Longitude}");
            };

            SelectedResourceLogo = string.Empty;

            ChangeStateCommand = new Command<MapViewStateEnum>((param) =>
            {
                if (param != stateEnum)
                    StateEnum = param;
            });

            SelectResourceCommand = new Command<string>((param) =>
            {
                ChangeStateCommand.Execute(MapViewStateEnum.Normal);
                MakeResourcePins(param);
            });

            RefreshCommand = new Command(async () =>
            {
                resourcePin = null;
                await OnAppearing();
            });

            MessagingCenter.Unsubscribe<AddNewResourcePinViewModel, Pin>(this, MessagingKeys.SetMapPins);
            MessagingCenter.Subscribe<AddNewResourcePinViewModel, Pin>(this, MessagingKeys.SetMapPins, (sender, arg) =>
            {
                Map.Pins.Add(new Pin
                {
                    Type = PinType.Place,
                    Address = arg.Address,
                    Label = arg.Label,
                    Position = new Position(arg.Position.Latitude, arg.Position.Longitude)
                });
            });
        }
        #endregion

        internal async Task OnAppearing()
        {
            ChangeStateCommand.Execute(MapViewStateEnum.Normal);
            if (resourcePin == null)
            {
                await InitializeGeolocation(true);
                await InitializeResourcePins(ResourceType.ChargingStation);
            }
        }

        private async Task GetLastLocation(Map map)
        {
            IsBusy = true;
            try
            {
                var location = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1)));
                else
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(SettingsService.Latitude, SettingsService.Longitude), Distance.FromKilometers(1)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task InitializeGeolocation(bool useMockLocation = false)
        {
            IsBusy = true;
            try
            {
                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    if (useMockLocation)
                    {
                        location.Latitude = 10.341238049657582;
                        location.Longitude = 123.93540581185819;
                    }

                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1)));
                    SettingsService.Latitude = location.Latitude;
                    SettingsService.Longitude = location.Longitude;
                }
            }
            catch (Xamarin.Essentials.FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Debug.WriteLine(fnsEx.Message);
                throw;
            }
            catch (Xamarin.Essentials.FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                await App.Current.MainPage.DisplayAlert("Phone location disabled.","Please enable your device location and run the app again.", "Okay");
                throw;
            }
            catch (Xamarin.Essentials.PermissionException pEx)
            {
                // Handle permission exception
                Debug.WriteLine(pEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task InitializeResourcePins(string resourceType)
        {
            IsBusy = true;
            try
            {
                int limit = 100;
                int cursor = 0;
                int remaining = 1;
                resourcePin = new ResourcePinModel();
                resourcePin.response = new Response();
                resourcePin.response.results = new List<Result>();

                do
                {
                    var ret = await ResourcePinService.GetResourcePins(limit, cursor);
                    if (ret != null)
                    {
                        cursor += limit;
                        remaining = ret.response.remaining;
                        if (remaining < limit) limit = remaining;


                        resourcePin.response.cursor = ret.response.cursor;
                        resourcePin.response.count = ret.response.count;
                        resourcePin.response.remaining = ret.response.remaining;
                        resourcePin.response.results.AddRange(ret.response.results);
                    }
                } while (remaining > 0);


                MakeResourcePins(resourceType);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void MakeResourcePins(string resourceType)
        {
            if (resourcePin == null) return;

            SelectedResourceLogo = resourceType;
            var pins = resourcePin.response.results.Where(i => !string.IsNullOrEmpty(i.ResourceType) && !string.IsNullOrEmpty(i.resourceStatus)
            && i.resourceStatus.Equals("Available") && i.ResourceType.Equals(resourceType));
            Map.Pins.Clear();
            foreach (var pin in pins)
            {
                if (pin != null && pin.geolocation != null && pin.geolocation.lng != 0 && pin.geolocation.lat != 0)
                {
                    Map.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Address = pin.geolocation.address,
                        Label = pin.locationName,
                        Position = new Position(pin.geolocation.lat, pin.geolocation.lng)
                    });
                }
            }
        }
    }
}
