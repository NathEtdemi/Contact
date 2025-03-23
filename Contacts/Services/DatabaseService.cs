using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Contacts.Models;
using Xamarin.Forms.Shapes;
using System.Diagnostics;

namespace Contacts.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Contact>();
        }

        public Task<List<Contact>> GetContactsAsync()
        {
            return _database.Table<Contact>().OrderBy(c => c.LastName).ToListAsync();
        }

        public Task<Contact> GetContactAsync(int id)
        {
            return _database.Table<Contact>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<int> SaveContactAsync(Contact contact)
        {
            if (contact.Id != 0)
                return _database.UpdateAsync(contact);
            return _database.InsertAsync(contact);
        }

        public Task<int> DeleteContactAsync(Contact contact)
        {
            return _database.DeleteAsync(contact);
        }

        public async Task DeleteContactAsync(int id)
        {
            try
            {
                var contact = await _database.Table<Contact>().FirstOrDefaultAsync(c => c.Id == id);
                if (contact != null)
                {
                    await _database.DeleteAsync(contact);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de la suppression du contact : {ex.Message}");
                throw;
            }
        }

    }
}
