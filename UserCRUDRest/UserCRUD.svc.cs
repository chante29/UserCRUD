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
using log4net;

namespace UserCRUDRest
{
    public class UserCRUD : IUserCRUD
    {
        private IUserTransaction userTransaction;
        private readonly ILog _logger;

        public UserCRUD()
        {
            this.userTransaction = Container.Resolve <IUserTransaction>();
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(UserCRUD));
        }

        public UserCRUD(IUserTransaction userTransaction)
        {
            this.userTransaction = userTransaction;
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(UserCRUD));
        }

        public int CreateUser(User user)
        {
            try
            {
                SharedLibrary.User commonUser = user.ToCommonUser();

                if(userTransaction.ValidateNewUser(commonUser))
                    userTransaction.AddNewUser(commonUser);

                return 0;

            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error creating user with message: {0} ", e.Message));
                throw;
            }
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
