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
using System.Web;
using System.Net;

namespace UserCRUDRest
{
    public class UserCRUD : IUserCRUD
    {
        #region Properties

        private IUserTransaction userTransaction;
        private readonly ILog _logger;
        
        #endregion

        #region Public Methods
        
        public UserCRUD()
        {
            this.userTransaction = Container.Resolve <IUserTransaction>();
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(UserCRUD));
        }

        /// <summary>
        /// Constructor only used in the tests
        /// </summary>
        /// <param name="userTransaction"></param>
        public UserCRUD(IUserTransaction userTransaction)
        {
            this.userTransaction = userTransaction;
            log4net.Config.XmlConfigurator.Configure();
            _logger = LogManager.GetLogger(typeof(UserCRUD));
        }

        public int CreateUser(User user)
        {
            int userId = 0;
            try
            {
                SharedLibrary.User commonUser = user.ToCommonUser();

                if(userTransaction.ValidateNewUser(commonUser))
                    userId = userTransaction.AddNewUser(commonUser);

                return  userId;

            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error creating user with message: {0} ", e.Message));
                TreatException(e);
                throw;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var users = userTransaction.GetAllUsers();
                if (users == null || users.Count == 0)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
                    return null;
                }

                return users.Select(user => user.ToDataContract()).ToList();

            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error GetAllUsers user with message: {0} ", e.Message));
                TreatException(e);
                throw;
            }
        }

        public User GetUser(int id)
        {
            try
            {
                var user = userTransaction.GetUser(id);

                if (user == null)
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.NoContent;

               return user.ToDataContract();

            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error creating user with message: {0} ", e.Message));
                TreatException(e);
                throw;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                SharedLibrary.User commonUser = user.ToCommonUser();

                if(userTransaction.ValidateUpdateUser(commonUser))
                     userTransaction.UpdateUser(commonUser);
            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error updating user with message: {0} ", e.Message));
                TreatException(e);
                throw;
            }
        }

        public void DeleteUser(string id)
        {
            try
            {
                userTransaction.DeleteUser(id);
            }
            catch (Exception e)
            {
                _logger.DebugFormat(string.Format("Error DeleteUser with message: {0} ", e.Message));
                TreatException(e);
                throw;
            }
        }

        #endregion

        #region Private Methods

        private static void TreatException(Exception ex)
        {

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
            }

            if (ex.GetType().Name.ToLower().Contains("webfaultexception"))
                throw ex;

            if (ex is ArgumentException)
                throw new WebFaultException<Error>(new Error { ErrorMessage = ex.Message, ErrorCode = (int)HttpStatusCode.BadRequest }, HttpStatusCode.BadRequest);

            throw new WebFaultException<Error>(new Error { ErrorMessage = ex.Message }, HttpStatusCode.InternalServerError);
        }

        #endregion
    }
}
