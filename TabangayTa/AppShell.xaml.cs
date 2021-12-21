using System;
using System.Collections.Generic;
using TabangayTa.ViewModels;
using TabangayTa.Views;
using Xamarin.Forms;

namespace TabangayTa
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
