using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Interface;
using OrganizationsEmployeesDictionaryWPF.Models;
using OrganizationsEmployeesDictionaryWPF.Service;
using OrganizationsEmployeesDictionaryWPF.View;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class MainWindowVM: BaseVM
    {
        private List<Organization> _organizations;
        private List<Employee> _employees;
        private List<object> _displayedItems;
        private readonly List<object> _originalItems;
        private string _searchText;
        private bool _isSearchEnabled;
        private bool _isAdd10Enabled;
        private object _selectedItem;

        public List<Organization> Organizations
        {
            get => _organizations;
            set
            {
                _organizations = value;
                OnPropertyChange(nameof(Organizations));
            }
        }
        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChange(nameof(Employees));
            }
        }
        public List<object> DisplayedItems
        {
            get => _displayedItems;
            set
            {
                _displayedItems = value;
                OnPropertyChange(nameof(DisplayedItems));
            }
        }
        public string SearchText 
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChange(nameof(SearchText));
                SearchItem(_searchText);
            } 
        }      
        public bool IsSearchEnabled
        {
            get => _isSearchEnabled;
            set
            {
                _isSearchEnabled = value;
                OnPropertyChange(nameof(IsSearchEnabled));
            }
        }
        public bool IsAdd10Enabled
        {
            get => _isAdd10Enabled;
            set
            {
                _isAdd10Enabled = value;
                OnPropertyChange(nameof(IsAdd10Enabled));
            }
        }
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChange(nameof(SelectedItem));
            }
        }
        

        public ICommand ShowOrganizationsCommand { get; }
        public ICommand ShowEmployeesCommand { get; }
        public ICommand AddButtonClickCommaand { get; }
        public ICommand Add10ButtonClickCommaand { get; }
        public ICommand DeleteButtonClickCommand { get; }
        public ICommand ShowItemDetailsCommand { get; }
        public ICommand ShowMessageCommand { get; }


        public MainWindowVM()
        {
            _originalItems = new List<object>();
            ShowOrganizationsCommand = new RelayCommand(ShowOrganizations);
            ShowEmployeesCommand = new RelayCommand(ShowEmployees);
            AddButtonClickCommaand = new RelayCommand(OnAddButton_Click);
            Add10ButtonClickCommaand = new RelayCommand(OnAdd10Button_Click);
            DeleteButtonClickCommand = new RelayCommand(OnDeleteButton_Click);
            ShowItemDetailsCommand = new RelayCommand(OnSelectedItem_Click);

            IsSearchEnabled = false;
            IsAdd10Enabled = true;
            Initialize();
        }
        private async void Initialize()
        {
            await DB.Initialize();
            await LoadData();
        }

        private async Task LoadData()
        {
            Organizations = await DB.GetAllTable<Organization>();
            Employees = await DB.GetAllTable<Employee>();

            if(Organizations.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Organization organization = new Organization
                    {
                        Name = $"Организация_{i}",
                        Adress = $"Адрес_{i}",
                        Inn = $"12300{i}",
                        Phone = $"123-123-00{i}"
                    };
                    await DB.AddAsync(organization);
                    Organizations.Add(organization);
                }
            }
            if (Employees.Count == 0)
            {
                Employee employee1 = new Employee 
                { 
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Age = 23,
                    Position = "Рабочий",
                    OrganizationId = 1,
                    PhotoPath = "Photo/Path" 
                };

                Employee employee2 = new Employee 
                { 
                    FirstName = "Петр",
                    LastName = "Миронов",
                    Age = 32,
                    Position = "Менеджер",
                    OrganizationId = 2,
                    PhotoPath = "Photo/Path" 
                };

                Employee employee3 = new Employee 
                { 
                    FirstName = "Вася",
                    LastName = "Федоров",
                    Age = 44,
                    Position = "Директор",
                    OrganizationId = 1,
                    PhotoPath = "Photo/Path" 
                };

                await DB.AddAsync(employee1);
                await DB.AddAsync(employee2);
                await DB.AddAsync(employee3);

                Employees.Add(employee1);
                Employees.Add(employee2);
                Employees.Add(employee3);
            }
            ShowOrganizations();

            foreach(var a in Employees)
            {
                var tempOrganization = Organizations.FirstOrDefault(m => m.Id == a.OrganizationId);

                    if (tempOrganization != null)
                    {
                        a.OrganizationName = tempOrganization.Name;
                    }
                    else
                    {
                        a.OrganizationName = "Организация удалена";
                    }

                
            }
            _originalItems.AddRange(Employees.Cast<object>());
        }

        private void ShowOrganizations()
        {
            DisplayedItems = Organizations.Cast<object>().ToList();
            IsSearchEnabled = false;
            IsAdd10Enabled = true;
        }
        private void ShowEmployees()
        {
            GetOrganizationName();
            DisplayedItems = Employees.Cast<object>().ToList();
            IsSearchEnabled = true;
            IsAdd10Enabled = false;
        }
        private void SearchItem(string searchText)
        {
            
            if (string.IsNullOrEmpty(searchText))
            {
                DisplayedItems = _originalItems.ToList();
            }
            else
            {
                var filtredItems = _originalItems.Where(m => m is Employee employee && (employee.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                employee.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || employee.Position.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || employee.OrganizationName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (int.TryParse(searchText, out int age) && employee.Age == age))).ToList();

                DisplayedItems = filtredItems.Cast<object>().ToList();
            }
        }
        private async void OnEmployeeAdded(object sender, EventArgs e)
        {
            Employees = await DB.GetAllTable<Employee>();
            GetOrganizationName();
        }
        private void OnSelectedItem_Click()
        {
            if (SelectedItem is Employee selectedEmployee)
            {
                var employeeDetailsVM = new EmployeeDetailsVM(selectedEmployee);
                var employeeDetailsView = new EmployeerDetailsView
                {
                    DataContext = employeeDetailsVM
                };
                employeeDetailsView.ShowDialog();
                GetOrganizationName();
            }
            else if(SelectedItem is Organization selectedOrganization)
            {
                var organizationDetailsVM = new OrganizationDetailsVM(selectedOrganization);
                var organizationDetailsView = new OrganizationDetailsView
                {
                    DataContext = organizationDetailsVM
                };
                organizationDetailsView.ShowDialog();
            }
        }
        private void OnAddButton_Click()
        {
            if (IsSearchEnabled == true)
            {
                EmployeeAddView employeeAddView = new EmployeeAddView();
                var employeeAddVM = (EmployeeAddVM)employeeAddView.DataContext;
                employeeAddView.Show();
            }
            else
            {
                OrganizationAddView organizationAddView = new OrganizationAddView();
                var organizationAddVM = (OrganizationAddVM)organizationAddView.DataContext;
                organizationAddView.Show();
            }

        }      
        private async void OnAdd10Button_Click()
        {
            MessageBoxResult result = MessageBox.Show(
            "Вы уверены, что хотите создать 10 организаций со 100 сотрудниками в каждой?",
            "Предупреждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Logger.Instance.LogInformation("Добавление 10 организаций и 1000 сотрудников запуено");
                List<Employee> employees = new List<Employee>();
                for (int i = 0; i < 10; i++)
                {
                    Organization organization = new Organization
                    {
                        Name = $"Организация-{i}",
                        Inn = $"00000-{i}",
                        Adress = $"Офси № {i}",
                        Phone = $"363-00-{i}"
                    };

                    await DB.AddAsync(organization);
                    

                    for (int j = 0; j < 100; j++)
                    {
                        Employee employee = new Employee
                        {
                            FirstName = $"Имя - {j}",
                            LastName = $"Фамилия - {j}",
                            Age = j,
                            Position = "Рабочий",
                            OrganizationId = organization.Id,
                        };
                        employees.Add(employee);

                    }
                }
                await DB.AddAllAsync(employees);
                Employees = await DB.GetAllTable<Employee>();
                Organizations = await DB.GetAllTable<Organization>();
                DisplayedItems = Organizations.Cast<object>().ToList();
                GetOrganizationName();
                MessageBoxResult message = MessageBox.Show(
                "Данные созданы",
                "",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            }



        }
        private async void OnDeleteButton_Click(object value)
        {
            if (value == null)
            {
                return;
            }

            if(value is Employee)
            {
                await DB.DeleteAsync(value);
                Employees = await DB.GetAllTable<Employee>();
                DisplayedItems = Employees.Cast<object>().ToList();

            }
            else if(value is Organization)
            {
                await DB.DeleteAsync(value);
                Organizations = await DB.GetAllTable<Organization>();
                DisplayedItems = Organizations.Cast<object>().ToList();
                GetOrganizationName();
            }
        }
        private void GetOrganizationName()
        {
            foreach (var a in Employees)
            {
                if (Organizations.FirstOrDefault(m => m.Id == a.OrganizationId) != null)
                {
                    a.OrganizationName = Organizations.FirstOrDefault(m => m.Id == a.OrganizationId).Name;
                }
                else
                {
                    a.OrganizationName = "Организация удалена";
                };
            }
        }
    }
}
