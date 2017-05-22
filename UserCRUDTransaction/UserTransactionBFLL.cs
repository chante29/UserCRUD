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
        #region Properties

        private readonly UserCRUDTransactionBLL _userCRUDTransactionBLL;

        #endregion
        

        #region Public Methods

        public UserTransactionBFLL()
        {
            _userCRUDTransactionBLL = new UserCRUDTransactionBLL();
        }

        public bool ValidateNewUser(User user)
        {
            return _userCRUDTransactionBLL.ValidateNewUser(user);
           
        }

        public bool ValidateUpdateUser(User user)
        {
            return _userCRUDTransactionBLL.ValidateUpdateUser(user);
        }

        public int AddNewUser(User user)
        {
            return _userCRUDTransactionBLL.AddNewUser(user);
        }

        public bool UpdateUser(User user)
        {
            return _userCRUDTransactionBLL.UpdateUser(user);
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}
