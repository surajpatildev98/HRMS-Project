using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface IEmployee
    {
        bool AddEmployee(EmployeeVM emp);      
        bool DeleteEmployee(int id);
        EmployeeVM? GetEmployeeById(int id);
        List<EmployeeVM> GetAllEmployees();
        List<Department> GetAllDepartments();
        List<Designation> GetAllDesignations();

    }

    public class EmployeeRepo : IEmployee
    {
        //HrmsDbContext ctx = new HrmsDbContext(); // Connection to the database

        HrmsDbContext ctx;// database connection through Connectionstring.....

        private readonly IMapper _mapper; // AutoMapper instance for mapping between entities and view models

        public EmployeeRepo(IMapper mapper,HrmsDbContext _ctx)
        {
             ctx = _ctx;
            _mapper = mapper;
        }
        public List<Department> GetAllDepartments()
        {
            return ctx.Departments.ToList();
        }

        public List<Designation> GetAllDesignations()
        {
            return ctx.Designations.ToList();
        }

        public bool AddEmployee(EmployeeVM emp)
        {
            try
            {
                var employee = _mapper.Map<Employee>(emp);

                if (employee.EmployeeId == 0)
                {
                    ctx.Employees.Add(employee); // For new employee
                }
                else
                {
                    ctx.Employees.Attach(employee); // Attach for update
                    ctx.Entry(employee).State = EntityState.Modified;
                }

                ctx.SaveChanges();
                return true;
            }
            catch (Exception er)
            {
                System.Diagnostics.Debug.WriteLine("Error in AddEmployee: " + er.ToString());
                return false;
            }
        }

        public bool DeleteEmployee(int id)
        {
            try
            {
                var o = ctx.Employees.Find(id);
                if (o != null)
                {
                    ctx.Employees.Remove(o);
                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception )
            {
              return false;
            }

        }

        public List<EmployeeVM> GetAllEmployees()
        {
            try
            {
                var employees = ctx.Employees.ToList();
                return _mapper.Map<List<EmployeeVM>>(employees);
            }
            catch (Exception)
            {
                return new List<EmployeeVM>();
            }

        }

        public EmployeeVM? GetEmployeeById(int id)
        {
            try
            {
                var employee = ctx.Employees.Find(id);
                if (employee == null)
                {
                    return null; // or throw an exception if preferred
                }
                return _mapper.Map<EmployeeVM>(employee);
            }
            catch (Exception)
            {
                return null; // or throw an exception if preferred
            }
          
        }
    }

}
