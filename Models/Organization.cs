using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationsEmployeesDictionaryWPF.Models
{
    public class Organization
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Inn { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
    }
}
