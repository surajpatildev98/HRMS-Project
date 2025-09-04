using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using My_Core_Project.DBContext;
using My_Core_Project.Models;
using My_Core_Project.Services;

namespace My_Core_Project.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        IDepartment _department;

        public DepartmentController(IMapper mapper, HrmsDbContext ctx, IDepartment department)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _department = department;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Department = _department.GetAllDepartment();
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult AddDepartment([FromBody] DepartmentVM department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }

            var result = _department.AddDepartment(department);
            if (result)
            {
                return Ok(new ResponseModel(0, department.DepartmentId == 0 ? "Department Added Successfully" : "Department Updated Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to save Department"));
            }
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult Delete(int id)
        {
            var result = _department.DeleteDepartment(id);
            if (result)
            {
                return Ok(new ResponseModel(0, "Department Deleted Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to Delete Department"));
            }
        }

        [HttpPost]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _department.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }
    }
}
