using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.Entities
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string SessionKey { get; set; } = null;
        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }
    }
}
