using Contacts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    public partial class ContactsPage : ContentPage
    {
        ContactsViewModel _viewModel;

        public ContactsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ContactsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}