using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Contacts.Models;
using Contacts.Services;
using Contacts.Views;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels
{
    [QueryProperty(nameof(ContactId), nameof(ContactId))]
    public class ContactDetailViewModel : BaseViewModel
    {
        private int contactId;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string comment;

        public int Id { get; set; }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
        }


        public string FullName => $"{FirstName} {LastName}";

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        
        public string Comment
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        public int ContactId
        {
            get
            {
                return contactId;
            }
            set
            {
                contactId = value;
                LoadContactId(value);
            }
        }

        public Command EditCommand { get; }
        public Command DeleteCommand { get; }

        public ContactDetailViewModel()
        {
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(async () => await OnDelete());
        }

        private async void OnEdit()
        {
            await Shell.Current.GoToAsync($"{nameof(ContactFormPage)}?ContactId={ContactId}");
        }

        public async void LoadContactId(int contactId)
        {
            try
            {
                var contact = await App.DatabaseService.GetContactAsync(contactId);
                Id = contact.Id;
                FirstName = contact.FirstName;
                LastName = contact.LastName;
                PhoneNumber = contact.PhoneNumber;
                Email = contact.Email;
                Comment = contact.Comment;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to Load Contact: {ex.Message}");
            }
        }

        private async Task OnDelete()
        {
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                "Confirmer la suppression",
                "Êtes-vous sûr de vouloir supprimer ce contact ?",
                "Oui", "Non");

            if (isConfirmed)
            {
                try
                {
                    await App.DatabaseService.DeleteContactAsync(Id);
                    await Shell.Current.GoToAsync("..");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erreur lors de la suppression du contact : {ex.Message}");
                    await Application.Current.MainPage.DisplayAlert(
                        "Erreur",
                        "Impossible de supprimer ce contact.",
                        "OK");
                }
            }
        }
    }
}
