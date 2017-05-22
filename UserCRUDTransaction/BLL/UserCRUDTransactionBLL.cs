using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCRUDTransaction.DAL;

namespace UserCRUDTransaction.BLL
{
    internal class UserCRUDTransactionBLL
    {
        #region Properties

        private const int MaxNameLength = 128;
        private readonly ILog _logger;

        private static string KeyConnection = "usercrudEntities";

        #endregion
        

        #region Internal Methods

        internal UserCRUDTransactionBLL()
        {
            _logger = LogManager.GetLogger(typeof(UserCRUDTransactionBLL));
        }

        internal int AddNewUser(SharedLibrary.User user)
        {
            try
            {
                _logger.InfoFormat("AddNewUser with user: {0}", user.Name);

                return UserCRUDDal.CreateUser(user.Name,
                                              user.Birthday,
                                              GetConnectionStringValue(KeyConnection));
            }
            catch (Exception ex)
            {
                _logger.Error("Error in AddNewUser", ex);
                throw;
            }
        }

        internal static string GetConnectionStringValue(string key)
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[key];

            if (cs == null)
                throw new ConfigurationErrorsException("No Connection string in config file for " + key);

            return cs.ConnectionString;
        }

        internal bool ValidateNewUser(SharedLibrary.User user)
        {
            if (!ValidateId(CorrectIdNewUser, user.Id))
                throw new ArgumentException("User Id in new user has to be zero");
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentException("User name must contain value");
            if(user.Name.Length > MaxNameLength)
                throw new ArgumentException(string.Format("User name length must be less than or equal to {0}", MaxNameLength));
            if (!ValidateDate(user.Birthday))
                throw new ArgumentException(string.Format("User birthday must be format yyyy-MM-dd"));
            if (!ValidateExistingDateBirthday(user.Birthday))
                throw new ArgumentException(string.Format("User birthday date has to be passed"));
            return true;
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


        private bool ValidateExistingDateBirthday(string birthday)
        {
            DateTime birthdayDate;

            DateTime.TryParseExact(birthday, "yyyy/MM/dd", null, DateTimeStyles.None, out birthdayDate);

            return DateTime.Now > birthdayDate;

        }

        private bool ValidateDate(string birthday)
        {
            DateTime birthdayDate;

            return (DateTime.TryParseExact(birthday, "yyyy/MM/dd", null, DateTimeStyles.None, out birthdayDate));


        }

        #endregion
    }
}
