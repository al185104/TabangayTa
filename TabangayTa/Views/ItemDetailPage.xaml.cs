using System.ComponentModel;
using TabangayTa.ViewModels;
using Xamarin.Forms;

namespace TabangayTa.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}