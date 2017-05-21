using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCRUDTransaction.DAL
{
    internal static class UserCRUDDal
    {
        internal static int CreateUser(string name, string birthday, string connectionString)
        {

            using (var context = new usercrudEntities(connectionString))
            {
                var user = new user { Name = name, Birthday = birthday };

                var userSave = context.user.Add(user);

                context.SaveChanges();
                return user.Id;
            }
        }
    }
}
