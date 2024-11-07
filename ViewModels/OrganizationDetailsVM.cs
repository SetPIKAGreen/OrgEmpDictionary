using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Models;
using OrganizationsEmployeesDictionaryWPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class OrganizationDetailsVM: BaseVM
    {
        public Organization SelectedOrganization { get; }
        public ICommand DeleteOrganizationCommand { get; }
        public ICommand ChangeOrganizationCommand { get; }
        public ICommand EmployeesOfOrganizationCommand { get; }

        public string Name => SelectedOrganization.Name;
        public string Inn => SelectedOrganization.Inn;
        public string Adress => SelectedOrganization.Adress;
        public string Phone => SelectedOrganization.Phone;
        public OrganizationDetailsVM(Organization organization)
        {
            SelectedOrganization = organization;
            DeleteOrganizationCommand = new RelayCommand(OnDeleteButton_Click);
            ChangeOrganizationCommand = new RelayCommand(OnChangeButton_Click);
            EmployeesOfOrganizationCommand = new RelayCommand(OnListOfEployees_Click);
        }
        private async void OnDeleteButton_Click()
        {
            if(SelectedOrganization == null || !(SelectedOrganization is Organization organization))
            {
                MessageBox.Show("Не выбана организация для удаления!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBoxResult result = MessageBox.Show(
            "Вы уверены, что хотите удалить эту организацию?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

            if(SelectedOrganization == null)
            {
                return;
            }

            await DB.DeleteAsync(SelectedOrganization);
            var mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
            mainVM.Organizations = await DB.GetAllTable<Organization>();
            mainVM.DisplayedItems = mainVM.Organizations.Cast<object>().ToList();
            CloseWindow();
        }

        private void OnChangeButton_Click()
        {
            OrganizationChangeVM organizationChangeVM = new OrganizationChangeVM(SelectedOrganization);
            OrganizationChangeView organizationChangeView = new OrganizationChangeView
            {
                DataContext = organizationChangeVM
            };
            organizationChangeView.ShowDialog();
            CloseWindow();
        }
        private void OnListOfEployees_Click()
        {
            /*переделать на нужный Вью*/
            EmployeesOfOrganizationVM employeesOfOrganizationVM = new EmployeesOfOrganizationVM(SelectedOrganization);
            EmployeesOfOrganizationView employeesOfOrganizationView = new EmployeesOfOrganizationView
            {
                DataContext = employeesOfOrganizationVM
            };
            employeesOfOrganizationView.ShowDialog();
        }

    }
}
