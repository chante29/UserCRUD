using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IUserTransaction
    {
        bool ValidateNewUser(User user);

        bool ValidateUpdateUser(User user);

        int AddNewUser(User user);

        bool UpdateUser(User user);

        List<User> GetAllUsers();

        User GetUser(int id);
    }
}
