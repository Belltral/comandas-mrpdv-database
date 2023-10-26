using System;
using System.Data.Entity;

namespace ComandasDB.Data.Internal
{
    public class CreateDatabase
    {
        public static void CreateDatabaseIfNoExists()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            Database.SetInitializer(new CreateDatabaseIfNotExists<ComandasMRPDVContext>());

            using (var db = new ComandasMRPDVContext())
            {
                db.Database.Initialize(false);
            }
        }
    }
}
