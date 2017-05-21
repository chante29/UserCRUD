using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCRUDTransaction.BLL;

namespace UserCRUDTransaction
{
    public class UserTransactionBFLL : IUserTransaction
    {

        private readonly UserCRUDTransactionBLL _userCRUDTransactionBLL;

        public UserTransactionBFLL()
        {
            _userCRUDTransactionBLL = new UserCRUDTransactionBLL();            
        }

        public bool ValidateNewUser(User user)
        {
            return true;
        }

        public bool ValidateUpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public int AddNewUser(User user)
        {
            return _userCRUDTransactionBLL.AddNewUser(user);
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
