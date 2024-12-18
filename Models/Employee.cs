﻿using OrganizationsEmployeesDictionaryWPF.DataBase;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizationsEmployeesDictionaryWPF.Models
{
    public class Employee
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int OrganizationId { get; set; }
        public string Position { get; set; }
        public string PhotoPath { get; set; }
        
        public string OrganizationName { get; set; }


    }
}
