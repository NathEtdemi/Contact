using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Contacts.Models;
using Xamarin.Essentials;
using System.IO;


namespace Contacts.ViewModels
{
    [QueryProperty(nameof(ContactId), nameof(ContactId))]
    public class ContactFormViewModel : BaseViewModel
    {
        private int contactId;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string comment;
        private string imagePath;

        public string ImagePath
        {
            get => imagePath;
            set => SetProperty(ref imagePath, value);
        }

        public Command AddPhotoCommand { get; }       

        public Command SaveCommand { get; }

        public ContactFormViewModel()
        {
            Title = "Nouveau";
            SaveCommand = new Command(async () => await SaveContact());
            AddPhotoCommand = new Command(async () => await OnAddPhoto());
        }

        public int ContactId
        {
            get => contactId;
            set
            {
                contactId = value;
                LoadContact(value);
            }
        }

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

        private async void LoadContact(int id)
        {
            try
            {
                Title = "Modifier";
                var contact = await App.DatabaseService.GetContactAsync(id);
                if (contact != null)
                {
                    FirstName = contact.FirstName;
                    LastName = contact.LastName;
                    PhoneNumber = contact.PhoneNumber;
                    Email = contact.Email;
                    Comment = contact.Comment;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Contact");
            }
        }

        private async Task SaveContact()
        {
            try
            {
                var contact = new Models.Contact
                {
                    Id = ContactId,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = PhoneNumber,
                    Email = Email,
                    Comment = Comment,
                    ImagePath = ImagePath
                };

                if (ContactId == 0)
                {
                    await App.DatabaseService.SaveContactAsync(contact);
                }
                else
                {
                    await App.DatabaseService.SaveContactAsync(contact);
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save contact: {ex.Message}");
            }
        }

        private async Task OnAddPhoto()
        {
            string action = await App.Current.MainPage.DisplayActionSheet(
                "Ajouter une photo",
                "Annuler",
                null,
                "Prendre une photo",
                "Choisir depuis la galerie");

            if (action == "Prendre une photo")
            {
                await CapturePhoto();
            }
            else if (action == "Choisir depuis la galerie")
            {
                await PickPhoto();
            }
        }

        private async Task CapturePhoto()
        {
            if (!await RequestPermissionsAsync())
                return;
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                    return;

                var filePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(newStream);
                }

                ImagePath = filePath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", $"Impossible de prendre une photo: {ex.Message}", "OK");
            }
        }

        private async Task PickPhoto()
        {
            if (!await RequestPermissionsAsync())
                return;
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo == null)
                    return;

                var filePath = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(filePath))
                {
                    await stream.CopyToAsync(newStream);
                }

                ImagePath = filePath;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erreur", $"Impossible de choisir une photo: {ex.Message}", "OK");
            }
        }

        private async Task<bool> RequestPermissionsAsync()
        {
            var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            var storageReadStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted || storageReadStatus != PermissionStatus.Granted)
            {
                var cameraRequest = await Permissions.RequestAsync<Permissions.Camera>();
                var storageRequest = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (cameraRequest != PermissionStatus.Granted || storageRequest != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Permissions Denied",
                        "Unable to access camera or storage. Please check app settings.",
                        "OK"
                    );
                    return false;
                }
            }

            return true;
        }
    }
}
