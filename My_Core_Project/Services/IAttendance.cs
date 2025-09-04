using AutoMapper;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface IAttendance
    {
        bool AddAttendance(AttendanceVM att);
        bool DeleteAttendance(int id);

        bool UpdateAttendance(AttendanceVM att);

        AttendanceVM? GetAttendanceById(int id);

        List<AttendanceVM> GetAllAttendances();

        //List<AttendanceVM> GetAttendanceByEmployeeId(int employeeId);

        List<AttendanceVM> GetAttendanceByDate(DateOnly date);

        List<EmployeeVM> GetAllEmployees();

    }

    public class AttendanceRepo : IAttendance
    {
        HrmsDbContext ctx; // database connection through Connectionstring.....
        private readonly IMapper _mapper; // AutoMapper instance for mapping between entities and view models
        public AttendanceRepo(IMapper mapper, HrmsDbContext _ctx)
        {
            ctx = _ctx;
            _mapper = mapper;
        }
        public bool AddAttendance(AttendanceVM att)
        {
            try
            {
                var attendance = _mapper.Map<Attendance>(att);
                ctx.Attendances.Add(attendance);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                return false;
            }
        }
        public bool DeleteAttendance(int id)
        {
            try
            {
                var attendance = ctx.Attendances.Find(id);
                if (attendance != null)
                {
                    ctx.Attendances.Remove(attendance);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exception (log it, rethrow it, etc.)
                return false;
            }
        }
        public bool UpdateAttendance(AttendanceVM att)
        {
            try
            {
                var attendance = _mapper.Map<Attendance>(att);
                ctx.Attendances.Update(attendance);
                ctx.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                // Handle exception (log it, rethrow it, etc.)
                return false;
            }
        }
        public AttendanceVM? GetAttendanceById(int id)
        {
            var attendance = ctx.Attendances.Find(id);
            if (attendance != null)
            {
                return _mapper.Map<AttendanceVM>(attendance);
            }
            return null;
        }
        public List<AttendanceVM> GetAllAttendances()
        {
            var attendances = ctx.Attendances.ToList();
            return _mapper.Map<List<AttendanceVM>>(attendances);
        }
        public List<AttendanceVM> GetAttendanceByDate(DateOnly date)
        {
            var attendances = ctx.Attendances.Where(a => a.AttendanceDate == date).ToList();
            return _mapper.Map<List<AttendanceVM>>(attendances);
        }
        public List<EmployeeVM> GetAllEmployees()
        {
            var employees = ctx.Employees.ToList();
            return _mapper.Map<List<EmployeeVM>>(employees);
        }

    }
}
