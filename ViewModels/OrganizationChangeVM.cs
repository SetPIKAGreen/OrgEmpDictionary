using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Models;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class OrganizationChangeVM:BaseVM
    {
        private int _organizationId;
        private string _name;
        private string _inn;
        private string _adress;
        private string _phone;

        public int OrganizationId
        {
            get => _organizationId;
            set
            {
                _organizationId = value;
                OnPropertyChange(nameof(OrganizationId));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChange(nameof(Name));
            }
        }
        public string Inn
        {
            get => _inn;
            set
            {
                _inn = value;
                OnPropertyChange(nameof(Inn));
            }
        }
        public string Adress
        {
            get => _adress;
            set
            {
                _adress = value;
                OnPropertyChange(nameof(Adress));
            }
        }
        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChange(nameof(Phone));
            }
        }


        public ICommand SaveOrganizationCommand { get; }

        public OrganizationChangeVM(Organization organization)
        {
            OrganizationId = organization.Id;
            Name = organization.Name;
            Inn = organization.Inn;
            Adress = organization.Adress;
            Phone = organization.Phone;

            SaveOrganizationCommand = new RelayCommand(OnSaveButton_Click);

        }

        private async void OnSaveButton_Click()
        {
            Organization organization = new Organization
            {
                Id = OrganizationId,
                Name = this.Name,
                Inn = this.Inn,
                Adress = this.Adress,
                Phone = this.Phone
            };

            await DB.UpdateAsync(organization);
            var mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
            mainVM.Organizations = await DB.GetAllTable<Organization>();
            mainVM.DisplayedItems = mainVM.Organizations.Cast<object>().ToList();
            CloseWindow();
        }

    }
}
