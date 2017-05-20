using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCRUDTransaction
{
    public class UserTransaction : IUserTransaction
    {
        public bool ValidateNewUser(IUser user)
        {
            return true;
        }

        public bool ValidateUpdateUser(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool AddNewUser(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(IUser user)
        {
            throw new NotImplementedException();
        }

        public List<IUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IUser GetUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
