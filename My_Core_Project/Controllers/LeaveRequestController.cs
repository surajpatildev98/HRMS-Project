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
    public class LeaveRequestController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        ILeaveRequest _requests;
        IEmployee _emp;

        public LeaveRequestController(IMapper mapper, HrmsDbContext ctx, ILeaveRequest request, IEmployee emp)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _requests = request;
            _emp = emp;
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Get all leave requests with employee names
            var leaveRequests = _requests.GetAllLeaveRequests();
            // Get all employees for dropdown
            var employees = _emp.GetAllEmployees();
            ViewBag.Employees = employees;
            // Create a list with employee names populated
            var leaveRequestsWithEmpNames = leaveRequests.Select(lr => new
            {
                lr.LeaveId,
                lr.EmployeeId,
                lr.LeaveType,
                lr.FromDate,
                lr.ToDate,
                lr.Reason,
                lr.Status,
                EmployeeName = employees.FirstOrDefault(emp => emp.EmployeeId == lr.EmployeeId)?.FirstName ?? "Unknown Employee"
            }).ToList();
            ViewBag.LeaveRequests = leaveRequestsWithEmpNames;

            return View();
        }

        [HttpPost]
        public IActionResult AddOrUpdateLeaveRequest([FromBody] LeaveRequestVM leaveRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(new ResponseModel(-1, "Validation Failed: " + errors));
            }

            try
            {
                bool result;
                if (leaveRequest.LeaveId == 0)
                    result = _requests.AddLeaveRequest(leaveRequest);
                else
                    result = _requests.UpdateLeaveRequest(leaveRequest);

                if (result)
                    return Ok(new ResponseModel(0, leaveRequest.LeaveId == 0 ? "Leave Request Added Successfully" : "Leave Request Updated Successfully"));
                else
                    return BadRequest(new ResponseModel(-2, "Failed to save Leave Request"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel(-3, "Exception: " + ex.Message));
            }
        }


        [HttpPost]
        public IActionResult DeleteLeaveRequest(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResponseModel(-1, "Invalid Leave Request ID"));
            }
            var result = _requests.DeleteLeaveRequest(id);
            if (result)
            {
                return Ok(new ResponseModel(1, "Leave Request Deleted Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-1, "Failed to Delete Leave Request"));
            }
        }

        [HttpPost]
        public IActionResult GetLeaveRequestById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ResponseModel(-1, "Invalid Leave Request ID"));
            }
            var leaveRequest = _requests.GetLeaveRequestById(id);
            if (leaveRequest != null)
            {
                return Ok(leaveRequest);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _emp.GetAllEmployees();
            if (employees != null && employees.Count > 0)
            {
                return Ok(employees);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "No Employees Found"));
            }
        }
        [HttpGet]
        public IActionResult GetAllLeaveRequests()
        {
            var leaveRequests = _requests.GetAllLeaveRequests();
            if (leaveRequests != null && leaveRequests.Count > 0)
            {
                return Ok(leaveRequests);
            }
            else
            {
                return NotFound(new ResponseModel(-1, "No Leave Requests Found"));
            }

        }
    }
}
