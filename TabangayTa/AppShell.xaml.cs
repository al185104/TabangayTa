using TabangayTa.Views;
using Xamarin.Forms;

namespace TabangayTa
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(AddNewResourcePinPage), typeof(AddNewResourcePinPage));
        }
    }
}
