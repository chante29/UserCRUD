using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCRUDTransaction.DAL;

namespace UserCRUDTransaction.BLL.Utils
{
    public static class ExtensionsMethods
    {
        public static UserCRUDTransaction.DAL.user ToDAL(this SharedLibrary.User user)
        {
            return new user
            {
                Id = user.Id,
                Name = user.Name,
                Birthday = user.Birthday
            };
        }

        public static SharedLibrary.User ToSharedLibrary(this UserCRUDTransaction.DAL.user user)
        {
            return user == null ? null : new User
                                                {
                                                    Id = user.Id,
                                                    Name = user.Name,
                                                    Birthday = user.Birthday
                                                };
        }
    }
}
