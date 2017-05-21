using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCRUDTransaction.BLL
{
    internal class UserCRUDTransactionBLL
    {
        private readonly ILog _logger;

        internal UserCRUDTransactionBLL()
        {
            _logger = LogManager.GetLogger(typeof(UserCRUDTransactionBLL));
        }

        internal int AddNewUser(SharedLibrary.User user)
        {
            /*try
            {
                _logger.InfoFormat("AddNewUser with user: {0}", user.);


                ValidateNameFolderForCreation(userEmail, idParent, folderName);

                //If user is secondary, we create folder by parent
                int userId = GetUserIdByEmail(userEmail);

                int libraryId = GetChannelLibraryIdByUserId(userId);
                return PlaylistDal.CreateChannelFolder(userId, libraryId, folderName, folderDescription, idParent);

            }
            catch (Exception ex)
            {
                _logger.Error("Error in CreateChannelFolder", ex);
                throw;
            }*/
            return 1;
        }
    }
}
