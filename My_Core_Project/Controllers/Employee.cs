using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;
using My_Core_Project.Services;

namespace My_Core_Project.Controllers
{
    [Authorize]
    public class Employee : Controller
    {
        IEmployee _emp;
        HrmsDbContext ctx;
        private readonly IMapper _mapper;

        public Employee(IEmployee _emp,IMapper mapper,HrmsDbContext _ctx) // 
        {
            this._emp = _emp;
            _mapper = mapper;
            ctx = _ctx;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.EmployeeList = _emp.GetAllEmployees();
            ViewBag.DepartmentList = new SelectList(_emp.GetAllDepartments(), "DepartmentId", "DepartmentName");
            ViewBag.DesignationList = new SelectList(_emp.GetAllDesignations(), "DesignationId", "Title");

            return View();
        }


        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public async Task<IActionResult> Index(EmployeeVM emp ,IFormFile? ProfileImageFile)
        {
            if (ModelState.IsValid)
            {
                // Image Upload
                if (ProfileImageFile != null && ProfileImageFile.Length >0)
                {
                   using (var memoryStream = new MemoryStream())                    
                   {
                        await ProfileImageFile.CopyToAsync(memoryStream);
                        emp.ProfileImage = memoryStream.ToArray();
                   }
                }
                else
                {
                    emp.ProfileImage = null; // If no file is uploaded, set to null

                }



                bool result = await Task.Run(() => _emp.AddEmployee(emp));
                var response = result
                    ? new { code = 0, message = emp.EmployeeId == 0 ? "Employee Added" : "Employee Updated" }
                    : new { code = -1, message = "Something went wrong" };

                return Json(response); // ✅ Ye karo
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { code = -2, message = "Validation failed", errors });
            }
        }



        [HttpGet]
        public IActionResult CheckEmailExist(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Ok(new ResponseModel(-2, "Email is required."));
            }

            var obj = _emp.GetAllEmployees()
                          .FirstOrDefault(a => a.Email != null && a.Email.Trim().ToLower() == email.Trim().ToLower());

            if (obj != null)
            {
                return Ok(new ResponseModel(-1, "This Email ID already in use!"));
            }

            return Ok(new ResponseModel(0, "Valid Email ID"));
        }
        [HttpGet]
        public IActionResult CheckMobileExist(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                return Ok(new ResponseModel(-2, "mobile is required."));
            }

            var obj = _emp.GetAllEmployees().FirstOrDefault(a => a.Phone != null && a.Phone.Trim().ToLower() == mobile.Trim().ToLower());

            if (obj != null)
            {
                return Ok(new ResponseModel(-1, "This Phone Number already in use!"));
            }

            return Ok(new ResponseModel(0, "Valid Phone Number "));
        }

        [HttpGet]
        public IActionResult GetEmployeeById(int id)
        {
            var emp = _emp.GetEmployeeById(id);
            if (emp == null)
                return NotFound();

            return Json(emp);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = ctx.Employees.Include(e => e.Department).Include(e=>e.Designation).ToList();
            var employeeDTOs = _mapper.Map<List<EmployeeVM>>(employees);
            return Json(employeeDTOs);
        }

        [HttpPost]
        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult Delete(int id)
        {
            var response = new ResponseModel();
            try
            {
                var employee = ctx.Employees.Find(id);
                if (employee != null)
                {
                    ctx.Employees.Remove(employee);
                    ctx.SaveChanges();
                    response.Code = 0;
                    response.Message = "Employee deleted successfully.";
                }
                else
                {
                    response.Code = 1;
                    response.Message = "Employee not found.";
                }
            }
            catch (Exception ex)
            {
                response.Code = -1;
                response.Message = "Error: " + ex.Message;
            }

            return Json(response);
        }




    }
}
