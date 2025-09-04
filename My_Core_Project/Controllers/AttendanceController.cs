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
    [RoleBasedAuthorization("Admin")]
    public class AttendanceController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        IAttendance _attendance;
        IEmployee _emp;


        public AttendanceController(IMapper mapper, HrmsDbContext ctx, IAttendance attendance, IEmployee emp)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _attendance = attendance;
            _emp = emp;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Get all attendances with employee names
            var attendances = _attendance.GetAllAttendances();

            // Get all employees for dropdown
            var employees = _emp.GetAllEmployees();
            ViewBag.Employees = employees;

            // Create a list with employee names populated
            var attendancesWithEmpNames = attendances.Select(a => new
            {
                a.AttendanceId,
                a.EmployeeId,
                a.AttendanceDate,
                a.InTime,
                a.OutTime,
                a.Status,
                EmployeeName = employees.FirstOrDefault(emp => emp.EmployeeId == a.EmployeeId)?.FirstName ?? "Unknown Employee"
            }).ToList();

            ViewBag.Attendance = attendancesWithEmpNames;
            return View();
        }

        [HttpPost]
        public IActionResult AddAttendance([FromBody] AttendanceVM attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }

            var result = _attendance.AddAttendance(attendance);
            if (result)
            {
                return Ok(new ResponseModel(0, "Attendance Added Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to save Attendance"));
            }
        }

        [HttpPost]
        public IActionResult UpdateAttendance([FromBody] AttendanceVM attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }

            var result = _attendance.UpdateAttendance(attendance);
            if (result)
            {
                return Ok(new ResponseModel(0, "Attendance Updated Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to update Attendance"));
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _attendance.DeleteAttendance(id);
            if (result)
            {
                return Ok(new ResponseModel(0, "Attendance Deleted Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to Delete Attendance"));
            }
        }

        [HttpPost]
        public IActionResult GetAttendanceById(int id)
        {
            var attendance = _attendance.GetAttendanceById(id);
            if (attendance == null)
            {
                return NotFound();
            }
            return Ok(attendance);
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _emp.GetAllEmployees();
            return Ok(employees);
        }

        [HttpPost]
        public IActionResult GetAttendanceByDate([FromBody] DateOnly date)
        {

            var attendances = _attendance.GetAttendanceByDate(date);

            // Get all employees for name mapping
            var employees = _emp.GetAllEmployees();

            // Create a list with employee names populated
            var attendancesWithEmpNames = attendances.Select(a => new
            {
                a.AttendanceId,
                a.EmployeeId,
                a.AttendanceDate,
                a.InTime,
                a.OutTime,
                a.Status,
                EmployeeName = employees.FirstOrDefault(emp => emp.EmployeeId == a.EmployeeId)?.FirstName ?? "Unknown Employee"
            }).ToList();

            return Ok(attendancesWithEmpNames);
        }
    }
}