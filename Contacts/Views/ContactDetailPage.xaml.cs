using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;
using Contacts.Services;
using Contacts.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage()
        {
            InitializeComponent();
            BindingContext = new ContactDetailViewModel();
        }
    }
}