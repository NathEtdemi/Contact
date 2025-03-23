using Contacts.ViewModels;
using Contacts.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Contacts
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ContactDetailPage), typeof(ContactDetailPage));
            Routing.RegisterRoute(nameof(ContactFormPage), typeof(ContactFormPage));
        }

    }
}
