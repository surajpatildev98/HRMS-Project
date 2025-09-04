using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;
using My_Core_Project.Services;

namespace My_Core_Project.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        IUser _user;
        public UserController(IMapper mapper, HrmsDbContext ctx, IUser user)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _user = user;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.User = _user.GetAllUsers();
            ViewBag.Employees = GetEmployeeList(); // Add employee list for dropdown
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult AddUser([FromBody] UserVM user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }

            var result = _user.AddUser(user);
            if (result)
            {
                return Ok(new ResponseModel(0, user.UserId == 0 ? "User Added Successfully" : "User Updated Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to save User"));
            }
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult Delete(int id)
        {
            var result = _user.DeleteUser(id);
            if (result)
            {
                return Ok(new ResponseModel(0, "User Deleted "));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to Delete User"));
            }
        }

        [HttpPost]
        public IActionResult GetUserById(int id)
        {
            var user = _user.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // New method to get employee list for dropdown
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = GetEmployeeList();
            return Json(employees);
        }

        // New method to get employee details by ID
        [HttpGet]
        public IActionResult GetEmployeeById(int employeeId)
        {
            try
            {
                var employee = ctx.Employees.Include(e => e.Designation).FirstOrDefault(e => e.EmployeeId == employeeId);
                if (employee == null)
                {
                    return NotFound(new { message = "Employee not found" });
                }

                return Json(new
                {
                    employeeId = employee.EmployeeId,
                    name = employee.FirstName,
                    role = employee.Designation?.Title ?? "No Designation",
                    department = employee.Department?.DepartmentName ?? "No Department"
                });
            }
            catch (Exception )
            {
                return BadRequest(new { message = "Error fetching employee details" });
            }
        }

        // Helper method to get employee list
        private List<object> GetEmployeeList()
        {
            try
            {
                return ctx.Employees
                    .Include(e => e.Designation)
                    .Where(e => e.Status == "Active")
                    .Select(e => new {
                        value = e.EmployeeId,
                        text = $"{e.EmployeeId} - {e.FirstName} ({e.Designation.Title ?? "No Designation"})"
                    })
                    .ToList<object>();
            }
            catch
            {
                return new List<object>();
            }
        }
    }
}