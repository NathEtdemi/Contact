using Contacts.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactFormPage : ContentPage
    {
        public ContactFormPage()
        {
            InitializeComponent();
            BindingContext = new ContactFormViewModel();
        }
    }
}