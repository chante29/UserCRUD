using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserCRUDRest.Utils
{
    public static class ExtensionMethods
    {

        public static SharedLibrary.User ToCommonUser(this User user)
        {
            return new SharedLibrary.User
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Birthday = user.Birthday
                            };
        }

        public static User ToDataContract(this SharedLibrary.User user)
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