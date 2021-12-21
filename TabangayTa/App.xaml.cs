using System;
using TabangayTa.Services;
using TabangayTa.Services.RequestProvider;
using TabangayTa.Services.ResourcePins;
using TabangayTa.Services.Settings;
using TabangayTa.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TabangayTa
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<SettingsService>();
            DependencyService.Register<RequestProvider>();
            DependencyService.Register<ResourcePinService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
