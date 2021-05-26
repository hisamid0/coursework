using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<User> Users { get; set; }  
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
        public User()
        {

        }
    }
}
