using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TabangayTa.Helpers;
using TabangayTa.Models;
using TabangayTa.Models.Request;
using TabangayTa.Services.ResourcePins;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TabangayTa.ViewModels
{
    [QueryProperty(nameof(Lat), nameof(Lat))]
    [QueryProperty(nameof(Lng), nameof(Lng))]
    public class AddNewResourcePinViewModel : BaseViewModel
    {
        private IResourcePinService ResourcePinService => DependencyService.Get<IResourcePinService>();

        #region Properties
        private double lat;

        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }
        private double lng;

        public double Lng
        {
            get { return lng; }
            set { lng = value; }
        }
        private string typeSelection = null;

        public string TypeSelection
        {
            get { return typeSelection; }
            set { SetProperty(ref typeSelection, value); }
        }
        private string status = null;

        public string Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }
        private string name = null;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        private string address = null;

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        #endregion

        #region Constructor
        public AddNewResourcePinViewModel()
        {
            Title = "Add new resource";

            SendResourceCommand = new Command(async() =>
            {
                if (string.IsNullOrEmpty(typeSelection) ||
                    string.IsNullOrEmpty(name) ||
                    string.IsNullOrEmpty(address) ||
                    string.IsNullOrEmpty(status) ||
                    lat == 0 ||
                    lng == 0 )
                    return;

                IsBusy = true;

                Geocoder geoCoder = new Geocoder();

                Position position = new Position(Lat, Lng);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                string _address = possibleAddresses.FirstOrDefault();
                
                if (string.IsNullOrEmpty(_address))
                    _address = Address;

                var ret = await ResourcePinService.AddResourcePins(new ResourceRequestModel
                {
                    geolocation = new Models.Request.Geolocation { 
                        address = _address,
                        lat = Lat,
                        lng = Lng
                    },
                    locationName = Name,
                    TextAddress = Address,
                    resourceStatus = Status,
                    ResourceType = TypeSelection
                });


                IsBusy = false;

                if (ret != null)
                {
                    MessagingCenter.Send(this, MessagingKeys.SetMapPins, new Pin
                    {
                        Type = PinType.Place,
                        Address = _address,
                        Label = Name,
                        Position = new Position(Lat, Lng)
                    });

                    await Shell.Current.DisplayAlert("Resource added succesfully",
                        "Thank you for adding a new resource, your contribution is much appreciated. Please continue to help us keep the locations up-to-date by posting more resources near you.", 
                        "Okay");

                    await Shell.Current.GoToAsync("..");
                }
            });
        }
        #endregion

        public ICommand SendResourceCommand { get; set; }
    }
}
