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
        internal static int CreateUser(string name, string birthday, string connectionString)
        {

            using (var context = new usercrudEntities(connectionString))
            {
                var user = new user { Name = name, Birthday = birthday };

                
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
    }
}
