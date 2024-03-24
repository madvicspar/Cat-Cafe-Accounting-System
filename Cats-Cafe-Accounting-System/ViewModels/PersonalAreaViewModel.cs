using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PersonalAreaViewModel : ObservableObject
    {
        private readonly ApplicationDbContext _dbContext;
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                SetProperty(ref isActive, value);
                OnPropertyChanged(nameof(IsActive));
            }
        }
        private EmployeeModel employee;
        public BitmapImage avatar;
        public BitmapImage Avatar
        {
            get { return avatar; }
            set
            {
                SetProperty(ref avatar, value);
                OnPropertyChanged(nameof(Avatar));
            }
        }
        private string photoAddress;
        public string PhotoAddress
        {
            get { return photoAddress; }
            set 
            { 
                SetProperty(ref photoAddress, value);
                CheckActivity();
            }
        }

        public EmployeeModel Employee
        {
            get { return employee; }
            set
            {
                SetProperty(ref employee, value);
                OnPropertyChanged(nameof(Employee));
                UpdatePhotoAddress();
            }
        }
        public ICommand UpdateEmployeeCommand { get; set; }
        public ICommand LoadPhotoCommand { get; set; }
        public ICommand DeletePhotoCommand { get; set; }
        public PersonalAreaViewModel(ApplicationDbContext context)
        {
            _dbContext = context;
            UpdateEmployeeCommand = new RelayCommand(ExecuteUpdateEmployeeCommand);
            LoadPhotoCommand = new RelayCommand(ExecuteLoadPhotoCommand);
            DeletePhotoCommand = new RelayCommand(ExecuteDeletePhotoCommand);
        }

        private void CheckActivity()
        {
            IsActive = PhotoAddress != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "UsersPhoto", "cat_default_icon.jpg");
        }

        private void UpdatePhotoAddress()
        {
            PhotoAddress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "UsersPhoto", "cat_default_icon.jpg");
            if (Employee != null)
            {
                var temp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "UsersPhoto", Employee.Username + "_icon.jpg");
                if (File.Exists(temp))
                    PhotoAddress = temp;
            }

            Avatar = BitmapFromUri(new Uri(PhotoAddress));
        }
        public void ExecuteUpdateEmployeeCommand()
        {
            _dbContext.Employees.Update(Employee);
            _dbContext.SaveChanges();
        }

        public void ExecuteLoadPhotoCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp|Все файлы|*.*";
            dlg.Title = "Загрузить аватар профиля";
            if (dlg.ShowDialog() == true)
            {
                ChangeAvatar(dlg.FileName);
            }
        }
        public void ChangeAvatar(string userPath)
        {
            string fileName = $"{Employee.Username}_icon.jpg";

            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "UsersPhoto", fileName);

            File.Copy(userPath, destinationPath, File.Exists(destinationPath));

            PhotoAddress = fileName;

            Application.Current.Dispatcher.Invoke(() =>
            {
                Avatar = null;
                Avatar = BitmapFromUri(new Uri(destinationPath));
            });
        }

        public static BitmapImage BitmapFromUri(Uri source)
        {
            var uriBuilder = new UriBuilder(source);
            uriBuilder.Query = $"nocache={Guid.NewGuid()}";

            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = uriBuilder.Uri;
            bitmap.EndInit();
            return bitmap;
        }

        public void ExecuteDeletePhotoCommand()
        {
            string fileName = $"{Employee.Username}_icon.jpg";

            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "UsersPhoto", fileName);

            File.Delete(destinationPath);

            UpdatePhotoAddress();
        }
    }
}