using Contacts.Services;
using Contacts.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contacts
{
    public partial class App : Application
    {
        private static DatabaseService databaseService;

        public static DatabaseService DatabaseService
        {
            get
            {
                if (databaseService == null)
                {
                    databaseService = new DatabaseService(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contacts.db3"));
                }

                return databaseService;
            }
        }
            
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
