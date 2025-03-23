using Contacts.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Contacts.Models;
using Contacts.Views;
using System.Diagnostics;

namespace Contacts.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private Contact _selectedContact;

        public string FullName => $"{_selectedContact.FirstName} {_selectedContact.LastName}";


        public ObservableCollection<Contact> Contacts { get; }
        public Command LoadContactsCommand { get; }
        public Command AddContactCommand { get; }
        public Command<Contact> ContactTapped { get; }
        public Command<Contact> ContactEditAsked { get; }
        public Command<Contact> ContactDeleteAsked { get; }


        public ContactsViewModel()
        {
            Title = "Contacts";
            Contacts = new ObservableCollection<Contact>();
            LoadContactsCommand = new Command(async () => await ExecuteLoadContactsCommand());

            ContactTapped = new Command<Contact>(OnContactSelected);
            ContactEditAsked = new Command<Contact>(OnEditContact);

            AddContactCommand = new Command<Contact>(OnAddContact);

            ContactDeleteAsked = new Command<Contact>(OnDeleteContact);
        }

        async Task ExecuteLoadContactsCommand()
        {
            IsBusy = true;

            try
            {
                Contacts.Clear();
                var contacts = await App.DatabaseService.GetContactsAsync();
                foreach (var contact in contacts)
                {
                    Contacts.Add(contact);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedContact = null;
        }

        public Contact SelectedContact
        {
            get => _selectedContact;
            set
            {
                SetProperty(ref _selectedContact, value);
                OnContactSelected(value);
            }
        }

        private async void OnAddContact(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ContactFormPage));
        }

        private async void OnEditContact(Contact contact)
        {
            if (contact == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ContactFormPage)}?ContactId={contact.Id}");
        }

        async void OnContactSelected(Contact contact)
        {
            if (contact == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ContactDetailPage)}?ContactId={contact.Id}");
        }

        private async void OnDeleteContact(Contact contact)
        {
            if (contact == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert(
                "Supprimer le contact",
                $"Voulez-vous vraiment supprimer {contact.FirstName} {contact.LastName} ?",
                "Oui",
                "Non");

            if (confirm)
            {
                await App.DatabaseService.DeleteContactAsync(contact);

                Contacts.Remove(contact);
            }
        }

    }
}
