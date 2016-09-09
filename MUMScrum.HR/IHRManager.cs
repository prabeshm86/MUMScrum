using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUMScrum.Model;
using MUMScrum.Model.Enum;

namespace MUMScrum.HR
{
    public interface IHRManager
    {
        Employee Login(string username, string password);
        CreateEmployeeStatus CreateEmployee(Employee employee);
        CreateEmployeeStatus UpdateEmployee(Employee employee);
        CreateEmployeeStatus UpdateProfile(Employee employee);
        void AssignRole(int empId, int roleId);
        void DeleteEmployee(int empId);
        IEnumerable<Employee> GetAllEmployees();
        IEnumerable<Employee> GetAllDevelopers();
        IEnumerable<Employee> GetAllTesters();
        Employee GetEmployeeById(int id);
        IEnumerable<Role> GetAllRoles();

        IEnumerable<Employee> GetScrumMaster();

    }
}
