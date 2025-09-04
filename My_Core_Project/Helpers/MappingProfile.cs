using AutoMapper;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Helpers
{
    public class MappingProfile : Profile
    {
        // This class is used to define the mapping configurations for AutoMapper.
        // It inherits from AutoMapper's Profile class and is used to create mappings between domain models and view models.
        // In this example, we are mapping the Employee entity to the EmployeeVM view model and vice versa.
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeVM>().ReverseMap();
            // Add more maps here
            CreateMap<Employee, EmployeeVM>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : ""))
            .ForMember(dest => dest.DesignationName, opt => opt.MapFrom(src => src.Designation != null ? src.Designation.Title : ""));
            // Map other properties as needed

            CreateMap<User, UserVM>().ReverseMap();
            CreateMap<Department, DepartmentVM>().ReverseMap();
            CreateMap<Designation, DesignationVM>().ReverseMap();
            CreateMap<Attendance, AttendanceVM>().ReverseMap();
            CreateMap<LeaveRequest, LeaveRequestVM>()
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FirstName + " " + src.Employee.LastName : "Unknown Employee"))
            .ReverseMap();
            CreateMap<Salary, SalaryVM>()
                            .ForMember(dest => dest.EmployeeName,opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FirstName + " " + src.Employee.LastName : "Unknown Employee"))
                            .ForMember(dest => dest.DesignationName,opt => opt.MapFrom(src => src.Employee != null && src.Employee.Designation != null ? src.Employee.Designation.Title : string.Empty))
                            .ReverseMap().ForMember(dest => dest.Employee, opt => opt.Ignore());





        }
    }
}
