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

        public ICommand ShowOrganizationsCommand { get; }
        public ICommand ShowEmployeesCommand { get; }


        public MainWindowVM()
        {
            ShowOrganizationsCommand = new RelayCommand(ShowOrganizations);
            ShowEmployeesCommand = new RelayCommand(ShowEmployees);

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
        }

        private void ShowOrganizations()
        {
            DisplayedItems = Organizations.Cast<object>().ToList();
        }
        private void ShowEmployees()
        {
            DisplayedItems = Employees.Cast<object>().ToList();
        }
    }
}
