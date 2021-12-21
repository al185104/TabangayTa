using System;
using System.Collections.Generic;
using System.ComponentModel;
using TabangayTa.Models;
using TabangayTa.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TabangayTa.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}