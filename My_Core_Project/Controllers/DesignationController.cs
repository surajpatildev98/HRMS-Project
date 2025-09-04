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
    public class DesignationController : Controller
    {
        private readonly IMapper _mapper;
        HrmsDbContext ctx;
        IDesignation _designation;

        public DesignationController(IMapper mapper, HrmsDbContext ctx, IDesignation designation)
        {
            _mapper = mapper;
            this.ctx = ctx;
            _designation = designation;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Get all designations with department names
            var designations = _designation.GetAllDesignations();

            // Get all departments for dropdown
            var departments = ctx.Departments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();
            ViewBag.Departments = departments;

            // Create a list with department names populated
            var designationsWithDeptNames = designations.Select(d => new
            {
                d.DesignationId,
                d.Title,
                d.DepartmentId,
                DepartmentName = departments.FirstOrDefault(dept => dept.DepartmentId == d.DepartmentId)?.DepartmentName ?? "No Department"
            }).ToList();

            ViewBag.Designation = designationsWithDeptNames;
            return View();
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult AddDesignation([FromBody] DesignationVM designation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseModel(-1, "Validation Failed"));
            }

            var result = _designation.AddDesignation(designation);
            if (result)
            {
                return Ok(new ResponseModel(0, designation.DesignationId == 0 ? "Designation Added Successfully" : "Designation Updated Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to save Designation"));
            }
        }

        [HttpPost]
        [RoleBasedAuthorization("Admin")]
        public IActionResult Delete(int id)
        {
            var result = _designation.DeleteDesignation(id);
            if (result)
            {
                return Ok(new ResponseModel(0, "Designation Deleted Successfully"));
            }
            else
            {
                return BadRequest(new ResponseModel(-2, "Failed to Delete Designation"));
            }
        }

        [HttpPost]
        public IActionResult GetDesignationById(int id)
        {
            var designation = _designation.GetDesignationById(id);
            if (designation == null)
            {
                return NotFound();
            }
            return Ok(designation);
        }

        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departments = ctx.Departments.Select(d => new { d.DepartmentId, d.DepartmentName }).ToList();
            return Ok(departments);
        }
    }
}