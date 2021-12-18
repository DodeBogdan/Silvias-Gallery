using System.Collections.Generic;
using System.Threading.Tasks;
using NuGet.Common;
using SilviasGallery.Utilities;
using SQLite;

namespace SilviasGallery.Services
{
    public class DataBaseService
    {
        #region Static Fields and Constants
        private static SQLiteAsyncConnection _database;

        public static readonly AsyncLazy<DataBaseService> Instance = new AsyncLazy<DataBaseService>(async () =>
        {
            var instance = new DataBaseService();
            var result = await _database.CreateTableAsync<User>();
            return instance;
        });
        #endregion

        #region Constructors
        public DataBaseService()
        {
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
        #endregion

        #region Public members
        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }

        public Task<User> GetUserAsync(int id)
        {
            return _database.Table<User>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            var userExist = GetUserAsync(user.Id).Result;

            if (userExist != null)
            {
                return _database.UpdateAsync(user);
            }
            return _database.InsertAsync(user);
        }
        #endregion
    }
}
