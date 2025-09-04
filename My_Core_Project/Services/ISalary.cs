using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface ISalary
    {
        bool AddSalary(SalaryVM salary);
       

        bool DeleteSalary(int salaryId);

        SalaryVM GetSalaryById(int salaryId);

        List<SalaryVM> GetAllSalaries();

        List<SalaryVM> GetSalariesByEmployeeId(int id);

        List<EmployeeVM> GetAllEmployees();
    }

    public class SalaryRepo : ISalary
    {
        private readonly IMapper _mapper;
        HrmsDbContext _ctx;
        public SalaryRepo(IMapper mapper, HrmsDbContext ctx)
        {
            _mapper = mapper;
            _ctx = ctx;
        }

        public bool AddSalary(SalaryVM _salary)
        {          
            try
    {
        if (_salary.SalaryId == 0) // Add case
        {
            var newSalary = _mapper.Map<Salary>(_salary);
            _ctx.Salaries.Add(newSalary);
        }
        else // Update case
        {
            var existingSalary = _ctx.Salaries.FirstOrDefault(s => s.SalaryId == _salary.SalaryId);
            if (existingSalary == null)
            {
                Console.WriteLine("Salary record not found for update.");
                return false;
            }

            _mapper.Map(_salary, existingSalary);
        }

        _ctx.SaveChanges();
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
        Console.WriteLine("Stack Trace: " + ex.StackTrace);
        return false;
    }
        }

        public bool DeleteSalary(int salaryId)
        {
            try
            {
                var salary = _ctx.Salaries.Find(salaryId);
                if (salary != null)
                {
                    _ctx.Salaries.Remove(salary);
                    _ctx.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }
        }

        public List<EmployeeVM> GetAllEmployees()
        {
            var employees = _ctx.Employees
                          .Include(e => e.Designation)
                          .ToList();

            return _mapper.Map<List<EmployeeVM>>(employees);
        }

        public List<SalaryVM> GetAllSalaries()
        {
            var salaries = _ctx.Salaries.Include(e => e.Employee).ThenInclude(d => d.Designation).ToList();
            return _mapper.Map<List<SalaryVM>>(salaries);

        }

        public List<SalaryVM> GetSalariesByEmployeeId(int id)
        {
            var salaries = _ctx.Salaries.Find(id);
            if (salaries == null)
            {
                return null;

            }
            return _mapper.Map<List<SalaryVM>>(_ctx.Salaries.Where(s => s.EmployeeId == id).ToList());
        }

        public SalaryVM GetSalaryById(int salaryId)
        {
            var salary = _ctx.Salaries.Find(salaryId);
            if (salary == null)
            {
                return null;
            }           
            
             return _mapper.Map<SalaryVM>(salary);

        }        
    }
}
