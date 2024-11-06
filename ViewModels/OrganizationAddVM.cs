using OrganizationsEmployeesDictionaryWPF.DataBase;
using OrganizationsEmployeesDictionaryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OrganizationsEmployeesDictionaryWPF.ViewModels
{
    public class OrganizationAddVM: BaseVM
    {
        private string _name;
        private string _inn;
        private string _adress;
        private string _phone;

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

        public ICommand AddOrganizationCommand { get; }

        public OrganizationAddVM()
        {
            AddOrganizationCommand = new RelayCommand(OnAddButton_Click);
        }

        private async void OnAddButton_Click()
        {
            
            Organization organization = new Organization
            {
                Name = this.Name,
                Inn = this.Inn,
                Adress = this.Adress,
                Phone = this.Phone
            };
            await DB.AddAsync(organization);
            var mainVM = (MainWindowVM)Application.Current.MainWindow.DataContext;
            mainVM.Organizations = await DB.GetAllTable<Organization>();
            mainVM.DisplayedItems = mainVM.Organizations.Cast<object>().ToList();
            CloseWindow();
        } 
        private void CloseWindow()
        {
            Window currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            currentWindow?.Close();
        }
    }
}
