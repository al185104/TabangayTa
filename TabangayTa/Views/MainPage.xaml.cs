using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TabangayTa.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TabangayTa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        readonly MainViewModel _viewmodel;
        CancellationTokenSource cts;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewmodel = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            //_ = InitializeGeolocation(true);
            await _viewmodel.OnAppearing();
            base.OnAppearing();
        }

        public async Task InitializeGeolocation(bool useMockLocation = false)
        {
            IsBusy = true;
            try
            {
                var request = new Xamarin.Essentials.GeolocationRequest(Xamarin.Essentials.GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Xamarin.Essentials.Geolocation.GetLocationAsync(request, cts.Token);
                if (location != null)
                {
                    if (useMockLocation)
                    {
                        location.Latitude = 10.341238049657582;
                        location.Longitude = 123.93540581185819;
                    }

                    //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(2)));
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
                Debug.WriteLine(fneEx.Message);
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
    }
}