using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IUserTransaction
    {
        bool ValidateNewUser(IUser user);

        bool ValidateUpdateUser(IUser user);

        bool AddNewUser(IUser user);

        bool UpdateUser(IUser user);

        List<IUser> GetAllUsers();

        IUser GetUser(int id);
    }
}
