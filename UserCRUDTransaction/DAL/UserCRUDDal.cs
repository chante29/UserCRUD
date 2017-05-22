using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCRUDTransaction.DAL
{
    internal static class UserCRUDDal
    {
        internal static int CreateUser(user user, string connectionString)
        {

            using (var context = new usercrudEntities(connectionString))
            {                
                bool saveFailed;
                do
                {
                    saveFailed = false;

                    try
                    {
                        var userSave = context.user.Add(user);
                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed); 
                return user.Id;
            }
        }

        internal static bool UpdateUser(user user, string connectionString)
        {

            using (var context = new usercrudEntities(connectionString))
            {
                bool saveFailed;
                do
                {
                    saveFailed = false;

                    try
                    {
                        var userInBBDD = context.user.FirstOrDefault(usr => usr.Id == user.Id);

                        userInBBDD.Name = user.Name;
                        userInBBDD.Birthday = user.Birthday;

                        context.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        ex.Entries.Single().Reload();
                    }

                } while (saveFailed);
                return true;
            }
        }
    }
}
