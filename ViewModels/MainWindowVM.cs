using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Models;

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

        public ICommand ShowOrganizationsCommand { get; }
        public ICommand ShowEmployeesCommand { get; }


        public MainWindowVM()
        {
            _originalItems = new List<object>();
            ShowOrganizationsCommand = new RelayCommand(ShowOrganizations);
            ShowEmployeesCommand = new RelayCommand(ShowEmployees);
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
                a.OrganizationName = Organizations.FirstOrDefault(m => m.Id == a.OrganizationId).Name;
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
                employee.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || employee.Position.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (int.TryParse(searchText, out int age) && employee.Age == age))).ToList();

                DisplayedItems = filtredItems.Cast<object>().ToList();
            }
        }
    }
}
