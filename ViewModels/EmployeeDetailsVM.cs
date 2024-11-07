using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Interface;
using OrganizationsEmployeesDictionaryWPF.Models;
using OrganizationsEmployeesDictionaryWPF.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class EmployeeDetailsVM: BaseVM
    {
      
        public Employee SelectedEmployee { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ChangeEmployeeCommand { get; }

        public string FirstName => SelectedEmployee.FirstName;
        public string LastName => SelectedEmployee.LastName;
        public string Position => SelectedEmployee.Position;
        public string PhotoPath => SelectedEmployee.PhotoPath;
        public int Age => SelectedEmployee.Age;
        private string _organizationName;
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                _organizationName = value;
                OnPropertyChange(nameof(OrganizationName));
            }
        }


        public EmployeeDetailsVM(Employee employee)
        {
            
            SelectedEmployee = employee;
            GetOrganizationName();
            DeleteEmployeeCommand = new RelayCommand(OnDeleteButton_Click);
            ChangeEmployeeCommand = new RelayCommand(OnChangeButton_Click);
            

        }

        private async void OnDeleteButton_Click()
        {
            
            if (SelectedEmployee == null || !(SelectedEmployee is Employee employee))
            {
                MessageBox.Show("Не выбран сотрудник для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            MessageBoxResult result = MessageBox.Show(
            "Вы уверены, что хотите удалить этого сотрудника?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if (SelectedEmployee == null)
            {
                return;
            }

            await DB.DeleteAsync(SelectedEmployee);
            var mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
            mainVM.Employees = await DB.GetAllTable<Employee>();
            mainVM.DisplayedItems = mainVM.Employees.Cast<object>().ToList();
            CloseWindow();
        }
        private void OnChangeButton_Click()

        {
            EmployeeChangeVM employeeChangeVM = new EmployeeChangeVM(SelectedEmployee);
            EmployeeChangeView employeeChangeView = new EmployeeChangeView
            {
                DataContext = employeeChangeVM
            };
            employeeChangeView.ShowDialog();
            CloseWindow();
        }
        private async void GetOrganizationName()
        {
            Organization organization = await DB.GetByIdAsync<Organization>(SelectedEmployee.OrganizationId);
            if(organization != null)
            {
                OrganizationName = organization.Name;
            }
            else
            {
                OrganizationName = "Организация удалена";
            }
        }

    }
}
