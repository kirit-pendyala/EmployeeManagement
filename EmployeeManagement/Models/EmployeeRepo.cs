using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class EmployeeRepo : IEmployeeRepo
    {

        private List<Employee> _employeeList;

        public EmployeeRepo()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "Mary@gmail.com" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT, Email = "John@gmail.com" },
                new Employee() { Id = 3, Name = "Sam", Department = Dept.Payroll, Email = "Sam@gmail.com" }
            };
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList; 
        }

        public Employee GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
