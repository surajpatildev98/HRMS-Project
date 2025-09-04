using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Core_Project.DBContext;
using My_Core_Project.Models;
using My_Core_Project.Services;

namespace My_Core_Project.Controllers
{
    [Authorize]
    [RoleBasedAuthorization("Admin")]
    public class SalaryController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        ISalary _salary;

        public SalaryController(IMapper mapper, HrmsDbContext ctx, ISalary salary)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _salary = salary;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Salaries = _salary.GetAllSalaries();
            return View();
        }

        [HttpPost]
        public IActionResult AddSalary([FromBody] SalaryVM salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }
            var result = _salary.AddSalary(salary);
            if (result)
            {
                return Ok(new ResponseModel(0, salary.SalaryId == 0 ? "Salary Added Successfully" : "Salary Updated Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to save Salary"));
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _salary.DeleteSalary(id);
            if (result)
            {
                return Ok(new ResponseModel(0, "Salary Deleted Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to Delete Salary"));
            }
        }
        [HttpGet]
        public IActionResult GetSalaryById(int id)
        {
            var salary = _salary.GetSalaryById(id);
            if (salary != null)
            {
                return Ok(salary);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "Salary not found"));
            }
        }

        [HttpGet]
        public IActionResult GetSalariesByEmployeeId(int id)
        {
            var salaries = _salary.GetSalariesByEmployeeId(id);
            if (salaries != null && salaries.Count > 0)
            {
                return Ok(salaries);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "No salaries found for this employee"));
            }
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _salary.GetAllEmployees();
            if (employees != null && employees.Count > 0)
            {
                return Ok(employees);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "No employees found"));
            }
        }

        [HttpGet]
        public IActionResult GetAllSalaries()
        {
            var salaries = _salary.GetAllSalaries();
            if (salaries != null && salaries.Count > 0)
            {
                return Ok(salaries);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "No salaries found"));
            }
        }
    }
}
