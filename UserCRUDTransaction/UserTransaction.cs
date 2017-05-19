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
        public bool ValidateNewUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool AddNewUser(User user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
