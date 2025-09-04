using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface ILeaveRequest
    {
        bool AddLeaveRequest(LeaveRequestVM leaveRequest);
       
        List<LeaveRequestVM> GetAllLeaveRequests();
        
         LeaveRequestVM? GetLeaveRequestById(int id);
        
        bool  UpdateLeaveRequest(LeaveRequestVM leaveRequest);
        
        bool DeleteLeaveRequest(int id);

        List<EmployeeVM> GetAllEmployees();
    }

    public class LeaveRequestRepo : ILeaveRequest
    {
         private readonly IMapper _mapper;
        HrmsDbContext ctx;
        public LeaveRequestRepo(IMapper mapper,HrmsDbContext _ctx)
        {
            _mapper = mapper;
            ctx = _ctx;
        }
        public bool AddLeaveRequest(LeaveRequestVM leaveRequest)
        {           
            try
            {
                var leave = _mapper.Map<LeaveRequest>(leaveRequest);
                ctx.LeaveRequests.Add(leave);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }

        }

        public bool DeleteLeaveRequest(int id)
        {
            try
            {
                var leaveRequest = ctx.LeaveRequests.Find(id);
                if (leaveRequest != null)
                {
                    ctx.LeaveRequests.Remove(leaveRequest);
                    ctx.SaveChanges();
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
            var employees = ctx.Employees.ToList();
            return _mapper.Map<List<EmployeeVM>>(employees);
        }

        public List<LeaveRequestVM> GetAllLeaveRequests()
        {
            var leaves = ctx.LeaveRequests
                .Include(lr => lr.Employee) // This is required for EmployeeName!
                .ToList();
            return _mapper.Map<List<LeaveRequestVM>>(leaves);
        }

        public LeaveRequestVM? GetLeaveRequestById(int id)
        {
            var leaveRequest = ctx.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return null;
            }
            return _mapper.Map<LeaveRequestVM>(leaveRequest);

        }

        public bool UpdateLeaveRequest(LeaveRequestVM leaveRequest)
        {
            try
            {
                var existingLeaveRequest = ctx.LeaveRequests.Find(leaveRequest.LeaveId);
                if (existingLeaveRequest == null)
                {
                    return false; // Leave request not found
                }
                // Map the updated properties from the view model to the entity
                _mapper.Map(leaveRequest, existingLeaveRequest);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
                return false;
            }

        }
    }

}
