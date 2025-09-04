using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface IDepartment
    {
        bool AddDepartment(DepartmentVM dept);
        bool DeleteDepartment(int id);
        DepartmentVM? GetDepartmentById(int id);
        List<DepartmentVM> GetAllDepartment();      

    }

    public class DepartmentRepo : IDepartment
    {
        HrmsDbContext _ctx;      // Database context for accessing the database
        private readonly IMapper _mapper;        // AutoMapper instance for mapping between entities and view models
        public DepartmentRepo(IMapper mapper, HrmsDbContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public List<DepartmentVM> GetAllDepartment()
        {
            var departments = _ctx.Departments.ToList();
            return _mapper.Map<List<DepartmentVM>>(departments);
        }
        public DepartmentVM? GetDepartmentById(int id)
        {
            var department = _ctx.Departments.Find(id);
            return department == null ? null : _mapper.Map<DepartmentVM>(department);
        }
        public bool AddDepartment(DepartmentVM dept)
        {
            try
            {
                var departmentEntity = _mapper.Map<Department>(dept);
                if (departmentEntity.DepartmentId == 0)
                {
                    _ctx.Departments.Add(departmentEntity); // For new department
                }
                else
                {
                    _ctx.Departments.Attach(departmentEntity); // Attach for update
                    _ctx.Entry(departmentEntity).State = EntityState.Modified;
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., log them)
                return false;
            }
        }
        public bool DeleteDepartment(int id)
        {
            try
            {
                var department = _ctx.Departments.Find(id);
                if (department != null)
                {
                    _ctx.Departments.Remove(department);
                    _ctx.SaveChanges();
                    return true;
                }
                return false; // Department not found
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., log them)
                return false;
            }
        }
    }
}
