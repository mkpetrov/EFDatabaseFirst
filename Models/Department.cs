using System.Collections.Generic;

namespace SoftUni.Models
{
    public class Department
    {
        public const int ResearchAndDevelopmentDepartmentId = 6;

        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
