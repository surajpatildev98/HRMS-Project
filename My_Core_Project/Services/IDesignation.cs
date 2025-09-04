using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface IDesignation
    {

        bool AddDesignation(DesignationVM dept);
        bool DeleteDesignation(int id);
        DesignationVM? GetDesignationById(int id);
        List<DesignationVM> GetAllDesignations();
    }
    public class DesignationRepo : IDesignation
    {
        HrmsDbContext _ctx;      // Database context for accessing the database
        private readonly IMapper _mapper;        // AutoMapper instance for mapping between entities and view models
        public DesignationRepo(IMapper mapper, HrmsDbContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public List<DesignationVM> GetAllDesignations()
        {
            var designations = _ctx.Designations.ToList();
            return _mapper.Map<List<DesignationVM>>(designations);
        }
        public DesignationVM? GetDesignationById(int id)
        {
            var designation = _ctx.Designations.Find(id);
            return designation == null ? null : _mapper.Map<DesignationVM>(designation);
        }
        public bool AddDesignation(DesignationVM dept)
        {
            try
            {
                var designationEntity = _mapper.Map<Designation>(dept);
                if (designationEntity.DesignationId == 0)
                {
                    _ctx.Designations.Add(designationEntity); // For new designation
                }
                else
                {
                    _ctx.Designations.Attach(designationEntity); // Attach for update
                    _ctx.Entry(designationEntity).State = EntityState.Modified;
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
        public bool DeleteDesignation(int id)
        {
            try
            {
                var designation = _ctx.Designations.Find(id);
                if (designation != null)
                {
                    _ctx.Designations.Remove(designation);
                    _ctx.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., log them)
                return false;
            }
        }
    }
}
