using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using SilviasGallery.Annotations;
using SilviasGallery.Services;
using Xamarin.Forms;

namespace SilviasGallery.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        #region Static Fields and Constants
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
        #endregion

        #region Fields
        private User _addNewUser = new User();
        private DataBaseService _dataBaseService;
        private User _user;
        private ObservableCollection<User> _users = new ObservableCollection<User>();
        #endregion

        #region Properties and Indexers
        public User AddNewUser
        {
            get => _addNewUser;
            set
            {
                _addNewUser = value;
                OnPropertyChanged(nameof(AddNewUser));
            }
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand DecreaseUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand IncreaseUserCommand { get; set; }

        public User SelectedUser
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public MainPageViewModel()
        {
            SetDataBase();
            InitializeCommands();
        }
        #endregion

        #region Private members
        private async void AddUser()
        {
            if (string.IsNullOrEmpty(AddNewUser.FirstName) || string.IsNullOrEmpty(AddNewUser.LastName))
                return;

            AddNewUser.Token = Guid.NewGuid().ToString().Substring(0, 8);
            Users.Add(AddNewUser.Clone());

            await Semaphore.WaitAsync();
            await _dataBaseService.SaveUserAsync(AddNewUser);
            Semaphore.Release();

            GetData();

            AddNewUser = new User();
        }

        private void DecreaseEditUser()
        {
            EditUser(-1);
        }

        private async void DeleteUser()
        {
            if (SelectedUser == null)
                return;

            await Semaphore.WaitAsync();
            await _dataBaseService.DeleteUserAsync(SelectedUser);
            Semaphore.Release();

            Users.Remove(SelectedUser);
        }

        private async void EditUser(int negativeOrPositive)
        {
            if (SelectedUser == null)
                return;

            if (negativeOrPositive == -1 && SelectedUser.NumberOfPersons - 1 < 0)
                return;

            var user = Users.Single(x => x.Id == SelectedUser.Id);

            Users.Remove(user);

            user.NumberOfPersons += negativeOrPositive;

            Users.Add(user);

            await Semaphore.WaitAsync();
            await _dataBaseService.SaveUserAsync(user);
            Semaphore.Release();
        }

        private async void GetData()
        {
            await Semaphore.WaitAsync();
            Users = new ObservableCollection<User>(await _dataBaseService.GetUsersAsync());
            Semaphore.Release();
        }

        private void IncreaseEditUser()
        {
            EditUser(1);
        }

        private void InitializeCommands()
        {
            AddUserCommand = new Command(AddUser);
            IncreaseUserCommand = new Command(IncreaseEditUser);
            DecreaseUserCommand = new Command(DecreaseEditUser);
            DeleteUserCommand = new Command(DeleteUser);
        }

        private async void SetDataBase()
        {
            await Semaphore.WaitAsync();
            _dataBaseService = await DataBaseService.Instance;
            Semaphore.Release();

            GetData();
        }
        #endregion

        #region Protected members
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
