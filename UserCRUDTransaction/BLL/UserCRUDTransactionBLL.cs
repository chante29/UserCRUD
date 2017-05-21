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
        private readonly ILog _logger;

        private static string KeyConnection = "usercrudEntities";

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
                                              user.Birthday.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture), 
                                              GetConnectionStringValue(KeyConnection));
            }
            catch (Exception ex)
            {
                _logger.Error("Error in AddNewUser", ex);
                throw;
            }
        }

        public static string GetConnectionStringValue(string key)
        {
            ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[key];

            if (cs == null)
                throw new ConfigurationErrorsException("No Connection string in config file for " + key);

            return cs.ConnectionString;
        }
    }
}
