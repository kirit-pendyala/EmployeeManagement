using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public interface IEmployeeRepo
    {
        Employee GetEmployee(int Id);


        // Basic CURD Operations
        // C - Create
        // U - Update
        // R - Read
        // D - Delete
        IEnumerable<Employee> GetAllEmployee();
        Employee Add(Employee employee);

        Employee Update(Employee employeeChanges);

        Employee Delete(int id);
    }
}
