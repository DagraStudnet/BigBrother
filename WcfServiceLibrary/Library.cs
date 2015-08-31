using System.ServiceModel;
using ClassLibrary.UserLibrary;
using SqliteDatabase;


namespace WcfServiceLibrary
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Library : ILibrary
    {
        public void AddUser(User user)
        {
            DBTransaction dbTransaction = DBTransaction.ReturnDatabaseInstance();
            dbTransaction.AddUser(user);
        }

        public bool IsAlive()
        {
            return true;
        }
    }
}