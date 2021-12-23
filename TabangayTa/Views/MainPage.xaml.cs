using System;
using System.Diagnostics;
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

        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewmodel = new MainViewModel();
        }

        protected override async void OnAppearing()
        {
            await _viewmodel.OnAppearing();
            base.OnAppearing();
        }
    }
}