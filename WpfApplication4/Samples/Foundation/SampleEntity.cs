using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPersistence;

namespace WpfApplication4
{
    [Entity]
    public class Employee
    {
        [Id]
        public virtual string ID { get; set; }
        [Basic]
        public virtual string FirstName { get; set; }
        [Basic]
        public virtual string LastName { get; set; }
    }
}