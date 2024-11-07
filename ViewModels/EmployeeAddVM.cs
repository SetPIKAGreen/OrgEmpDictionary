using OrganizationsEmployeesDictionaryWPF.Models;
using OrganizationsEmployeesDictionaryWPF.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrganizationsEmployeesDictionaryWPF.View;
using OrganizationsEmployeesDictionaryWPF.Interface;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class EmployeeAddVM: BaseVM
    {
        

        
        private List<Organization> _organizations;
        private string _firstName;
        private string _lastName;
        private int _age;
        private string _position;
        private string _photoPath;
        private Organization _selectedOrganization;
 

        public List<Organization> Organizations
        {
            get => _organizations;
            set
            {
                _organizations = value;
                OnPropertyChange(nameof(Organizations));
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChange(nameof(FirstName));
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChange(nameof(LastName));
            }
        }
        public int Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChange(nameof(Age));
            }
        }
        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChange(nameof(Position));
            }
        }
        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                _photoPath = value;
                OnPropertyChange(nameof(PhotoPath));
            }
        }
        public Organization SelectedOrganization
        {
            get => _selectedOrganization;
            set
            {
                _selectedOrganization = value;
                OnPropertyChange(nameof(SelectedOrganization));
            }
        }

        public ICommand AddButtonClickCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand OpenPhotoPathCommand { get; }

        public event EventHandler EmployeeAdded;

        public EmployeeAddVM()
        {
            
            LoadOrganizationsFromDB();
            AddButtonClickCommand = new RelayCommand(OnAddButton_Click);
            OpenPhotoPathCommand = new RelayCommand(OpenPhotoPath);
        }

        private async void OnAddButton_Click()
        {
            if (SelectedOrganization == null)
            {
                MessageBox.Show("Пожалуйста, выберите организацию перед добавлением сотрудника.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Employee employee = new Employee
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Age = this.Age,
                Position = this.Position,
                OrganizationId = SelectedOrganization.Id,
                PhotoPath = this.PhotoPath
            };

                await DB.AddAsync(employee);

                var mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
                mainVM.Employees = await DB.GetAllTable<Employee>();
                mainVM.DisplayedItems = mainVM.Employees.Cast<object>().ToList();
                CloseWindow();
            


        }
        private async void LoadOrganizationsFromDB()
        {
            Organizations = await DB.GetAllTable<Organization>();
        }
        private void OpenPhotoPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image file (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                string imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
                Directory.CreateDirectory(imagesFolder);

                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(selectedFilePath)}";
                string destinationPath = Path.Combine(imagesFolder, uniqueFileName);

                File.Copy(selectedFilePath, destinationPath, true);
                PhotoPath = destinationPath;
            }
        }


    }
}
