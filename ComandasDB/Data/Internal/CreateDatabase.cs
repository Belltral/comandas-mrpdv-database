using System;
using System.Data.Entity;

namespace ComandasDB.Data.Internal
{
    public class CreateDatabase
    {
        public static void CreateDatabaseIfNoExists()
        {
            string appDataDirectory = AppDomain.CurrentDomain.BaseDirectory + "App_Data";

            try
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", appDataDirectory);

                Database.SetInitializer(new CreateDatabaseIfNotExists<ComandasMRPDVContext>());
                using (var db = new ComandasMRPDVContext())
                {
                    db.Database.Initialize(false);
                }

                AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
        }
    }
}
