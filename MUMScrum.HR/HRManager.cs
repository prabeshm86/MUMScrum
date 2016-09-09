using System;
using System.Collections.Generic;
using MUMScrum.Model;
using MUMScrum.DataAccess;
using System.Linq;
using MUMScrum.Model.Enum;

namespace MUMScrum.HR
{
    public class HRManager : IHRManager
    {
        private IUnitOfWork unitOfWork = new UnitOfWork();
        public void AssignRole(int empId, int roleId)
        {
            throw new NotImplementedException();
        }

        public CreateEmployeeStatus CreateEmployee(Employee employee)
        {
            if (unitOfWork.EmployeeRepository.Get(i => i.UserName.ToLower() == employee.UserName.ToLower()).Any())
                return CreateEmployeeStatus.DulplicateUsername;

            unitOfWork.EmployeeRepository.Insert(employee);
            unitOfWork.SaveChanges();
            return CreateEmployeeStatus.Success;
        }

        public void DeleteEmployee(int empId)
        {
            var emp = unitOfWork.EmployeeRepository.GetByID(empId);
            unitOfWork.EmployeeRepository.Delete(emp);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return unitOfWork.EmployeeRepository.Get();
        }


       public  IEnumerable<Employee> GetScrumMaster()
        {
            return unitOfWork.EmployeeRepository.Get(e => (e.Role.RoleName == "ScrumMaster"));
        }

        
        public IEnumerable<Employee> GetAllDevelopers()
        {
            return unitOfWork.EmployeeRepository.Get(c => c.RoleId == (int)RoleEnum.Developer);
        }

        

        public IEnumerable<Employee> GetAllTesters()
        {
            return unitOfWork.EmployeeRepository.Get(c => c.RoleId == (int)RoleEnum.Tester);
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return unitOfWork.RoleRepository.Get();
        }

        public Employee GetEmployeeById(int id)
        {
            return unitOfWork.EmployeeRepository.GetByID(id);
        }

        public Employee Login(string username, string password)
        {
            var employees = unitOfWork.EmployeeRepository
                .Get(i => i.Password == password && i.UserName.ToLower() == username.ToLower() && i.IsDeactivated == false);

            if (employees.Any())
            {
                return employees.FirstOrDefault();
            }
            return null;
        }

        public CreateEmployeeStatus UpdateEmployee(Employee employee)
        {
            if (unitOfWork.EmployeeRepository.Get(i => i.UserName.ToLower() == employee.UserName.ToLower() && i.Id != employee.Id).Any())
                return CreateEmployeeStatus.DulplicateUsername;

            unitOfWork.EmployeeRepository.Update(employee);
            unitOfWork.SaveChanges();
            return CreateEmployeeStatus.Success;
        }

        public CreateEmployeeStatus UpdateProfile(Employee employee)
        {
            if (unitOfWork.EmployeeRepository.Get(i => i.UserName.ToLower() == employee.UserName.ToLower() && i.Id != employee.Id).Any())
                return CreateEmployeeStatus.DulplicateUsername;
            var dbEmp = unitOfWork.EmployeeRepository.Get(i => i.Id == employee.Id).FirstOrDefault();
            dbEmp.FirstName = employee.FirstName;
            dbEmp.UserName = employee.UserName;
            dbEmp.LastName = employee.LastName;
            dbEmp.Password = employee.Password;
            unitOfWork.EmployeeRepository.Update(dbEmp);
            unitOfWork.SaveChanges();
            return CreateEmployeeStatus.Success;
        }
    }
}
