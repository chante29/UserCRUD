using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using UserCRUDRest.Utils;
using Microsoft.Practices.Unity;

namespace UserCRUDRest
{
    public class UserCRUD : IUserCRUD
    {
        private IUserTransaction userTransaction;

        public UserCRUD()
        {
            this.userTransaction = Container.Resolve <IUserTransaction>();
        }

        public int CreateUser(User user)
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

        public void UpdateUser(User User)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string id)
        {
            throw new NotImplementedException();
        }
    }
}
