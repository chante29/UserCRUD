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
            if (!ValidateId(CorrectIdNewUser, user.Id))
                throw new ArgumentException("User Id in new user has to be zero");

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

        #endregion
        

        #region Private Methods

        private bool ValidateId(Func<int, bool> CorrectIdUser, int id)
        {
            return CorrectIdUser(id);
        }

        private bool CorrectIdNewUser(int id)
        {
            return id == 0;
        }

        #endregion

        
    }
}
