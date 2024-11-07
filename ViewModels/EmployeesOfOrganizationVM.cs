using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class EmployeesOfOrganizationVM: BaseVM
    {
        private List<Employee> _selectedEmloyeesList;
        private string _title;

        public List<Employee> SelectedEmloyeesList
        {
            get => _selectedEmloyeesList;
            set
            {
                _selectedEmloyeesList = value;
                OnPropertyChange(nameof(SelectedEmloyeesList));
            }
        }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChange(nameof(Title));
            }
        }
        public EmployeesOfOrganizationVM(Organization organization)
        {
            GetEmployeesOfOrganization(organization.Id);
            Title = $"Организация: {organization.Name}";
        }
        private async void GetEmployeesOfOrganization(int value)
        {
            var TempList = await DB.GetAllTable<Employee>();
            SelectedEmloyeesList = TempList.Where(a => a.OrganizationId == value).ToList();
        }
    }
}
