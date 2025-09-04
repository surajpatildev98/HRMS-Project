using AutoMapper;
using Microsoft.EntityFrameworkCore;
using My_Core_Project.DBContext;
using My_Core_Project.Models;

namespace My_Core_Project.Services
{
    public interface IUser
    {
        bool AddUser(UserVM user);
        bool DeleteUser(int id);
        UserVM? GetUserById(int id);
        List<UserVM> GetAllUsers();

        UserVM Login(string UserName, string Password);

    }

    public class UserRepo : IUser
    {
         HrmsDbContext _ctx;      // Database context for accessing the database
        private readonly IMapper _mapper;        // AutoMapper instance for mapping between entities and view models
        public UserRepo(IMapper mapper, HrmsDbContext ctx)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public List<UserVM> GetAllUsers()
        {
            var users = _ctx.Users.ToList();
            return _mapper.Map<List<UserVM>>(users);
        }
        public UserVM? GetUserById(int id)
        {
            var user = _ctx.Users.Find(id);
            return user == null ? null : _mapper.Map<UserVM>(user);
        }
        public bool AddUser(UserVM user)
        {
            try
            {
                var userEntity = _mapper.Map<User>(user);
                if (userEntity.UserId == 0)
                {
                    _ctx.Users.Add(userEntity); // For new user
                }
                else
                {
                    _ctx.Users.Attach(userEntity); // Attach for update
                    _ctx.Entry(userEntity).State = EntityState.Modified;
                }
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                // Handle exceptions (e.g., log them)
                return false;
            }
        }
        public bool DeleteUser(int id)
        {
            try
            {
                var user = _ctx.Users.Find(id);
                if (user == null) return false;
                _ctx.Users.Remove(user);
                _ctx.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                // Handle exceptions (e.g., log them)
                return false;
            }
        }

        public List<UserVM> GetAllUsers(string UserName, string Password)
        {
            throw new NotImplementedException();
        }
        

        public UserVM Login(string UserName, string Password)
        {
          var obj = _ctx.Users.Where(u => u.Username.ToLower().Trim() == UserName && u.Password.ToLower().Trim() == Password).ToList();
            return _mapper.Map<UserVM>(obj.FirstOrDefault());
        }
    }
}
